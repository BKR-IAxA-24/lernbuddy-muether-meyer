using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muether_Meyer_Nachhilfe.common
{
    internal class Zeitspanne
    {

        private int wochentagID;
        private int schuelerID;
        
        //FORMAT HHMM
        private int startzeit;

        //FORMAT HHMM
        private int endzeit;

        public int WochentagID
        {
            get { return wochentagID; }
            set { wochentagID = value; }
        }

        public int SchuelerID
        {
            get { return schuelerID; }
            set { schuelerID = value; }
        }

        public int Startzeit
        {
            get { return startzeit; }
            set {

                if (value < 0000 || value > 2400)
                {
                    startzeit = 0000;
                }

                startzeit = value; 
            }
        }

        public int Endzeit
        {
            get { return endzeit; }
            set
            {

                if (value < 0000 || value > 2400)
                {
                    endzeit = 0000;
                }

                endzeit = value;
            }
        }


        public Zeitspanne()
        {
        }



        public Zeitspanne(int wochentagID, int schuelerID, int startzeit, int endzeit)
        {
            WochentagID = wochentagID;
            SchuelerID = schuelerID;
            Startzeit = startzeit;
            Endzeit = endzeit;
        }

        public string FormatTime(int time)
        {
            string timeString = time.ToString();
            while (timeString.Length < 4)
            {
                timeString = "0" + timeString;
            }
            return $"{timeString.Substring(0, 2)}:{timeString.Substring(2, 2)}";
        }

    }
}
