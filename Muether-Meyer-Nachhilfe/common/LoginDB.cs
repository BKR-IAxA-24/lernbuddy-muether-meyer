using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents.DocumentStructures;

namespace Muether_Meyer_Nachhilfe.common
{
    internal class LoginDB
    {
        private int loginID;
        private string email;
   
        private int admin;


        public LoginDB(int loginID, string email, int admin)
        {
            this.loginID = loginID;
            this.email = email;
            this.admin = admin;
        }


        public int LoginID
        {
            get { return loginID; }
            set { loginID = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public int Admin
        {
            get { return admin; }
            set { admin = value; }
        }


    }
}
