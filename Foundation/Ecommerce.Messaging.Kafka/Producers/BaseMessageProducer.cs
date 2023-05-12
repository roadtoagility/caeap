// Copyright (C) 2023  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using Confluent.Kafka;
using DFlow.Validation;
using Ecommerce.Capabilities;
using Ecommerce.Capabilities.Messaging;
using Ecommerce.Capabilities.Persistence.State;
using Ecommerce.Capabilities.Supporting;
using Ecommerce.Messaging.Kafka.Producers.Serializers;

namespace Ecommerce.Messaging.Kafka.Producers;

public abstract class BaseMessageProducer<TValue>: IMessageProducer<TValue> where TValue:class
{    
    private const string EcommerceBrokerEndpoints = "ECOMMERCE_BROKER_ENDPOINTS";
    private const string EcommerceTopicProductCreated = "ECOMMERCE_TOPIC_PRODUCT_CHANGELOG";
    protected IProducer<string, TValue> Producer { get; }
    protected string TopicDestination { get; }

    protected BaseMessageProducer(IConfig config)
    {
        var configBrokers = config.FromEnvironment(EcommerceBrokerEndpoints);
        var configTopicPublishing = config.FromEnvironment(EcommerceTopicProductCreated);

        if (configBrokers.IsSucceded == false)
        {
            throw new ArgumentException(EcommerceBrokerEndpoints);
        }
        
        if (!configTopicPublishing.IsSucceded || string.IsNullOrEmpty(configTopicPublishing.Succeded))
        {
            throw new ArgumentException(EcommerceTopicProductCreated);
        }
        
        TopicDestination = configTopicPublishing.Succeded;
        
        ProducerConfig producerConfig = new ProducerConfig
        {
            BootstrapServers = configBrokers.Succeded,
            RequestTimeoutMs = 4000,
            Acks = Acks.Leader, // can be set to All 
            // EnableIdempotence = true, // this includes a unique id every message published
            RetryBackoffMs = 3, // number off reties in case off failure
            MessageTimeoutMs = 3000, // timmeout for sucessefull message delivery confirmation,
            // Debug = "all"
        };

        Producer = Build<string>(producerConfig);
    }
    
    private IProducer<TKey, TValue> Build<TKey>(ProducerConfig config)
    {
        var producer = new ProducerBuilder<TKey, TValue>(config)
            .SetValueSerializer(new MySimpleWrapJsonSerializer<TValue>())
            .Build();
        return producer;
    }
    
    public abstract Task<Result<bool, Failure>> Produce(TValue change, CancellationToken cancellationToken);
}