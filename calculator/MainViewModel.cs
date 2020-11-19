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
        private string expression = "";
        bool mustBeCleared = false;
        public string Expression
        {
            get => expression;
            set
            {
                if (mustBeCleared)
                {
                    mustBeCleared = false;
                    expression = "";
                }
                else 
                {
                    expression = value;
                }
                    OnPropertyChanged(nameof(Expression));
            }
        }
        
        private ICommand addChar;
        public ICommand AddChar {
            get => addChar ?? new RelayCommand<string>(
            (x) => Expression = CanAddExpression(x) == true ? Expression + x : Expression,
            (x) => true);
        }

        private ICommand removeChar;
        public ICommand RemoveChar
        {
            get => removeChar ?? new RelayCommand(
                () => Expression = Expression.Substring(0, Expression.Length-1),
                () => Expression.Length != 0);
        }

        private ICommand compute;
        public ICommand Compute
        {
            get => compute ?? new RelayCommand(
                () => 
                { 
                    string result = Computer.Compute(Expression);
                    if (result == null || result.Length == 0)
                    {
                        Expression = "Ошибка!";
                        mustBeCleared = true;
                    }
                    else if (!(Char.IsDigit(result[0]) || result[0] == '-' && Char.IsDigit(result[1])))
                    {
                        Expression = result;
                        mustBeCleared = true;
                    }
                    else Expression = result;
                },
                () => true);
        }

        private ICommand clear;
        public ICommand Clear
        {
            get => clear ?? new RelayCommand(
                () => Expression = "",
                () => true);
        }

        private bool CanAddExpression(string x)
        {
            if (x.Length != 1)
                return false;
            if (x == "0")
            {
                if (Expression.Length == 0)
                    return true;
                if (Expression.Last() == '0')
                {
                    for (int i = Expression.Length - 1; i != 0; i--)
                    {
                        char temp = Expression[i];
                        if (temp == ',' || Char.IsDigit(temp) && temp != '0')
                            return true;
                        else if (temp == '*' || temp == '+' || temp == '-' || temp == '÷')
                            return false;
                    }
                    return false;
                }
            }
            if (Char.IsDigit(x[0]))
            {
                if (Expression.Length != 0 && Expression.Last() == '0')
                {
                    for (int i = Expression.Length - 1; i != 0; i--)
                    {
                        char temp = Expression[i];
                        if (temp == ',' || Char.IsDigit(temp) && temp != '0')
                            return true;
                        else if (temp == '*' || temp == '+' || temp == '-' || temp == '÷')
                            break;
                    }
                    Expression = Expression.Substring(0, Expression.Length - 1);
                }
                return true;
            }
            if (x == ",")
            {
                if (Expression.Length == 0 || !Char.IsDigit(Expression.Last()) && Expression.Last() != ',')
                {
                    Expression += "0";
                    return true;
                }
                for (int i = Expression.Length - 1; i != 0 && (Expression[i] == ',' || Char.IsDigit(Expression[i])); i--)
                    if (Expression[i] == ',')
                        return false;
                return true;
            }
            if (x == "-" && Expression.Length == 0)
                return true;
            if (Expression.Length == 0)
                return false;
            if (x == "*" || x == "÷" || x == "-" || x == "+")
            {
                string lastChar = Expression.Last().ToString();
                if (lastChar == "*" || lastChar == "÷" || lastChar == "-" || lastChar == "+")
                {
                    if (x == lastChar)
                        return false;
                    else
                    {
                        Expression =Expression.Substring(0, Expression.Length - 1);
                        return true;
                    }
                }
                if (lastChar == ",")
                    Expression = Expression.Substring(0, Expression.Length - 1);
                return true;
            }
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


        
    }
}