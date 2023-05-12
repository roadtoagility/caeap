using Ecommerce.Capabilities.Persistence.States;

namespace Ecommerce.Capabilities.Persistence.State;

public sealed record ProductBaseState(Guid Id, string Name, string Description
        , double Weight, byte[] RowVersion)
    : BaseState(RowVersion);