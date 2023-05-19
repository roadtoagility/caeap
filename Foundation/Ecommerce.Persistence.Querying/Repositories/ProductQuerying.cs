// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Linq.Expressions;
using DFlow.Persistence.Repositories;
using Ecommerce.Capabilities.Persistence.States;
using Ecommerce.Capabilities.Querying.Repositories;
using Ecommerce.Capabilities.Querying.Views;
using Ecommerce.Domain;
using Microsoft.EntityFrameworkCore;


namespace Ecommerce.Persistence.Querying.Repositories;

public class ProductQuerying : IRepository<ProductView, ProductView>
{
    private readonly EcommerceQueryingDbContext _dbContext;

    public ProductQuerying(EcommerceQueryingDbContext dbContext)
    {
        this._dbContext = dbContext;
    }
    public async Task Add(ProductView entity)
    {
        var cancel = new CancellationTokenSource();

        var oldState = await this._dbContext.Set<ProductView>()
            .AsNoTracking()
            .Where(e => e.Id.Equals(entity.Id))
            .FirstOrDefaultAsync(cancel.Token);

        if (oldState == null)
        {
            this._dbContext.Set<ProductView>().Add(entity);
        }
        else
        {
            this._dbContext.Entry(oldState).CurrentValues.SetValues(entity);
        }
    }

    public Task<IReadOnlyList<ProductView>> FindAsync(Expression<Func<ProductView, bool>> predicate, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task Remove(ProductView entity)
    {
        var cancel = new CancellationTokenSource();

        var oldState = await this._dbContext.Set<ProductView>()
            .AsNoTracking()
            .Where(e => e.Id.Equals(entity.Id))
            .FirstOrDefaultAsync(cancel.Token);

        if (oldState == null)
        {
            throw new ArgumentException(
                $"O produto {entity.Name} com identificação {entity.Id} não foi encontrado.");
        }
        
        this._dbContext.Set<ProductView>().Remove(entity);
    }

    public async Task<ProductView> GetById(ProductId id, CancellationToken cancellation)
    {
        return await this._dbContext.Set<ProductView>()
            .Where(p => p.Id.Equals(id.Value)).AsNoTracking()
            .FirstAsync(cancellation);
    }
}