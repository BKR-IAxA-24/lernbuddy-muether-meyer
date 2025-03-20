using Muether_Meyer_Nachhilfe.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Muether_Meyer_Nachhilfe.Pages
{
    /// <summary>
    /// Interaktionslogik für Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        SQLManager db = new SQLManager();
        public Dashboard()
        {
            InitializeComponent();
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
                                   Bildungsgang = k.Bezeichnung
                               };

            dataOutput.ItemsSource = combinedData.ToList();
        }
    }
}
