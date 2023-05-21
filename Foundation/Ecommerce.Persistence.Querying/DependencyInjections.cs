// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using DFlow.Persistence;
using DFlow.Persistence.Repositories;
using Ecommerce.Capabilities.Querying.Views;
using Ecommerce.Persistence.Querying.Repositories;
using Microsoft.Extensions.DependencyInjection;


namespace Ecommerce.Persistence.Querying;

public static class DependencyInjections
{
    public static void AddQueryRepositories(this IServiceCollection services)
    {
        services.AddScoped<EcommerceQueryingDbContext>();
        services.AddScoped<IRepository<ProductView,ProductView>, ProductQuerying>();
        services.AddScoped<IDbSession<IRepository<ProductView,ProductView>>, 
            DbSession<IRepository<ProductView,ProductView>>>();
    }
}