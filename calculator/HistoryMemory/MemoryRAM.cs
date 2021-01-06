using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calculator.HistoryMemory
{
    class MemoryRAM
    {
        public ObservableCollection<double> MemoryCollection { get; } = new ObservableCollection<double>();
        public MemoryRAM() { }

        public void Add(double value)
        {
            MemoryCollection.Insert(0, value);
        }

        public void Delete(int index)
        {
            MemoryCollection.RemoveAt(index);
        }

        public void Increase(double value, int index)
        {
            MemoryCollection[index] += value;
        }

        public void Decrease(double value, int index)
        {
            MemoryCollection[index] -= value;
        }

        public void Clear()
        {
            MemoryCollection.Clear();
        }
    }
}
