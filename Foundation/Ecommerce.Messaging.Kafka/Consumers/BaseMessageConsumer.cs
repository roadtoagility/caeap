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


namespace Ecommerce.Messaging.Kafka.Consumers;

public abstract class BaseMessageConsumer<TKey, TValue> : IMessageConsumer where TValue : class
{
    private const string EcommerceBrokerEndpoints = "ECOMMERCE_BROKER_ENDPOINTS";
    private const string EcommerceTopicProductChangelog = "ECOMMERCE_TOPIC_PRODUCT_CHANGELOG";
    private const string ConsumerGroupProductChangelog = "ProductChangelog";
    private readonly ConsumerConfig _consumerConfig;
    private readonly string _topicDestination;

    protected BaseMessageConsumer(IConfig config)
    {
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
            EnableAutoCommit = true,
            // AutoCommitIntervalMs = 5000,  // update to kafka
            EnableAutoOffsetStore = false, // runs in a separated thread in a failure do can lost a message
            // we can call consumer.StoreOffset(result) to update last message processed
            //this gave to us a at-lest once garantee
            IsolationLevel = IsolationLevel.ReadCommitted // just read a commited transaction

        };

        _topicDestination = configTopicConsuming.Succeded;
    }

    public abstract Task<Result<bool, Failure>> ProcessMessage(
        Message<TKey, TValue> message, CancellationToken cancellationToken);

    public async Task<Result<bool, Failure>> Consume(CancellationToken cancellationToken)
    {
        var consumer = new ConsumerBuilder<TKey, TValue>(this._consumerConfig)
            .SetValueDeserializer(new JsonDeserializer<TValue>().AsSyncOverAsync())
            .Build();

        consumer.Subscribe(_topicDestination);

        while (!cancellationToken.IsCancellationRequested)
        {
            var result = consumer.Consume(3000);

            if (result != null)
            {
                var processed = await ProcessMessage(result.Message, cancellationToken);

                if (processed.IsSucceded)
                {
                    consumer.StoreOffset(result);
                }
            }
        }

        consumer.Close();

        return Result<bool, Failure>.SucceedFor(true);
    }

}