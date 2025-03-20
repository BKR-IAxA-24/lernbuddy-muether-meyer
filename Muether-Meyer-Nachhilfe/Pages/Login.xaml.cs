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
    /// Interaktionslogik für Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        SQLManager sqlmanager  = new SQLManager();

        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            if (txtEmail.Text == "" || txtPassword.Password == "")
            {
                MessageBox.Show("Bitte füllen Sie alle Felder aus!");
            }
            else
            {
              
                if (sqlmanager.loginToDB(txtEmail.Text, txtPassword.Password))
                {
                   MessageBox.Show("Erfolgreich eingeloggt!");
                
                }
                else
                {
                    MessageBox.Show("Falsche E-Mail oder Passwort!");
                }




            }



        }

        private void btnRegisterHere_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
