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
    /// Interaktionslogik für NachhilfeVerwaltung.xaml
    /// </summary>
    public partial class NachhilfeVerwaltung : Page
    {
        SQLManager db = new SQLManager();
        public NachhilfeVerwaltung()
        {
            InitializeComponent(); List<Nachhilfegesuch> nachhilfegesuches = db.getNachhilfegesuches("offen");
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
        //btnDelete_Click
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dataOutput.SelectedItem != null)
            {
                var selected = dataOutput.SelectedItem as dynamic;
                Nachhilfegesuch nachhilfegesuch = db.getNachhilfegesuches("offen").Where(ng => ng.Beschreibung == selected.Beschreibung).FirstOrDefault();
                db.deleteNachhilfegesuch(nachhilfegesuch.GesuchID);
                MessageBox.Show("Nachhilfegesuch wurde gelöscht");
            }
        }
    }
}
