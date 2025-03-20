using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents.DocumentStructures;

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
        
        private string email;


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

        public string Email
        {
            get { return email; }
            set { email = value; }
        }


        //Konstruktor 
        public Schueler(int schuelerID, string vorname, string nachname, Genders geschlecht,  int klassenID, string email)
        {
            SchuelerID = schuelerID;
            Vorname = vorname;
            Nachname = nachname;
            Geschlecht = geschlecht;
            BildungsgangID = bildungsgangID;
            KlassenID = klassenID;
            Email = email;
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
