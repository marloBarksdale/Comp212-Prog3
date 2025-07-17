using OMS.Commands;
using OMS.Views;
using System.Windows.Input;

namespace OMS.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private object? _currentView;

        public MainWindowViewModel()
        {
            // Initialize with the ListOrderDetailsView
            CurrentView = new ListOrderDetailsView();

            // Initialize commands
            ShowListOrderDetailsCommand = new RelayCommand(ShowListOrderDetails);
            ShowAddNewItemCommand = new RelayCommand(ShowAddNewItem);
            ExitCommand = new RelayCommand(Exit);
        }

        public object? CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public ICommand ShowListOrderDetailsCommand { get; }
        public ICommand ShowAddNewItemCommand { get; }
        public ICommand ExitCommand { get; }

        private void ShowListOrderDetails(object? parameter)
        {
            CurrentView = new ListOrderDetailsView();
        }

        private void ShowAddNewItem(object? parameter)
        {
            CurrentView = new AddNewItemView();
        }

        private void Exit(object? parameter)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}