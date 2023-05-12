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

public sealed class ProductUpdateHandler : ICommandHandler<ProductUpdate, Guid>
{
    private readonly IDbSession<IProductRepository> _sessionDb;

    public ProductUpdateHandler(IDbSession<IProductRepository> sessionDb)
    {
        this._sessionDb = sessionDb;
    }

    public Task<Result<Guid, IReadOnlyList<Failure>>> Execute(ProductUpdate command)
    {
        return Execute(command, CancellationToken.None);
    }

    public async Task<Result<Guid, IReadOnlyList<Failure>>> Execute(ProductUpdate command, CancellationToken cancellationToken)
    {
        var product = await this._sessionDb.Repository.GetById(ProductId.From(command.Id),cancellationToken);
        
        var aggregate = ProductAggregationRoot.Create(product);
        
        aggregate.Update(ProductDescription.From(command.Description),ProductWeight.From(command.Weight));

        if (!aggregate.IsValid)
        {
            return Result<Guid, IReadOnlyList<Failure>>.FailedFor(aggregate.Failures);
        }

        await this._sessionDb.Repository.Add(aggregate.GetChange());
        await this._sessionDb.SaveChangesAsync(cancellationToken);
        
        return Result<Guid, IReadOnlyList<Failure>>.SucceedFor(aggregate.GetChange().Identity.Value);
    }
}