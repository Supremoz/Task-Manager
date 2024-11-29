using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Security.Cryptography;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace Task_Manager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_signup_Click(object sender, RoutedEventArgs e)
        {
            string username = Signup_username.Text;
            string password = Signup_pass.Password;
            string email = Signup_email.Text;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("All fields must be filled out.");
                return;
            }
            if (!IsValidPassword(password))
            {
                MessageBox.Show("Password must be at least 6 characters long and contain at least one uppercase letter, one number, and one special character.");
                return;
            }

            if (!email.EndsWith(".com"))
            {
                MessageBox.Show("Email is invalid!");
                return;
            }

            string hashedPassword = HashPassword(password);

            string connectionString = "server=localhost;database=accountmanagement;user=root;password=2817;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "INSERT INTO users (username, password, email) VALUES (@username, @password, @Email)";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", hashedPassword); // Save hashed password
                        cmd.Parameters.AddWithValue("@Email", email);

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show($"Account created successfully for {username}.");
                            ClearFields();
                        }
                        else
                        {
                            MessageBox.Show("Account creation failed. Please try again.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private bool IsValidPassword(string password)
        {
            
            return Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{6,}$");
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2")); // Convert byte to hex string
                }
                return builder.ToString();
            }
        }

        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            string username = Login_username.Text;
            string password = Login_pass.Password;
            string connectionString = "server=localhost;database=accountmanagement;user=root;password=2817;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT password FROM users WHERE username = @username";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@username", username);

                        string storedHashedPassword = cmd.ExecuteScalar()?.ToString();

                        if (storedHashedPassword != null && storedHashedPassword == HashPassword(password))
                        {
                            MessageBox.Show("Login successful.");

                            // Open Log-in.xaml and close the current window
                            Log_in loginWindow = new Log_in(); // Assumes Log-in.xaml is a Window named Log_in
                            loginWindow.Show();
                            this.Close(); // Close the current MainWindow
                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void btn_createacc_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
            SignUpView.Visibility = Visibility.Visible;
            LoginView.Visibility = Visibility.Collapsed;
        }

        private void btn_backtologin_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
            SignUpView.Visibility = Visibility.Collapsed;
            LoginView.Visibility = Visibility.Visible;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && (textBox.Text == "Username" || textBox.Text == "Password" || textBox.Text == "Email"))
            {
                textBox.Text = "";
                textBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                if (textBox.Name == "Signup_username")
                {
                    textBox.Text = "Username";
                }
                else if (textBox.Name == "Signup_email")
                {
                    textBox.Text = "Email";
                }
                textBox.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }
        private void Login_pass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Hide placeholder when text is entered
            LoginPlaceholderText.Visibility = string.IsNullOrEmpty(Login_pass.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Login_pass_GotFocus(object sender, RoutedEventArgs e)
        {
            LoginPlaceholderText.Visibility = Visibility.Collapsed;
        }

        private void Login_pass_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Login_pass.Password))
            {
                LoginPlaceholderText.Visibility = Visibility.Visible;
            }
        }

        private void Signup_pass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Hide placeholder when the user types in the PasswordBox
            PlaceholderText.Visibility = string.IsNullOrEmpty(Signup_pass.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Signup_pass_GotFocus(object sender, RoutedEventArgs e)
        {
            PlaceholderText.Visibility = Visibility.Collapsed;
        }

        private void Signup_pass_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Signup_pass.Password))
            {
                PlaceholderText.Visibility = Visibility.Visible;
            }
        }
        private void ClearFields()
        {
            Signup_username.Text = "Username";
            Signup_email.Text = "Email";
            Signup_pass.Password = string.Empty;
            PlaceholderText.Visibility = Visibility.Visible;

            Login_username.Text = "Username";
            Login_pass.Password = string.Empty;
            LoginPlaceholderText.Visibility = Visibility.Visible;

            Signup_username.Foreground = new SolidColorBrush(Colors.Gray);
            Signup_email.Foreground = new SolidColorBrush(Colors.Gray);
        }

    }
}