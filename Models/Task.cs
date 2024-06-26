﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp1.ViewModels;

namespace WpfApp1.Models
{
    public class Task
    {
        public int Id { get; set; }
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

        private void AddNewTask()
        {
            throw new NotImplementedException();
        }
    }

    public enum TaskStatus
    {
        InProgress,
        Completed,
        NotStarted,
        Late,
        Archieved,
        Deleted
    }

    public enum TaskCategory
    {
        Work,
        Personal,
        Shopping,
        Study,
        Other
    }

    public enum  TaskImportance
    {
        Low,
        Medium,
        High,
        Critical
    }
}
