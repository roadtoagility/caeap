// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.BusinessObjects;
using Ecommerce.Capabilities.Persistence.State;
using Ecommerce.Domain;

namespace Ecommerce.Persistence.ExtensionMethods;

public static class BusinessObjectsExtensions
{
    public static ProductBaseState ToProductState(this Product product)
    {
        return new(
            product.Identity.Value,
            product.Name.Value,
            product.Description.Value,
            product.Weight.Value,
            BitConverter.GetBytes(product.Version.Value));
    }

    public static Product ToProduct(this ProductBaseState baseState)
    {
        return Product.From(
            ProductId.From(baseState.Id),
            ProductName.From(baseState.Name),
            ProductDescription.From(baseState.Description),
            ProductWeight.From(baseState.Weight),
            VersionId.From(BitConverter.ToInt32(baseState.RowVersion)));
    }
}