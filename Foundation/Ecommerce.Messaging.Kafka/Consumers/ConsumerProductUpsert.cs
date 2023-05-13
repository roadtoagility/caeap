// Copyright (C) 2023  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry.Serdes;
using DFlow.Validation;
using Ecommerce.Capabilities;
using Ecommerce.Capabilities.Messaging;
using Ecommerce.Capabilities.Supporting;
using Ecommerce.Domain.Events.Exported;
using Microsoft.Extensions.Logging;


namespace Ecommerce.Messaging.Kafka.Consumers;

public class ConsumerProductUpsert: IProductAgregateConsumer
{
    private const string EcommerceBrokerEndpoints = "ECOMMERCE_BROKER_ENDPOINTS";
    private const string EcommerceTopicProductChangelog = "ECOMMERCE_TOPIC_PRODUCT_CHANGELOG";
    private const string ConsumerGroupProductChangelog = "ProductChangelog";

    private readonly ConsumerConfig _consumerConfig;
    private readonly string _topicDestination;
    private readonly ILogger<ConsumerProductUpsert> _logger;
    
    public ConsumerProductUpsert(IConfig config, ILogger<ConsumerProductUpsert> logger)
    {
        _logger = logger;
        var configBrokers = config.FromEnvironment(EcommerceBrokerEndpoints);
        var configTopicConsuming = config.FromEnvironment(EcommerceTopicProductChangelog);
        
      
        if (configBrokers.IsSucceded == false)
        {
            throw new ArgumentException(EcommerceBrokerEndpoints);
        }

        if (!configTopicConsuming.IsSucceded || string.IsNullOrEmpty(configTopicConsuming.Succeded))
        {
            throw new ArgumentException(EcommerceTopicProductChangelog);
        }

        _consumerConfig = new ConsumerConfig
        {
            BootstrapServers = configBrokers.Succeded,
            ClientId = $"{ConsumerGroupProductChangelog}-{Guid.NewGuid().ToString("N")}",
            GroupId = ConsumerGroupProductChangelog, // grupo de instancia até o limite de partições
            AutoOffsetReset = AutoOffsetReset.Earliest,
            
            // at-least once - let kafka cliente manage the commits, but I do control the offsetstore
            // https://docs.confluent.io/kafka-clients/dotnet/current/overview.html#store-offsets
            EnableAutoCommit = true, // commit é controlado pela kafka cliente
            AutoCommitIntervalMs = 5000,  // update to kafka - pode estar alinhado com com o volume de mensagens
            EnableAutoOffsetStore = false, // em falhas da aplicação sem processar mensagens o offsetstore não avança
                                            // a mensagem é reenviada
            
            // we can call consumer.StoreOffset(result) to update last message processed
            //this gave to us a at-lest once garantee
            IsolationLevel = IsolationLevel.ReadCommitted // apenas ler mensagens gravadas, principalmente das que 
                                                        // em transações, enquanto não houver um commit da transçaão
                                                        // a mensagem não será recebida

        };

        _topicDestination = configTopicConsuming.Succeded;
    }

    private Task<Result<bool, Failure>> ProcessMessage(
        Message<string, ProductAggregate> message, CancellationToken cancellationToken)
    {
        if(!cancellationToken.IsCancellationRequested)
        {
            var data = message.Value;

            _logger.LogInformation($"Mensagem recebida {data.EventType} {data.ProductCreated.Name}");

            return Task.FromResult(Result<bool, Failure>.SucceedFor(true));
        }
        
        return Task.FromResult(Result<bool, Failure>.FailedFor(Failure.For("Cancelada","Operação cancelada.")));;
    }
    

    public async Task<Result<bool, Failure>> Consume(CancellationToken cancellationToken)
    {
        using var consumer = new ConsumerBuilder<string, ProductAggregate>(this._consumerConfig)
            .SetValueDeserializer(new ProtobufDeserializer<ProductAggregate>().AsSyncOverAsync())
            .SetErrorHandler((_, e) =>  _logger.LogError($"Error: {e.Reason}", e))
            .Build();

        consumer.Subscribe(_topicDestination);
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {

                var result = consumer.Consume(cancellationToken);

                if (result != null)
                {
                    var processed = await ProcessMessage(result.Message, cancellationToken);

                    if (processed.IsSucceded)
                    {
                        consumer.StoreOffset(result);
                        _logger.LogDebug(message: $"Atualizando offset @{result.TopicPartitionOffset}", result.TopicPartitionOffset);
                    }
                }
            }
            catch (InvalidDataException ex)
            {
                _logger.LogError($"Atualizando offset {ex.Message}", ex);
            }
        }

        consumer.Close();

        return Result<bool, Failure>.SucceedFor(true);
    }

}