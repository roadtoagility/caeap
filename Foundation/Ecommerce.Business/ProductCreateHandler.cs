// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.Persistence;
using DFlow.Validation;
using Ecommerce.Capabilities;
using Ecommerce.Capabilities.Persistence.Repositories;
using Ecommerce.Domain;
using Ecommerce.Domain.Aggregates;

namespace Ecommerce.Business;

public sealed class ProductCreateHandler : ICommandHandler<ProductCreate, Guid>
{
    private readonly IDbSession<IProductRepository> _sessionDb;

    public ProductCreateHandler(IDbSession<IProductRepository> sessionDb)
    {
        this._sessionDb = sessionDb;
    }

    public Task<Result<Guid, IReadOnlyList<Failure>>> Execute(ProductCreate command)
    {
        return Execute(command, CancellationToken.None);
    }

    public async Task<Result<Guid, IReadOnlyList<Failure>>> 
        Execute(ProductCreate command, CancellationToken cancellationToken)
    {
        var aggregate = ProductAggregationRoot.Create(ProductName.From(command.Name),
            ProductDescription.From(command.Description),
            ProductWeight.From(command.Weight),
            ProductPrice.From(command.Price)
            );
        
        if (aggregate.IsValid)
        {
            await this._sessionDb.Repository.Add(aggregate.GetChange());
            await this._sessionDb.SaveChangesAsync(cancellationToken);
            return Result<Guid, IReadOnlyList<Failure>>.SucceedFor(aggregate.GetChange().Identity.Value);
        }

        return Result<Guid, IReadOnlyList<Failure>>.FailedFor(aggregate.Failures);
    }
}