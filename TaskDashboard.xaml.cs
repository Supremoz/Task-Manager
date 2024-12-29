﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using MySql.Data.MySqlClient;
using static Task_Manager.TaskDashboard;

namespace Task_Manager
{
    public partial class TaskDashboard : Window
    {
        
        public class UserData
        {
            public string Username { get; set; }
        }
        private bool _isLoading = false;
        // Store the username in a private field
        private string _username;
        private List<TaskItem> _allTasks = new List<TaskItem>();
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
        public class TaskItem : INotifyPropertyChanged
        {
            public int TaskId { get; set; } // Unique Task ID from database
            public string TaskName { get; set; }
            public string Description { get; set; }
            public string Priority { get; set; }
            public string Deadline { get; set; }
            public string Category { get; set; }

            private string _status = "In Progress";
            public string Status
            {
                get => _status;
                set
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                    OnPropertyChanged(nameof(StatusBool)); // Notify checkbox binding
                }
            }

            public bool StatusBool
            {
                get => Status == "Completed";
                set
                {
                    Status = value ? "Completed" : "In Progress";
                    OnPropertyChanged(nameof(Status));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }



        private void CategoryDropdownButton_Click(object sender, RoutedEventArgs e)
        {
            // Toggle the visibility of the popup
            CategoryDropdownPopup.IsOpen = !CategoryDropdownPopup.IsOpen;
        }



        // Load tasks from the database
        private void LoadTasks()
        {
            _isLoading = true; // Start loading mode to prevent checkbox events

            string userTaskTable = $"{_username}_tasks";
            string connectionString = $"server=localhost;database=accountmanagement;user=root;password=1234;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = $"SELECT id, task_name, description, priority, category, due_date, status FROM {userTaskTable}";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            TaskList.Clear(); // Clear current task list
                            _allTasks.Clear();

                            while (reader.Read())
                            {
                                var task = new TaskItem
                                {
                                    TaskId = Convert.ToInt32(reader["id"]),
                                    TaskName = reader["task_name"].ToString(),
                                    Description = reader["description"].ToString(),
                                    Priority = reader["priority"].ToString(),
                                    Category = reader["category"].ToString(),
                                    Deadline = Convert.ToDateTime(reader["due_date"]).ToString("yyyy-MM-dd"),
                                    Status = Convert.ToInt32(reader["status"]) == 0 ? "In Progress" : "Completed"
                                };

                                _allTasks.Add(task);
                            }
                        }
                    }

