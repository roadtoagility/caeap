// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Ecommerce.Domain;
using Ecommerce.Tests.Domain.DataProviders;
using Xunit;

namespace Ecommerce.Tests.Domain;

public class BusinessObjectTests
{
    [Theory]
    [ClassData(typeof(ProductNameValidInputProvider))]
    public void CreateValidProductName(string input, bool expected)
    {
        var name = ProductName.From(input);
        Assert.Equal(expected, name.ValidationStatus.IsValid);
    }

    [Theory]
    [ClassData(typeof(ProductNameInvalidInputProvider))]
    public void CreateInvalidProductName(string input, bool expected)
    {
        var name = ProductName.From(input);
        Assert.Equal(expected, name.ValidationStatus.IsValid);
    }

    [Theory]
    [ClassData(typeof(ProductWeightValidInputProvider))]
    public void CreateValidWeight(float input, ProductWeight expected)
    {
        var name = ProductWeight.From(input);
        Assert.Equal(expected, name);
    }

    [Theory]
    [ClassData(typeof(ProductWeightInvalidInputProvider))]
    public void CreateInvalidWeight(float input, ProductWeight expected)
    {
        var name = ProductWeight.From(input);
        Assert.Equal(expected, name);
    }

    [Theory]
    [ClassData(typeof(ProductDescriptionValidInputProvider))]
    public void CreateValidDescription(string input, bool expected)
    {
        var name = ProductDescription.From(input);
        Assert.Equal(expected, name.ValidationStatus.IsValid);
    }

    [Theory]
    [ClassData(typeof(ProductDescriptionInvalidInputProvider))]
    public void CreateInvalidDescription(string input, bool expected)
    {
        var name = ProductDescription.From(input);
        Assert.Equal(expected, name.ValidationStatus.IsValid);
    }
}