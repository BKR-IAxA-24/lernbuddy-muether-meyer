using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muether_Meyer_Nachhilfe.common
{
    internal class wochentag
    {

        private int wochentagID;
        private string tag;


        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public int WochentagID
        {
            get { return wochentagID; }
            set { wochentagID = value; }
        }


        public wochentag()
        {
        }

        public wochentag(int wochentagID, string tag)
        {
            this.wochentagID = wochentagID;
            this.tag = tag;
        }


    }
}
