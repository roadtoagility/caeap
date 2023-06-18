// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using Microsoft.AspNetCore.Mvc;

namespace Stock.Consumer.Querying.ApiEndpoints;

public static class EndpointRoutes
{
    public static void StateChangeApis(WebApplication app)
    {
        // app.MapGet("/api/v1/products", async ([FromServices] IQueryHandler<ProductList, ProductView> handler) =>
        // {
        //     var result = await handler.Execute(new ProductList("", "", 1, 10));
        //     if (result.IsSucceded == false)
        //     {
        //         return Results.BadRequest(result.Failed);
        //         
        //     }
        //
        //     return Results.Ok(result.Succeded);
        // });
    }
}