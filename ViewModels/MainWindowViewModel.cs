using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WpfApp1.DataService;
using WpfApp1.Models;
using WpfApp1.Views;

namespace WpfApp1.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public TaskViewModel TaskViewModel { get; set; }
        private NewTaskWindow _newTaskWindow;

        public MainWindowViewModel()
        {
            TaskViewModel = new TaskViewModel();
            TaskViewModel.TaskAdded += TaskViewModel.LoadTasks;

            _newTaskWindow = new NewTaskWindow();
            _newTaskWindow.TaskAdded += TaskViewModel.LoadTasks;
        }

        public ICommand IOpenNewWindow => new RelayCommand(OpenNewWindow);

        private void OpenNewWindow()
        {
            _newTaskWindow.Show();
        }
        private Task _selectedTask;

        public Task SelectedTask
        {
            get { return _selectedTask; }
            set
            {
                _selectedTask = value;
                OnPropertyChanged("SelectedTask");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
    
}
