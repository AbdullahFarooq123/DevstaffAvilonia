using BackgroundJobs.Services.Classes;
using DataContext;
using GlobalExtensionMethods;
using System;
using DevStaff.Models;

namespace DevStaff.ViewModels;

public partial class HomeViewModel
{
    #region UICallbacks

    private void SelectProjectControl(ProjectUi projectUiDto)
    {
        if (Session.SelectedProject.HasValue() && Session.SelectedProject.Value().IsRunning)
        {
            //await Toaster.Show($"Another Project ('{Session.SelectedProject.Name}') Already Running");
            return;
        }

        ResetProjects();
        SelectProject(projectUiDto);
    }

    private async void SelectAndRunProjectControl(ProjectUi projectUiDto)
    {
        if (Session.SelectedProject.HasValue() && Session.SelectedProject.Value().IsRunning)
        {
            await StopProjectActivities();
            if (Session.SelectedProject.Value().Id == projectUiDto.Id)
                return;
        }

        SelectProjectControl(projectUiDto);
        RunSelectedProjectControl();
    }

    private async void RunSelectedProjectControl()
    {
        if (Session.SelectedProject.HasValue())
        {
            if (!Session.SelectedProject.Value().IsRunning)
                StartProjectActivities();
            else
                await StopProjectActivities();
        }
        else
        {
            //Show prompt with message :Select a project to start:
        }
    }

    #endregion UICallbacks

    #region TimerCallbacks

    private async void LocalIntervalActivitiesCallback(object? sender)
    {
        await UpdateIntervalActivity();
        CreateNewIntervalEntries();
        NotifyPropertyChange(nameof(LastUpdatedAt));
    }

    private void RunProjectCallback(object? sender)
    {
        IncrementUserActivity(ActivityIndicator.WorkingTime);
        NotifySelectedProjectTime();
        NotifyTodayTime();
        NotifyProjectTime();
    }

    private void IdleTimeCallback(object? sender)
    {
        var seconds = 1;
        if (_backgroundJobService.GetIdleTimeInterval() == _appSettings.AllowedIdleTimeMin)
            seconds = TimerUtilities.MinutesToSeconds(_appSettings.AllowedIdleTimeMin);
        IncrementUserActivity(ActivityIndicator.IdleTime, seconds);
        _backgroundJobService.ResetIdleTimeJobInterval();
    }

    private static void ScreenshotCallback(string content) =>
        Session.Screenshots.Add(new Screenshot
        {
            Name = DateTime.UtcNow.ToString("dd:MM:yyyy HH:mm tt").Replace(":", "_"),
            Content = content,
            ProjectId = Session.SelectedProject.Value().Id,
            CreatedAt = DateTime.UtcNow,
            IsInSync = false
        });

    private void MouseActivityCallback() => SetUserInputActivity(ActivityIndicator.Mouse);
    private void KeyboardActivityCallback() => SetUserInputActivity(ActivityIndicator.Keyboard);

    #endregion TimerCallbacks
}