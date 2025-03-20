using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muether_Meyer_Nachhilfe.common
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
        public DateTime dateTime
        {
            get => erstellt();
            set { dateTime = value; }
        }
        public string CreatedAt
        {
            get => dateTime.ToString();
            set { CreatedAt = value; }
        }
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


        public Nachhilfegesuch(int gesuchID, int schuelerID, int fachID, string beschreibung, long timestamp, Status status)
        {
            this.gesuchID = gesuchID;
            this.schuelerID = schuelerID;
            this.fachID = fachID;
            this.beschreibung = beschreibung;
            this.timestamp = timestamp;
            this.status = status;
        }
        private DateTime erstellt()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timestamp);
            DateTime dateTime = dateTimeOffset.DateTime;
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            DateTime createdat = TimeZoneInfo.ConvertTime(dateTime, tzi);
            return createdat;
        }
    }
}
