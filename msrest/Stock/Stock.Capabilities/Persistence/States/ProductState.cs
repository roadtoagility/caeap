namespace Stock.Capabilities.Persistence.States;

public sealed record ProductState(Guid Id, string Name, string Description
        , float Weight, float Price, byte[] RowVersion)
    : BaseState(RowVersion);