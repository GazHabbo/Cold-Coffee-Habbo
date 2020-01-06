using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace Reflection.Kernel.Database
{
    public class DatabaseManager
    {
        private MySqlConnection connection;

        public DatabaseManager()
        {
            this.connection = new MySqlConnection();
        }

        public void Open(string host, string user, string pass, string db)
        {
            var sb = new MySqlConnectionStringBuilder();
            sb.Server = host;
            sb.UserID = user;
            sb.Password = pass;
            sb.Database = db;
            sb.Pooling = true;
            sb.MinimumPoolSize = 5;
            sb.MaximumPoolSize = 15;
            var connstr = sb.ToString();
            this.connection.ConnectionString = connstr;

            this.connection.Open();
        }

        public DataTable GetTable(string query, params MySqlParameter[] parameters)
        {
            var command = this.connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddRange(parameters);
            var data = new DataTable();
            new MySqlDataAdapter(command).Fill(data);
            return data;
        }

        public DataRow GetRow(string query, params MySqlParameter[] parameters)
        {
            var data = GetTable(query, parameters);

            if (data.Rows.Count > 0)
            {
                return data.Rows[0];
            }

            return default(DataRow);
        }

        public void Execute(string query, params MySqlParameter[] parameters)
        {
            var command = this.connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddRange(parameters);
            command.ExecuteNonQuery();
        }

        public int Insert(string query, params MySqlParameter[] parameters)
        {
            var command = this.connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddRange(parameters);
            command.ExecuteNonQuery();
            return (int)command.LastInsertedId;
        }

        public string GetString(string query, params MySqlParameter[] parameters)
        {
            var command = this.connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddRange(parameters);
            return (string)command.ExecuteScalar();
        }

        public int GetInt(string query, params MySqlParameter[] parameters)
        {
            var command = this.connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddRange(parameters);
            return (int)command.ExecuteScalar();
        }

        public bool GetBool(string query, params MySqlParameter[] parameters)
        {
            var command = this.connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddRange(parameters);
            return (bool)command.ExecuteScalar();
        }

        public bool HasRows(string query, params MySqlParameter[] parameters)
        {
            var row = GetRow(query, parameters);
            return (row != null);
        }

        public bool HasRows(string query, out DataRow row, params MySqlParameter[] parameters)
        {
            var Row = GetRow(query, parameters);

            if (Row != null)
            {
                row = Row;
                return true;
            }

            row = null;
            return false;
        }
    }
}
