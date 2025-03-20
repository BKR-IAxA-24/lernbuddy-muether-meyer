using Muether_Meyer_Nachhilfe.common;
using Muether_Meyer_Nachhilfe.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Muether_Meyer_Nachhilfe
{
    // Define a local WochentagViewModel class since the original Wochentag class can't be found
    public class WochentagViewModel : INotifyPropertyChanged
    {
        public string Name { get; set; }

        private string _startzeit;
        public string Startzeit
        {
            get => _startzeit;
            set
            {
                if (_startzeit != value)
                {
                    _startzeit = value;
                    OnPropertyChanged(nameof(Startzeit));
                }
            }
        }

        private string _endzeit;
        public string Endzeit
        {
            get => _endzeit;
            set
            {
                if (_endzeit != value)
                {
                    _endzeit = value;
                    OnPropertyChanged(nameof(Endzeit));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public partial class MainWindow : Window
    {
        public ObservableCollection<WochentagViewModel> WochentageListe { get; set; }
        SQLManager sqlmanager = new SQLManager();

        public MainWindow()
        {
            InitializeComponent();
            Dashboard dashboard = new Dashboard();
            dashboard.Show();

            var wochentage = sqlmanager.GetWochentags();
            WochentageListe = new ObservableCollection<WochentagViewModel>();
            foreach (var wochentag in wochentage)
            {
                WochentageListe.Add(new WochentagViewModel
                {
                    Name = wochentag.Tag,
                    Startzeit = "",
                    Endzeit = ""
                });
            }
            DataContext = this;

            lstFachAuswahl.Items.Clear();
            foreach (var item in sqlmanager.getFaecher())
            {
                lstFachAuswahl.Items.Add(item.Bezeichnung);
            };
            cmbGeschlecht.Items.Clear();
            foreach (var gender in Enum.GetValues(typeof(Schueler.Genders)))
            {
                cmbGeschlecht.Items.Add(gender);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
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
                    // Hier könnte man noch lehrerhatfach oder andere Zuordnungen erstellen
                    // Je nachdem wie die Anwendung aufgebaut ist
                }
            }
        }

        private void lstFachAuswahl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            #region check schüler
            //generate a random number 1-5
            Random rnd = new Random();
            int random = rnd.Next(1, 5);

            var classes = sqlmanager.getKlassen();
            //check the classes for the bezeichnung and give the id back
            int classID = 0;
            var foundClass = sqlmanager.existKlasse(txtClass.Text.ToUpper());
            if (foundClass == true)
            {
                foreach (var klasse in classes)
                {
                    if (klasse.Bezeichnung.Equals(txtClass.Text.ToUpper()))
                    {
                        classID = klasse.KlassenID;
                        break;
                    }
                }
            }
            else
            {
                sqlmanager.createKlasse(txtClass.Text.ToUpper(), random);
                MessageBox.Show("Klasse angelegt");
                classes = sqlmanager.getKlassen();
                // Get the newly created class ID
                foreach (var klasse in classes)
                {
                    if (klasse.Bezeichnung.Equals(txtClass.Text.ToUpper()))
                    {
                        classID = klasse.KlassenID;
                        break;
                    }
                }
            }

            //get selected item from combobox
            if (cmbGeschlecht.SelectedItem == null)
            {
                MessageBox.Show("Bitte wählen Sie ein Geschlecht aus.");
                return;
            }

            string gender = cmbGeschlecht.SelectedItem.ToString();
            if (gender == "m")
            {
                sqlmanager.createSchueler(txtFirstName.Text, txtLastName.Text, (Schueler.Genders)1, txtEMail.Text, classID);
            }
            else if (gender == "w")
            {
                sqlmanager.createSchueler(txtFirstName.Text, txtLastName.Text, (Schueler.Genders)2, txtEMail.Text, classID);
            }
            else if (gender == "d")
            {
                sqlmanager.createSchueler(txtFirstName.Text, txtLastName.Text, (Schueler.Genders)3, txtEMail.Text, classID);
            }
            #endregion

            #region check Gesuch
            // Check if a subject is selected
            if (lstFachAuswahl.SelectedItem == null)
            {
                MessageBox.Show("Bitte wählen Sie ein Fach aus.");
                return;
            }

            var schueler = sqlmanager.getSchueler();
            var sID = (from s in schueler
                       where s.Vorname == txtFirstName.Text
                       where s.Nachname == txtLastName.Text
                       select s.SchuelerID).FirstOrDefault();

            var faecher = sqlmanager.getFaecher();
            var fID = (from f in faecher
                       where f.Bezeichnung == lstFachAuswahl.SelectedItem.ToString()
                       select f.FachID).FirstOrDefault();

            sqlmanager.createNachhilfegesucht(sID, fID, txtDescription.Text);
            #endregion

            #region enter timestamps
            // Get weekdays from database for reference
            var wochentage = sqlmanager.GetWochentags();

            // Loop through the DataGrid items and save time slots
            foreach (WochentagViewModel tag in WochentageListe)
            {
                // Skip if either start or end time is not set
                if (string.IsNullOrWhiteSpace(tag.Startzeit) || string.IsNullOrWhiteSpace(tag.Endzeit))
                    continue;

                // Find the weekday ID
                var wochentagID = 0;
                foreach (var wochentag in wochentage)
                {
                    if (wochentag.Tag == tag.Name)
                    {
                        wochentagID = wochentag.WochentagID;
                        break;
                    }
                }

                if (wochentagID != 0)
                {
                    // Create time slot
                    sqlmanager.createZeitspanne(wochentagID, sID, int.Parse(tag.Startzeit), int.Parse(tag.Endzeit));
                }
            }

            MessageBox.Show("Nachhilfegesuch erfolgreich aufgegeben!");
            #endregion
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
        }
    }
}