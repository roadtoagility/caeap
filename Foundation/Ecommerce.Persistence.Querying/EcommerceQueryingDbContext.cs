// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Ecommerce.Capabilities.Persistence.States;
using Ecommerce.Capabilities.Querying.Views;
using Ecommerce.Capabilities.Supporting;
using Ecommerce.Persistence.Querying.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Persistence.Querying;

public sealed class EcommerceQueryingDbContext : DbContext
{
    private const string EcommerceQueryingDatabase = "ECOMMERCE_QUERYING_DATABASE";
    private readonly string _connectionString;
    
    public EcommerceQueryingDbContext(IConfig config)
    {
        var result = config.FromEnvironment(EcommerceQueryingDatabase);

        if (!result.IsSucceded)
        {
            throw new ArgumentException(EcommerceQueryingDatabase);
        }

        _connectionString = result.Succeded;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new ProductViewMapping().Configure(modelBuilder.Entity<ProductView>());
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