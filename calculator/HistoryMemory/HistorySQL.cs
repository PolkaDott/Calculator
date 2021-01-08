using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace calculator.HistoryMemory
{
    class HistorySQL : IHistory
    {
        public ObservableCollection<Expression> HistoryCollection { get; } = new ObservableCollection<Expression>();
        private string filePath;
        public HistorySQL(string path = "calculator.db") 
        {
            filePath = path;
            if (HistoryCollection is null)
                HistoryCollection = new ObservableCollection<Expression>();
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
                SQLiteCommand command = new SQLiteCommand("SELECT * FROM HistoryData", connection);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Expression expression = new Expression();
                    expression.Answer = dt.Rows[i][1].ToString();
                    expression.Formula = dt.Rows[i][0].ToString();
                    HistoryCollection.Insert(0, expression);
                }
            }
        }

        public void Add(Expression expression)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source = " + filePath))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = @"INSERT INTO HistoryData
                                            VALUES (@formula, @answer)";
                command.Parameters.AddWithValue("@formula", expression.Formula);
                command.Parameters.AddWithValue("@answer", expression.Answer);
                command.ExecuteNonQuery();
            }
            HistoryCollection.Insert(0, expression);
        }

        public void Clear()
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source = " + filePath))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = "DELETE FROM HistoryData";
                command.ExecuteNonQuery();
            }
            HistoryCollection.Clear();
        }
    }
}
