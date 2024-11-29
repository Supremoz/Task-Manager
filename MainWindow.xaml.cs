using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Task_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _actualPassword = string.Empty;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_signup_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Account Created");
        }

        private void btn_backtologin_Click(object sender, RoutedEventArgs e)
        {
            SignUpView.Visibility = Visibility.Collapsed;
            LoginView.Visibility = Visibility.Visible;
        }

        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Login Successfully");
        }

        private void btn_createacc_Click(object sender, RoutedEventArgs e)
        {
            LoginView.Visibility = Visibility.Collapsed;
            SignUpView.Visibility = Visibility.Visible;
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

            if (textBox != null)
            {
                // Restore placeholder if text is empty
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    if (textBox.Name == "Login_username")
                    {
                        textBox.Text = "Username";
                    }
                    else if (textBox.Name == "Login_pass")
                    {
                        textBox.Text = "Password";
                    }

                    textBox.Foreground = new SolidColorBrush(Colors.Gray); // Placeholder color
                }
            }
        }
        private void PasswordTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;

            Login_pass.Text += "*"; // Display asterisk
            Login_pass.CaretIndex = Login_pass.Text.Length; // Move caret to the end
            _actualPassword += e.Text; // Store actual input
        }

        private void PasswordTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back && _actualPassword.Length > 0)
            {
                e.Handled = true;


                Login_pass.Text = Login_pass.Text.Remove(Login_pass.Text.Length - 1);
                _actualPassword = _actualPassword.Remove(_actualPassword.Length - 1);

                Login_pass.CaretIndex = Login_pass.Text.Length;
            }
        }
    }
}