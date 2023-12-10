using System;
using System.ComponentModel;
using DataContext;
using GlobalExtensionMethods;

namespace DevStaff.Models;

public class ProjectUi : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChange(string propertyName) =>
        PropertyChanged?.Invoke(sender: this, e: new PropertyChangedEventArgs(propertyName));

    public int Id { get; init; }
    public required string Name { get; init; }
    public UserActivity? UserActivity { get; init; }
    public bool IsSelected { get; set; }
    public bool IsRunning { get; set; }
    public bool Selected => IsSelected && !IsRunning;

    public string Time =>
        UserActivity.HasValue() ? $@"{UserActivity.Value().TimeSpent:hh\:mm}" : $@"{TimeSpan.Zero:hh\:mm}";

    public void NotifyTimeChanged() => NotifyPropertyChange(nameof(Time));
    public void NotifyIsSelected() => NotifyPropertyChange(nameof(IsSelected));
    public void NotifyIsRunning() => NotifyPropertyChange(nameof(IsRunning));
}