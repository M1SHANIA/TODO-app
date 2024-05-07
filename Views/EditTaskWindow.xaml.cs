using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.Models;
using WpfApp1.ViewModels;

namespace WpfApp1.Views
{
    /// <summary>
    /// Логика взаимодействия для EditTaskWindow.xaml
    /// </summary>
    public partial class EditTaskWindow : Window
    {
        public Task Task { get; private set; }
        public ICommand SaveCommand { get; private set; }

        public EditTaskWindow(Task task)
        {
            InitializeComponent();
            Task = task;
            SaveCommand = new RelayCommand(() => DialogResult = true);
            DataContext = this;
        }
    }

}
