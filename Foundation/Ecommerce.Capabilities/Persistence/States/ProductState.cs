namespace Ecommerce.Capabilities.Persistence.States;

public sealed record ProductState(Guid Id, string Name, string Description
        , double Weight, byte[] RowVersion)
    : BaseState(RowVersion);