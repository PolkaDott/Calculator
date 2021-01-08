using System.Collections.ObjectModel;
using System.Linq;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System;

namespace calculator.HistoryMemory
{
    class MemorySQL : IMemory
    {
        public ObservableCollection<double> MemoryCollection { get; } = new ObservableCollection<double>();
        private string filePath;
        public MemorySQL(string path = "calculator.db") 
        {
            filePath = path;
            if (MemoryCollection is null)
                MemoryCollection = new ObservableCollection<double>();
            if (File.Exists(filePath) == false)
            {
                SQLiteConnection.CreateFile(filePath);
                using (SQLiteConnection connection = new SQLiteConnection("Data Source = " + filePath))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        command.CommandText =
                            @"CREATE TABLE MemoryData (
                                value VARCHAR NOT NULL)";
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                        command.CommandText =
                            @"CREATE TABLE HistoryData (
                                formula VARCHAR NOT NULL,
                                answer VARCHAR NOT NULL)";
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                    }
                }
            }
            using (SQLiteConnection connection = new SQLiteConnection("Data Source = " + filePath))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand("SELECT * FROM MemoryData", connection);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    double result;
                    if (double.TryParse(dt.Rows[i][0].ToString(), out result) == false)
                        throw new Exception("БД содержит недопустимые значения!");
                    MemoryCollection.Insert(0, result);
                }
            }
        }

        private void Execute(string query)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source = " + filePath))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = query;
                command.ExecuteNonQuery();
            }
        }

        public void Add(double value)
        {
            MemoryCollection.Insert(0, value);
            using (SQLiteConnection connection = new SQLiteConnection("Data Source = " + filePath))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = @"INSERT INTO MemoryData
                                            VALUES (@value)";
                command.Parameters.AddWithValue("@value", value.ToString());
                command.ExecuteNonQuery();
            }
        }

        public void Delete()
        {
            if (MemoryCollection.Count() > 0)
                MemoryCollection.RemoveAt(0);
            using (SQLiteConnection connection = new SQLiteConnection("Data Source = " + filePath))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = @"DELETE FROM MemoryData
                                            WHERE rowid=@rowid";
                command.Parameters.AddWithValue("@rowid", MemoryCollection.Count()+1);
                command.ExecuteNonQuery();
            }
        }

        public void Increase(double value)
        {
            MemoryCollection[0] += value;
            using (SQLiteConnection connection = new SQLiteConnection("Data Source = " + filePath))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = @"UPDATE MemoryData
                                            SET value = @value 
                                            WHERE rowid = @rowid";
                command.Parameters.AddWithValue("@rowid", MemoryCollection.Count());
                command.Parameters.AddWithValue("@value", MemoryCollection[0].ToString());
                command.ExecuteNonQuery();
            }
        }

        public void Decrease(double value)
        {
            MemoryCollection[0] -= value;
            using (SQLiteConnection connection = new SQLiteConnection("Data Source = " + filePath))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = @"UPDATE MemoryData
                                            SET value = @value 
                                            WHERE rowid = @rowid";
                command.Parameters.AddWithValue("@rowid", MemoryCollection.Count());
                command.Parameters.AddWithValue("@value", MemoryCollection[0].ToString());
                command.ExecuteNonQuery();
            }
        }

        public void Clear()
        {
            MemoryCollection.Clear();
            using (SQLiteConnection connection = new SQLiteConnection("Data Source = " + filePath))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = "DELETE FROM MemoryData";
                command.ExecuteNonQuery();
            }
        }

        public bool IsEmpty()
        {
            return MemoryCollection.Count() == 0;
        }
    }
}
