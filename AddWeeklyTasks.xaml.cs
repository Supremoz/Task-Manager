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
    /// Interaction logic for AddWeeklyTasks.xaml
    /// </summary>
    public partial class AddWeeklyTasks : Window
    {
        public AddWeeklyTasks()
        {
            InitializeComponent();
        }

        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DropShadowEffect shadowEffect = new DropShadowEffect()
            {
                BlurRadius = 10,
                ShadowDepth = 2,
                Color = Color.FromRgb(136, 136, 136)
            }; // Adjust the color if needed

            // Apply the effect to the DashboardBtn
            DashboardBtn.Effect = shadowEffect;
        }

    }
}
