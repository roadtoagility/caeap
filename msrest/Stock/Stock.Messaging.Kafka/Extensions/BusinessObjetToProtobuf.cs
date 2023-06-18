// Copyright (C) 2023  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using Stock.Capabilities.Persistence.States;
using Google.Protobuf.WellKnownTypes;
using Stock.Domain.Events.Exported;
using ProductCreatedEventProto = Stock.Domain.Events.Exported.ProductCreatedEvent;
using ProductUpdatedEventProto = Stock.Domain.Events.Exported.ProductUpdatedEvent;
using ProductCreatedEvent = Stock.Domain.Events.ProductCreatedEvent;
using ProductUpdatedEvent = Stock.Domain.Events.ProductUpdatedEvent;

namespace Stock.Messaging.Kafka.Extensions;

public static class BusinessObjetToProtobuf
{
    public static ProductAggregate ToProtobuf(this AggregateState stateChange)
    {
        var exported = new ProductAggregate
        {
            EventProcessingTimeMs = DateTimeOffset.Now.ToUnixTimeMilliseconds(), 
            EventType = stateChange.EventType
        };

        switch (stateChange.EventType)
        {
            case nameof(ProductCreatedEvent):
                exported.ProductCreated = new Domain.Events.Exported.ProductCreatedEvent
                {
                    Id = stateChange.EventData.RootElement.GetProperty("Id").GetString(),
                    Name = stateChange.EventData.RootElement.GetProperty("Name").GetString(),
                    Description = stateChange.EventData.RootElement.GetProperty("Description").GetString(),
                    Weight = stateChange.EventData.RootElement.GetProperty("Weight").GetDouble(),
                    EventTime = Timestamp.FromDateTimeOffset(stateChange.EventData.RootElement
                        .GetProperty("When").GetDateTimeOffset())
                };
                break;
            case nameof(ProductUpdatedEvent):
                exported.ProductUpdated = new Domain.Events.Exported.ProductUpdatedEvent
                {
                    Id = stateChange.EventData.RootElement.GetProperty("Id").GetString(),
                    Description = stateChange.EventData.RootElement.GetProperty("Description").GetString(),
                    Weight = stateChange.EventData.RootElement.GetProperty("Weight").GetDouble(),
                    EventTime = Timestamp.FromDateTimeOffset(stateChange.EventData.RootElement
                        .GetProperty("When").GetDateTimeOffset())
                };
                break;
        }

        return exported;

    }
    public static ProductAggregate ToProtobuf(this ProductCreatedEvent domainEvent)
    {
        return new ProductAggregate
        {
            EventProcessingTimeMs = DateTimeOffset.Now.ToUnixTimeMilliseconds(), 
            EventType = domainEvent.GetType().Name,
            ProductCreated = new Domain.Events.Exported.ProductCreatedEvent
            {
                Description = domainEvent.Description,
                Id = domainEvent.Id.ToString("D"),
                Name = domainEvent.Name,
                Weight = domainEvent.Weight,
                EventTime = Timestamp.FromDateTimeOffset(domainEvent.When)
            }
        };
        
    }

    public static ProductAggregate ToProtobuf(this ProductUpdatedEvent domainEvent)
    {
        return new ProductAggregate
        {
            EventProcessingTimeMs = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
            EventType = domainEvent.GetType().Name,
            ProductUpdated = new Domain.Events.Exported.ProductUpdatedEvent()
            {
                Description = domainEvent.Description,
                Id = domainEvent.Id.ToString("D"),
                Weight = domainEvent.Weight,
                EventTime = Timestamp.FromDateTimeOffset(domainEvent.When)
            }
        };
    }
}
