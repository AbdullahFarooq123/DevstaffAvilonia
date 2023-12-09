using Avalonia.Controls;
using Avalonia.Input;
using GlobalExtensionMethods;
using System;
using DevStaff.Models;
using DevStaff.ViewModels;

namespace DevStaff.Views;

public partial class Home : Window
{
    public Home() => InitializeComponent();

    #region Events

    private void SelectProject(object? sender, PointerReleasedEventArgs _)
    {
        if (sender is not Border border || DataContext is not HomeViewModel viewModel) return;
        if (border.Tag.HasNoValue()) return;
        if (GetProject(border.Tag, viewModel) is not { } project) return;
        viewModel.SelectProjectCallback(project);
        viewModel.NotifySelectedProjectName();
        viewModel.NotifySelectedProjectTime();
        viewModel.NotifySelectedProjectRunning();
        viewModel.NotifyProjects();
    }

    private void RunSelectedProject(object? sender, PointerReleasedEventArgs _)
    {
        if (sender is not Image || DataContext is not HomeViewModel viewModel) return;
        viewModel.RunSelectedProjectCallback();
        viewModel.NotifySelectedProjectRunning();
        viewModel.NotifyProjects();
    }

    private void SelectAndRunProject(object? sender, PointerReleasedEventArgs _)
    {
        if (sender is not Image image || DataContext is not HomeViewModel viewModel) return;
        if (image.Tag.HasNoValue()) return;
        if (GetProject(image.Tag, viewModel) is not { } project) return;
        viewModel.SelectAndRunProjectCallback(project);
        viewModel.NotifySelectedProjectName();
        viewModel.NotifySelectedProjectTime();
        viewModel.NotifySelectedProjectRunning();
        viewModel.NotifyProjects();
    }

    private ProjectUi GetProject(object? tag, HomeViewModel viewModel)
    {
        if (int.TryParse(tag.Value().ToString(), out var projectId))
            return viewModel.GetProjectById(projectId) ??
                   throw new InvalidOperationException($"No project found with id {projectId}");
        throw new InvalidCastException("Project id is not an int");
    }

    #endregion Events
}