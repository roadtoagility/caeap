// Copyright (C) 2023  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text;
using System.Text.Json;
using Confluent.Kafka;

namespace Ecommerce.Messaging.Kafka.Producers.Serializers;

public class MySimpleWrapJsonSerializer<TValue>: IAsyncSerializer<TValue> where TValue: class
{
    public Task<byte[]> SerializeAsync(TValue data, SerializationContext context)
    {
        return Task.FromResult(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data, data.GetType())));
    }
}