                    // Reorder tasks after loading
                    ReorderTaskList();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"Database Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    _isLoading = false; // End loading mode
                }
            }
        }

        private void TaskStatus_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is TaskItem task)
            {
                if (checkBox.IsKeyboardFocusWithin || checkBox.IsMouseOver)
                {
                    var result = MessageBox.Show(
                        $"Mark task '{task.TaskName}' as Completed?",
                        "Confirm Task Completion",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        UpdateTaskStatusInDatabase(task.TaskId, 1);
                        task.Status = "Completed";
                        checkBox.IsChecked = true;

                        MessageBox.Show("Task marked as Completed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        ReorderTaskList(); // Reorder after marking as completed
                    }
                    else
                    {
                        checkBox.IsChecked = false; // Revert checkbox if canceled
                    }
                }
            }
        }

        private void TaskStatus_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is TaskItem task)
            {
                if (checkBox.IsKeyboardFocusWithin || checkBox.IsMouseOver)
                {
                    var result = MessageBox.Show(
                        $"Mark task '{task.TaskName}' as In Progress?",
                        "Confirm Task Status",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        UpdateTaskStatusInDatabase(task.TaskId, 0);
                        task.Status = "In Progress";
                        checkBox.IsChecked = false;

                        MessageBox.Show("Task is In Progress!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        ReorderTaskList(); // Reorder after marking as in progress
                    }
                    else
                    {
                        checkBox.IsChecked = true; // Revert checkbox if canceled
                    }
                }
            }
        }

        private void ReorderTaskList()
        {
            // Reorder TaskList: "In Progress" tasks first, then "Completed"
            var reorderedTasks = _allTasks
                .OrderBy(task => task.Status == "Completed") // Completed tasks come last
                .ThenBy(task => task.TaskId) // Maintain original order for tasks with the same status
                .ToList();

            TaskList.Clear();

            foreach (var task in reorderedTasks)
            {
                TaskList.Add(task);
            }
        }


        private void UpdateTaskStatusInDatabase(int taskId, int status)
        {
            string userTaskTable = $"{_username}_tasks";
            string connectionString = $"server=localhost;database=accountmanagement;user=root;password=1234;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = $@"UPDATE `{userTaskTable}` SET status = @status WHERE id = @taskId";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@status", status);
                        cmd.Parameters.AddWithValue("@taskId", taskId);

                        cmd.ExecuteNonQuery();
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

            string connectionString = $"server=localhost;database=accountmanagement;user=root;password=1234;";

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
        private void CategoryFilterListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryFilterListBox.SelectedItem is ListBoxItem selectedItem)
            {
                string selectedCategory = selectedItem.Content.ToString();

                // Call the method to filter tasks based on the selected category
                FilterTasks(selectedCategory);

                // Close the popup after selection
                CategoryDropdownPopup.IsOpen = false;
            }
        }
        private void FilterTasks(string category)
        {
            TaskList.Clear(); // Clear the current task list

            // Filter tasks based on the selected category
            var filteredTasks = _allTasks.AsQueryable();

            switch (category)
            {
                case "All Tasks":
                    filteredTasks = _allTasks.AsQueryable(); // Show all tasks
                    break;
                case "Work":
                    filteredTasks = filteredTasks.Where(t => t.Category.Equals("Work", StringComparison.OrdinalIgnoreCase));
                    break;
                case "Personal":
                    filteredTasks = filteredTasks.Where(t => t.Category.Equals("Personal", StringComparison.OrdinalIgnoreCase));
                    break;
                default:
                    break;
            }

            // Add filtered tasks to the TaskList
            foreach (var task in filteredTasks)
            {
                TaskList.Add(task);
            }
        }
        private void FilterTasks(string filterType, string filterValue)
        {
            TaskList.Clear();

            var filteredTasks = _allTasks.AsQueryable();

            switch (filterType)
            {
                case "PRIORITY":
                    filteredTasks = filteredTasks.Where(t => t.Priority.Equals(filterValue, StringComparison.OrdinalIgnoreCase));
                    break;
                case "DEADLINE":
                    filteredTasks = filteredTasks.Where(t => t.Deadline == filterValue);
                    break;
            }

            foreach (var task in filteredTasks)
            {
                TaskList.Add(task);
            }
        }

            private void SortTasks(string filterType, string sortOrder)
            {
                var taskList = _allTasks;
                IEnumerable<TaskItem> sortedTasks = taskList;

                switch (filterType)
                {
                    case "Priority":
                        var priorityOrder = new Dictionary<string, int>
                {
                    { "Low", 1 },
                    { "Medium", 2 },
                    { "High", 3 }
                };

                        sortedTasks = sortOrder == "Ascending" 
                            ? taskList.OrderBy(task => priorityOrder[task.Priority])
                            : taskList.OrderByDescending(task => priorityOrder[task.Priority]); 
                        break;

                    case "Deadline":
                        sortedTasks = sortOrder == "Ascending"
                            ? taskList.OrderBy(task => DateTime.Parse(task.Deadline))
                            : taskList.OrderByDescending(task => DateTime.Parse(task.Deadline));
                        break;

                    default:
                        MessageBox.Show($"Sorting by {filterType} is not supported.", "Sort Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }

                TaskList.Clear();
                foreach (var task in sortedTasks)
                {
                    TaskList.Add(task);
                }
            }

        private void FilterDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FilterDropdown.SelectedItem is ListBoxItem selectedItem)
            {
                string filterType = selectedItem.Content.ToString();

                switch (filterType)
                {

                    case "PRIORITY":
                        SortTasks("Priority", "Descending"); 
                        break;

                    case "DEADLINE":
                        SortTasks("Deadline", "Ascending"); 
                        break;

                    default:
                        MessageBox.Show("Unknown filter type selected.", "Filter Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
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

        private void Edit_task_Click(object sender, RoutedEventArgs e)
        {
            TaskListView.Visibility = Visibility.Collapsed;
            EditTaskView.Visibility = Visibility.Visible;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.Trim().ToLower(); // Get the search text and convert to lowercase

            // Clear the current task list
            TaskList.Clear();

            // Filter tasks based on the search text
            var filteredTasks = _allTasks.Where(task =>
                task.TaskName.ToLower().Contains(searchText) ||
                task.Description.ToLower().Contains(searchText)).ToList();

            // Add filtered tasks to the TaskList
            foreach (var task in filteredTasks)
            {
                TaskList.Add(task);
            }
        }
    }
    

}
