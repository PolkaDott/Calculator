using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.IO;

namespace calculator.HistoryMemory
{
    class HistoryJSON : IHistory
    {
        public ObservableCollection<Expression> HistoryCollection { get; } = new ObservableCollection<Expression>();
        private string filePath;
        public HistoryJSON(string path = "history.json") {
            filePath = path;
            if (File.Exists(path) == false)
            {
                FileStream stream = File.Create(path);
                stream.Close();
                HistoryCollection = new ObservableCollection<Expression>();
                return;
            }
            var lastData = File.ReadAllText(path);
            HistoryCollection = JsonConvert.DeserializeObject<ObservableCollection<Expression>>(lastData);
            if (HistoryCollection is null)
                HistoryCollection = new ObservableCollection<Expression>();
        }

        public void Add(Expression expression)
        {
            HistoryCollection.Insert(0, expression);
            Load();
        }

        public void Clear()
        {
            HistoryCollection.Clear();
            Load();
        }

        public void Load()
        {
            if (File.Exists(filePath) == false)
            {
                File.Create(filePath);
            }
            var data = JsonConvert.SerializeObject(HistoryCollection);
            File.WriteAllText(filePath, data);
        }
    }
}
