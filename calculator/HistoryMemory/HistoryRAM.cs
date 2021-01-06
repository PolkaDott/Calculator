using System.Collections.ObjectModel;

namespace calculator.HistoryMemory
{
    class HistoryRAM : IHistory
    {
        public ObservableCollection<Expression> HistoryCollection { get; } = new ObservableCollection<Expression>();
        public HistoryRAM() { }

        public void Add(Expression expression)
        {
            HistoryCollection.Insert(0, expression);
        }

        public void Clear()
        {
            HistoryCollection.Clear();
        }
    }
}
