using Avalonia.Controls;
using Avalonia.Input;
using DevstaffAvilonia.ViewModels;
using GlobalExtensionMethods;
using Models;
using System;
namespace DevstaffAvilonia.Views;

public partial class Home : Window
{
	public Home() => InitializeComponent();

	#region Events
	private void SelectProject(object? sender, PointerReleasedEventArgs e)
	{
		if (sender is Border border && DataContext is HomeViewModel viewModel)
		{
			if (border.Tag.HasNoValue()) return;
			if (GetProject(border.Tag, viewModel) is ProjectUI project)
			{
				viewModel.SelectProjectCallback(project);
				viewModel.NotifySelectedProjectName();
				viewModel.NotifySelectedProjectTime();
				viewModel.NotifySelectedProjectRunning();
				viewModel.NotifyProjects();
			}
		}
	}
	private void RunSelectedProject(object? sender, PointerReleasedEventArgs e)
	{
		if (sender is Image image && DataContext is HomeViewModel viewModel)
		{
			viewModel.RunSelectedProjectCallback();
			viewModel.NotifySelectedProjectRunning();
			viewModel.NotifyProjects();
		}
	}
	private void SelectAndRunProject(object? sender, PointerReleasedEventArgs e)
	{
		if (sender is Image image && DataContext is HomeViewModel viewModel)
		{
			if (image.Tag.HasNoValue()) return;
			if (GetProject(image.Tag, viewModel) is ProjectUI project)
			{
				viewModel.SelectAndRunProjectCallback(project);
				viewModel.NotifySelectedProjectName();
				viewModel.NotifySelectedProjectTime();
				viewModel.NotifySelectedProjectRunning();
				viewModel.NotifyProjects();
			}
		}
	}
	private ProjectUI GetProject(object? tag, HomeViewModel viewModel)
	{
		if (int.TryParse(tag.Value().ToString(), out int projectId))
			return viewModel.GetProjectById(projectId) ??
				throw new InvalidOperationException($"No project found with id {projectId}");
		throw new InvalidCastException("Project id is not an int");
	}
	#endregion Events
}
