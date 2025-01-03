using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using static Task_Manager.TaskDashboard;
using System.IO;


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
            LoadTasksToGrid();
        }

        // Define TaskItem class for DataGrid
        public class TaskItem : INotifyPropertyChanged
        {
            public int TaskId { get; set; }
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
                    OnPropertyChanged(nameof(StatusBool));
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

            private Brush _priorityColor;
            public Brush PriorityColor
            {
                get => _priorityColor;
                set
                {
                    _priorityColor = value;
                    OnPropertyChanged(nameof(PriorityColor));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            // Set the priority color based on the priority level
            public void SetPriorityColor()
            {
                switch (Priority.ToLower())
                {
                    case "high":
                        PriorityColor = Brushes.Red;
                        break;
                    case "in progress":
                        PriorityColor = Brushes.Orange;
                        break;
                    case "low":
                        PriorityColor = Brushes.Green;
                        break;
                    default:
                        PriorityColor = Brushes.Orange; // Default color if no match
                        break;
                }
            }
        }


        private MySqlConnection GetDatabaseConnection()
        {
            string connectionString = $"server=localhost;database=accountmanagement;user=root;password=1234;";
            return new MySqlConnection(connectionString);
        }


        private void CategoryDropdownButton_Click(object sender, RoutedEventArgs e)
        {
            // Toggle the visibility of the popup
            CategoryDropdownPopup.IsOpen = !CategoryDropdownPopup.IsOpen;
        }



        // Load tasks from the database
        private void LoadTasks()
        {
            _isLoading = true;

            string userTaskTable = $"{_username}_tasks";
            string connectionString = $"server=localhost;database=accountmanagement;user=root;password=1234;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = $"SELECT id, task_name, description, priority, category, due_date, status FROM `{userTaskTable}`";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            TaskList.Clear();
                            _allTasks.Clear();

                            while (reader.Read())
                            {
                                DateTime dueDate = Convert.ToDateTime(reader["due_date"]);
                                string status = reader["status"].ToString();

                                var task = new TaskItem
                                {
                                    TaskId = Convert.ToInt32(reader["id"]),
                                    TaskName = reader["task_name"].ToString(),
                                    Description = reader["description"].ToString(),
                                    Priority = reader["priority"].ToString(),
                                    Category = reader["category"].ToString(),
                                    Deadline = dueDate.ToString("yyyy-MM-dd"),
                                    Status = status
                                };

                                // Additional dynamic status mapping (if needed)
                                if (status == "In Progress" && dueDate < DateTime.Now)
                                {
                                    task.Status = "Overdue";
                                }

                                _allTasks.Add(task);
                            }
                        }
                    }

                    // Count tasks by status after loading
                    int completedCount = _allTasks.Count(t => t.Status == "Completed");
                    int inProgressCount = _allTasks.Count(t => t.Status == "In Progress");
                    int notStartedCount = _allTasks.Count(t => t.Status == "Pending");
                    int overdueCount = _allTasks.Count(t => t.Status == "Overdue");

                    // Update TextBlocks on UI thread
                    Dispatcher.Invoke(() =>
                    {
                        text_completed.Text = completedCount.ToString();
                        text_inprogress.Text = inProgressCount.ToString();
                        text_notstarted.Text = notStartedCount.ToString();
                        text_overdue.Text = overdueCount.ToString();
                    });

                    ReorderTaskList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    _isLoading = false;
                }
            }
        }
        private void LoadTasksToGrid()
        {
            string userTaskTable = $"{_username}_tasks";
            string connectionString = $"server=localhost;database=accountmanagement;user=root;password=1234;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = $"SELECT id, task_name, description, priority, category, due_date, status FROM `{userTaskTable}`";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            _allTasks.Clear(); // Clear the existing tasks

                            while (reader.Read())
                            {
                                DateTime dueDate = Convert.ToDateTime(reader["due_date"]);
                                string status = reader["status"].ToString();

                                var task = new TaskItem
                                {
                                    TaskId = Convert.ToInt32(reader["id"]),
                                    TaskName = reader["task_name"].ToString(),
                                    Description = reader["description"].ToString(),
                                    Priority = reader["priority"].ToString(),
                                    Category = reader["category"].ToString(),
                                    Deadline = dueDate.ToString("yyyy-MM-dd"),
                                    Status = status
                                };

                                task.SetPriorityColor(); // Set the priority color based on the priority

                                // Add the task to the list
                                _allTasks.Add(task);
                            }

                            // Update the DataGrid (TaskListTableTaskSummary) directly with the fetched tasks
                            TaskListTableTaskSummary.ItemsSource = _allTasks; // Initially display all tasks
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }



        private int CalculateOverdueTasks()
        {
            return _allTasks.Count(t =>
                t.Status == "In Progress" && DateTime.Parse(t.Deadline) < DateTime.Now);
        }


        private void TaskStatus_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is TaskItem task)
            {
                if (checkBox.IsKeyboardFocusWithin || checkBox.IsMouseOver)
                {
                    var result = MessageBox.Show(
                        $"Would you like to mark task '{task.TaskName}' as:\n\n" +
                        "Yes → Completed\nNo → In Progress",
                        "Confirm Task Status",
                        MessageBoxButton.YesNoCancel,
                        MessageBoxImage.Question);

                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            UpdateTaskStatusInDatabase(task.TaskId, "Completed");
                            task.Status = "Completed";
                            checkBox.IsChecked = true;
                            MessageBox.Show("Task marked as Completed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            break;

                        case MessageBoxResult.No:
                            UpdateTaskStatusInDatabase(task.TaskId, "In Progress");
                            task.Status = "In Progress";
                            checkBox.IsChecked = false;
                            MessageBox.Show("Task marked as In Progress!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            break;

                        case MessageBoxResult.Cancel:
                            checkBox.IsChecked = !checkBox.IsChecked; // Revert to the previous state
                            break;
                    }

                    ReorderTaskList(); // Refresh the task list after status change
                }
            }
        }


        private void TaskStatus_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is TaskItem task)
            {
                if (checkBox.IsKeyboardFocusWithin || checkBox.IsMouseOver)
                {
                    // Automatically set task status to "In Progress" both in the database and locally
                    UpdateTaskStatusInDatabase(task.TaskId, "In Progress");
                    task.Status = "In Progress";
                    checkBox.IsChecked = false;

                    MessageBox.Show("Task marked as In Progress!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    ReorderTaskList(); // Refresh the task list after status change

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

            // Validation for past dates
            if (dueDate.Value.Date < DateTime.Today)
            {
                MessageBox.Show("Due Date cannot be in the past. Please select a valid future date.",
                                "Invalid Due Date", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // SQL Query to add task
            string userTaskTable = $"{_username}_tasks";
            string query = $@"INSERT INTO `{userTaskTable}` (task_name, due_date, description, category, priority) 
                      VALUES (@taskName, @dueDate, @description, @category, @priority)";

            ExecuteNonQuery(query, cmd =>
            {
                cmd.Parameters.AddWithValue("@taskName", taskName);
                cmd.Parameters.AddWithValue("@dueDate", dueDate.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@category", category);
                cmd.Parameters.AddWithValue("@priority", priority);
            }, "Task added successfully!", "Failed to add task.");


            // Create a new TaskItem
            var newTask = new TaskItem
            {
                TaskName = taskName,
                Description = description,
                Priority = priority,
                Category = category, // Ensure the category is set
                Deadline = dueDate.Value.ToString("yyyy-MM-dd"),
                Status = "Pending"
            };

            // Add task to the beginning of the local task list
            TaskList.Insert(0, newTask); // Insert at the top of the list

            // Update the UI to reflect the new TaskList
            TaskListTableTaskSummary.ItemsSource = null; // Reset the binding
            TaskListTableTaskSummary.ItemsSource = TaskList; // Rebind the updated TaskList

            ClearTaskFields(); // Clear the input fields after adding the task


        }


        // Generalized method for executing database commands
        private void ExecuteNonQuery(string query, Action<MySqlCommand> parameterize, string successMessage, string errorMessage)
        {
            using (MySqlConnection connection = GetDatabaseConnection())
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        parameterize(cmd);
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show(successMessage, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                FilterTasks("CATEGORY", selectedCategory);

                // Close the popup after selection
                CategoryDropdownPopup.IsOpen = false;
            }
        }

        private void FilterTasks(string filterType, string filterValue)
        {
            TaskList.Clear(); // Clear the current task list

            var filteredTasks = _allTasks.AsQueryable();

            switch (filterType)
            {
                case "CATEGORY":
                    switch (filterValue)
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
                    break;

                case "PRIORITY":
                    filteredTasks = filteredTasks.Where(t => t.Priority.Equals(filterValue, StringComparison.OrdinalIgnoreCase));
                    break;

                case "DEADLINE":
                    filteredTasks = filteredTasks.Where(t => t.Deadline == filterValue);
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

        private void SortTasks(string filterType, string sortOrder)
        {
            var taskList = _allTasks.AsEnumerable(); // Use _allTasks for sorting

            // First, separate tasks into Active (Pending, In Progress) and Completed
            var activeTasks = taskList.Where(task => task.Status != "Completed").AsEnumerable();
            var completedTasks = taskList.Where(task => task.Status == "Completed").AsEnumerable();

            // Define priorityOrder outside the switch block
            var priorityOrder = new Dictionary<string, int>
    {
        { "Low", 1 },
        { "Medium", 2 },
        { "High", 3 }
    };

            // Sort Active Tasks
            switch (filterType)
            {
                case "Priority":
                    activeTasks = sortOrder == "Ascending"
                        ? activeTasks.OrderBy(task => priorityOrder.ContainsKey(task.Priority) ? priorityOrder[task.Priority] : 99)
                        : activeTasks.OrderByDescending(task => priorityOrder.ContainsKey(task.Priority) ? priorityOrder[task.Priority] : 0);
                    break;

                case "Deadline":
                    activeTasks = sortOrder == "Ascending"
                        ? activeTasks.OrderBy(task => DateTime.Parse(task.Deadline))
                        : activeTasks.OrderByDescending(task => DateTime.Parse(task.Deadline));
                    break;

                default:
                    MessageBox.Show($"Sorting by {filterType} is not supported.", "Sort Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
            }

            // Sort Completed Tasks
            switch (filterType)
            {
                case "Priority":
                    completedTasks = sortOrder == "Ascending"
                        ? completedTasks.OrderBy(task => priorityOrder.ContainsKey(task.Priority) ? priorityOrder[task.Priority] : 99)
                        : completedTasks.OrderByDescending(task => priorityOrder.ContainsKey(task.Priority) ? priorityOrder[task.Priority] : 0);
                    break;

                case "Deadline":
                    completedTasks = sortOrder == "Ascending"
                        ? completedTasks.OrderBy(task => DateTime.Parse(task.Deadline))
                        : completedTasks.OrderByDescending(task => DateTime.Parse(task.Deadline));
                    break;
            }

            // Combine Active Tasks and Completed Tasks
            var finalSortedTasks = activeTasks.Concat(completedTasks).ToList();

            // Clear the current task list and add sorted tasks
            TaskList.Clear();
            foreach (var task in finalSortedTasks)
            {
                TaskList.Add(task);
            }

            // Update the UI
            TaskListTableTaskSummary.ItemsSource = TaskList;
        }

        private void FilterDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FilterDropdown.SelectedItem is ListBoxItem selectedItem)
            {
                string filterType = selectedItem.Content.ToString();

                switch (filterType)
                {
                    case "PRIORITY":
                        FilterTasks("PRIORITY", "High");
                        SortTasks("Priority", "Descending");
                        break;

                    case "DEADLINE":
                        FilterTasks("DEADLINE", "2025-01-01"); // Example: Filter based on specific deadline
                        SortTasks("Deadline", "Ascending");
                        break;

                    default:
                        MessageBox.Show("Unknown filter type selected.", "Filter Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            }
        }

        private void ClearTaskFields()
        {
            TaskNameTxt.Text = string.Empty;
            CustomDatePicker.SelectedDate = null;
            DescriptionTxt.Text = string.Empty;
            CategoryComboBox.SelectedIndex = 0; 
            PriorityComboBox.SelectedIndex = 0; 
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

        private void CancelEditButton_Click(object sender, RoutedEventArgs e)
        {
            EditTaskView.Visibility = Visibility.Collapsed;
            TaskListView.Visibility = Visibility.Visible;
        }
        private void TaskSummaryButton_Click(object sender, RoutedEventArgs e)
        {
            EditTaskView.Visibility = Visibility.Collapsed;
            TaskListView.Visibility = Visibility.Collapsed;
            TaskSummary.Visibility = Visibility.Visible;
            LoadTasksToGrid();
        }
        private void DashButton_Click(object sender, RoutedEventArgs e)
        {
            
            TaskListView.Visibility = Visibility.Visible;
            EditTaskView.Visibility = Visibility.Collapsed;
            TaskSummary.Visibility = Visibility.Collapsed;
            CreateTaskView.Visibility = Visibility.Collapsed;
            LoadTasks();
        }

        private void Edit_task_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListTable.SelectedItem is TaskItem selectedTask)
            {
                TaskName.Text = selectedTask.TaskName; 
                EditDescriptionTxt.Text = selectedTask.Description;

                var categoryItem = EditCategoryComboBox.Items
                    .OfType<ComboBoxItem>()
                    .FirstOrDefault(item => item.Content.ToString() == selectedTask.Category);
                EditCategoryComboBox.SelectedItem = categoryItem;

                var priorityItem = EditPriorityComboBox.Items
                    .OfType<ComboBoxItem>()
                    .FirstOrDefault(item => item.Content.ToString() == selectedTask.Priority);
                EditPriorityComboBox.SelectedItem = priorityItem;

                if (DateTime.TryParse(selectedTask.Deadline, out DateTime dueDate))
                {
                    EditCustomDatePicker.SelectedDate = dueDate;
                }

                EditTaskView.Visibility = Visibility.Visible;
                TaskListView.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("No task selected for editing.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListTable.SelectedItem is TaskItem selectedTask)
            {
                // Update the TaskItem object with new values
                selectedTask.TaskName = TaskName.Text.Trim();
                selectedTask.Description = EditDescriptionTxt.Text.Trim();
                selectedTask.Category = (EditCategoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                selectedTask.Priority = (EditPriorityComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                selectedTask.Deadline = EditCustomDatePicker.SelectedDate?.ToString("yyyy-MM-dd");

                // Call the new update method to update the task in the database
                UpdateTaskInDatabaseNew(selectedTask);

                // Refresh the DataGrid (reorder list or reset the task list)
                ReorderTaskList();

                // Hide the edit view and show the task list view
                EditTaskView.Visibility = Visibility.Collapsed;
                TaskListView.Visibility = Visibility.Visible;

                MessageBox.Show("Task updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListTable.SelectedItem is TaskItem selectedTask)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete the task '{selectedTask.TaskName}'?",
                    "Confirm Deletion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    DeleteTaskFromDatabase(selectedTask.TaskId);
                    _allTasks.Remove(selectedTask);
                    ReorderTaskList();

                    EditTaskView.Visibility = Visibility.Collapsed;
                    TaskListView.Visibility = Visibility.Visible;

                    MessageBox.Show("Task deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        private void UpdateTaskStatusInDatabase(int taskId, string status)
        {
            string userTaskTable = $"{_username}_tasks";
            string connectionString = $"server=localhost;database=accountmanagement;user=root;password=1234;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = $"UPDATE `{userTaskTable}` SET status = @status WHERE id = @taskId";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@status", status);
                        cmd.Parameters.AddWithValue("@taskId", taskId);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Database Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void UpdateTaskInDatabaseNew(TaskItem updatedTask)
        {
            string userTaskTable = $"{_username}_tasks";
            string connectionString = $"server=localhost;database=accountmanagement;user=root;password=1234;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = $@"UPDATE `{userTaskTable}` 
                              SET task_name = @taskName, description = @description, category = @category, 
                                  priority = @priority, due_date = @dueDate
                              WHERE id = @taskId";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@taskName", updatedTask.TaskName);
                        cmd.Parameters.AddWithValue("@description", updatedTask.Description);
                        cmd.Parameters.AddWithValue("@category", updatedTask.Category);
                        cmd.Parameters.AddWithValue("@priority", updatedTask.Priority);
                        cmd.Parameters.AddWithValue("@dueDate", updatedTask.Deadline);
                        cmd.Parameters.AddWithValue("@taskId", updatedTask.TaskId);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Database Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }



        private void DeleteTaskFromDatabase(int taskId)
        {
            string userTaskTable = $"{_username}_tasks";
            string connectionString = $"server=localhost;database=accountmanagement;user=root;password=1234;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = $@"DELETE FROM `{userTaskTable}` WHERE id = @taskId";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
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
        private void TaskListTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TaskListTable.SelectedItem is TaskItem selectedTask)
            {
                TaskName.Text = selectedTask.TaskName;
                EditDescriptionTxt.Text = selectedTask.Description;
                EditCategoryComboBox.SelectedItem = EditCategoryComboBox.Items
                    .Cast<ComboBoxItem>()
                    .FirstOrDefault(item => item.Content.ToString() == selectedTask.Category);
                EditPriorityComboBox.SelectedItem = EditPriorityComboBox.Items
                    .Cast<ComboBoxItem>()
                    .FirstOrDefault(item => item.Content.ToString() == selectedTask.Priority);

                if (DateTime.TryParse(selectedTask.Deadline, out DateTime dueDate))
                {
                    EditCustomDatePicker.SelectedDate = dueDate;
                }

                EditTaskView.Visibility = Visibility.Visible;
                TaskListView.Visibility = Visibility.Collapsed;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.Trim().ToLower(); 

            TaskList.Clear();

            var filteredTasks = _allTasks.Where(task =>
                task.TaskName.ToLower().Contains(searchText) ||
                task.Description.ToLower().Contains(searchText)).ToList();

            foreach (var task in filteredTasks)
            {
                TaskList.Add(task);
            }
        }
        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Do you want to download your tasks as a CSV file?",
                "Confirm Export",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                ExportTasksToCsv();
            }
        }

        private void ExportTasksToCsv()
        {
            // Calculate task counts
            var dueTasks = _allTasks.Count(t => DateTime.Parse(t.Deadline) < DateTime.Now && t.Status != "Completed");
            var completedTasks = _allTasks.Count(t => t.Status.Equals("Completed", StringComparison.OrdinalIgnoreCase));
            var inProgressTasks = _allTasks.Count(t => t.Status.Equals("In Progress", StringComparison.OrdinalIgnoreCase));
            var pendingTasks = _allTasks.Count(t => t.Status.Equals("Pending", StringComparison.OrdinalIgnoreCase));

            // Create CSV file content
            StringBuilder csvContent = new StringBuilder();

            // Add header
            csvContent.AppendLine("Task Name,Description,Priority,Category,Deadline,Status");

            // Add tasks
            foreach (var task in _allTasks)
            {
                csvContent.AppendLine($"{task.TaskName},{task.Description},{task.Priority},{task.Category},{task.Deadline},{task.Status}");
            }

            // Add task counts summary
            csvContent.AppendLine();
            csvContent.AppendLine("TASK COUNTS");
            csvContent.AppendLine($"Due Tasks: {dueTasks}");
            csvContent.AppendLine($"Completed Tasks: {completedTasks}");
            csvContent.AppendLine($"In Progress Tasks: {inProgressTasks}");
            csvContent.AppendLine($"Pending Tasks: {pendingTasks}");

            // Prompt user to save the file
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv",
                FileName = "Tasks.csv"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, csvContent.ToString());
                MessageBox.Show("Tasks exported successfully!", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void FilterDropdownButton_Click(object sender, RoutedEventArgs e)
        {
            // Toggle the visibility of the popup
            TaskDropdownPopup.IsOpen = !TaskDropdownPopup.IsOpen;
        }

        private void filterDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (filterDropdown.SelectedItem is ListBoxItem selectedItem)
            {
                string filterType = selectedItem.Content.ToString();

                switch (filterType)
                {
                    case "Deadline":
                        // Call the method to filter tasks by deadline
                        Filtertasks("DEADLINE", DateTime.Today.ToString("yyyy-MM-dd")); // Example: Filter for today's date
                        Sorttasks("Deadline", "Ascending"); // Sort by deadline in ascending order
                        break;

                    case "Priority":
                        // Call the method to filter tasks by priority
                        Filtertasks("PRIORITY", "High"); // Example: Filter for High priority
                        Sorttasks("Priority", "Descending"); // Sort by priority in descending order
                        break;

                    default:
                        MessageBox.Show("Unknown filter type selected.", "Filter Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }

                // Close the popup after selection
                TaskDropdownPopup.IsOpen = false;
            }
        }

        private void Filtertasks(string filterType, string filterValue)
        {
            TaskList.Clear(); // Clear the current task list
            var filteredTasks = _allTasks.AsQueryable();



            switch (filterType)
            {
                case "PRIORITY":
                    filteredTasks = filteredTasks.Where(t => t.Priority.Equals(filterValue, StringComparison.OrdinalIgnoreCase));
                    break;

                case "DEADLINE":
                    // Ensure the format matches the Deadline property
                    filteredTasks = filteredTasks.Where(t => t.Deadline.Equals(filterValue, StringComparison.OrdinalIgnoreCase));
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
        private void Sorttasks(string filterType, string sortOrder)
        {
            // Define priorityOrder globally in the method
            var priorityOrder = new Dictionary<string, int>
    {
        { "High", 3 },
        { "Medium", 2 },
        { "Low", 1 }
    };

            // Step 1: Separate tasks into "Active" (Pending & In Progress) and "Completed"
            var activeTasks = _allTasks.Where(task => task.Status != "Completed").AsEnumerable();
            var completedTasks = _allTasks.Where(task => task.Status == "Completed").AsEnumerable();

            // Step 2: Sort Active Tasks
            switch (filterType)
            {
                case "Priority":
                    activeTasks = sortOrder == "Ascending"
                        ? activeTasks.OrderBy(task => priorityOrder.ContainsKey(task.Priority) ? priorityOrder[task.Priority] : 99)
                        : activeTasks.OrderByDescending(task => priorityOrder.ContainsKey(task.Priority) ? priorityOrder[task.Priority] : 0);
                    break;

                case "Deadline":
                    activeTasks = sortOrder == "Ascending"
                        ? activeTasks.OrderBy(task => DateTime.Parse(task.Deadline))
                        : activeTasks.OrderByDescending(task => DateTime.Parse(task.Deadline));
                    break;

                default:
                    MessageBox.Show($"Sorting by {filterType} is not supported.", "Sort Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
            }

            // Step 3: Sort Completed Tasks Separately
            switch (filterType)
            {
                case "Priority":
                    completedTasks = sortOrder == "Ascending"
                        ? completedTasks.OrderBy(task => priorityOrder.ContainsKey(task.Priority) ? priorityOrder[task.Priority] : 99)
                        : completedTasks.OrderByDescending(task => priorityOrder.ContainsKey(task.Priority) ? priorityOrder[task.Priority] : 0);
                    break;

                case "Deadline":
                    completedTasks = sortOrder == "Ascending"
                        ? completedTasks.OrderBy(task => DateTime.Parse(task.Deadline))
                        : completedTasks.OrderByDescending(task => DateTime.Parse(task.Deadline));
                    break;
            }

            // Step 4: Combine Active Tasks and Completed Tasks
            var finalSortedTasks = activeTasks.Concat(completedTasks).ToList();

            // Step 5: Clear TaskList and Update UI
            TaskList.Clear();
            foreach (var task in finalSortedTasks)
            {
                TaskList.Add(task);
            }

            // Update UI
            TaskListTableTaskSummary.ItemsSource = TaskList;
        }


    }


}
