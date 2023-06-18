namespace Stock.Capabilities.Querying.Views;

public sealed record ProductView(Guid Id, string Name, string Description, double Weight, bool IsDeleted)
{

}