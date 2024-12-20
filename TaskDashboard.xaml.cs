using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Task_Manager
{
    /// <summary>
    /// Interaction logic for TaskDashboard.xaml
    /// </summary>
    public partial class TaskDashboard : Window
    {
        public TaskDashboard(ObservableCollection<Task> tasks)
        {
            InitializeComponent();
            PopulateStackPanel(tasks);
        }
        public TaskDashboard()
        {
            InitializeComponent();
        }

        private void PopulateStackPanel(ObservableCollection<Task> tasks)
        {
            foreach (var task in tasks)
            {
                var taskBlock = new TextBlock
                {
                    Text = $"{task.TaskName} - {task.TaskDescription} - {task.PriorityLevel} - {task.DueDate}",
                    FontSize = 16,
                    Margin = new System.Windows.Thickness(5)
                };
                TaskStackPanel.Children.Add(taskBlock);
            }
        }

        private void DropdownButton_Click(object sender, RoutedEventArgs e)
        {
            DropdownPopup.IsOpen = !DropdownPopup.IsOpen;
        }
    }
}
