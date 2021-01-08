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
                Execute("CREATE TABLE MemoryData (value VARCHAR NOT NULL)");
                Execute("CREATE TABLE HistoryData (formula VARCHAR NOT NULL, answer VARCHAR NOT NULL)");
            }
            using (SQLiteConnection connection = new SQLiteConnection("Data Source = " + filePath))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand("SELECT * FROM MemoryData", connection);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    double result;
                    if (double.TryParse(table.Rows[i][0].ToString(), out result) == false)
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
            Execute(string.Format("INSERT INTO MemoryData VALUES('{0}')", value));
        }

        public void Delete()
        {
            if (MemoryCollection.Count() > 0)
                MemoryCollection.RemoveAt(0);
            Execute(string.Format("DELETE FROM MemoryData " +
                                         "WHERE rowid = {0}", MemoryCollection.Count() + 1));
        }

        public void Increase(double value)
        {
            MemoryCollection[0] += value;
            Execute(string.Format("UPDATE MemoryData " +
                                      "SET value = {0} " +
                                      "WHERE rowid = {1}", MemoryCollection[0], MemoryCollection.Count()));
        }

        public void Decrease(double value)
        {
            MemoryCollection[0] -= value;
            Execute(string.Format("UPDATE MemoryData " +
                                      "SET value = {0} " +
                                      "WHERE rowid = {1}", MemoryCollection[0], MemoryCollection.Count()));
        }

        public void Clear()
        {
            MemoryCollection.Clear();
            Execute(string.Format("DELETE FROM MemoryData"));
        }

        public bool IsEmpty()
        {
            return MemoryCollection.Count() == 0;
        }
    }
}
