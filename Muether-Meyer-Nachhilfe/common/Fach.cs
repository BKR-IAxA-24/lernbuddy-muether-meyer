using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muether_Meyer_Nachhilfe.common
{
    internal class Fach
    {

		private int fachID;

		private string bezeichnung;

		public string Bezeichnung
		{
			get { return bezeichnung; }
			set { bezeichnung = value; }
		}



		public int FachID
		{
			get { return fachID; }
			set { fachID = value; }
		}

        public Fach()
        {
        }

        public Fach(int fachID, string bezeichnung)
        {
            this.fachID = fachID;
            this.bezeichnung = bezeichnung;
        }




        }
}
