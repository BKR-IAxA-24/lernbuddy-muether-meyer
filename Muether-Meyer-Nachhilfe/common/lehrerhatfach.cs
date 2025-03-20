using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muether_Meyer_Nachhilfe.common
{
    internal class lehrerhatfach
    {

        private int tutorID;
        public int TutorID
        {
            get { return tutorID; }
            set { tutorID = value; }
        }
        private int fachID;
        public int FachID
        {
            get { return fachID; }
            set { fachID = value; }
        }
        public lehrerhatfach()
        {
        }
        public lehrerhatfach(int tutorID, int fachID)
        {
            this.tutorID = tutorID;
            this.fachID = fachID;
        }


    }
}
