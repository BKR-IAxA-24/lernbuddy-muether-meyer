using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muether_Meyer_Nachhilfe
{
    internal class Nachhilfegesuch
    {

        private int gesuchID;
        public int GesuchID
        {
            get { return gesuchID; }
            set { gesuchID = value; }
        }

        private int schuelerID;
        public int SchuelerID
        {
            get { return schuelerID; }
            set { schuelerID = value; }
        }

        private int fachID;
        public int FachID
        {
            get { return fachID; }
            set { fachID = value; }
        }

        private string beschreibung;

        public string Beschreibung
        {
            get { return beschreibung; }
            set { beschreibung = value; }
        }

        private long timestamp;

        public long Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }


        private Status status;

        public Status GesuchStatus
        {
            get { return status; }
            set { status = value; }
        }

        public enum Status
        {
            offen,
            erledigt
        }



    }
}
