using Services.Interfaces;

namespace Services.Classes;

public class CleanupService : ICleanupService
{
    private ICleanupService.EventHandler? _handler;
    public void Cleanup() => _handler?.Invoke();

    public void SetCleanUpCallback(ICleanupService.EventHandler e) => _handler = e;
}