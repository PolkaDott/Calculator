using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calculator.HistoryMemory
{
    class MemoryRAM : IMemory
    {
        public ObservableCollection<double> MemoryCollection { get; } = new ObservableCollection<double>();
        public MemoryRAM() { }

        public void Add(double value)
        {
            MemoryCollection.Insert(0, value);
        }

        public void Delete()
        {
            if (MemoryCollection.Count() > 0)
                MemoryCollection.RemoveAt(0);
        }

        public void Increase(double value)
        {
            MemoryCollection[0] += value;
        }

        public void Decrease(double value)
        {
            MemoryCollection[0] -= value;
        }

        public void Clear()
        {
            MemoryCollection.Clear();
        }

        public bool IsEmpty()
        {
            return MemoryCollection.Count() == 0;
        }
    }
}
