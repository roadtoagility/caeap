// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using Stock.Capabilities.Messaging;
using Stock.Capabilities.Persistence.States;
using Microsoft.Extensions.DependencyInjection;
using Stock.Messaging.Kafka.Consumers;
using Stock.Messaging.Kafka.Producers;
using Stock.Messaging.Kafka.Services;

namespace Stock.Messaging.Kafka;

public static class DependencyInjections
{
    public static void AddProducers(this IServiceCollection services)
    {
        services.AddScoped<IMessageProducer<AggregateState>, ProducerProductChangelog>();
    }
    
    public static void AddConsumers(this IServiceCollection services)
    {
        services.AddSingleton<IProductAggregateConsumer, ConsumerProductUpsert>();
        services.AddHostedService<ProductUpsertHostedService>();
    }
}