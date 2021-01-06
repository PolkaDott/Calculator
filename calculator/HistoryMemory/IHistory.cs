using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using calculator;

namespace calculator.HistoryMemory
{
    public interface IHistory
    {
        ObservableCollection<Expression> HistoryCollection { get; }
        void Add(Expression expression);
        void Clear();
    }
}
