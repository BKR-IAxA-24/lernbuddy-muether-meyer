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


            var combinedData = from ng in nachhilfegesuches
                               join f in fachs on ng.FachID equals f.FachID
                               select new
                               {
                                   Fach = f.Bezeichnung,
                                   Beschreibung = ng.Beschreibung,
                                   Created_at = ng.CreatedAt,
                                   Bildungsgang = "Bildungsgang" // Hier können Sie den tatsächlichen Bildungsgang einfügen, falls vorhanden
                               };

            dataOutput.ItemsSource = combinedData.ToList();
        }
    }
}
