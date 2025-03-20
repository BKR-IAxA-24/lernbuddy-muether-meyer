using Muether_Meyer_Nachhilfe.common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public partial class MainWindow : Window
    {
        public ObservableCollection<Wochentag> WochentageListe { get; set; }
        SQLManager sqlmanager = new SQLManager();

        public MainWindow()
        {
            InitializeComponent();

            WochentageListe = new ObservableCollection<Wochentag>
            {
                new Wochentag { Name = "Montag" },
                new Wochentag { Name = "Dienstag" },
                new Wochentag { Name = "Mittwoch" },
                new Wochentag { Name = "Donnerstag" },
                new Wochentag { Name = "Freitag" },
                new Wochentag { Name = "Samstag" },
                new Wochentag { Name = "Sonntag" }
            };

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
                    classID = klasse.KlassenID;
                    break;
                }
            }
            else
            {
                sqlmanager.createKlasse(txtClass.Text.ToUpper(), random);
                MessageBox.Show("Klasse angelegt");
                classes = sqlmanager.getKlassen();
                return;

            }
            //get selected item from combobox
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
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

        }
    }



    public class Wochentag : INotifyPropertyChanged
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
}
