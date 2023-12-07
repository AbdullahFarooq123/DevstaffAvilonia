using DataContext;
using GlobalExtensionMethods;
using System;
using System.ComponentModel;

namespace Models;

public class ProjectUI : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;
	protected void NotifyPropertyChange(string propertyName) =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	public int Id { get; set; }
	public string Name { get; set; }
	public UserActivity? UserActivity { get; set; }
	public bool IsSelected { get; set; } = false;
	public bool IsRunning { get; set; } = false;
	public bool Selected => IsSelected && !IsRunning;
	public string Time
	{
		get
		{
			if (UserActivity.HasValue())
				return $"{UserActivity.Value().TimeSpent:hh\\:mm}";
			return $"{TimeSpan.Zero:hh\\:mm}";
		}
	}

	public void NotifyTimeChanged() => NotifyPropertyChange(nameof(Time));
	public void NotifyIsSelected() => NotifyPropertyChange(nameof(IsSelected));
	public void NotifyIsRunning() => NotifyPropertyChange(nameof(IsRunning));
}
