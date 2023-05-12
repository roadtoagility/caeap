// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections.Immutable;
using DFlow.Persistence;
using DFlow.Validation;
using Ecommerce.Capabilities;
using Ecommerce.Capabilities.Persistence.Repositories;
using Ecommerce.Domain;

namespace Ecommerce.Business;

public static class ProductViewExtensions
{
    public static ProductView ToProductView(this Product product)
    {
        return new(
            product.Identity.Value,
            product.Name.Value,
            product.Description.Value,
            product.Weight.Value);
    }
}

public record ProductView(Guid ProductId, string ProductName, string ProductDescription, double ProductWeight);

public sealed class ProductListHandler : IQueryHandler<ProductList, IReadOnlyList<ProductView>>
{
    private readonly IDbSession<IProductRepository> _sessionDb;

    public ProductListHandler(IDbSession<IProductRepository> sessionDb)
    {
        this._sessionDb = sessionDb;
    }

    public Task<Result<IReadOnlyList<ProductView>, IReadOnlyList<Failure>>> Execute(ProductList filter)
    {
        return Execute(filter, CancellationToken.None);
    }

    public async Task<Result<IReadOnlyList<ProductView>, IReadOnlyList<Failure>>> 
        Execute(ProductList filter, CancellationToken cancellationToken)
    {
        var products = await this._sessionDb.Repository
            .FindAsync(f => f.Name.Contains(filter.Name)
                            || f.Description.Contains(filter.Description)
                , cancellationToken);

        var view = products
            .Select(e => e.ToProductView()).ToImmutableList();
        
        return Result<IReadOnlyList<ProductView>, IReadOnlyList<Failure>>.SucceedFor(view);
    }
}