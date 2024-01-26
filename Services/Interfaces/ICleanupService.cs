namespace Services.Interfaces;

public interface ICleanupService
{
    public delegate Task EventHandler();

    public void SetCleanUpCallback(EventHandler e);
}