using System.Collections.ObjectModel;

namespace calculator.HistoryMemory
{
    class HistoryJSON : IHistory
    {
        public ObservableCollection<Expression> HistoryCollection { get; } = new ObservableCollection<Expression>();
        public HistoryJSON() { }

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
