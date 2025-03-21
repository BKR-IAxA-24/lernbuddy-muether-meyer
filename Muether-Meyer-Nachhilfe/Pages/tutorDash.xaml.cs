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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Muether_Meyer_Nachhilfe.Pages
{
    /// <summary>
    /// Interaktionslogik für tutorDash.xaml
    /// </summary>
    public partial class tutorDash : Page
    {
        SQLManager db = new SQLManager();

        public tutorDash()
        {
            InitializeComponent();
            this.Loaded += TutorDash_Loaded;
        }

        private void TutorDash_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<Nachhilfegesuch> nachhilfegesuches = db.getNachhilfegesuches("offen");
                List<Fach> fachs = db.getFaecher();
                List<bildungsgang> bildungsgangs = db.getBildungsgang();
                List<Klasse> klassen = db.getKlassen();
                List<Schueler> schueler = db.getSchueler();

                var combinedData = from ng in nachhilfegesuches
                                   join f in fachs on ng.FachID equals f.FachID
                                   join s in schueler on ng.SchuelerID equals s.SchuelerID
                                   join k in klassen on s.KlassenID equals k.KlassenID
                                   join bg in bildungsgangs on k.BildungsgangID equals bg.BildungsgangID
                                   select new
                                   {
                                       Fach = f.Bezeichnung,
                                       Beschreibung = ng.Beschreibung,
                                       Created_at = ng.CreatedAt,
                                       Bildungsgang = k.Bezeichnung,
                                       schueler = s.Vorname + " " + s.Nachname
                                   };

                dataOutput.ItemsSource = combinedData.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Data Loading Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the button that was clicked
            Button button = sender as Button;

            // Get the data item associated with this row
            var item = button.DataContext;

            if (item != null)
            {
                // Format the display to show each property on its own line
                string details = "";
                var properties = item.GetType().GetProperties();

                foreach (var prop in properties)
                {
                    details += $"{prop.Name}: {prop.GetValue(item)}\n";
                }

                MessageBox.Show("Nachhilfe Anfrage angenommen" );
                
            }
        }
    }
}