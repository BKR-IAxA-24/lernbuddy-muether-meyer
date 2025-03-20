using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muether_Meyer_Nachhilfe.common
{
	internal class Tutor
	{
		private int tutorID;

		public int TutorID
		{
			get { return tutorID; }
			set { tutorID = value; }
		}

		private int schuelerID;

		public int SchuelerID
		{
			get { return schuelerID; }
			set { schuelerID = value; }
		}


		private float genehmigt;

		public bool Genehmigt
		{
			get
			{
				if (genehmigt == 1)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			set
			{

				if (value == true)
				{
					genehmigt = 1;
				}
				else
				{
					genehmigt = 0;
				}


			}
		}

		public float getGenehmigtFromBool(bool genehmigt)
		{

            if (genehmigt == true)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }

		private int loginID;

		public int LoginID { get => loginID; set => loginID = value; }


    }
}
