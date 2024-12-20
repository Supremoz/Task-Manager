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
    /// Interaction logic for AddWeeklyTasks.xaml
    /// </summary>
    public partial class AddWeeklyTasks : Window
    {
        public static ObservableCollection<Task> Tasks { get; set; } = new ObservableCollection<Task>();
        public AddWeeklyTasks()
        {
            InitializeComponent();
            DataGridTasks.ItemsSource = Tasks;
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            Tasks.Add(new Task
            {
                TaskName = "Sample Task", // Replace with actual input values
                TaskDescription = "Sample Description",
                PriorityLevel = "Medium",
                DueDate = "12/31/2024"
            });
        }

        private void CreateList_Click(object sender, RoutedEventArgs e)
        {
            var taskDashboard = new TaskDashboard(Tasks); // Pass tasks to TaskDashboard
            taskDashboard.Show();
            this.Close();
        }
    }
}
