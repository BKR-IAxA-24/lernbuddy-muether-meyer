using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;


namespace Muether_Meyer_Nachhilfe.common
{
    /// <summary>
    /// class Dbase
    /// creates a connection object to a database and contains
    /// methods to manage database operations
    /// </summary>
    class Dbase
    {
        private string serverName = "localhost";
        private string databaseName = string.Empty;
        private string uid = "root";
        private string passwd = string.Empty;
        private string connString = string.Empty;
        private MySqlConnection connection = null;
        private MySqlCommand command = null;


        #region Konstruktor
        /// <summary>
        /// Verbindung mit der Datenbank mit Standardwerten, außer des Datenbanknamens.
        /// </summary>
        /// <param name="_database">Name der Datenbank</param>

        public Dbase(string _servername, string _database, string _uid, string _passwd)
        {
            serverName = _servername;
            databaseName = _database;
            uid = _uid;
            passwd = _passwd;
            Connect();
        }
        #endregion

        /// <summary>
        /// Stellt eine neue Verbindung mit der Datenbank her und testet diese.
        /// </summary>
        private void Connect()
        {
            connString = $"SERVER={serverName};DATABASE={databaseName};UID={uid};PWD={passwd}";
            try
            {
                connection = new MySqlConnection(connString);
                connection.Open();
                connection.Close();
            }
            catch (MySqlException ex)
            {
                if (connection != null && connection.State == ConnectionState.Open)
                    connection.Close();
                connection = null;
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Trennt eine Verbindung zur Datenbank.
        /// </summary>
        private void Disconnect()
        {
            command?.Dispose();
            command = null;
            connection?.Dispose();
            connection = null;
        }

        /// <summary>
        /// Gibt true zurück, wenn eine Verbindung zur Datenbank besteht.
        /// </summary>
        public bool ConnectionExist
        {
            get
            {
                if (connection == null) return false;
                try
                {
                    connection.Open();
                    connection.Close();
                }
                catch
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Führt allgemeine Querys, wie "INSERT INTO", "UPDATE", "DELETE FROM" etc. aus.
        /// </summary>
        /// <param name="_query">Auszuführende SQL-Query</param>
        public void ExecuteQuery(string _query)
        {
            try
            {
                connection.Open();
                command = new MySqlCommand(_query, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (MySqlException ex)
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
                throw new Exception(ex.Message);
            }
        }

        #region ToDataTable
        /// <summary>
        /// Führt eine SQL-Query aus und gibt diese in Form eines DataTables zurück.
        /// </summary>
        /// <param name="_query">Auszuführende SQL-Query</param>
        /// <returns>Rückgabe des befüllten DataTables</returns>
        public DataTable QueryToDataTable(string _query)
        {
            DataTable dtData = new DataTable();
            try
            {
                connection.Open();
                MySqlDataAdapter adp = new MySqlDataAdapter(_query, connection);
                adp.Fill(dtData);
                connection.Close();
                return dtData;
            }
            catch (MySqlException ex)
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
                throw new Exception(ex.Message);
                return new DataTable();
            }
        }

        /// <summary>
        /// Führt eine SQL-Query aus, die nur eine Tabelle besitzt, und gibt diese als DataTable zurück.
        /// </summary>
        /// <param name="_table">Name der auszugebenden Tabelle</param>
        /// <returns>Rückgabe des befüllten DataTables</returns>
        public DataTable TableToDataTable(string _table)
        {
            DataTable dtData = new DataTable();
            string query = $"SELECT * FROM {_table}";
            try
            {
                connection.Open();
                MySqlDataAdapter adp = new MySqlDataAdapter(query, connection);
                adp.Fill(dtData);
                connection.Close();
                return dtData;
            }
            catch (MySqlException ex)
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
                throw new Exception(ex.Message);
                return new DataTable();
            }
        }
        #endregion

        /// <summary>
        /// Dekonstruktor
        /// </summary>
        ~Dbase()
        {
            Disconnect();
        }
    }
}
