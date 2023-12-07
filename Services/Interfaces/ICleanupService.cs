namespace Services.Interfaces;

public interface ICleanupService
{
	public delegate Task EventCallback();
	public void SetCleanUpCallback(EventCallback e);
	public Task Cleanup();
}
