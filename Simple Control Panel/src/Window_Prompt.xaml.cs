using Shell2;
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
using System.Windows.Shapes;

namespace RunProject
{
    /// <summary>
    /// Interaction logic for Window_Prompt.xaml
    /// </summary>
    public partial class Window_Prompt : Window
    {
        public Window_Prompt()
        {
            InitializeComponent();
            Button_ExitApp.Visibility = Visibility.Hidden;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_ExitAppOnly(object sender, RoutedEventArgs e)
        {

        }

        private void ExitApp(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow objMainWindow)
            {
                objMainWindow.Close();
                this.Close();
            }
        }

        private void ShowExitButton(object sender, RoutedEventArgs e)
        {
            Button_ExitApp.Visibility = Visibility.Visible;
        }
    }
}
