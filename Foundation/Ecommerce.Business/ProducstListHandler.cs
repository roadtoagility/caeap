// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.Persistence;
using DFlow.Persistence.Repositories;
using DFlow.Validation;
using Ecommerce.Capabilities;
using Ecommerce.Capabilities.Querying.Views;

namespace Ecommerce.Business;

public sealed class ProductListHandler : IQueryHandler<ProductList, IReadOnlyList<ProductView>>
{
    private readonly IDbSession<IRepository<ProductView,ProductView>> _sessionDb;

    public ProductListHandler(IDbSession<IRepository<ProductView,ProductView>> sessionDb)
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
        var result = await this._sessionDb.Repository
            .FindAsync(f => f.Name.Contains(filter.Name)
                            || f.Description.Contains(filter.Description)
                , cancellationToken);
       
        return Result<IReadOnlyList<ProductView>, IReadOnlyList<Failure>>.SucceedFor(result);
    }
}