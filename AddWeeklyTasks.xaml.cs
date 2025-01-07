using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
namespace Task_Manager
{
    /// <summary>
    /// Interaction logic for AddWeeklyTasks.xaml
    /// </summary>
    public partial class AddWeeklyTasks : Window
    {
        private string adminUsername;
        private DataTable userDataTable;

        public AddWeeklyTasks(string username)
        {
            InitializeComponent();
            adminUsername = username;
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

            // Display the admin username in the TextBlock
            Admin_name.Text = adminUsername;
            LoadUsersWithTasks();
        }

        private void LoadUsersWithTasks()
        {
            string connectionString = "server=localhost;database=accountmanagement;user=root;password=1234;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Get all users
                    string userQuery = "SELECT username FROM users";
                    using (MySqlCommand userCmd = new MySqlCommand(userQuery, connection))
                    {
                        using (MySqlDataReader userReader = userCmd.ExecuteReader())
                        {
                            userDataTable = new DataTable(); // Initialize the DataTable
                            userDataTable.Columns.Add("username", typeof(string));
                            userDataTable.Columns.Add("task_count", typeof(int));

                            while (userReader.Read())
                            {
                                string username = userReader.GetString("username");
                                int taskCount = 0;

                                // Open a new connection to count tasks for the user
                                using (MySqlConnection taskConnection = new MySqlConnection(connectionString))
                                {
                                    try
                                    {
                                        taskConnection.Open();
                                        string taskTable = $"{username}_tasks";
                                        string taskCountQuery = $"SELECT COUNT(*) FROM `{taskTable}`";

                                        using (MySqlCommand taskCmd = new MySqlCommand(taskCountQuery, taskConnection))
                                        {
                                            taskCount = Convert.ToInt32(taskCmd.ExecuteScalar());
                                        }
                                    }
                                    catch (MySqlException ex)
                                    {
                                        // Table doesn't exist or an error occurred
                                        Console.WriteLine($"Error counting tasks for {username}: {ex.Message}");
                                        taskCount = 0;
                                    }
                                }

                                // Add user data to DataTable
                                DataRow row = userDataTable.NewRow();
                                row["username"] = username;
                                row["task_count"] = taskCount;
                                userDataTable.Rows.Add(row);
                            }

                            // Bind data to DataGrid
                            TaskListTable.ItemsSource = userDataTable.DefaultView;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                // If the search box is empty, show all users
                TaskListTable.ItemsSource = userDataTable.DefaultView;
            }
            else
            {
                // Filter the DataTable based on the search text
                var filteredRows = userDataTable.AsEnumerable()
                    .Where(row => row.Field<string>("username").ToLower().Contains(searchText));

                // Create a new DataView for the filtered results
                DataView filteredView = filteredRows.CopyToDataTable().DefaultView;
                TaskListTable.ItemsSource = filteredView;
            }
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                // If the search box is empty, show all users
                TaskListTable.ItemsSource = userDataTable.DefaultView;
            }
            else
            {
                // Filter the DataTable based on the search text
                var filteredRows = userDataTable.AsEnumerable()
                    .Where(row => row.Field<string>("username").ToLower().Contains(searchText));

                // Create a new DataView for the filtered results
                DataView filteredView = filteredRows.CopyToDataTable().DefaultView;
                TaskListTable.ItemsSource = filteredView;
            }
        }

        private void ViewTasks_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListTable.SelectedItem is DataRowView selectedRow)
            {
                string username = selectedRow["username"].ToString();

                // Display the selected username in the TextBlock
                user_display.Text = username;

                // Show the task summary section
                TaskListSummaryGrid.Visibility = Visibility.Visible;

                // Load tasks for the selected user
                LoadUserTasks(username);
            }
        }

        private void LoadUserTasks(string username)
        {
            string connectionString = "server=localhost;database=accountmanagement;user=root;password=1234;";
            string taskTable = $"{username}_tasks";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = $"SELECT task_name AS TaskName, description AS Category, priority AS Priority, due_date AS Deadline, status AS Status FROM `{taskTable}`";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            // Add a new column for font color based on priority
                            dataTable.Columns.Add("PriorityColor", typeof(string));

                            foreach (DataRow row in dataTable.Rows)
                            {
                                string priority = row["Priority"].ToString();

                                switch (priority)
                                {
                                    case "Low":
                                        row["PriorityColor"] = "Green";
                                        break;
                                    case "Medium":
                                        row["PriorityColor"] = "Orange";
                                        break;
                                    case "High":
                                        row["PriorityColor"] = "Red";
                                        break;
                                    default:
                                        row["PriorityColor"] = "Black"; // Default color
                                        break;
                                }
                            }

                            TaskListTableTaskSummary.ItemsSource = dataTable.DefaultView;
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"An error occurred while loading tasks: {ex.Message}");
                }
            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            TaskListSummaryGrid.Visibility = Visibility.Collapsed;
            TaskListView.Visibility = Visibility.Visible;
            LoadUsersWithTasks();
        }
        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            string username = user_display.Text; // Assuming user_display contains the selected username

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Please select a user.");
                return;
            }

            // Display confirmation dialog
            MessageBoxResult result = MessageBox.Show(
                $"Are you sure you want to delete the user '{username}' and all their tasks?",
                "Confirm Deletion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            // If the user chooses "No", cancel the operation
            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            string connectionString = "server=localhost;database=accountmanagement;user=root;password=1234;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Start a transaction to ensure both the user and their tasks are deleted atomically
                    using (MySqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Delete user data from the users table
                            string userDeleteQuery = "DELETE FROM users WHERE username = @username";
                            using (MySqlCommand cmd = new MySqlCommand(userDeleteQuery, connection))
                            {
                                cmd.Parameters.AddWithValue("@username", username);
                                cmd.Transaction = transaction;
                                cmd.ExecuteNonQuery();
                            }

                            // Drop the user's task table
                            string taskDeleteQuery = $"DROP TABLE IF EXISTS `{username}_tasks`";
                            using (MySqlCommand taskCmd = new MySqlCommand(taskDeleteQuery, connection))
                            {
                                taskCmd.Transaction = transaction;
                                taskCmd.ExecuteNonQuery();
                            }

                            // Commit the transaction
                            transaction.Commit();

                            MessageBox.Show($"User '{username}' and their tasks have been deleted successfully.", "Deletion Successful", MessageBoxButton.OK, MessageBoxImage.Information);

                            // Redirect to TaskListView
                            TaskListSummaryGrid.Visibility = Visibility.Collapsed;
                            TaskListView.Visibility = Visibility.Visible;

                            // Reload the user list
                            LoadUsersWithTasks();
                        }
                        catch (Exception ex)
                        {
                            // Rollback the transaction if an error occurs
                            transaction.Rollback();
                            MessageBox.Show($"An error occurred: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while connecting to the database: {ex.Message}");
                }
            }

        }



    }
}
