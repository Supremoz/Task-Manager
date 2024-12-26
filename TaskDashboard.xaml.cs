using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using MySql.Data.MySqlClient;

namespace Task_Manager
{
    public partial class TaskDashboard : Window
    {
        public class UserData
        {
            public string Username { get; set; }
        }

        // Store the username in a private field
        private string _username;

        // ObservableCollection to hold tasks dynamically
        public ObservableCollection<TaskItem> TaskList { get; set; } = new ObservableCollection<TaskItem>();

        public TaskDashboard(UserData userData)
        {
            InitializeComponent();
            _username = userData.Username;
            username_Dashboard.Text = _username;

            DashboardImageScale.ScaleX = 1.15;
            DashboardImageScale.ScaleY = 1.15;

            TaskListTable.ItemsSource = TaskList; // Bind ObservableCollection to DataGrid

            LoadTasks(); // Load tasks into the DataGrid on startup
        }

        // Define TaskItem class for DataGrid
        public class TaskItem
        {
            public string TaskName { get; set; }
            public string Description { get; set; }
            public string Priority { get; set; }
            public string Deadline { get; set; }
            public string Status { get; set; } = "Pending";
        }

        // Load tasks from the database
        private void LoadTasks()
        {
            string userTaskTable = $"{_username}_tasks";
            string connectionString = $"server=localhost;database=accountmanagement;user=root;password=2817;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = $"SELECT task_name, description, priority, due_date FROM `{userTaskTable}`";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            TaskList.Clear(); // Clear current task list

                            while (reader.Read())
                            {
                                TaskList.Add(new TaskItem
                                {
                                    TaskName = reader["task_name"].ToString(),
                                    Description = reader["description"].ToString(),
                                    Priority = reader["priority"].ToString(),
                                    Deadline = Convert.ToDateTime(reader["due_date"]).ToString("yyyy-MM-dd"),
                                    Status = "Pending"
                                });
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"Database Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Add Task to Database and Update DataGrid
        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            // Collect data from UI elements
            string taskName = TaskNameTxt.Text.Trim();
            DateTime? dueDate = CustomDatePicker.SelectedDate;
            string description = DescriptionTxt.Text.Trim();
            string category = (CategoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string priority = (PriorityComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Validation for empty fields
            if (string.IsNullOrEmpty(taskName) ||
                dueDate == null ||
                string.IsNullOrEmpty(description) ||
                string.IsNullOrEmpty(category) ||
                category == "Select Category" ||
                string.IsNullOrEmpty(priority) ||
                priority == "TYPE OF PRIORITY")
            {
                MessageBox.Show("All fields (Task Name, Due Date, Description, Category, and Priority) must be filled before saving.",
                                "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Use the username to target the correct task table
            string userTaskTable = $"{_username}_tasks";

            string connectionString = $"server=localhost;database=accountmanagement;user=root;password=2817;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = $@"INSERT INTO `{userTaskTable}` (task_name, due_date, description, category, priority) 
                                      VALUES (@taskName, @dueDate, @description, @category, @priority)";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@taskName", taskName);
                        cmd.Parameters.AddWithValue("@dueDate", dueDate.Value.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@category", category);
                        cmd.Parameters.AddWithValue("@priority", priority);

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Task added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                            // Add the new task to the DataGrid dynamically
                            TaskList.Add(new TaskItem
                            {
                                TaskName = taskName,
                                Description = description,
                                Priority = priority,
                                Deadline = dueDate.Value.ToString("yyyy-MM-dd"),
                                Status = "Pending"
                            });

                            ClearTaskFields();
                        }
                        else
                        {
                            MessageBox.Show("Failed to add task. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"Database Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Helper method to clear task fields after saving
        private void ClearTaskFields()
        {
            TaskNameTxt.Text = string.Empty;
            CustomDatePicker.SelectedDate = null;
            DescriptionTxt.Text = string.Empty;
            CategoryComboBox.SelectedIndex = 0; // Reset to default selection
            PriorityComboBox.SelectedIndex = 0; // Reset to default selection
        }

        private void DropdownButton_Click(object sender, RoutedEventArgs e)
        {
            DropdownPopup.IsOpen = !DropdownPopup.IsOpen;
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DropdownPopup.IsOpen = !DropdownPopup.IsOpen;
        }

        private void PriorityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PriorityComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var selectedColor = selectedItem.Foreground;
                PriorityComboBox.Foreground = selectedColor;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DropShadowEffect shadowEffect = new DropShadowEffect()
            {
                BlurRadius = 10,
                ShadowDepth = 2,
                Color = Color.FromRgb(136, 136, 136)
            };

            DashboardBtn.Effect = shadowEffect;
        }

        private void CreateTaskButton_Click(object sender, RoutedEventArgs e)
        {
            CreateTaskView.Visibility = Visibility.Visible;
            TaskListView.Visibility = Visibility.Collapsed;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            TaskListView.Visibility = Visibility.Visible;
            CreateTaskView.Visibility = Visibility.Collapsed;
        }
    }
}
