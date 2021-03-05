using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLInventoryPacking.WinApps.Services
{
    public class DbCreator
    {
        SQLiteConnection _dbConnection;
        SQLiteCommand _command;
        string _sqlCommand;
        string _dbPath = Environment.CurrentDirectory + "\\DB";
        string _dbFilePath;

        public DbCreator()
        {
            CreateDbFile();
            CreateDbConnection();
            CreateTables();
        }

        public void CreateDbFile()
        {
            if (!string.IsNullOrEmpty(_dbPath) && !Directory.Exists(_dbPath))
                Directory.CreateDirectory(_dbPath);
            _dbFilePath = _dbPath + "\\packing-inventory.db";
            if (!File.Exists(_dbFilePath))
            {
                SQLiteConnection.CreateFile(_dbFilePath);
            }
        }

        public string CreateDbConnection()
        {
            var strCon = string.Format("Data Source={0};", _dbFilePath);
            _dbConnection = new SQLiteConnection(strCon);
            _dbConnection.Open();
            _command = _dbConnection.CreateCommand();
            return strCon;
        }

        public void CreateTables()
        {
            if (!CheckIfExist("IPSettings"))
            {
                _sqlCommand = "CREATE TABLE IPSettings(IP TEXT NOT NULL)";
                ExecuteQuery(_sqlCommand);
            }

        }

        public bool CheckIfExist(string tableName)
        {
            _command.CommandText = "SELECT name FROM sqlite_master WHERE name='" + tableName + "'";
            var result = _command.ExecuteScalar();

            return result != null && result.ToString() == tableName ? true : false;
        }

        public void ExecuteQuery(string sqlCommand)
        {
            var triggerCommand = _dbConnection.CreateCommand();
            triggerCommand.CommandText = sqlCommand;
            triggerCommand.ExecuteNonQuery();
        }

        public bool CheckIfTableContainsData(string tableName)
        {
            _command.CommandText = "SELECT COUNT(*) FROM " + tableName;
            var result = _command.ExecuteScalar();

            return Convert.ToInt32(result) > 0 ? true : false;
        }


        public void SetIP(string IP)
        {
            _sqlCommand = $"DELETE FROM IPSettings";
            ExecuteQuery(_sqlCommand);
            _sqlCommand = $"INSERT INTO IPSettings (IP) VALUES ('{IP}')";
            ExecuteQuery(_sqlCommand);
        }

        public string GetIP()
        {
            var ips = new List<string>();
            _sqlCommand = "SELECT IP FROM IPSettings";

            _command.CommandText = _sqlCommand;
            var reader = _command.ExecuteReader();
            if (reader.FieldCount > 0)
                while (reader.Read())
                {
                    ips.Add(Convert.ToString(reader["IP"]));
                }

            return ips.LastOrDefault();
        }
    }
}
