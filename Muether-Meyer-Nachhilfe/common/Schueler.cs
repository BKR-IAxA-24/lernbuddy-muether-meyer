using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muether_Meyer_Nachhilfe.common
{
    internal class Schueler
    {
        private int schuelerID;
        private string vorname;
        private string nachname;
        private Genders geschlecht;
        private int bildungsgangID;
        private int klassenID;



        //Getter und Setter 
        public int SchuelerID
        {
            get { return schuelerID; }
            set
            {
                schuelerID = value;
            }
        }
        public string Vorname
        {
            get { return vorname; }
            set { vorname = value; }
        }
        public string Nachname
        {
            get { return nachname; }
            set { nachname = value; }
        }
        public Genders Geschlecht
        {
            get { return geschlecht; }
            set { geschlecht = value; }
        }
        public int BildungsgangID
        {
            get { return bildungsgangID; }
            set { bildungsgangID = value; }
        }
        public int KlassenID
        {
            get { return klassenID; }
            set { klassenID = value; }
        }

        //Konstruktor 
        public Schueler(int schuelerID, string vorname, string nachname, Genders geschlecht, int bildungsgangID, int klassenID)
        {
            SchuelerID = schuelerID;
            Vorname = vorname;
            Nachname = nachname;
            Geschlecht = geschlecht;
            BildungsgangID = bildungsgangID;
            KlassenID = klassenID;
        }

        // ENUM für Geschlecht 
        public enum Genders
        {
            m,
            w,
            d
        }
    }
}
