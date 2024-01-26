using System.ComponentModel;

namespace DevStaff.ViewModels;

public class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void NotifyPropertyChange(string propertyName) =>
        PropertyChanged?.Invoke(sender: this, e: new PropertyChangedEventArgs(propertyName));
}