// Copyright (C) 2023  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Security.Cryptography;
using Stock.Business;

namespace StockApp.Workers;

public class ProductGenerator: BackgroundService
{
    private readonly ILogger<ProductGenerator> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    
    public ProductGenerator(IHttpClientFactory httpClientFactory,
        IServiceProvider services,
        ILogger<ProductGenerator> logger)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        
        _logger.LogInformation("Consumer running");
        
        while(!stoppingToken.IsCancellationRequested)
        {
            var httpClient = _httpClientFactory.CreateClient("StockApi");
            var httpResponseMessage = await httpClient.PostAsJsonAsync(
                "/api/v1/products", new ProductCreate( Description: $"Produto {RandomNumberGenerator.GetInt32(100,100000)}",
                    Name:$"Produto_{RandomNumberGenerator.GetInt32(100,100000)}" ,
                    Weight: RandomNumberGenerator.GetInt32(1,100),
                    Price: RandomNumberGenerator.GetInt32(20,3100)));
            
            await Task.Delay(RandomNumberGenerator.GetInt32(1000,3000), stoppingToken);
        }
    }
}