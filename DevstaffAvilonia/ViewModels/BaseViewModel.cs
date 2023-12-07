using System.ComponentModel;

namespace DevstaffAvilonia.ViewModels;

public class BaseViewModel : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;
	protected void NotifyPropertyChange(string propertyName) =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
