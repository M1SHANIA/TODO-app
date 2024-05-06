using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using WpfApp1.DataService;
using WpfApp1.Models;

namespace WpfApp1.ViewModels
{
    // ViewModel for the Task model
    public class TaskViewModel : INotifyPropertyChanged
    {
        // Instance of the TaskDataService
        private readonly TaskDataService _taskDataService;

        // Collection of tasks
        private ObservableCollection<Task> _tasks;

        // Property for the tasks collection
        public ObservableCollection<Task> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                OnPropertyChanged(nameof(Tasks));
            }
        }

        // Event for property changed
        public event PropertyChangedEventHandler PropertyChanged;

        // Method to load tasks from the data service
        private void LoadTasks()
        {
            var taskList = _taskDataService.LoadTasks();
            Tasks = new ObservableCollection<Task>(taskList);
        }

        // Method to add a new task using the data service
        public void AddNewTask(Task newTask)
        {
            _taskDataService.AddTask(newTask);
            LoadTasks();
        }

        // Method to update a task using the data service
        public void UpdateTask(Task updatedTask)
        {
            _taskDataService.UpdateTask(updatedTask);
            LoadTasks();
        }

        // Method to delete a task using the data service
        public void DeleteTask(int taskId)
        {
            _taskDataService.DeleteTask(taskId);
            LoadTasks();
        }

        // Constructor for the ViewModel
        TaskViewModel()
        {
            _taskDataService = new TaskDataService();
        }

        // Method to invoke the PropertyChanged event
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
