﻿using System;
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
using Muether_Meyer_Nachhilfe.Pages;
namespace Muether_Meyer_Nachhilfe.common
{
    internal class SQLManager
    {
        private Dbase db;
        private string PEPPER = "dsakldsakjdsakdsakjdsakjdsakjdsalkjdslkjdsalkjdsalkj";

        public SQLManager()
        {
            db = new Dbase("localhost", "nachhilfedb", "root", "");
        }

        #region Login und Registrierung

        /// <summary>
        /// Fügt einen neuen Benutzer in die Datenbank ein.
        /// </summary>
        /// <param tag="pEmail"></param>
        /// <param tag="pUserPassword"></param>
        /// <returns></returns>
       
           public bool loginToDB(string pEmail, string pUserPassword)
            {
            // Das Klartext-Passwort wird mit dem Pepper gehashed
            string pepperHashedPassword = toHash($"{pUserPassword}-*{PEPPER}");

            // SQL-Abfrage für die Login-Prüfung
            string query = $@"
    SELECT u.admin, (SHA2(CONCAT('{pepperHashedPassword}', u.salt), 256) = u.Passwort) AS result
    FROM login AS u
    WHERE u.EMail = '{pEmail}';
    ";

            try
            {
                // SQL-Abfrage ausführen und Ergebnis abrufen
                DataTable resultTable = db.QueryToDataTable(query);

                if (resultTable.Rows.Count == 0)
                {
                    return false; // Benutzer existiert nicht
                }

                // Admin-Status und Passwort-Überprüfungsergebnis abrufen
                bool isAdmin = Convert.ToBoolean(resultTable.Rows[0]["admin"]);
                bool passwordCorrect = Convert.ToBoolean(resultTable.Rows[0]["result"]);

                if (passwordCorrect)
                {
                    if (isAdmin)
                    {
                        return true; // Admin-Anmeldung erfolgreich
                    }
                    else
                    {
                        // Überprüfen, ob der Benutzer als Tutor genehmigt wurde
                        string tutorQuery = $@"
                SELECT t.Genehmigt
                FROM tutor AS t
                JOIN login AS l ON t.LoginID = l.LoginID
                WHERE l.EMail = '{pEmail}';
                ";

                        DataTable tutorResultTable = db.QueryToDataTable(tutorQuery);

                        if (tutorResultTable.Rows.Count == 0)
                        {
                            return false; // Benutzer ist kein Tutor
                        }

                        bool isApprovedTutor = Convert.ToBoolean(tutorResultTable.Rows[0]["Genehmigt"]);
                       
                        if (isApprovedTutor)
                        {
                            
                            return true; // Genehmigter Tutor-Anmeldung erfolgreich
                        }
                        else
                        {
                            
                            return false; // Tutor nicht genehmigt
                        }
                    }
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
        /// <param tag="pKlartext">Klartext-Passwort</param>
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
        /// <param tag="pUserName">Der Benutzername (E-Mail) des neuen Benutzers.</param>
        /// <param tag="pUserPassword">Das Passwort des neuen Benutzers im Klartext.</param>
        /// <param tag="isAdmin">Gibt an, ob der neue Benutzer Administratorrechte hat.</param>
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
            // KORRIGIERT: admin ohne Anführungszeichen, da es ein numerischer Wert ist
            string insertRow = $@"
INSERT INTO login (EMail, Passwort, admin, salt)
SELECT
    '{pUserName}',
    SHA2(CONCAT('{pepperHashedPassword}', s.salt), 256),
    {admin},
    s.salt
FROM
    (SELECT FLOOR(RAND() * 10000000) AS salt) AS s;
";

            db.ExecuteQuery(insertRow);
        }

        public List<LoginDB> getLogins()
        {
            List<LoginDB> logins = new List<LoginDB>();

            DataTable dataTable = db.TableToDataTable("login");
            if (dataTable == null)
            {
                return null;
            }

            if (dataTable.Rows.Count == 0)
            {
                return null;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                int loginID = Convert.ToInt32(row["LoginID"]);
                string email = row["EMail"].ToString();
                int admin = Convert.ToInt32(row["admin"]);
                LoginDB login = new LoginDB(loginID, email, admin);
                logins.Add(login);
            }
            return logins;

        }

        public LoginDB getLogin(string email)
        {
            List<LoginDB> logins = getLogins();
            foreach (LoginDB login in logins)
            {
                if (login.Email == email) return login;
            }
            return null;
        }

        public bool existLogin(string email)
        {
            List<LoginDB> logins = getLogins();
            foreach (LoginDB login in logins)
            {
                if (login.Email == email) return true;
            }
            return false;
        }


        #endregion

        #region Fach

        /// <summary>
        /// Ruft eine Liste aller Fächer aus der Datenbank ab.
        /// </summary>
        /// <returns>Eine Liste von Fach-Objekten oder null, wenn keine Daten gefunden wurden.</returns>
        public List<Fach> getFaecher()
        {
            List<Fach> faches = new List<Fach>();
            DataTable dataTable = db.TableToDataTable("fach");

            if (dataTable == null)
            {
                return null;
            }

            if (dataTable.Rows.Count == 0)
            {
                return null;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                int fachID = Convert.ToInt32(row["fachID"]);
                string bezeichnung = row["Bezeichnung"].ToString();
                Fach fach = new Fach(fachID, bezeichnung);
                faches.Add(fach);
            }

            return faches;


        }
        /// <summary>
        /// Überprüft, ob ein Fach mit dem angegebenen Namen existiert.
        /// </summary>
        /// <param tag="name">Der Name des Faches.</param>
        /// <returns>True, wenn das Fach existiert, andernfalls false.</returns>
        public bool existFach(string bezeichnung)
        {

            List<Fach> faches = getFaecher();

            foreach (Fach fach in faches)
            {

                if (fach.Bezeichnung == bezeichnung) return true;

            }

            return false;


        }

        /// <summary>
        /// Überprüft, ob ein Fach mit der angegebenen Fach-ID existiert.
        /// </summary>
        /// <param tag="fachID">Die ID des Faches.</param>
        /// <returns>True, wenn das Fach existiert, andernfalls false.</returns>
        public bool existFach(int fachID)
        {
            // Eine Liste aller Fächer abrufen
            List<Fach> faches = getFaecher();

            // Überprüfen, ob ein Fach mit der angegebenen Fach-ID existiert
            foreach (Fach fach in faches)
            {
                if (fach.FachID == fachID) return true;
            }

            return false;
        }

        /// <summary>
        /// Erstellt ein neues Fach in der Datenbank.
        /// </summary>
        /// <param name="bezeichnung">Die Bezeichnung des neuen Faches.</param>
        /// <returns>True, wenn das Fach erfolgreich erstellt wurde, andernfalls false.</returns>
        public bool createFach(string bezeichnung)
        {
            // Überprüfen, ob ein Fach mit dem angegebenen Namen bereits existiert
            if (existFach(bezeichnung)) return false;

            // SQL-Query zum Einfügen des neuen Faches in die Datenbank
            string query = $@"INSERT INTO `fach`(`Bezeichnung`) VALUES ('{bezeichnung}')";

            // SQL-Query ausführen
            db.ExecuteQuery(query);

            return true;
        }


        /// <summary>
        /// Löscht ein Fach aus der Datenbank.
        /// </summary>
        /// <param name="fachID">Die ID des Faches, das gelöscht werden soll.</param>
        /// <returns>True, wenn das Fach erfolgreich gelöscht wurde, andernfalls false.</returns>
        public bool deleteFach(int fachID)
        {
            // Überprüfen, ob ein Fach mit der angegebenen Fach-ID existiert
            if (!existFach(fachID)) return false;

            // SQL-Query zum Löschen des Faches aus der Datenbank
            string query = $@"DELETE FROM `fach` WHERE `FachID` = '{fachID}'";

            // SQL-Query ausführen
            db.ExecuteQuery(query);

            return true;
        }







        #endregion

        #region Klasse
        /// <summary>
        /// Ruft eine Liste aller Klassen aus der Datenbank ab.
        /// </summary>
        /// <returns>Eine Liste von Klasse-Objekten oder null, wenn keine Daten gefunden wurden.</returns>
        public List<Klasse> getKlassen()
        {
            List<Klasse> klassen = new List<Klasse>();

            DataTable dataTable = db.TableToDataTable("klasse");
            if (dataTable == null)
            {
                return null;
            }

            if (dataTable.Rows.Count == 0)
            {
                return null;
            }


            foreach (DataRow row in dataTable.Rows)
            {
                int klasseID = Convert.ToInt32(row["KlassenID"]);
                string bezeichnung = row["Bezeichnung"].ToString();
                int bildungsgangID = Convert.ToInt32(row["BID"]);
                Klasse klasse = new Klasse(klasseID, bezeichnung, bildungsgangID);
                klassen.Add(klasse);
            }

            return klassen;

        }
        /// <summary>
        /// Überprüft, ob eine Klasse mit dem angegebenen Namen existiert.
        /// </summary>
        /// <param tag="name">Der Name der Klasse.</param>
        /// <returns>True, wenn die Klasse existiert, andernfalls false.</returns>
        /// <return>False</return>
        public bool existKlasse(string name)
        {
            List<Klasse> klassen = getKlassen();
            foreach (Klasse klasse in klassen)
            {
                if (klasse.Bezeichnung == name) return true;
            }
            return false;

        }

        /// <summary>
        /// Überprüft, ob eine Klasse mit der angegebenen Klassen-ID existiert.
        /// </summary>
        /// <param tag="klasseID">Die ID der Klasse.</param>
        /// <returns>True, wenn die Klasse existiert, andernfalls false.</returns>
        public bool existKlasse(int klasseID)
        {
            List<Klasse> klassen = getKlassen();

            foreach (Klasse klasse in klassen)
            {
                if (klasse.KlassenID == klasseID) return true;
            }

            return false;
        }


        /// <summary>
        /// Erstellt eine neue Klasse in der Datenbank.
        /// </summary>
        /// <param tag="name">Der Name der Klasse.</param>
        /// <param tag="bildungsgangID">Die ID des Bildungsgangs.</param>
        /// <returns>True, wenn die Klasse erfolgreich erstellt wurde, andernfalls false.</returns>
        public bool createKlasse(string name, int bildungsgangID)
        {
            // Überprüfen, ob eine Klasse mit dem angegebenen Namen bereits existiert
            if (existKlasse(name)) return false;

            // SQL-Query zum Einfügen der neuen Klasse in die Datenbank
            string query = $@"INSERT INTO `klasse`(`Bezeichnung`, `BID`) VALUES ('{name}','{bildungsgangID}')";

            // SQL-Query ausführen
            db.ExecuteQuery(query);

            return true;
        }

        public bool deleteKlasse(int klasseID)
        {
            if (!existKlasse(klasseID)) return false;
            string query = $@"DELETE FROM `klasse` WHERE `KlassenID` = '{klasseID}'";
            db.ExecuteQuery(query);
            return true;
        }


        #endregion

        #region Nachhilfegesuch

        /// <summary>
        /// Ruft eine Liste aller Nachhilfegesuche aus der Datenbank ab und filtert nach dem angegebenen Status.
        /// </summary>
        /// <param tag="orderby">Der Status, nach dem gefiltert werden soll ("offen" oder "erledigt").</param>
        /// <returns>Eine Liste von Nachhilfegesuch-Objekten oder null, wenn keine Daten gefunden wurden.</returns>
        public List<Nachhilfegesuch> getNachhilfegesuches(string orderby)
        {
            List<Nachhilfegesuch> nachhilfegesuches = new List<Nachhilfegesuch>();

            DataTable dataTable = db.QueryToDataTable("SELECT * FROM `nachhilfegesuch` ORDER BY `nachhilfegesuch`.`ErstelltAm` DESC ");
            if (dataTable == null)
            {
                return null;
            }

            if (dataTable.Rows.Count == 0)
            {
                return null;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                int gesuchID = Convert.ToInt32(row["GesuchID"]);
                int schuelerID = Convert.ToInt32(row["SchuelerID"]);
                int fachID = Convert.ToInt32(row["FachID"]);
                string beschreibung = row["Beschreibung"].ToString();
                DateTime erstellt = Convert.ToDateTime(row["ErstelltAm"]);
                long timestamp = new DateTimeOffset(erstellt).ToUnixTimeSeconds();

                Nachhilfegesuch.Status status = (Nachhilfegesuch.Status)Enum.Parse(typeof(Nachhilfegesuch.Status), row["Status"].ToString());

                Nachhilfegesuch nachhilfegesuch = new Nachhilfegesuch(gesuchID, schuelerID, fachID, beschreibung, timestamp, status);

                if (orderby == "offen")
                {
                    if (nachhilfegesuch.GesuchStatus == Nachhilfegesuch.Status.offen)
                    {
                        nachhilfegesuches.Add(nachhilfegesuch);
                    }
                }
                else if (orderby == "erledigt")
                {
                    if (nachhilfegesuch.GesuchStatus == Nachhilfegesuch.Status.erledigt)
                    {
                        nachhilfegesuches.Add(nachhilfegesuch);
                    }
                }
                else
                {
                    nachhilfegesuches.Add(nachhilfegesuch);
                }
            }

            return nachhilfegesuches;
        }

        /// <summary>
        /// Überprüft, ob ein Nachhilfegesuch mit der angegebenen Schüler-ID und Fach-ID existiert.
        /// </summary>
        /// <param tag="schuelerID">Die ID des Schülers.</param>
        /// <param tag="fachID">Die ID des Faches.</param>
        /// <returns>True, wenn das Nachhilfegesuch existiert, andernfalls false.</returns>
        public bool existNachhilfegesuch(int schuelerID, int fachID)
        {
            List<Nachhilfegesuch> nachhilfegesuches = getNachhilfegesuches("");

            foreach (Nachhilfegesuch nachhilfegesuch in nachhilfegesuches)
            {
                if (nachhilfegesuch.SchuelerID == schuelerID && nachhilfegesuch.FachID == fachID) return true;
            }
            return false;


        }

        public bool existNachhilfegesuch(int gesuchtID)
        {
            List<Nachhilfegesuch> nachhilfegesuches = getNachhilfegesuches("");

            foreach (Nachhilfegesuch nachhilfegesuch in nachhilfegesuches)
            {
                if (nachhilfegesuch.GesuchID == gesuchtID) return true;
            }
            return false;


        }

        /// <summary>
        /// Erstellt ein neues Nachhilfegesuch in der Datenbank.
        /// </summary>
        /// <param tag="schuelerID">Die ID des Schülers.</param>
        /// <param tag="fachID">Die ID des Faches.</param>
        /// <param tag="beschreibung">Die Beschreibung des Nachhilfegesuchs.</param>
        /// <returns>True, wenn das Nachhilfegesuch erfolgreich erstellt wurde.</returns>
        public bool createNachhilfegesucht(int schuelerID, int fachID, string beschreibung)
        {
            Nachhilfegesuch.Status status = Nachhilfegesuch.Status.offen;
            string erstellt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string query = $@"INSERT INTO `nachhilfegesuch`(`SchuelerID`, `FachID`, `Beschreibung`, `ErstelltAm`, `Status`) VALUES ('{schuelerID}','{fachID}','{beschreibung}','{erstellt}','{status}')";

            db.ExecuteQuery(query);
            return true;
        }

        /// <summary>
        /// Löscht ein Nachhilfegesuch aus der Datenbank.
        /// </summary>
        /// <param tag="gesuchID">Die ID des Nachhilfegesuchs, das gelöscht werden soll.</param>
        /// <returns>True, wenn das Nachhilfegesuch erfolgreich gelöscht wurde.</returns>
        public bool deleteNachhilfegesuch(int gesuchID)
        {
            if (!existNachhilfegesuch(gesuchID)) return false;
            string query = $@"DELETE FROM `nachhilfegesuch` WHERE `GesuchID` = '{gesuchID}'";
            db.ExecuteQuery(query);
            return true;
        }

        #endregion

        #region Schueler

        /// <summary>
        /// Ruft eine Liste aller Schüler aus der Datenbank ab.
        /// </summary>
        /// <returns>Eine Liste von Schueler-Objekten oder null, wenn keine Daten gefunden wurden.</returns>
        public List<Schueler> getSchueler()
        {
            List<Schueler> schuelers = new List<Schueler>();
            DataTable dataTable = db.TableToDataTable("schueler");
            if (dataTable == null)
            {
                return null;
            }
            if (dataTable.Rows.Count == 0)
            {
                return null;
            }
            foreach (DataRow row in dataTable.Rows)
            {
                int schuelerID = Convert.ToInt32(row["SchuelerID"]);
                string vorname = row["Vorname"].ToString();
                string nachname = row["Nachname"].ToString();
                Schueler.Genders genders = (Schueler.Genders)Enum.Parse(typeof(Schueler.Genders), row["Geschlecht"].ToString());
                string email = row["EMail"].ToString();
                int klasseID = Convert.ToInt32(row["KlassenID"]);
    

                Schueler schueler = new Schueler(schuelerID, vorname, nachname, genders, klasseID, email);

                schuelers.Add(schueler);
            }
            return schuelers;
        }

        public Schueler getSchueler(string email)
        {
            List<Schueler> schuelers = getSchueler();
            foreach (Schueler schueler in schuelers)
            {
                if (schueler.Email == email) return schueler;
            }
            return null;
        }

        /// <summary>
        /// Überprüft, ob ein Schüler mit der angegebenen E-Mail-Adresse existiert.
        /// </summary>
        /// <param name="email">Die E-Mail-Adresse des Schülers.</param>
        /// <returns>True, wenn der Schüler existiert, andernfalls false.</returns>
        public bool existSchueler(string email)
        {
            List<Schueler> schuelers = getSchueler();
            foreach (Schueler schueler in schuelers)
            {
                if (schueler.Email == email) return true;
            }
            return false;
        }

        /// <summary>
        /// Überprüft, ob ein Schüler mit der angegebenen Schüler-ID existiert.
        /// </summary>
        /// <param name="schuelerID">Die ID des Schülers.</param>
        /// <returns>True, wenn der Schüler existiert, andernfalls false.</returns>
        public bool existSchueler(int schuelerID)
        {
            List<Schueler> schuelers = getSchueler();
            foreach (Schueler schueler in schuelers)
            {
                if (schueler.SchuelerID == schuelerID) return true;
            }
            return false;
        }

        /// <summary>
        /// Erstellt einen neuen Schüler in der Datenbank.
        /// </summary>
        /// <param name="vorname">Der Vorname des Schülers.</param>
        /// <param name="nachname">Der Nachname des Schülers.</param>
        /// <param name="genders">Das Geschlecht des Schülers.</param>
        /// <param name="email">Die E-Mail-Adresse des Schülers.</param>
        /// <param name="klasseID">Die ID der Klasse des Schülers.</param>
        /// <returns>True, wenn der Schüler erfolgreich erstellt wurde, andernfalls false.</returns>
        public bool createSchueler(string vorname, string nachname, Schueler.Genders genders, string email, int klasseID)
        {
            if (existSchueler(email)) return false;

            // KORRIGIERT: Fehlerhafte Anführungszeichen und fehlende Kommata zwischen den Werten
            string query = $@"INSERT INTO `schueler`(`Vorname`, `Nachname`, `Geschlecht`, `EMail`, `KlassenID`) 
                     VALUES ('{vorname}', '{nachname}', '{genders}', '{email}', {klasseID})";

            db.ExecuteQuery(query);
            return true;
        }

        /// <summary>
        /// Löscht einen Schüler aus der Datenbank.
        /// </summary>
        /// <param name="schuelerID">Die ID des Schülers, der gelöscht werden soll.</param>
        /// <returns>True, wenn der Schüler erfolgreich gelöscht wurde, andernfalls false.</returns>
        public bool deleteSchueler(int schuelerID)
        {
            if (!existSchueler(schuelerID)) return false;
            string query = $@"DELETE FROM `schueler` WHERE `SchuelerID` = '{schuelerID}'";
            db.ExecuteQuery(query);
            return true;
        }


        #endregion

        #region Tutor


        public List<Tutor> getTutoren()
        {
            List<Tutor> tutoren = new List<Tutor>();
            DataTable dataTable = db.TableToDataTable("tutor");
            if (dataTable == null)
            {
                return null;
            }
            if (dataTable.Rows.Count == 0)
            {
                return null;
            }
            foreach (DataRow row in dataTable.Rows)
            {
                int tutorID = Convert.ToInt32(row["TutorID"]);
                int schuelerID = Convert.ToInt32(row["SchuelerID"]);
                float genehmigt = Convert.ToSingle(row["Genehmigt"]);
                int loginID = Convert.ToInt32(row["LoginID"]);

                bool genehmigtBool = genehmigt == 1 ? true : false;

                Tutor tutor = new Tutor(tutorID, schuelerID, genehmigtBool, loginID);
                tutoren.Add(tutor);
            }
            return tutoren;
        }


        /// <summary>
        /// Überprüft, ob ein Tutor mit der angegebenen Tutor-ID existiert.
        /// </summary>
        /// <param name="tutorID">Die ID des Tutors.</param>
        /// <returns>True, wenn der Tutor existiert, andernfalls false.</returns>
        public bool existTutor(int tutorID)
        {
            List<Tutor> tutoren = getTutoren();
            foreach (Tutor tutor in tutoren)
            {
                if (tutor.TutorID == tutorID) return true;
            }
            return false;
        }

        /// <summary>
        /// Erstellt einen neuen Tutor in der Datenbank.
        /// </summary>
        /// <param name="tutorID">Die ID des Tutors.</param>
        /// <param name="schuelerID">Die ID des Schülers.</param>
        /// <param name="genehmigt">Gibt an, ob der Tutor genehmigt ist.</param>
        /// <param name="loginID">Die ID des Logins.</param>
        public void createTutor(int schuelerID, bool genehmigt, int loginID)
        {
            int genehmigtInt = genehmigt ? 1 : 0;

            int count = getLogins().Count();



            // KORRIGIERT: Fehlendes Anführungszeichen am Ende und numerische Werte ohne Anführungszeichen
            string query = $@"INSERT INTO `tutor`(`TutorID`, `SchuelerID`, `Genehmigt`, `LoginID`) VALUES ('{count++}','{schuelerID}','{genehmigtInt}','{loginID}')";

            db.ExecuteQuery(query);
        }

        /// <summary>
        /// Löscht einen Tutor aus der Datenbank.
        /// </summary>
        /// <param name="tutorID">Die ID des Tutors, der gelöscht werden soll.</param>
        /// <returns>True, wenn der Tutor erfolgreich gelöscht wurde, andernfalls false.</returns>
        public bool deleteTutor(int tutorID)
        {
            if (!existTutor(tutorID)) return false;
            string query = $@"DELETE FROM `tutor` WHERE `TutorID` = '{tutorID}'";
            db.ExecuteQuery(query);
            return true;
        }


        /// <summary>
        /// Aktualisiert den Genehmigungsstatus eines Tutors in der Datenbank.
        /// </summary>
        /// <param name="tutorID">Die ID des Tutors.</param>
        /// <param name="genehmigt">Der neue Genehmigungsstatus des Tutors.</param>
        /// <returns>True, wenn der Genehmigungsstatus erfolgreich aktualisiert wurde, andernfalls false.</returns>
        public bool updateTutorGenehmigt(int tutorID, int genehmigt)
        {
          
            string query = $@"UPDATE `tutor` SET `Genehmigt` = '{genehmigt}' WHERE `tutor`.`SchuelerID` = '{tutorID}'";
            db.ExecuteQuery(query);
            return true;
        }


        #endregion

        #region Bildungsgang

        /// <summary>
        /// Ruft eine Liste aller Bildungsgänge aus der Datenbank ab.
        /// </summary>
        /// <returns>Eine Liste von Bildungsgang-Objekten oder null, wenn keine Daten gefunden wurden.</returns>
        public List<bildungsgang> getBildungsgang()
        {
            List<bildungsgang> bildungsgangs = new List<bildungsgang>();
            DataTable dataTable = db.TableToDataTable("bildungsgang");
            if (dataTable == null)
            {
                return null;
            }
            if (dataTable.Rows.Count == 0)
            {
                return null;
            }
            foreach (DataRow row in dataTable.Rows)
            {
                int bildungsgangID = Convert.ToInt32(row["BildungsgangID"]);
                string bezeichnung = row["Bezeichnung"].ToString();
                bildungsgang bildungsgang = new bildungsgang(bildungsgangID, bezeichnung);
                bildungsgangs.Add(bildungsgang);
            }
            return bildungsgangs;
        }

        /// <summary>
        /// Überprüft, ob ein Bildungsgang mit der angegebenen Bildungsgang-ID existiert.
        /// </summary>
        /// <param name="bildungsgangID">Die ID des Bildungsgangs.</param>
        /// <returns>True, wenn der Bildungsgang existiert, andernfalls false.</returns>
        public bool existBildungsgang(int bildungsgangID)
        {
            List<bildungsgang> bildungsgangs = getBildungsgang();
            foreach (bildungsgang bildungsgang in bildungsgangs)
            {
                if (bildungsgang.BildungsgangID == bildungsgangID) return true;
            }
            return false;
        }

        /// <summary>
        /// Überprüft, ob ein Bildungsgang mit der angegebenen Bezeichnung existiert.
        /// </summary>
        /// <param name="bezeichnung">Die Bezeichnung des Bildungsgangs.</param>
        /// <returns>True, wenn der Bildungsgang existiert, andernfalls false.</returns>
        public bool existBildungsgang(string bezeichnung)
        {
            List<bildungsgang> bildungsgangs = getBildungsgang();
            foreach (bildungsgang bildungsgang in bildungsgangs)
            {
                if (bildungsgang.Bezeichnung == bezeichnung) return true;
            }
            return false;
        }

        /// <summary>
        /// Erstellt einen neuen Bildungsgang in der Datenbank.
        /// </summary>
        /// <param name="bezeichnung">Die Bezeichnung des neuen Bildungsgangs.</param>
        /// <returns>True, wenn der Bildungsgang erfolgreich erstellt wurde, andernfalls false.</returns>
        public bool createBildungsgang(string bezeichnung)
        {
            if (existBildungsgang(bezeichnung)) return false;
            string query = $@"INSERT INTO `bildungsgang`(`Bezeichnung`) VALUES ('{bezeichnung}')";
            db.ExecuteQuery(query);
            return true;
        }

        /// <summary>
        /// Löscht einen Bildungsgang aus der Datenbank.
        /// </summary>
        /// <param name="bildungsgangID">Die ID des Bildungsgangs, der gelöscht werden soll.</param>
        /// <returns>True, wenn der Bildungsgang erfolgreich gelöscht wurde, andernfalls false.</returns>
        public bool deleteBildungsgang(int bildungsgangID)
        {
            if (!existBildungsgang(bildungsgangID)) return false;
            string query = $@"DELETE FROM `bildungsgang` WHERE `BildungsgangID` = '{bildungsgangID}'";
            db.ExecuteQuery(query);
            return true;
        }

        #endregion

        #region zeitspanne

        /// <summary>
        /// Ruft eine Liste aller Zeitspannen aus der Datenbank ab.
        /// </summary>
        /// <returns>Eine Liste von Zeitspanne-Objekten oder null, wenn keine Daten gefunden wurden.</returns>
        public List<Zeitspanne> GetZeitspannes()
        {
            DataTable dataTable = db.TableToDataTable("zeitspanne");
            List<Zeitspanne> zeitspannes = new List<Zeitspanne>();
            if (dataTable == null)
            {
                return null;
            }

            if (dataTable.Rows.Count == 0)
            {
                return null;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                int wochentagID = Convert.ToInt32(row["WTID"]);
                int schuelerID = Convert.ToInt32(row["SID"]);
                string start = row["Start"].ToString();
                string ende = row["Ende"].ToString();

                Zeitspanne zeitspanne = new Zeitspanne(wochentagID, schuelerID, Convert.ToInt32(start), Convert.ToInt32(ende));


                zeitspannes.Add(zeitspanne);
            }

            return zeitspannes;

        }

        /// <summary>
        /// Überprüft, ob eine Zeitspanne mit den angegebenen Parametern existiert.
        /// </summary>
        /// <param name="wochentagID">Die ID des Wochentags.</param>
        /// <param name="schuelerID">Die ID des Schülers.</param>
        /// <param name="start">Die Startzeit der Zeitspanne.</param>
        /// <param name="end">Die Endzeit der Zeitspanne.</param>
        /// <returns>True, wenn die Zeitspanne existiert, andernfalls false.</returns>
        public bool existZeitspanne(int wochentagID, int schuelerID , int start , int end)
        {
            List<Zeitspanne> zeitspannes = GetZeitspannes();
            foreach (Zeitspanne zeitspanne in zeitspannes)
            {
                if (zeitspanne.WochentagID == wochentagID && zeitspanne.SchuelerID == schuelerID && zeitspanne.Startzeit == start && zeitspanne.Endzeit == end ) return true;
            }
            return false;
        }

        /// <summary>
        /// Erstellt eine neue Zeitspanne in der Datenbank.
        /// </summary>
        /// <param name="wochentagID">Die ID des Wochentags.</param>
        /// <param name="schuelerID">Die ID des Schülers.</param>
        /// <param name="start">Die Startzeit der Zeitspanne.</param>
        /// <param name="ende">Die Endzeit der Zeitspanne.</param>
        /// <returns>True, wenn die Zeitspanne erfolgreich erstellt wurde, andernfalls false.</returns>
        public bool createZeitspanne(int wochentagID, int schuelerID, int start, int ende)
        {
            if (existZeitspanne(wochentagID, schuelerID,  start,  ende)) return false;
            string query = $@"INSERT INTO `zeitspanne`(`WTID`, `SID`, `Start`, `Ende`) VALUES ('{wochentagID}','{schuelerID}','{start}','{ende}')";
            db.ExecuteQuery(query);
            return true;
        }

        /// <summary>
        /// Ruft eine Liste aller Zeitspannen eines bestimmten Schülers aus der Datenbank ab.
        /// </summary>
        /// <param name="schuelerID">Die ID des Schülers.</param>
        /// <returns>Eine Liste von Zeitspanne-Objekten.</returns>
        public List<Zeitspanne> getZeitspaneebySchueler(int schuelerID)
        {
            List<Zeitspanne> zeitspannes = GetZeitspannes();
           
            foreach (Zeitspanne zeitspanne in zeitspannes)
            {
               
                if (zeitspanne.SchuelerID != schuelerID)
                {
                    zeitspannes.Remove(zeitspanne);
                }


            }
            return zeitspannes;
        }

        /// <summary>
        /// Löscht eine Zeitspanne aus der Datenbank.
        /// </summary>
        /// <param name="wochentagID">Die ID des Wochentags.</param>
        /// <param name="schuelerID">Die ID des Schülers.</param>
        /// <param name="start">Die Startzeit der Zeitspanne.</param>
        /// <param name="ende">Die Endzeit der Zeitspanne.</param>
        /// <returns>True, wenn die Zeitspanne erfolgreich gelöscht wurde, andernfalls false.</returns
        public bool deleteZeitspanne(int wochentagID, int schuelerID, int start, int ende)
        {
            if (!existZeitspanne(wochentagID, schuelerID, start, ende)) return false;
            string query = $@"DELETE FROM `zeitspanne` WHERE `WTID` = '{wochentagID}' AND `SID` = '{schuelerID}' AND `Start` = '{start}' AND `Ende` = '{ende}'";
            db.ExecuteQuery(query);
            return true;
        }


        #endregion


        #region Wochentag

        /// <summary>
        /// Ruft eine Liste aller Wochentage aus der Datenbank ab.
        /// </summary>
        /// <returns>Eine Liste von Wochentag-Objekten oder null, wenn keine Daten gefunden wurden.</returns>
        public List<wochentag> GetWochentags()
        {
            List<wochentag> wochentags = new List<wochentag>();
            DataTable dataTable = db.TableToDataTable("wochentage");
            if (dataTable == null)
            {
                return null;
            }
            if (dataTable.Rows.Count == 0)
            {
                return null;
            }
            foreach (DataRow row in dataTable.Rows)
            {
                int wochentagID = Convert.ToInt32(row["WTID"]);
                string bezeichnung = row["Tag"].ToString();
                wochentag wochentag = new wochentag(wochentagID, bezeichnung);
                wochentags.Add(wochentag);
            }
            return wochentags;
        }

        /// <summary>
        /// Überprüft, ob ein Wochentag mit der angegebenen Wochentag-ID existiert.
        /// </summary>
        /// <param name="wochentagID">Die ID des Wochentags.</param>
        /// <returns>True, wenn der Wochentag existiert, andernfalls false.</returns>
        public bool existWochentag(int wochentagID)
        {
            List<wochentag> wochentags = GetWochentags();
            foreach (wochentag wochentag in wochentags)
            {
                if (wochentag.WochentagID == wochentagID) return true;
            }
            return false;
        }

        /// <summary>
        /// Überprüft, ob ein Wochentag mit der angegebenen Bezeichnung existiert.
        /// </summary>
        /// <param name="tag">Die Bezeichnung des Wochentags.</param>
        /// <returns>True, wenn der Wochentag existiert, andernfalls false.</returns>
        public bool existWochentag(string tag)
        {
            List<wochentag> wochentags = GetWochentags();
            foreach (wochentag wochentag in wochentags)
            {
                if (wochentag.Tag == tag) return true;
            }
            return false;
        }




        #endregion


        #region lehrerhatfach

        /// <summary>
        /// Ruft eine Liste aller Lehrer-Fach-Zuordnungen aus der Datenbank ab.
        /// </summary>
        /// <returns>Eine Liste von lehrerhatfach-Objekten oder null, wenn keine Daten gefunden wurden.</returns>
        public List<lehrerhatfach> lehrerhatfach()
        {
            List<lehrerhatfach> lehrerhatfachs = new List<lehrerhatfach>();
            DataTable dataTable = db.TableToDataTable("lehrerhatfach");
            if (dataTable == null)
            {
                return null;
            }
            if (dataTable.Rows.Count == 0)
            {
                return null;
            }
            foreach (DataRow row in dataTable.Rows)
            {
                int lehrerID = Convert.ToInt32(row["TutorID"]);
                int fachID = Convert.ToInt32(row["FachID"]);
                lehrerhatfach lehrerhatfach = new lehrerhatfach(lehrerID, fachID);
                lehrerhatfachs.Add(lehrerhatfach);
            }
            return lehrerhatfachs;
        }
        /// <summary>
        /// Überprüft, ob eine Lehrer-Fach-Zuordnung mit den angegebenen Parametern existiert.
        /// </summary>
        /// <param name="lehrerID">Die ID des Lehrers.</param>
        /// <param name="fachID">Die ID des Faches.</param>
        /// <returns>True, wenn die Lehrer-Fach-Zuordnung existiert, andernfalls false.</returns>
        public bool existlehrerhatfach(int lehrerID, int fachID)
        {
            List<lehrerhatfach> lehrerhatfachs = lehrerhatfach();
            foreach (lehrerhatfach lehrerhatfach in lehrerhatfachs)
            {
                if (lehrerhatfach.TutorID == lehrerID && lehrerhatfach.FachID == fachID) return true;
            }
            return false;
        }

        /// <summary>
        /// Erstellt eine neue Lehrer-Fach-Zuordnung in der Datenbank.
        /// </summary>
        /// <param name="lehrerID">Die ID des Lehrers.</param>
        /// <param name="fachID">Die ID des Faches.</param>
        /// <returns>True, wenn die Lehrer-Fach-Zuordnung erfolgreich erstellt wurde, andernfalls false.</returns>
        public bool createlehrerhatfach(int lehrerID, int fachID)
        {
            if (existlehrerhatfach(lehrerID, fachID)) return false;
            string query = $@"INSERT INTO `lehrerhatfach`(`TutorID`, `FachID`) VALUES ('{lehrerID}','{fachID}')";
            db.ExecuteQuery(query);
            return true;
        }

        /// <summary>
        /// Löscht eine Lehrer-Fach-Zuordnung aus der Datenbank.
        /// </summary>
        /// <param name="lehrerID">Die ID des Lehrers.</param>
        /// <param name="fachID">Die ID des Faches.</param>
        /// <returns>True, wenn die Lehrer-Fach-Zuordnung erfolgreich gelöscht wurde, andernfalls false.</returns>
        public bool deletelehrerhatfach(int lehrerID, int fachID)
        {
            if (!existlehrerhatfach(lehrerID, fachID)) return false;
            string query = $@"DELETE FROM `lehrerhatfach` WHERE `TutorID` = '{lehrerID}' AND `FachID` = '{fachID}'";
            db.ExecuteQuery(query);
            return true;
        }

        #endregion






    }
}