// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Linq.Expressions;
using DFlow.Persistence.Repositories;
using Stock.Capabilities.Querying.Views;
using Stock.Domain;
using Microsoft.EntityFrameworkCore;
using Stock.Querying;


namespace Stock.Persistence.Querying.Repositories;

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

    // https://www.postgresql.org/docs/14/textsearch.html
    // https://www.postgresql.org/docs/14/textsearch-tables.html
    // https://www.npgsql.org/efcore/mapping/full-text-search.html?tabs=pg12%2Cv5
    // 
    public async Task<IReadOnlyList<ProductView>> FindAsync(Expression<Func<ProductView, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<ProductView>().AsNoTracking()
            .Where(predicate)
            .ToListAsync(cancellationToken);
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