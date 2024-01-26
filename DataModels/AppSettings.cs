namespace DataModels;

public record AppSettings(
    int AllowedIdleTimeMin,
    int ScreenshotIntervalMin,
    int LinuxInputRefreshSec,
    int SyncIntervalMin
);