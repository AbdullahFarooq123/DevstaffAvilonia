using Services.Interfaces;
using GlobalExtensionMethods;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using DynamicData;
using BackgroundJobs.Services.Interfaces;
using DataModels;
using DevStaff.Models;
using HelperServices;

namespace DevStaff.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    private readonly IBackgroundJobService _backgroundJobService;
    private readonly IDataContextService _dataContextService;
    private readonly ICleanupService _cleanupService;
    private readonly AppSettings _appSettings;
    private readonly AppDimensions _appDimensions;
    private readonly ObservableCollection<ProjectUi> _projects;

    #region Ctor

    public HomeViewModel(
        IBackgroundJobService backgroundJobService,
        IDataContextService dataContextService,
        ICleanupService cleanupService,
        AppDimensions appDimensions,
        AppSettings appSettings)
    {
        _backgroundJobService = backgroundJobService;
        _dataContextService = dataContextService;
        _cleanupService = cleanupService;
        _appSettings = appSettings;
        _appDimensions = appDimensions;
        _projects = new ObservableCollection<ProjectUi>();
        InitComponents();
    }

    #endregion Ctor

    #region ViewModel Properties

    public int MaxWidth => _appDimensions.MaxWidth;
    public int MinWidth => _appDimensions.MinWidth;
    public int DefaultWidth => _appDimensions.DefaultWidth;
    public int MinHeight => _appDimensions.MinHeight;
    public int DefaultHeight => _appDimensions.DefaultHeight;

    public string SelectedProjectName => Session.SelectedProject.HasValue()
        ? Session.SelectedProject.Value().Name
        : "No Project Selected";

    public bool SelectedProjectRunning => Session.SelectedProject?.IsRunning ?? false;

    public string SelectedProjectTime
    {
        get
        {
            if (Session.SelectedProject.HasValue() && Session.SelectedProject.Value().UserActivity.HasValue())
                return $@"{Session.SelectedProject.Value().UserActivity.Value().TimeSpent:hh\:mm\:ss}";
            return $@"{TimeSpan.Zero:hh\:mm\:ss}";
        }
    }

    public List<ProjectUi> Projects
    {
        get
        {
            if (Session.ProjectSearchString.IsNotNullOrEmpty())
                return _projects
                    .Where(project => project.Name.ToLower().Contains(Session.ProjectSearchString.ToLower())).ToList();
            return Session.Projects.ToList();
        }
    }

    public string ProjectSearchString
    {
        get => Session.ProjectSearchString;
        set
        {
            if (Session.ProjectSearchString == value) return;
            Session.ProjectSearchString = value;
            NotifyPropertyChange(nameof(Projects));
        }
    }

    public string TodayTime
    {
        get
        {
            var totalTime = Session.Projects
                .Select(project =>
                    project.UserActivity?.TimeSpent ??
                    throw new InvalidOperationException($"No user activity for project id : {project.Id}"))
                .Aggregate(TimeSpan.Zero, (current, time) => current.Add(time));
            return $"Today: {totalTime:hh\\:mm}";
        }
    }

    public string UserOrganization => Session.LoggedInUser.OrganizationName ?? "";
    public string LastUpdatedAt => $"Last updated at: {Session.LastUpdatedAt:dd/MM/yyyy hh:mm tt}";

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

    public ProjectUi? GetProjectById(int projectId) =>
        _projects.FirstOrDefault(project => project.Id == projectId);

    #endregion Exposed Helpers

    #region Private Helper Methods

    private void InitComponents()
    {
        LoadData(1);
        _projects.AddRange(Session.Projects);
        _backgroundJobService.RegisterCallbacks(
            activityTimeCallback: RunProjectCallback,
            idleTimeCallback: IdleTimeCallback,
            screenshotsCallback: ScreenshotCallback,
            mouseActCallback: MouseActivityCallback,
            keyboardActCallback: KeyboardActivityCallback,
            dataSyncCallback: LocalIntervalActivitiesCallback
        );
    }

    #endregion Private Helper Methods

    #region Exposed Callbacks

    public void SelectProjectCallback(ProjectUi project) => SelectProjectControl(project);
    public void SelectAndRunProjectCallback(ProjectUi project) => SelectAndRunProjectControl(project);
    public void RunSelectedProjectCallback() => RunSelectedProjectControl();

    #endregion Exposed Callbacks
}