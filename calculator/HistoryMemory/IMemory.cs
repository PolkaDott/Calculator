using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace calculator.HistoryMemory
{
    public interface IMemory
    {
        ObservableCollection<double> MemoryCollection { get; }
        void Add(double value);
        void Delete();
        void Increase(double value);
        void Decrease(double value);
        void Clear();
        bool IsEmpty();
    }
}
