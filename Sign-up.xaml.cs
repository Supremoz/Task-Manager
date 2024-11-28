using System;
using System.Collections.Generic;
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
   
    public partial class Sign_up : Window
    {
        private string _actualPassword = string.Empty;
        public Sign_up()
        {
            InitializeComponent();
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
                    if (textBox.Name == "UsernameTextBox")
                    {
                        textBox.Text = "Username";
                    }
                    else if (textBox.Name == "EmailTextBox")
                    {
                        textBox.Text = "Email";
                    }

                    textBox.Foreground = new SolidColorBrush(Colors.Gray); // Placeholder color
                }
            }
        }

            private void PasswordTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true; 

            PasswordTextBox.Text += "*"; // Display asterisk
            PasswordTextBox.CaretIndex = PasswordTextBox.Text.Length; // Move caret to the end
            _actualPassword += e.Text; // Store actual input
        }

        private void PasswordTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back && _actualPassword.Length > 0)
            {
                e.Handled = true; 

                
                PasswordTextBox.Text = PasswordTextBox.Text.Remove(PasswordTextBox.Text.Length - 1);
                _actualPassword = _actualPassword.Remove(_actualPassword.Length - 1);

                PasswordTextBox.CaretIndex = PasswordTextBox.Text.Length;
            }
        }
        private void PasswordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_actualPassword))
            {
                PasswordTextBox.Text = "Password"; // Restore placeholder
                PasswordTextBox.Foreground = new SolidColorBrush(Colors.Gray); // Placeholder color
            }
        }
    }
}
