// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Collections.Immutable;
using System.Linq.Expressions;
using Stock.Capabilities.Messaging;
using Stock.Capabilities.Persistence.States;
using Stock.Domain;
using Microsoft.EntityFrameworkCore;
using Stock.Capabilities.Persistence.Repositories;
using Stock.Persistence.ExtensionMethods;


namespace Stock.Persistence.Repositories;

public class ProductRepositoryWithOutbox : IProductRepository
{
    private readonly StockDbContext _dbContext;
    private readonly int initialPageNumber = 1;
    private readonly int recordPageSizeLimit = 20;
    

    public ProductRepositoryWithOutbox(StockDbContext dbContext)
    {
        this._dbContext = dbContext;
    }
    public async Task Add(Product entity)
    {
        var entry = entity.ToProductState();

        var cancel = new CancellationTokenSource();

        var oldState = await this._dbContext.Set<ProductState>()
            .AsNoTracking()
            .Where(e => e.Id.Equals(entity.Identity.Value))
            .FirstOrDefaultAsync(cancel.Token);

        if (oldState == null)
        {
            this._dbContext.Set<ProductState>().Add(entry);
        }
        else
        {
            var currentId = BitConverter.ToInt32(oldState.RowVersion) + 1;
            if (currentId > entity.Version.Value)
            {
                throw new DbUpdateConcurrencyException("This version is not the most updated for this object.");
            }

            this._dbContext.Entry(oldState).CurrentValues.SetValues(entry);
        }
        
        var outbox = entity.ToOutBox();
        await this._dbContext
            .Set<AggregateState>()
            .AddRangeAsync(outbox,cancel.Token);
    }

    public async Task Remove(Product entity)
    {
        var cancel = new CancellationTokenSource();

        var oldState = await this._dbContext.Set<ProductState>()
            .AsNoTracking()
            .Where(e => e.Id.Equals(entity.Identity.Value))
            .FirstOrDefaultAsync(cancel.Token);

        if (oldState == null)
        {
            throw new ArgumentException(
                $"O produto {entity.Name} com identificação {entity.Identity} não foi encontrado.");
        }

        var entry = entity.ToProductState();
        this._dbContext.Set<ProductState>().Remove(entry);
        
        var outbox = entity.ToOutBox();
        await this._dbContext
            .Set<AggregateState>()
            .AddRangeAsync(outbox,cancel.Token);
    }

    public async Task<IReadOnlyList<Product>> FindAsync(Expression<Func<ProductState, bool>> predicate
        , CancellationToken cancellationToken)
    {
        return await FindAsync(predicate, this.initialPageNumber, this.recordPageSizeLimit, cancellationToken);
    }

    public async Task<Product> GetById(ProductId id, CancellationToken cancellation)
    {
        var result = await FindAsync(p => p.Id.Equals(id.Value), cancellation);

        return result.Count == 0 ? Product.Empty() : result.First();
    }

    public async Task<IReadOnlyList<Product>> FindAsync(Expression<Func<ProductState, bool>> predicate,
        int pageNumber,
        int pageSize, CancellationToken cancellationToken)
    {
        try
        {
            return await this._dbContext.Set<ProductState>()
                .Where(predicate).AsNoTracking()
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .Select(t => t.ToProduct())
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }
        catch (InvalidOperationException)
        {
            return ImmutableList<Product>.Empty;
        }
    }
}