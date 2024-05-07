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
    // ViewModel for the Task model
    public class TaskViewModel : INotifyPropertyChanged
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsComplete { get; set; }
        public TimeSpan Timer { get; set; }
        public TaskStatus TaskStatus { get; set; }
        public TaskCategory TaskCategory { get; set; }
        public TaskImportance TaskImportance { get; set; }

        public ObservableCollection<TaskChecklist> TaskChecklist { get; set; }
        public ICommand DeleteTaskCommand { get; private set; }

        public ICommand EditTaskCommand { get; private set; }

        public event Action TaskAdded = delegate { };


        public ICommand IAddNewTask => new RelayCommand(AddNewTask);
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
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterTasks();
            }
        }

        private Task _selectedTask;
        public Task SelectedTask
        {
            get { return _selectedTask; }
            set
            {
                _selectedTask = value;
                OnPropertyChanged(nameof(SelectedTask));
            }
        }

        private ObservableCollection<Task> _filteredTasks;
        public ObservableCollection<Task> FilteredTasks
        {
            get => _filteredTasks;
            set
            {
                _filteredTasks = value;
                OnPropertyChanged(nameof(FilteredTasks));
            }
        }

        private void EditTask()
        {
            var editTaskWindow = new EditTaskWindow(SelectedTask);
            editTaskWindow.ShowDialog();
            _taskDataService.UpdateTask(SelectedTask);
        }

        private bool CanEditTask()
        {
            return SelectedTask != null;
        }


        // Event for property changed
        public event PropertyChangedEventHandler PropertyChanged;

        // Method to load tasks from the data service
        public void LoadTasks()
        {
            var taskList = _taskDataService.LoadTasks();
            Tasks = new ObservableCollection<Task>(taskList);
        }

        // Method to add a new task using the data service
        public void AddNewTask()
        {
           
            Task newTask = new Task
            {   
                Title = this.Title,
                Description =this.Description,
                Id= _taskDataService.GenerateId(),
                DueDate = this.DueDate,
                IsComplete = false,
                StartDate = DateTime.Now,
                TaskCategory = TaskCategory.Study,
                TaskChecklist=this.TaskChecklist,
                TaskImportance = TaskImportance.Critical,
                TaskStatus = TaskStatus.Late,
                Timer = new TimeSpan(0),
            };  
            _taskDataService.AddTask(newTask);

            ClearFields();
            LoadTasks();
            TaskAdded.Invoke();
        }

        private void ClearFields()
        {
            Title = "";
            Description = "";
            DueDate = DateTime.Now;
            TaskChecklist.Clear();
            
            UpdateForm();

        }

        private void UpdateForm()
        {
            OnPropertyChanged(Title);
            OnPropertyChanged(Description);
            OnPropertyChanged(nameof(DueDate));
            OnPropertyChanged(nameof(TaskChecklist));
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
        public TaskViewModel()
        {
            _taskDataService = new TaskDataService();
            TaskChecklist = new ObservableCollection<TaskChecklist>();
            DueDate = DateTime.Now;
            LoadTasks();
            DeleteTaskCommand = new RelayCommand<Task>(DeleteTask);
            FilteredTasks = Tasks;
            EditTaskCommand = new RelayCommand(EditTask, CanEditTask);
        }
        private void FilterTasks()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                FilteredTasks = Tasks;
            else
                FilteredTasks = new ObservableCollection<Task>(Tasks.Where(t => t.Title.Contains(SearchText)));
        }
        private void DeleteTask(Task task)
        {
            if (task != null)
            {
                Tasks.Remove(task);
                _taskDataService.DeleteTask(task.Id);
            }
        }

        // Method to invoke the PropertyChanged event
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
