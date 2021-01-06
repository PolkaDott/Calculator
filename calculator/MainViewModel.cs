using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using calculator.HistoryMemory;

namespace calculator
{
    class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel() {  }
        Expression expression = new Expression();
        public IHistory History { get; } = new HistoryRAM();
        public IMemory Memory { get; } = new MemoryRAM();
        public bool HasError { get; set; } = false;
        public string ExpressionField
        {
            get => expression.Text;
            set => expression.Text = value;
        }

        private ICommand addCharBind;
        public ICommand AddCharBind
        {
            get => addCharBind ?? new RelayCommand<string>(
            (x) => 
            {
                expression.Push(x);
                OnPropertyChanged(nameof(ExpressionField));
            },
            (x) => true);
        }

        private ICommand removeCharBind;
        public ICommand RemoveCharBind
        {
            get => removeCharBind ?? new RelayCommand(
                () => {
                    if (expression.Text.Length == 1)
                        expression.Null();
                    else
                        expression.RemoveLastChar();
                    OnPropertyChanged(nameof(ExpressionField));
                },
                () => true);
        }

        private ICommand changePanelMemory;
        public ICommand ChangePanelMemory
        {
            get => changePanelMemory ?? new RelayCommand<System.Windows.Controls.TabControl>(
                (x) => {
                    if (x.Visibility == Visibility.Visible)
                        x.Visibility = Visibility.Collapsed;
                    else
                        x.Visibility = Visibility.Visible;
                }, (x) => true);
        }

        private ICommand closeApp;
        public ICommand CloseApp
        {
            get => closeApp ?? new RelayCommand<Window>((x) => { x.Close(); }, (x) => true);
        }

        private ICommand computeBind;
        public ICommand ComputeBind
        {
            get => computeBind ?? new RelayCommand(
                () => 
                { 
                    Computer.ComputeAndOut(expression);
                    if (!expression.HasError && expression.Formula != expression.Answer)
                        History.Add(expression);
                    OnPropertyChanged(nameof(ExpressionField));
                }, () => !HasError);
        }

        private ICommand clearHistory;
        public ICommand ClearHistory
        {
            get => clearHistory ?? new RelayCommand(
                () =>
                {
                    History.Clear();
                    OnPropertyChanged(nameof(ExpressionField));
                },
                () => true);
        }

        private ICommand clearMemory;
        public ICommand ClearMemory
        {
            get => clearMemory ?? new RelayCommand(
                () =>
                {
                    Memory.Clear();
                    OnPropertyChanged(nameof(ExpressionField));
                },
                () => true);
        }

        private ICommand clearBind;
        public ICommand ClearBind
        {
            get => clearBind ?? new RelayCommand(
                () => 
                {
                    expression.Null();
                    OnPropertyChanged(nameof(ExpressionField));
                },
                () => true);
        }

        private ICommand actMemoryCellBind;
        public ICommand ActMemoryCellBind
        {
            get => actMemoryCellBind ?? new RelayCommand<string>(
                (x) =>
                {
                    Computer.ComputeAndOut(expression);
                    if (expression.MustBeCleared)
                        return;
                    else if (x == "MS")
                    {
                        double result;
                        if (double.TryParse(expression.Text, out result))
                        {
                            if (Memory.IsEmpty() || result != Memory.MemoryCollection[0])
                                Memory.Add(result);
                        }
                    }
                    else if (Memory.IsEmpty())
                        return;
                    else if (x == "MC")
                        Memory.Delete();
                    else if (x == "M+")
                    {
                        double result;
                        if (double.TryParse(expression.Text, out result))
                            Memory.Increase(result);
                    }
                    else if (x == "M-")
                    {
                        double result;
                        if (double.TryParse(expression.Text, out result))
                            Memory.Decrease(result);
                    }
                    else
                        throw new Exception("What's that? It's wrong in ActMemoryCellBind()!");
                    OnPropertyChanged(nameof(ExpressionField));
                },
                (x) => !HasError);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            HasError = !Validator.Validate(expression.Text);
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}