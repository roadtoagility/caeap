// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using Ecommerce.Business;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWebAPI.ApiEndpoints;

// https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-8.0
public static class EndpointRouteHealthchecks
{
    public static void HealthCheckspis(WebApplication app)
    {
        app.MapPost("/api/v1/products", async ([FromBody] ProductCreate command
            , [FromServices]ICommandHandler<ProductCreate, Guid> handler) =>
        {
            return Results.Ok();
        });

        app.MapPut("/api/v1/products/{productId:guid}", async ([FromRoute]Guid productId, 
            [FromBody] ProductUpdateDetail command,
            [FromServices] ICommandHandler<ProductUpdate, Guid> handler) =>
        {
            var result = await handler.Execute(new ProductUpdate(productId, command.Description, command.Weight));
        
            if (result.IsSucceded == false)
            {
                return Results.BadRequest(result.Failed);
            }
        
            return Results.Ok(result.Succeded);
        });
        
        app.MapGet("/api/v1/products", async ([FromServices] IQueryHandler<ProductList, ProductView> handler) =>
        {
            var result = await handler.Execute(new ProductList("", "", 1, 10));
            if (result.IsSucceded == false)
            {
                return Results.BadRequest(result.Failed);
                
            }

            return Results.Ok(result.Succeded);
        });
    }
}