using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace PoCLib
{
    public class SQLiteClient
    {
        public const string DataBaseFileName = "DataTransfer.sqlite";
        public void CreateDataBase()
        {
            SQLiteConnection.CreateFile(DataBaseFileName);

            ExecuteCommand(GetCreateDataBaseString());
        }

        private string GetCreateDataBaseString()
        {
            return @"create table param_int (pid int, pos int, value int);
create table param_float (pid int, pos int, value decimal(18,2));
create table param_int_array (pid int, pos int);
create table param_int_array_value (pid int, pos int, array_index int, value int);
create table param_float_array (pid int, pos int);
create table param_float_array_value (pid int, pos int, array_index int, value decimal(18,2));";
        }

        public void ExecuteCommand(string sql) 
        {
            SQLiteConnection dbConnection = new SQLiteConnection($"Data Source={DataBaseFileName};Version=3;");
            dbConnection.Open();

            SQLiteCommand command = new SQLiteCommand(dbConnection)
            {
                CommandText = sql
            };
            command.ExecuteNonQuery();

            dbConnection.Close();
        }
    }
}
