using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace calculator
{
    public class Expression
    {
        public bool MustBeCleared { get; set; }
        public bool HasError { get; set; }
        private string text = "0";
        public string Text { get => text; set => text = value; }
        public string Formula { get; set; }
        public string Answer { get; set; }
        public string EntireExpression { get => Formula + " = " + Answer;  }
        public Expression() { }
        public Expression(Expression exp)
        {
            MustBeCleared = exp.MustBeCleared;
            HasError = exp.HasError;
            text = exp.text;
            Formula = exp.Formula;
            Answer = exp.Answer;
        }
        public void Push(string x)
        {
            if (MustBeCleared)
            {
                MustBeCleared = false;
                text = "0";
            }
            else
            {

                Add(x);
            }
        }
        public void Null()
        {
            text = "0";
        }
        public void RemoveLastChar()
        {
            if (text.Length > 0)
                text = text.Substring(0, text.Length - 1);
        }

        private bool Add(string x)
        {
            if (x == "()")
            {
                
                char last = text.Length > 0 ? text.Last() : '1';
                if (text.Length == 0 || last == '+' || last == '-' || last == '*' || last == '÷' || last == '(' || text == "0")
                {   // x = "("
                    if (text == "0")
                        text = "(";
                    else
                        text = text + "(";
                }
                else if (Char.IsDigit(last) || last == ',' || last == ')')
                {   // x = ")"
                    int countBraces1 = new Regex(@"\(").Matches(text).Count,
                        countBraces2 = new Regex(@"\)").Matches(text).Count;
                    if (countBraces1 == countBraces2)
                        return false;
                    if (last == ',')
                        RemoveLastChar();
                    text = text + ")";
                }
                else return false;
                return true;
            }
            if (x == "0")
            {
                if (text.Length == 0)
                {
                    text = text + x;
                    return true;
                }
                if (text.Last() == '0')
                {
                    for (int i = text.Length - 1; i != 0; i--)
                    {
                        char temp = text[i];
                        if (temp == ',' || Char.IsDigit(temp) && temp != '0')
                        {
                            text = text + x;
                            return true;
                        }
                        else if (temp == '*' || temp == '+' || temp == '-' || temp == '÷' || temp == '(')
                            return false;
                    }
                    return false;
                }
            }
            if (Char.IsDigit(x[0]))
            {
                if (text.Length != 0 && text.Last() == '0')
                {
                    for (int i = text.Length - 1; i != 0; i--)
                    {
                        char temp = text[i];
                        if (temp == ',' || Char.IsDigit(temp) && temp != '0')
                        {
                            text = text + x;
                            return true;
                        }
                        else if (temp == '*' || temp == '+' || temp == '-' || temp == '÷')
                            break;
                    }
                    RemoveLastChar();
                }
                if (text.Length != 0 && text.Last() == ')')
                    return false;
                text = text + x;
                return true;
            }
            if (x == ",")
            {
                if (text.Last() == ')')
                    return false;
                if (text.Length == 0 || !Char.IsDigit(text.Last()) && text.Last() != ',')
                {
                    text += "0" + x;
                    return true;
                }
                for (int i = text.Length - 1; i != 0 && (text[i] == ',' || Char.IsDigit(text[i])); i--)
                    if (text[i] == ',')
                        return false;
                text = text + x;
                return true;
            }
            if (x == "-" && text.Length == 0)
            {
                text = text + x;
                return true;
            }
            if (text.Length == 0)
                return false;
            if (x == "*" || x == "÷" || x == "-" || x == "+")
            {
                if (text.Last() == '(')
                    return false;
                string lastChar = text.Last().ToString();
                if (lastChar == "*" || lastChar == "÷" || lastChar == "-" || lastChar == "+")
                    RemoveLastChar();
                else if (lastChar == ",")
                    RemoveLastChar();
                text = text + x;
                return true;
            }
            return false;
        }
    }
}
