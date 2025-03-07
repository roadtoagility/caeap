﻿using System;
using System.Diagnostics;
using System.Linq;
using Ecommerce.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Tests.Integration.Support
{
    public class EcommerceSUT<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<EcommerceAppDbContext>));

                Debug.Assert(descriptor != null, nameof(descriptor) + " != null");
                services.Remove(descriptor);

                services.AddDbContext<EcommerceAppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("EmMemoriaBancoParaTeste");
                    options.EnableSensitiveDataLogging();
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<EcommerceAppDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<EcommerceSUT<TStartup>>>();

                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();

                    try
                    {
                        var dbClientInit = new IntegrationClientDataset(db);
                        dbClientInit.InitializeDbForTests();
                        var dbProjectInit = new IntegrationProjectDataset(db);
                        dbProjectInit.InitializeDbForTests();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                                            "database with test messages. Error: {Message}", ex.Message);
                    }
                }
            });
        }
    }
}