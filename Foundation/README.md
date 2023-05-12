## Use Case it's a simple Ecommerce

E-commerce use case from Debezium tutorial with few updates.

## What you will see



## Project structure

- Ecommerce.Tests - for testing
- Ecommerce - it's a library with all layers for simplification
- Web api - self-hosted http apis

## Run migrations

```bash
~/Ecommerce/EcommerceWebAPI$ dotnet ef migrations  add  Initial --context EcommerceAppDbContext --project EcommerceWebAPI.csproj --output-dir ../Ecommerce/Persistence/Migrations
```



