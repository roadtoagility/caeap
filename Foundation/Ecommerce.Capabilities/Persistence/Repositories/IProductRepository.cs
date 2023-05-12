// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Linq.Expressions;
using DFlow.Persistence.Repositories;
using Ecommerce.Capabilities.Persistence.State;
using Ecommerce.Domain;

namespace Ecommerce.Capabilities.Persistence.Repositories;

public interface IProductRepository : IRepository<ProductBaseState, Product>
{
    Task<Product> GetById(ProductId id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Product>> FindAsync(Expression<Func<ProductBaseState, bool>> predicate, int pageNumber, int pageSize,
        CancellationToken cancellationToken);
}