using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace calculator.HistoryMemory
{
    class MemoryJSON : IMemory
    {
        public ObservableCollection<double> MemoryCollection { get; } = new ObservableCollection<double>();
        private string filePath;
        public MemoryJSON(string path = "memory.json") {
            filePath = path;
            if (File.Exists(path) == false)
            {
                FileStream stream = File.Create(path);
                stream.Close();
                MemoryCollection = new ObservableCollection<double>();
                return;
            }
            var lastData = File.ReadAllText(path);
            MemoryCollection = JsonConvert.DeserializeObject<ObservableCollection<double>>(lastData);
            if (MemoryCollection is null)
                MemoryCollection = new ObservableCollection<double>();
        }

        public void Add(double value)
        {
            MemoryCollection.Insert(0, value);
            Load();
        }

        public void Delete()
        {
            if (MemoryCollection.Count() > 0)
                MemoryCollection.RemoveAt(0);
            Load();
        }

        public void Increase(double value)
        {
            MemoryCollection[0] += value;
            Load();
        }

        public void Decrease(double value)
        {
            MemoryCollection[0] -= value;
            Load();
        }

        public void Clear()
        {
            MemoryCollection.Clear();
            Load();
        }

        public bool IsEmpty()
        {
            return MemoryCollection.Count() == 0;
        }

        public void Load()
        {
            if (File.Exists(filePath) == false)
            {
                File.Create(filePath);
            }
            var data = JsonConvert.SerializeObject(MemoryCollection);
            File.WriteAllText(filePath, data);
        }
    }
}
