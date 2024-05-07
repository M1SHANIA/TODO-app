using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using WpfApp1.Models;

namespace WpfApp1.DataService
{
    public class TaskDataService
    {
        // Define file path, folder name and file name
        private readonly string _filePath;
        private readonly string folderName = "WpfApp1";
        private readonly string fileName = "tasks.json";

        public TaskDataService()
        {
            // Get the path to the Application Data folder
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            // Combine the Application Data path with the folder name
            string appFolder = Path.Combine(appDataPath, folderName);
            // Combine the app folder path with the "data" folder
            string dataFolder = Path.Combine(appFolder, "data");
            // If the data folder doesn't exist, create it
            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }
            // Combine the data folder path with the file name to get the full file path
            _filePath = Path.Combine(dataFolder, fileName);

            // Initialize the file
            InitializeFile();
        }

        private void InitializeFile()
        {
            // If the file doesn't exist, create it and write an empty JSON array to it
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, JsonSerializer.Serialize(new List<Task>()));
            }
            // Open the folder containing the file (for debugging purposes)
            //Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folderName));
        }

        // Load tasks from the file
        public List<Task> LoadTasks()
        {
            string fileContent = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Task>>(fileContent);
        }

        // Save tasks to the file
        public void SaveTasks(List<Task> tasks)
        {
            string json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        // Add a new task to the file
        public void AddTask(Task newTask)
        {
            // Generate an ID for the new task
            newTask.Id = GenerateId();

            // Load the existing tasks
            var task = LoadTasks();
            // Add the new task to the list
            task.Add(newTask);
            // Save the updated list back to the file
            SaveTasks(task);
        }

        public void UpdateTask(Task updatedTask)
        {
            // Load the existing tasks
            var tasks = LoadTasks();
            // Find the task to update
            var taskIndex = tasks.FindIndex(t => t.Id == updatedTask.Id);
            // If the task was found, update it
            if (taskIndex != -1)
            {
                tasks[taskIndex] = updatedTask;
                SaveTasks(tasks);
            }
            // Save the updated list back to the file
            SaveTasks(tasks);
        }

        public void DeleteTask(int taskId)
        {
            // Load the existing tasks
            var tasks = LoadTasks();
            // Find the task to delete
            var taskIndex = tasks.FindIndex(t => t.Id == taskId);
            // If the task was found, delete it
            if (taskIndex != -1)
            {
                tasks.RemoveAll(t => t.Id == taskId);
                SaveTasks(tasks);
            }
        }

        // Generate a new ID for a task
        public int GenerateId()
        {
            // Load the existing tasks
            var tasks = LoadTasks();

            // If there are no tasks, return 1
            if (!tasks.Any())
            {
                return 1;
            }

            // Otherwise, return one more than the maximum existing ID
            int maxId = tasks.Max(t => t.Id);
            return maxId + 1;
        }
    }
}
