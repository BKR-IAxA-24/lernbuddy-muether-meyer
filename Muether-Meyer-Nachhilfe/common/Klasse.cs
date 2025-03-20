using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muether_Meyer_Nachhilfe.common
{
    internal class Klasse
    {

        private int klassenID;
        private string bezeichnung;
        private int bildungsgangID;

        public int KlassenID
        {
            get { return klassenID; }
            set { klassenID = value; }
        }

        public string Bezeichnung
        {
            get { return bezeichnung; }
            set { bezeichnung = value; }
        }

        public int BildungsgangID
        {
            get { return bildungsgangID; }
            set { bildungsgangID = value; }
        }

        public Klasse()
        {
        }

        public Klasse(int klassenID, string bezeichnung, int bildungsgangID)
        {
            this.klassenID = klassenID;
            this.bezeichnung = bezeichnung;
            this.bildungsgangID = bildungsgangID;
        }




    }
}
