using GlobalExtensionMethods;
using Services.Interfaces;
using static Services.Interfaces.ICleanupService;

namespace Services.Classes;

public class CleanupService : ICleanupService
{
    private EventCallback? _eventHandler;

    public async Task Cleanup()
    {
        if (_eventHandler.HasValue())
            await _eventHandler.Value().Invoke();
    }

    public void SetCleanUpCallback(EventCallback e) => _eventHandler = e;
}