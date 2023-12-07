using BackgroundJobs.Services.Classes;
using DataContext;
using DataModel;
using GlobalExtensionMethods;
using Models;
using System;
using System.Linq;

namespace DevstaffAvilonia.ViewModels;

public partial class HomeViewModel
{
	#region UICallbacks
	private void SelectProjectControl(ProjectUI ProjectUIDto)
	{
		if (Session.SelectedProject.HasValue() && Session.SelectedProject.Value().IsRunning)
		{
			//await Toaster.Show($"Another Project ('{Session.SelectedProject.Name}') Already Running");
			return;
		}
		ResetProjects();
		SelectProject(ProjectUIDto);
	}
	private async void SelectAndRunProjectControl(ProjectUI projectUIDto)
	{
		if (Session.SelectedProject.HasValue() && Session.SelectedProject.Value().IsRunning)
		{
			await StopProjectActivities();
			if (Session.SelectedProject.Value().Id == projectUIDto.Id)
				return;
		}
		SelectProjectControl(projectUIDto);
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
			//await Toaster.Show("Select a project to start!");
			;
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
		int seconds = 1;
		if (_backgroundJobService.GetIdleTimeInterval() == _appSettings.AllowedIdleTime_Mins)
			seconds = TimerUtilities.MinutesToSeconds(_appSettings.AllowedIdleTime_Mins);
		IncrementUserActivity(ActivityIndicator.IdleTime, seconds);
		_backgroundJobService.ResetIdleTimeJobInterval();
	}
	private void ScreenshotCallback(string Content) =>
		Session.Screenshots.Add(new Screenshot
		{
			Name = DateTime.UtcNow.ToString("dd:MM:yyyy HH:mm tt").Replace(":", "_"),
			Content = Content,
			ProjectId = Session.SelectedProject.Value().Id,
			CreatedAt = DateTime.UtcNow,
		});
	private void MouseActivityCallback() => SetUserInputActivity(ActivityIndicator.Mouse);
	private void KeyboardActivityCallback() => SetUserInputActivity(ActivityIndicator.Keyboard);
	#endregion TimerCallbacks
}
