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
    /// Interaktionslogik für SchuelerVerwaltung.xaml
    /// </summary>
    public partial class SchuelerVerwaltung : Page
    {
        SQLManager db = new SQLManager();
        public SchuelerVerwaltung()
        {
            InitializeComponent();
            List<Nachhilfegesuch> nachhilfegesuches = db.getNachhilfegesuches("offen");
            List<Fach> fachs = db.getFaecher();
            List<bildungsgang> bildungsgangs = db.getBildungsgang();
            List<Klasse> klassen = db.getKlassen();
            List<Schueler> schueler = db.getSchueler();

            var combinedData = from s in schueler
                               join k in klassen on s.KlassenID equals k.KlassenID
                               join bg in bildungsgangs on k.BildungsgangID equals bg.BildungsgangID
                               select new
                               {
                                   FirstName = s.Vorname,
                                   LastName = s.Nachname,
                                   Class = k.Bezeichnung
                               };

            dataOutput.ItemsSource = combinedData.ToList();
        }
    }
}
