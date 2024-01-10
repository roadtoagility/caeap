using NodaTime;

namespace Stock.Capabilities.Persistence.States;

public record BaseState(byte[] RowVersion)
{
    public bool IsDeleted { get; set; }
    // public Instant CreateAt { get; set; }
}