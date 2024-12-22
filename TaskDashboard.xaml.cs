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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Task_Manager
{
    /// <summary>
    /// Interaction logic for TaskDashboard.xaml
    /// </summary>
    public partial class TaskDashboard : Window
    {
        public TaskDashboard()
        {
            InitializeComponent();
            DashboardImageScale.ScaleX = 1.15;
            DashboardImageScale.ScaleY = 1.15;
        }

        private void DropdownButton_Click(object sender, RoutedEventArgs e)
        {
            DropdownPopup.IsOpen = !DropdownPopup.IsOpen;
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DropdownPopup.IsOpen = !DropdownPopup.IsOpen;
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PriorityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PriorityComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                // Get the Foreground color of the selected ComboBoxItem
                var selectedColor = selectedItem.Foreground;

                // Set the ComboBox's Foreground color to match the selected item
                PriorityComboBox.Foreground = selectedColor;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DropShadowEffect shadowEffect = new DropShadowEffect()
            {
                BlurRadius = 10,
                ShadowDepth = 2,
                Color = Color.FromRgb(136, 136, 136) // Adjust the color if needed
            };

            // Apply the effect to the DashboardBtn
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
