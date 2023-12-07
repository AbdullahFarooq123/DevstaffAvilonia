using DataModel;
using Services.Interfaces;
using GlobalExtensionMethods;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using DynamicData;
using BackgroundJobs.Services.Interfaces;
using HelperServices;
using Models;

namespace DevstaffAvilonia.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
	private readonly IBackgroundJobService _backgroundJobService;
	private readonly IDataContextService _dataContextService;
	private readonly ICleanupService _cleanupService;
	private readonly AppSettings _appSettings;
	private readonly AppDimentions _appDimentions;
	private readonly ObservableCollection<ProjectUI> _projects;

	#region Ctor
	public HomeViewModel(
		IBackgroundJobService backgroundJobService,
		IDataContextService dataContextService,
		ICleanupService cleanupService,
		AppDimentions appDimentions,
		AppSettings appSettings)
	{
		_backgroundJobService = backgroundJobService;
		_dataContextService = dataContextService;
		_cleanupService = cleanupService;
		_appSettings = appSettings;
		_appDimentions = appDimentions;
		_projects = new ObservableCollection<ProjectUI>();
		InitComponents();
	}
	#endregion Ctor

	#region ViewModel Properties
	public int MaxWidth => _appDimentions.MaxWidth;
	public int MinWidth => _appDimentions.MinWidth;
	public int DefaultWidth => _appDimentions.DefaultWidth;
	public int MinHeight => _appDimentions.MinHeight;
	public int DefaultHeight => _appDimentions.DefaultHeight;
	public string SelectedProjectName
	{
		get
		{
			if (Session.SelectedProject.HasValue())
				return Session.SelectedProject.Value().Name;
			return "No Project Selected";
		}
	}
	public bool SelectedProjectRunning => Session.SelectedProject?.IsRunning ?? false;
	public string SelectedProjectTime
	{
		get
		{
			if (Session.SelectedProject.HasValue() && Session.SelectedProject.Value().UserActivity.HasValue())
				return $"{Session.SelectedProject.Value().UserActivity.Value().TimeSpent:hh\\:mm\\:ss}";
			return $"{TimeSpan.Zero:hh\\:mm\\:ss}";
		}
	}
	public List<ProjectUI> Projects
	{
		get
		{
			if (Session.ProjectSearchString.IsNotNullOrEmpty())
				return _projects.Where(project => project.Name.ToLower().Contains(Session.ProjectSearchString.ToLower())).ToList();
			return Session.Projects.ToList();
		}
	}
	public string ProjectSearchString
	{
		get => Session.ProjectSearchString;
		set
		{
			if (Session.ProjectSearchString != value)
			{
				Session.ProjectSearchString = value;
				NotifyPropertyChange(nameof(Projects));
			}
		}
	}
	public string TodayTime
	{
		get
		{
			var totalTime = TimeSpan.Zero;
			foreach (var time in Session.Projects.Select(project => project.UserActivity?.TimeSpent ?? throw new InvalidOperationException($"No user activity for project id : {project.Id}")))
				totalTime = totalTime.Add(time);
			return $"Today: {totalTime:hh\\:mm}";
		}
	}
	public string UserOrganization => Session.LoggedInUser?.OrganizationName ?? "";
	public string LastUpdatedAt => $"Last updated at: {Session.LastUpdatedAt.ToString("dd/MM/yyyy hh:mm tt")}";
	#endregion ViewModel Properties

	#region Notify Methods
	public void NotifySelectedProjectRunning() => NotifyPropertyChange(nameof(SelectedProjectRunning));
	public void NotifySelectedProjectName() => NotifyPropertyChange(nameof(SelectedProjectName));
	public void NotifySelectedProjectTime() => NotifyPropertyChange(nameof(SelectedProjectTime));
	public void NotifyTodayTime() => NotifyPropertyChange(nameof(TodayTime));
	public void NotifyProjects() => NotifyPropertyChange(nameof(Projects));
	public void NotifyProjectTime() => Session.SelectedProject.Value().NotifyTimeChanged();
	public void NotifyProjectRunning() => Session.SelectedProject.Value().NotifyIsRunning();
	public void NotifyProjectSelected() => Session.SelectedProject.Value().NotifyIsSelected();
	#endregion Notify Methods

	#region Exposed Helpers
	public ProjectUI? GetProjectById(int projectId) => _projects.Where(project => project.Id == projectId).FirstOrDefault();
	#endregion Exposed Helpers

	#region Private Helper Methods
	private void InitComponents()
	{
		LoadData(1);
		_projects.AddRange(Session.Projects);
		_backgroundJobService.RegisterCallbacks(
			activityTimeCallback: RunProjectCallback,
			idleTimeCallback: IdleTimeCallback,
			screenshotsActivityCallback: ScreenshotCallback,
			mouseActivityCallback: MouseActivityCallback,
			keyboardActivityCallback: KeyboardActivityCallback,
			dataSyncCallback: LocalIntervalActivitiesCallback
			);
	}
	#endregion Private Helper Methods

	#region Exposed Callbacks
	public void SelectProjectCallback(ProjectUI project) => SelectProjectControl(project);
	public void SelectAndRunProjectCallback(ProjectUI project) => SelectAndRunProjectControl(project);
	public void RunSelectedProjectCallback() => RunSelectedProjectControl();
	#endregion Exposed Callbacks
}
