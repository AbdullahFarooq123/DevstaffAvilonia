using BackgroundJobs.Services.Classes;
using DataContext;
using GlobalExtensionMethods;
using System;
using DevStaff.Models;

namespace DevStaff.ViewModels;

public partial class HomeViewModel
{
    #region UICallbacks

    private void SelectProjectControl(ProjectUi projectUi)
    {
        if (Session.SelectedProject.HasValue() && Session.SelectedProject.Value().IsRunning)
        {
            //await Toaster.Show($"Another Project ('{Session.SelectedProject.Name}') Already Running");
            return;
        }

        ResetProjects();
        SelectProject(projectUi: projectUi);
    }

    private async void SelectAndRunProjectControl(ProjectUi projectUi)
    {
        if (Session.SelectedProject.HasValue() && Session.SelectedProject.Value().IsRunning)
        {
            await StopProjectActivities();
            if (Session.SelectedProject.Value().Id == projectUi.Id)
                return;
        }

        SelectProjectControl(projectUi: projectUi);
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
        NotifyPropertyChange(propertyName: nameof(LastUpdatedAt));
    }

    private void RunProjectCallback(object? sender)
    {
        IncrementUserActivity(activityIndicator: ActivityIndicator.WorkingTime);
        NotifySelectedProjectTime();
        NotifyTodayTime();
        NotifyProjectTime();
    }

    private void IdleTimeCallback(object? sender)
    {
        var seconds = 1;
        if (_backgroundJobService.GetIdleTimeInterval() == _appSettings.AllowedIdleTimeMin)
            seconds = TimerUtilities.MinToSec(min: _appSettings.AllowedIdleTimeMin);
        IncrementUserActivity(activityIndicator: ActivityIndicator.IdleTime, incrementBy: seconds);
        _backgroundJobService.ResetIdleTimeJobInterval();
    }

    private static void ScreenshotCallback(object? sender, string content) =>
        Session.Screenshots.Add(item: new Screenshot
        {
            Name = DateTime.UtcNow.ToFileName(),
            Content = content,
            ProjectId = Session.SelectedProject.Value().Id,
            CreatedAt = DateTime.UtcNow,
            IsInSync = false
        });

    private void MouseActivityCallback(object? sender, EventArgs eventArgs) =>
        SetUserInputActivity(activityIndicator: ActivityIndicator.Mouse);

    private void KeyboardActivityCallback(object? sender, EventArgs eventArgs) =>
        SetUserInputActivity(activityIndicator: ActivityIndicator.Keyboard);

    #endregion TimerCallbacks
}