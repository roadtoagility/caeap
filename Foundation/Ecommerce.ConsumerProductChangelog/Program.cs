// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using Ecommerce.ConsumerProductChangelog;
using Ecommerce.Messaging.Kafka;
using Ecommerce.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
    .AddEnvironmentVariables("ECOMMERCE_");

builder.Services.AddSupporting();

//hosted services
builder.Services.AddConsumers();

var app = builder.Build();

app.Run();

//It's for integration testing propose :(
// https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0
public partial class Program { }