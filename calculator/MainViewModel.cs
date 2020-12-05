using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace calculator
{
    class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel() { }
        MemoryCell memoryCell = new MemoryCell();
        Expression expression = new Expression();
        public string ExpressionField
        {
            get => expression.Text; // попробовать без свойства тоже надо
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
                this.Compute, () => true);
        }

        private void Compute()
        {
            string result = Computer.Compute(expression.Text);
            if (result == null || result.Length == 0)
            {
                expression.Text = "Ошибка!";
                expression.MustBeCleared = true;
            }
            else if (!(Char.IsDigit(result[0]) || result[0] == '-' && Char.IsDigit(result[1])))
            {
                expression.Text = result;
                expression.MustBeCleared = true;
            }
            else expression.Text = result;
            OnPropertyChanged(nameof(ExpressionField));
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
                    Compute();
                    if (expression.MustBeCleared)
                        return;
                    else if (x == "MR")
                        expression.Text = memoryCell.Get();
                    else if (x == "MC")
                        memoryCell.Zero();
                    else if (x == "M+")
                        expression.Text = memoryCell.Add(expression.Text);
                    else if (x == "M-")
                        expression.Text = memoryCell.Sub(expression.Text);
                    else
                        throw new Exception("What's that? It's wrong in ActMemoryCellBind()!");
                    OnPropertyChanged(nameof(ExpressionField));
                },
                (x) => true);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}