namespace Stock.Capabilities.Persistence.States;

public sealed record ProductState(Guid Id, string Name, string Description
        , float Weight, decimal Price, byte[] RowVersion)
    : BaseState(RowVersion);