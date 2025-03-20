using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muether_Meyer_Nachhilfe.common
{
    internal class bildungsgang
    {

        private int bildungsgangID;
        public int BildungsgangID
        {
            get { return bildungsgangID; }
            set { bildungsgangID = value; }
        }

        private string bezeichnung;
        public string Bezeichnung
        {
            get { return bezeichnung; }
            set { bezeichnung = value; }
        }

        public bildungsgang()
        {
        }

        public bildungsgang(int bildungsgangID, string bezeichnung)
        {
            this.bildungsgangID = bildungsgangID;
            this.bezeichnung = bezeichnung;
        }


    }
}
