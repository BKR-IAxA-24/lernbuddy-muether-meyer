using Muether_Meyer_Nachhilfe.common;
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

namespace Muether_Meyer_Nachhilfe.Pages
{
    /// <summary>
    /// Interaktionslogik für tutorpanel.xaml
    /// </summary>


    public partial class tutorpanel : Window
    {
        // Add a property to track admin status
        SQLManager db = new SQLManager();
        private bool IsAdmin { get; set; }

        public tutorpanel(bool pIsAdmin)
        {
            InitializeComponent();

            // Check admin status from database
            CheckAdminStatus(pIsAdmin);

            // Initialize the dashboard
            MainFrame.Navigate(new tutorDash());
        }

        private void CheckAdminStatus(bool pIsAdmin)
        {
            IsAdmin = pIsAdmin;
            lblUserRole.Content = IsAdmin ? "Admin" : "Tutor";
            AdminSection.Visibility = IsAdmin ? Visibility.Visible : Visibility.Collapsed;
        }

        private void NavigateToPage(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                switch (button.Content.ToString())
                {
                    case "Schüler verwalten":
                        MainFrame.Navigate(new SchuelerVerwaltung());
                        break;
                    case "Tutoren verwalten":
                        MainFrame.Navigate(new TutorVerwaltung());
                        break;
                    case "Nachhilfegesuch löschen":
                        MainFrame.Navigate(new NachhilfeVerwaltung());
                        break;
                }
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Suche...")
            {
                textBox.Text = string.Empty;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Suche...";
            }
        }
        //btnLogout_Click
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }
    }
}