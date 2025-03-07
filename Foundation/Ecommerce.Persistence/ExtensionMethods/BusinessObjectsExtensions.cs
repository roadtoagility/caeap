// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.BusinessObjects;
using Ecommerce.Capabilities.Persistence.States;
using Ecommerce.Domain;

namespace Ecommerce.Persistence.ExtensionMethods;

public static class BusinessObjectsExtensions
{
    public static ProductState ToProductState(this Product product)
    {
        return new(
            product.Identity.Value,
            product.Name.Value,
            product.Description.Value,
            product.Weight.Value,
            product.Price.Value,
            BitConverter.GetBytes(product.Version.Value));
    }

    public static Product ToProduct(this ProductState state)
    {
        return Product.From(
            ProductId.From(state.Id),
            ProductName.From(state.Name),
            ProductDescription.From(state.Description),
            ProductWeight.From(state.Weight),
            ProductPrice.From(state.Price),
            VersionId.From(BitConverter.ToInt32(state.RowVersion)));
    }
}