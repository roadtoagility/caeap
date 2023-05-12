// Copyright (C) 2023  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Text.Json;
using Confluent.Kafka;
using DFlow.Validation;
using Ecommerce.Capabilities;
using Ecommerce.Capabilities.Persistence.State;
using Ecommerce.Capabilities.Persistence.States;
using Ecommerce.Capabilities.Supporting;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Messaging.Kafka.Consumers;

public class ConsumerProductUpsert: BaseMessageConsumer<string,AggregateState>
{
    private readonly ILogger<ConsumerProductUpsert> _logger;
    
    public ConsumerProductUpsert(IConfig config, ILogger<ConsumerProductUpsert> logger)
    :base(config)
    {
        _logger = logger;
    }

    public override Task<Result<bool, Failure>> ProcessMessage(Message<string, AggregateState> message, CancellationToken cancellationToken)
    {
        var data = message.Value;
            
        _logger.LogDebug("Mensagem recebida ", data);
        
        return Task.FromResult(Result<bool, Failure>.SucceedFor(true));
    }
}