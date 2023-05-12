// Copyright (C) 2023  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using Confluent.Kafka;
using DFlow.Validation;
using Ecommerce.Capabilities;
using Ecommerce.Capabilities.Messaging;
using Ecommerce.Capabilities.Persistence.State;
using Ecommerce.Capabilities.Supporting;
using Ecommerce.Messaging.Kafka.Consumers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Messaging.Kafka.Services;

public class ProductUpsertHostedService: BackgroundService
{
    private readonly ILogger<ConsumerProductUpsert> _logger;
    private readonly IMessageConsumer _consumer;
    
    public ProductUpsertHostedService(IMessageConsumer consumer, 
        ILogger<ConsumerProductUpsert> logger)
    {
        _logger = logger;
        _consumer = consumer;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Consumer running");
        
        if(!stoppingToken.IsCancellationRequested)
        {
            await _consumer.Consume(stoppingToken);
        }
    }
}