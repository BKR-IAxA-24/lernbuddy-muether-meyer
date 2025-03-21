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
    /// Interaktionslogik für Registrieren.xaml
    /// </summary>
    public partial class Registrieren : Window
    {

        SQLManager sqlmanager = new SQLManager();


        public Registrieren()
        {
            InitializeComponent();

            lstFachAuswahl.Items.Clear();
            foreach (var item in sqlmanager.getFaecher())
            {
                lstFachAuswahl.Items.Add(item.Bezeichnung);
            }

            cmbGeschlecht.Items.Clear();

            cmbGeschlecht.Items.Clear();
            foreach (var gender in Enum.GetValues(typeof(Schueler.Genders)))
            {
                cmbGeschlecht.Items.Add(gender);
            }




        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            


        }

        private void lstFachAuswahl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtTutorfach_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtKlasse_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void cmbGeschlecht_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtNachname_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtVorname_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnRegister_Click_1(object sender, RoutedEventArgs e)
        {
            // Eingabevalidierung
            if (string.IsNullOrWhiteSpace(txtVorname.Text))
            {
                MessageBox.Show("Bitte geben Sie einen Vornamen ein.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNachname.Text))
            {
                MessageBox.Show("Bitte geben Sie einen Nachnamen ein.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Bitte geben Sie eine gültige E-Mail-Adresse ein.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (cmbGeschlecht.SelectedItem == null)
            {
                MessageBox.Show("Bitte wählen Sie ein Geschlecht aus.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtKlasse.Text))
            {
                MessageBox.Show("Bitte geben Sie eine Klasse ein.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPasswort.Password) || txtPasswort.Password.Length < 6)
            {
                MessageBox.Show("Bitte geben Sie ein Passwort mit mindestens 6 Zeichen ein.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (lstFachAuswahl.SelectedItems.Count == 0)
            {
                MessageBox.Show("Bitte wählen Sie mindestens ein Fach aus.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
              
                if (sqlmanager.existSchueler(txtEmail.Text))
                {
                    MessageBox.Show("Diese E-Mail-Adresse ist bereits registriert.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                
                sqlmanager.createUserDB(txtEmail.Text, txtPasswort.Password, false);

           
                int klassenID = 0;
                var klassen = sqlmanager.getKlassen();
                foreach (var klasse in klassen)
                {
                    if (klasse.Bezeichnung == txtKlasse.Text)
                    {
                        klassenID = klasse.KlassenID;
                        break;
                    }
                }

               
                if (klassenID == 0)
                {
                    sqlmanager.createKlasse(txtKlasse.Text, 1);

                    // Nach dem Erstellen holen wir erneut die Klassen-ID
                    klassen = sqlmanager.getKlassen();
                    foreach (var klasse in klassen)
                    {
                        if (klasse.Bezeichnung == txtKlasse.Text)
                        {
                            klassenID = klasse.KlassenID;
                            break;
                        }
                    }
                }

                
                Schueler.Genders geschlecht = (Schueler.Genders)cmbGeschlecht.SelectedItem;
                if (!sqlmanager.createSchueler(txtVorname.Text, txtNachname.Text, geschlecht, txtEmail.Text, klassenID))
                {
                    MessageBox.Show("Bei der Registrierung ist ein Fehler aufgetreten.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }


                
                Schueler schueler = sqlmanager.getSchueler(txtEmail.Text);

                if (schueler == null)
                {
                    MessageBox.Show("Bei der Registrierung ist ein Fehler aufgetreten.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                LoginDB loginDB = sqlmanager.getLogin(schueler.Email);
                if (loginDB == null)
                {
                    MessageBox.Show("Bei der Registrierung ist ein Fehler aufgetreten.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }


                sqlmanager.createTutor(schueler.SchuelerID , false , loginDB.LoginID );

                foreach (var selectedItem in lstFachAuswahl.SelectedItems)
                {
                    string fachBezeichnung = selectedItem.ToString();

                    // Fach-ID ermitteln
                    int fachID = 0;
                    var faecher = sqlmanager.getFaecher();
                    foreach (var fach in faecher)
                    {
                        if (fach.Bezeichnung == fachBezeichnung)
                        {
                            fachID = fach.FachID;
                            break;
                        }
                    }

                    if (fachID != 0)
                    {
                       
                    }
                }

                MessageBox.Show("Registrierung erfolgreich abgeschlossen!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);

                // Zur Login-Seite weiterleiten
                Login loginWindow = new Login();
                loginWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bei der Registrierung ist ein Fehler aufgetreten: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Hilfsmethode zur Validierung der E-Mail-Adresse
        private bool IsValidEmail(string email)
        {
            return true;
        }
    }
}
