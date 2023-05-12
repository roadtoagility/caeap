// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.Persistence;
using Ecommerce.Capabilities.Persistence.Repositories;
using Ecommerce.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Persistence;

public static class DependencyInjections
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddDbContext<EcommerceAppDbContext>();
        services.AddScoped<IProductRepository, ProductRepositoryWithEvents>();
        services.AddScoped<IDbSession<IProductRepository>, DbSession<IProductRepository>>();
    }
}