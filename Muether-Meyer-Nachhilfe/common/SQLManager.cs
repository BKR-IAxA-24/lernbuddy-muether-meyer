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
using System.Windows;
namespace Muether_Meyer_Nachhilfe.common
{
    internal class SQLManager
    {
        private DBase db;
        private string PEPPER = "dsakldsakjdsakdsakjdsakjdsakjdsalkjdslkjdsalkjdsalkj";

        public SQLManager()
        {
            db = new DBase("localhost", "nachhilfedb", "root", "");
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


        /// <summary>
        /// Fügt einen neuen Benutzer in die Datenbank ein.
        /// </summary>
        /// <param name="pEmail"></param>
        /// <param name="pUserPassword"></param>
        /// <returns></returns>
        public bool loginToDB(string pEmail, string pUserPassword)
        {
            // Das Klartext-Passwort wird mit dem Pepper gehashed
            string pepperHashedPassword = toHash($"{pUserPassword}-*{PEPPER}");

            // SQL-Abfrage für die Login-Prüfung
            string query = $@"
        SELECT (SHA2(CONCAT('{pepperHashedPassword}', u.salt), 256) = u.Passwort) AS result
        FROM login AS u
        where u.EMail = '{pEmail}';
        ";

            try
            {
                // SQL-Abfrage ausführen und Ergebnis abrufen
                DataTable resultTable = db.QueryToDataTable(query);

                //setzt den Tinyint in die Variable result
                string result = resultTable.Rows[0]["result"].ToString();

                //prüft ob es gleich 1 ist
                if (result == "1")
                {
                    return true; // Anmeldung erfolgreich
                }
                else
                    if (result == "2")
                {
                    return false; // Anmeldung fehlgeschlagen
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler bei der Anmeldung: {ex.Message}");
            }

            return false; // Anmeldung fehlgeschlagen
        }

        /// <summary>
        /// Hash-Funktion für Passwörter
        /// <param name="pKlartext">Klartext-Passwort</param>
        /// <returns></returns>
        /// </summary>
       
        public string toHash(string pKlartext)
        {
            // SHA-256-Instanz erstellen
            using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
            {
                // Eingabe in ein Byte-Array umwandeln
                byte[] inputBytes = Encoding.UTF8.GetBytes(pKlartext);

                // Hash berechnen
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // Byte-Array in einen Hexadezimal-String umwandeln
                string hash = BitConverter.ToString(hashBytes);

                // Bindestriche entfernen
                hash = hash.Replace("-", "");

                // Kleinbuchstaben erzeugen
                hash = hash.ToLower();

                // Den fertigen Hash zurückgeben
                return hash;
            }
        }

        /// <summary>
        /// Erstellt einen neuen Benutzer in der Datenbank.
        /// </summary>
        /// <param name="pUserName">Der Benutzername (E-Mail) des neuen Benutzers.</param>
        /// <param name="pUserPassword">Das Passwort des neuen Benutzers im Klartext.</param>
        /// <param name="isAdmin">Gibt an, ob der neue Benutzer Administratorrechte hat.</param>
        public void createUserDB(string pUserName, string pUserPassword, bool isAdmin)
        {
            int admin = 0;
            if (isAdmin)
            {
                admin = 1;
            }

            // Das Klartext-Passwort wird mit dem Pepper gehashed
            string pepperHashedPassword = toHash($"{pUserPassword}-*{PEPPER}");

            // Der Benutzer wird in die Login-Tabelle eingefügt
            string insertRow = $@"
    INSERT INTO login (EMail, Passwort, admin, salt)
    SELECT
        '{pUserName}',
        SHA2(CONCAT('{pepperHashedPassword}', s.salt), 256),
        '{admin}',
        s.salt
    FROM
        (SELECT FLOOR(RAND() * 10000000) AS salt) AS s;
    ";

            db.ExecuteQuery(insertRow);
        }


        public List<Fach> getFaecher()
        {
            List<Fach> faches = new List<Fach>();

            DataTable dataTable = db.TableToDataTable("fach");

            foreach (DataRow row in dataTable.Rows)
            {
                int fachID = Convert.ToInt32(row["fachID"]);
                string bezeichnung = row["bezeichnung"].ToString();
                Fach fach = new Fach(fachID, bezeichnung);
                faches.Add(fach);
            }

            return faches;


        }
        public bool existFach(string name)
        {

            List<Fach> faches = getFaecher();

            foreach (Fach fach in faches)
            {

                if (fach.Equals(name)) return true;

            }

            return false;


        }

        


    }
}