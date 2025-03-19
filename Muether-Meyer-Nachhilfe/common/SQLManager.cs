using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Muether_Meyer_Nachhilfe.common;
using System.Windows.Controls;
using System.Data;
using System.Windows.Documents;
namespace Muether_Meyer_Nachhilfe.common
{
    internal class SQLManager
    {
        private Dbase db = new Dbase("localhost", "nachhilfedb", "root", "");

        public SQLManager()
        {
        }
        public DataTable getStudents(string table)
        {
            switch (table)
            {
                case "tutor":
                    return db.QueryToDataTable("SELECT * FROM tutor");
                case "student":
                    return db.QueryToDataTable("SELECT * FROM schueler");
                case "nachhilfe":
                    return db.QueryToDataTable("SELECT * FROM nachhilfe");
                default:
                    break;
            }
            return null;
        }
        public DataTable Update()
        {
            return null;
        }
        public bool Delete(string table, int id)
        {
            switch (table)
            {
                case "tutor":
                     db.ExecuteQuery($"delete * FROM tutor where TutorID = '{id}'");
                    return true;
                case "student":
                    db.QueryToDataTable($"SELECT * FROM schueler where SchuelerID = '{id}'");
                    return true;
                case "^nachhilfe":
                     db.QueryToDataTable($"SELECT * FROM nachhilfe where GesuchID = '{id}'");
                    return true;
                default:
                    return false;
            }
        }
    }
}