// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.BusinessObjects;
using Stock.Domain;
using Stock.Tests.Domain.DataProviders;
using Xunit;

namespace Stock.Tests.Domain;

public class ProductTests
{
    [Theory]
    [ClassData(typeof(ProductValidInputProvider))]
    public void CreateValidProduct(
        ProductId productId,
        ProductName name,
        ProductDescription description,
        ProductWeight weight,
        ProductPrice price,
        VersionId versionId, Product expected)
    {
        var product = Product.From(productId, name, description, weight, price, versionId);
        Assert.Equal(expected, product);
    }
}