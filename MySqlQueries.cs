using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MySqlQueriesNamespace
{
    class MySqlQueries
    {
        MySqlConnection conn;
        public MySqlQueries(string connectionString)
        {
            conn = new MySqlConnection(connectionString);
        }

        public bool TestConnection()
        {
            try
            {
                conn.Open();
                conn.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<string> SHOW_TABLES()
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SHOW TABLES", conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            List<string> tableNames = new List<string>();
            while (reader.Read())
            {
                tableNames.Add(reader.GetString(0));
            }
            reader.Close();
            conn.Close();

            return tableNames;
        }
        /// <summary>
        /// TODO: generalize this method
        /// </summary>
        /// <param name="newTableName"></param>
        public void CREATE_TABLE(string newTableName)
        {
            conn.Open();
            string query = " CREATE TABLE " + newTableName + "(" + "\n" +
                           " ID          INT NOT NULL AUTO_INCREMENT,                   \n" +
                           " Question     VARCHAR(255) NOT NULL,                         \n" +
                           " Answer      VARCHAR(255) NOT NULL,                         \n" +
                           " PRIMARY KEY (ID) );                                        \n";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void DROP_TABLE(string table)
        {
            conn.Open();
            string query = " DROP TABLE " + table;
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        /// <summary>
        /// Selects everything from table
        /// </summary>
        /// <param name="table">Which table to query</param>
        public List<Dictionary<string, string>> SELECT_ALL(string table)
        {
            conn.Open();
            string query = "SELECT * FROM " + table;
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
            // Go through each row
            while (reader.Read())
            {
                rows.Add(new Dictionary<string, string>());
                Dictionary<string, string> rowDict = rows[rows.Count() - 1];
                // Go through each column within row
                for (int i = 0; i < reader.FieldCount; i++)
                    rowDict.Add(reader.GetName(i), reader.GetString(i));
            }
            conn.Close();
            return rows;
        }
        /// <summary>
        /// Selects everything from table, ordered by column
        /// </summary>
        /// <param name="table">Which table to query</param>
        /// <param name="ORDER_BY_column">Which column to order by</param>
        /// <param name="ascending">true for asc, false for desc</param>
        public List<Dictionary<string, string>> SELECT_ALL(string table, string ORDER_BY_column, bool ascending)
        {
            conn.Open();
            string direction = ascending == true ? "ASC" : "DESC";
            string query = "SELECT * FROM " + table + "\n" +
                           "ORDER BY " + ORDER_BY_column + " " + direction;
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
            // Go through each row
            while (reader.Read())
            {
                rows.Add(new Dictionary<string, string>());
                Dictionary<string, string> rowDict = rows[rows.Count() - 1];
                // Go through each column within row
                for (int i = 0; i < reader.FieldCount; i++)
                    rowDict.Add(reader.GetName(i), reader.GetString(i));
            }
            conn.Close();
            return rows;
        }
        public List<Dictionary<string, string>> SELECT_ALL_WHERE(string table, string WHERE_column, int WHERE_value)
        {
            conn.Open();
            string query = "SELECT * FROM " + table + " WHERE " + WHERE_column + "=" + WHERE_value;
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
            // Go through each row
            while (reader.Read())
            {
                rows.Add(new Dictionary<string, string>());
                Dictionary<string, string> rowDict = rows[rows.Count() - 1];
                // Go through each column within row
                for (int i = 0; i < reader.FieldCount; i++)
                    rowDict.Add(reader.GetName(i), reader.GetString(i));
            }
            conn.Close();
            return rows;
        }
        public List<Dictionary<string, string>> SELECT_ALL_WHERE(string table, string WHERE_column, int WHERE_value, string ORDER_BY_column, bool ascending)
        {
            conn.Open();
            string direction = ascending == true ? "ASC" : "DESC";
            string query = "SELECT * FROM " + table + " WHERE " + WHERE_column + "=" + WHERE_value + " ORDER BY " + ORDER_BY_column + " " + direction;
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
            // Go through each row
            while (reader.Read())
            {
                rows.Add(new Dictionary<string, string>());
                Dictionary<string, string> rowDict = rows[rows.Count() - 1];
                // Go through each column within row
                for (int i = 0; i < reader.FieldCount; i++)
                    rowDict.Add(reader.GetName(i), reader.GetString(i));
            }
            conn.Close();
            return rows;
        }
        public List<Dictionary<string, string>> SELECT_ALL_WHERE(string table, string[] WHERE_columns, int[] WHERE_values, string logic_operator, string ORDER_BY_column, bool ascending)
        {
            conn.Open();
            string direction = ascending == true ? "ASC" : "DESC";
            string query = "SELECT * FROM " + table + "\n WHERE " + WHERE_columns[0] + "=" + WHERE_values[0] + " ";
            for (int i = 1; i < WHERE_columns.Length; i++)
            {
                query += logic_operator + " " + WHERE_columns[i] + "=" + WHERE_values[i] + "\n ";
            }
            query += "ORDER BY " + ORDER_BY_column + " " + direction;
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
            // Go through each row
            while (reader.Read())
            {
                rows.Add(new Dictionary<string, string>());
                Dictionary<string, string> rowDict = rows[rows.Count() - 1];
                // Go through each column within row
                for (int i = 0; i < reader.FieldCount; i++)
                    rowDict.Add(reader.GetName(i), reader.GetString(i));
            }
            conn.Close();
            return rows;
        }
        public void INSERT(string table, string[] columnNames, string[] values)
        {
            conn.Open();
            string query = "INSERT INTO " + table + "(";

            for (int i = 0; i < values.Length; i++)
                query += columnNames[i] + ", ";

            query = query.Substring(0, query.Length - 2); // removes last comma
            query += ")\n VALUES (";

            for (int i = 0; i < values.Length; i++)
                query += "'" + values[i] + "', ";

            query = query.Substring(0, query.Length - 2); // removes last comma
            query += ")";

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();

            conn.Close();
        }
        /// <summary>
        /// Deletes entire row where column has certain value
        /// </summary>
        /// <param name="table">Which table to query</param>
        /// <param name="column">Which column to look for value</param>
        /// <param name="value">Row with this value in column will be deleted</param>
        public void DELETE_WHERE(string table, string column, int value)
        {
            conn.Open();
            string query = "DELETE FROM " + table + "\n" +
                           "WHERE " + column + " = " + value;
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();

            conn.Close();
        }
        /// <summary>
        /// Deletes entire row where column has certain value
        /// </summary>
        /// <param name="table">Which table to query</param>
        /// <param name="column">Which column to look for value</param>
        /// <param name="value">Row with this value in column will be deleted</param>
        public void DELETE_WHERE(string table, string column, string value)
        {
            conn.Open();
            string query = "DELETE FROM " + table + "\n" +
                           "WHERE " + column + " = '" + value + "'";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();

            conn.Close();
        }
        /// <summary>
        /// Deletes entire row where column has certain value
        /// </summary>
        /// <param name="table">Which table to query</param>
        /// <param name="column">Which columns to look for value</param>
        /// <param name="values">Row with these values in columns will be deleted</param>
        public void DELETE_WHERE(string table, string[] columns, string[] values)
        {
            conn.Open();
            string query = "DELETE FROM " + table + "\n" +
                           "WHERE ";
            for (int i = 0; i < columns.Length; i++)
            {
                query += columns[i] + " = '" + values[i] + "'";
                if (i < columns.Length - 1)
                    query += " AND ";
            }

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();

            conn.Close();
        }
        /// <summary>
        /// Deletes all contents in table
        /// </summary>
        /// <param name="table">Which table to be cleared</param>
        public void TRUNCATE_TABLE(string table)
        {
            conn.Open();
            string query = "TRUNCATE TABLE " + table;
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();

            conn.Close();
        }
    }
}
