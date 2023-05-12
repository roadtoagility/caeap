// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Ecommerce.Capabilities.Persistence.State;
using Ecommerce.Capabilities.Persistence.States;
using Ecommerce.Capabilities.Supporting;
using Ecommerce.Persistence.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ecommerce.Persistence;

public sealed class EcommerceAppDbContext : DbContext
{
    private const string EcommerceModelDatabase = "ECOMMERCE_MODEL_DATABASE";
    private readonly string _connectionString;
    
    public EcommerceAppDbContext(IConfig config)
    {
        var result = config.FromEnvironment(EcommerceModelDatabase);

        if (!result.IsSucceded)
        {
            throw new ArgumentException(EcommerceModelDatabase);
        }

        _connectionString = result.Succeded;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString,o => o.UseNodaTime());
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new ProductStateMapping().Configure(modelBuilder.Entity<ProductBaseState>());
        new AggregateStateMapping().Configure(modelBuilder.Entity<AggregateState>());
    }
    
    public override int SaveChanges()
    {
        UpdateSoftDeleteLogic();
        return base.SaveChanges();
    }

    private void UpdateSoftDeleteLogic()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.CurrentValues["IsDeleted"] = true;
            }
            else
            {
                entry.CurrentValues["IsDeleted"] = false;
            }
        }
    }
}