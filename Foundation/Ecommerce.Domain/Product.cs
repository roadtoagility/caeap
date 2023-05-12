// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.BusinessObjects;

namespace Ecommerce.Domain;

public sealed class Product : EntityBase<ProductId>
{
    public Product(ProductId identity, ProductName name, ProductDescription description, ProductWeight weight,
        VersionId version)
        : base(identity, version)
    {
        Description = description;
        Name = name;
        Weight = weight;

        AppendValidationResult(identity.ValidationStatus.Failures);
        AppendValidationResult(description.ValidationStatus.Failures);
        AppendValidationResult(name.ValidationStatus.Failures);
        AppendValidationResult(weight.ValidationStatus.Failures);
    }

    public ProductName Name { get; }

    public ProductWeight Weight { get; }
    public ProductDescription Description { get; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Identity;
        yield return Name;
        yield return Description;
        yield return Weight;
    }

    public static Product From(ProductId id, ProductName name, ProductDescription description
        , ProductWeight weight, VersionId version)
    {
        return new Product(id, name, description, weight, version);
    }

    public static Product NewProduct(ProductName name, ProductDescription description
        , ProductWeight weight)
    {
        return From(ProductId.NewId(), name, description, weight, VersionId.New());
    }

    public static Product Empty()
    {
        return From(ProductId.Empty, ProductName.Empty, ProductDescription.Empty
            , ProductWeight.Empty, VersionId.Empty());
    }

    public static Product CombineDescriptionAndWeight(Product aggregateRootEntity, ProductDescription description, ProductWeight weight)
    {
        return From(aggregateRootEntity.Identity, aggregateRootEntity.Name, description, weight,
            VersionId.Next(aggregateRootEntity.Version));
    }
}