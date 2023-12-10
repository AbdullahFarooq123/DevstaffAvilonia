using System;
using System.Collections.Generic;
using DataContext;

namespace DevStaff.Models;

public static class Session
{
    public static string OrganizationName { get; set; } = "";
    public static string ProjectSearchString { get; set; } = "";
    public static string TimerLimits { get; set; } = "No Limits";
    public static DateTime LastUpdatedAt { get; set; } = DateTime.MinValue;
    public static TimeSpan TodayTime { get; set; } = TimeSpan.Zero;
    public static User LoggedInUser { get; set; } = null!;
    public static ProjectUi? SelectedProject { get; set; }
    public static UserActivity? UserActivity { get; set; }
    public static List<Screenshot> Screenshots { get; set; } = new();
    public static List<ProjectUi> Projects { get; set; } = new();
}