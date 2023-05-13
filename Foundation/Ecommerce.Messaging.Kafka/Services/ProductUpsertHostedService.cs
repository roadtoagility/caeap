// Copyright (C) 2023  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using Ecommerce.Capabilities.Messaging;
using Ecommerce.Messaging.Kafka.Consumers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Messaging.Kafka.Services;

public class ProductUpsertHostedService: BackgroundService
{
    private readonly ILogger<ProductUpsertHostedService> _logger;
    private readonly IProductAgregateConsumer _consumer;
    
    public ProductUpsertHostedService(IProductAgregateConsumer consumer, 
        ILogger<ProductUpsertHostedService> logger)
    {
        _logger = logger;
        _consumer = consumer;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        
        _logger.LogInformation("Consumer running");
        
        if(!stoppingToken.IsCancellationRequested)
        {
            await _consumer.Consume(stoppingToken);
        }
    }
}