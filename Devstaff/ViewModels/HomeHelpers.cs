using DataContext;
using System.Threading.Tasks;
using System;
using GlobalExtensionMethods;
using System.Collections.Generic;
using System.Linq;
using DevStaff.Models;

namespace DevStaff.ViewModels;

internal enum ActivityIndicator
{
    IdleTime,
    WorkingTime,
    Keyboard,
    Mouse
}

public partial class HomeViewModel
{
    #region HelperMethods

    private static TimeSpan IncrementTimeSeconds(TimeSpan timeSpan, int incrementBySeconds = 1) =>
        timeSpan.Add(TimeSpan.FromSeconds(incrementBySeconds));

    private static void IncrementUserActivity(ActivityIndicator activityIndicator, int incrementBy = 1)
    {
        switch (activityIndicator)
        {
            case ActivityIndicator.WorkingTime:
                Session.SelectedProject.Value().UserActivity.Value().TimeSpent =
                    IncrementTimeSeconds(Session.SelectedProject.Value().UserActivity.Value().TimeSpent, incrementBy);
                Session.UserActivity.Value().TimeSpent =
                    IncrementTimeSeconds(Session.UserActivity.Value().TimeSpent, incrementBy);
                break;
            case ActivityIndicator.IdleTime:
                Session.SelectedProject.Value().UserActivity.Value().IdolTime =
                    IncrementTimeSeconds(Session.SelectedProject.Value().UserActivity.Value().IdolTime, incrementBy);
                Session.UserActivity.Value().IdolTime =
                    IncrementTimeSeconds(Session.UserActivity.Value().IdolTime, incrementBy);
                break;
            case ActivityIndicator.Mouse:
                Session.SelectedProject.Value().UserActivity.Value().Clicks =
                    Session.SelectedProject.Value().UserActivity.Value().Clicks + incrementBy;
                Session.UserActivity.Value().Clicks = Session.UserActivity.Value().Clicks + incrementBy;
                break;
            case ActivityIndicator.Keyboard:
                Session.SelectedProject.Value().UserActivity.Value().KeyPresses =
                    Session.SelectedProject.Value().UserActivity.Value().KeyPresses + incrementBy;
                Session.UserActivity.Value().KeyPresses = Session.UserActivity.Value().KeyPresses + incrementBy;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(activityIndicator), activityIndicator, null);
        }
    }

    private async void SetUserInputActivity(ActivityIndicator activityIndicator)
    {
        await Task.Run(() =>
        {
            if (_backgroundJobService.GetIdleTimeInterval() == 1)
            {
                /*Show prompt for idle time to save it or
                 reassign it*/
            }

            IncrementUserActivity(activityIndicator);
            if (_backgroundJobService.GetIdleTimeInterval() == 1)
            {
                //ChangeTimerIntervalSec(_projectIdleTimeJob, 1);
            }
        });
    }

    private static void SelectProject(ProjectUi projectUiDto)
    {
        Session.SelectedProject = projectUiDto;
        Session.SelectedProject.IsSelected = true;
    }

    private static void ResetProjects() =>
        Session.Projects.ForEach(project =>
        {
            project.IsSelected = false;
            project.IsRunning = false;
        });

    private async Task UpdateIntervalActivity()
    {
        var userActivity = await _dataContextService.InsertActivity(Session.UserActivity.Value());
        var userIntervalEntity =
            await _dataContextService.InsertIntervalActivity(userActivity?.Id, Session.SelectedProject?.Id);
        await _dataContextService.InsertScreenshots(userIntervalEntity?.Id, Session.Screenshots);
        await _dataContextService.UpdateUserActivity(Session.SelectedProject.Value().UserActivity.Value());
    }

    private void StartProjectActivities()
    {
        CreateNewIntervalEntries();
        Session.SelectedProject.Value().IsRunning = true;
        //await Toaster.Show($"Timer Started on {Session.SelectedProject.Name}");
        _backgroundJobService.HookJobs();
    }

    private async Task StopProjectActivities()
    {
        _backgroundJobService.UnHookJobs();
        Session.SelectedProject.Value().IsRunning = false;
        if (Session.SelectedProject.HasValue())
        {
            //await Toaster.Show($"Timer Stopped on {Session.SelectedProject.Name}");
        }

        await UpdateIntervalActivity();
        DisposeIntervalEntries();
    }

    private void CreateNewIntervalEntries()
    {
        Session.UserActivity = new UserActivity
        {
            TimeSpent = TimeSpan.Zero,
            IdolTime = TimeSpan.Zero,
            Clicks = 0,
            KeyPresses = 0
        };
        Session.Screenshots.Clear();
    }

    private void DisposeIntervalEntries()
    {
        Session.UserActivity = null;
        Session.Screenshots.Clear();
    }

    private Task PerformCleanUp()
    {
        if (!Session.SelectedProject.HasValue() || !Session.SelectedProject.Value().IsRunning)
            return Task.CompletedTask;
        _backgroundJobService.UnHookJobs();
        return UpdateIntervalActivity();
    }

    private async void LoadData(int? loggedInUserId)
    {
        _cleanupService.SetCleanUpCallback(PerformCleanUp);
        while (true)
        {
            var userLoaded = await _dataContextService.LoadUser(loggedInUserId);
            if (userLoaded.HasValue())
            {
                var projects = await _dataContextService.LoadProjects(loggedInUserId);
                Session.LoggedInUser = userLoaded.Value();
                Session.Projects = MapProjectToProjectUi(projects.Value());
                Session.LastUpdatedAt = await _dataContextService.LoadLastUpdatedEntry();
                Session.OrganizationName = Session.LoggedInUser.OrganizationName;
                NotifyPropertyChange(nameof(Projects));
                NotifyPropertyChange(nameof(TodayTime));
                NotifyPropertyChange(nameof(UserOrganization));
                break;
            }

            await PopulateDummyData();
        }
    }

    private static List<ProjectUi> MapProjectToProjectUi(IEnumerable<Project> projects) =>
        projects.Select(project => new ProjectUi
        {
            Id = project.Id,
            Name = project.Name,
            UserActivity = project.UserActivity,
        }).ToList();

    private async Task PopulateDummyData()
    {
        var dummyUser = new User
        {
            OrganizationName = "Devsinc"
        };
        await _dataContextService.InsertUser(dummyUser);
        await CreateDummyProjects(new[]
        {
            "Avionte",
            "Karmak",
            "AHQ"
        }, dummyUser.Id);
    }

    private async Task CreateDummyProjects(IEnumerable<string> projectNames, int userId)
    {
        foreach (var projectName in projectNames)
        {
            var projectActivity = new UserActivity
            {
                TimeSpent = TimeSpan.Zero,
                IdolTime = TimeSpan.Zero,
                Clicks = 0,
                KeyPresses = 0,
            };
            await _dataContextService.InsertActivity(projectActivity);
            var project = new Project
            {
                Name = projectName,
                UserActivityId = projectActivity.Id,
                UserId = userId
            };
            await _dataContextService.InsertProject(project);
        }
    }

    #endregion HelperMethods
}