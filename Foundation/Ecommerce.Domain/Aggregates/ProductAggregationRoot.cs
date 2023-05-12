using DFlow.Aggregates;
using Ecommerce.Domain.Events;

namespace Ecommerce.Domain.Aggregates;

public sealed class ProductAggregationRoot : AggregateBase<Product, ProductId>
{
    public ProductAggregationRoot(Product product)
    : base(product)
    {
        if (product.IsValid)
        {
            Apply(product);

            if (product.IsNew())
            {
                Raise(ProductCreatedEvent.For(product));
            }
        }
        else
        {
            AppendValidationResult(product.Failures);
        }
    }

    public static ProductAggregationRoot Create(ProductName name, ProductDescription description, ProductWeight weight)
    {
        var product = Product.NewProduct(name, description, weight);
        return new ProductAggregationRoot(product);
    }

    public static ProductAggregationRoot Create(Product product)
    {
        return new ProductAggregationRoot(product);
    }
    
    public void Update(ProductDescription description, ProductWeight weight)
    {
        var current = Product.CombineDescriptionAndWeight(Root, description, weight);
        
        if (current.IsValid)
        {
            Apply(current);
            Raise(ProductUpdatedEvent.For(current));
        }
        
        AppendValidationResult(current.Failures);
    }
}