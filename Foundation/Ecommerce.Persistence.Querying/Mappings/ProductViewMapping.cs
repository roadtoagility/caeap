// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Ecommerce.Capabilities.Querying.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Persistence.Querying.Mappings;

public class ProductViewMapping : IEntityTypeConfiguration<ProductView>
{
    public void Configure(EntityTypeBuilder<ProductView> builder)
    {
        builder.ToTable("products");
        builder.Property(e => e.Id)
            .HasColumnName("id").ValueGeneratedNever().IsRequired();
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name).HasColumnName("name");
        builder.Property(e => e.Description).HasColumnName("description");
        builder.Property(e => e.Weight).HasColumnName("weight");

        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        builder.HasQueryFilter(user => EF.Property<bool>(user, "IsDeleted") == false);
    }
}