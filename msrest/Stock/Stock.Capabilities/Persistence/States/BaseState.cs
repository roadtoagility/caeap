namespace Stock.Capabilities.Persistence.States;

public record BaseState(byte[] RowVersion)
{
    public bool IsDeleted { get; set; }
}