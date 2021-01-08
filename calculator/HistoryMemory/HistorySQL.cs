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
                Execute("CREATE TABLE MemoryData (value VARCHAR NOT NULL)");
                Execute("CREATE TABLE HistoryData (formula VARCHAR NOT NULL, answer VARCHAR NOT NULL)");
            }
            using (SQLiteConnection connection = new SQLiteConnection("Data Source = " + filePath))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand("SELECT * FROM HistoryData", connection);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    Expression expression = new Expression();
                    expression.Answer = table.Rows[i][1].ToString();
                    expression.Formula = table.Rows[i][0].ToString();
                    HistoryCollection.Insert(0, expression);
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

        public void Add(Expression expression)
        {
            Execute(string.Format("INSERT INTO HistoryData " +
                                            "VALUES('{0}', '{1}')", expression.Formula, expression.Answer));
            HistoryCollection.Insert(0, expression);
        }


        public void Clear()
        {
            Execute("DELETE FROM HistoryData");
            HistoryCollection.Clear();
        }
    }
}
