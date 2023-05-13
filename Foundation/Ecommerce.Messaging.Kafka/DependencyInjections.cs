// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using Ecommerce.Capabilities.Messaging;
using Ecommerce.Capabilities.Persistence.States;
using Ecommerce.Messaging.Kafka.Consumers;
using Ecommerce.Messaging.Kafka.Producers;
using Ecommerce.Messaging.Kafka.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Messaging.Kafka;

public static class DependencyInjections
{
    public static void AddProducers(this IServiceCollection services)
    {
        services.AddScoped<IMessageProducer<AggregateState>, ProducerProductChangelog>();
    }
    
    public static void AddConsumers(this IServiceCollection services)
    {
        services.AddSingleton<IProductAgregateConsumer, ConsumerProductUpsert>();
        services.AddHostedService<ProductUpsertHostedService>();
    }
}