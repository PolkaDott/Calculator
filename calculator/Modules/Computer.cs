using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calculator
{
    static public class Computer
    {
        public static void ComputeAndOut(Expression expression)
        {
            string result = Compute(expression.Text);
            expression.HasError = false;
            if (result == null || result.Length == 0)
            {
                expression.Text = "Ошибка!";
                expression.HasError = true;
                expression.MustBeCleared = true;
                return;
            }
            else if (!(Char.IsDigit(result[0]) || result[0] == '-' && Char.IsDigit(result[1]))) // ответ - не число
            {
                expression.MustBeCleared = true;
                expression.Formula = expression.Text;
            }
            else
                expression.Formula = expression.Text;
            expression.Answer = result;
            expression.Text = result;
        }

        public static string Compute (string expression)
        {
            if (expression.Length == 0)
                return null;
            List<string> elements = ParseExpression(expression);
            if (elements == null)
                return null;
            string result = ComputeList(elements);
            return result;
        }

        private static List<string> ParseExpression(string expression)
        {
            List<string> elements = new List<string>();
            string tempElement = "";
            for (int i = 0; i < expression.Length; i++)
            {
                char tempChar = expression[i];
                if (Char.IsDigit(tempChar) || tempChar == ',' || i == 0 && tempChar == '-')
                {
                    tempElement += tempChar;
                    if (i == expression.Length - 1)
                    {
                        elements.Add(tempElement);
                    }
                }
                else if (tempChar == '+' || tempChar == '-' || tempChar == '*' || tempChar == '÷' || tempChar == '(' || tempChar == ')')
                {
                    if (tempElement != "")
                        elements.Add(tempElement);
                    elements.Add(tempChar.ToString());
                    tempElement = "";
                }
                else return null;
            }
            return elements;
        }

        private static string ComputeList(List<string> elements)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                string element = elements[i];
                if (element == "(")
                {
                    int closingBrace = elements.LastIndexOf(")");
                    if (closingBrace == -1)
                        throw new Exception();
                    List<string> subListElements = elements.GetRange(i + 1, closingBrace - i - 1);
                    string subResult = ComputeList(subListElements);
                    elements[i] = subResult;
                    elements.RemoveRange(i + 1, closingBrace-i);
                    i = 0;
                }
            }
            for (int i = 0; i < elements.Count; i++)
            {
                double result = 0;
                string element = elements[i];
                if (element == "*")
                    result = Convert.ToDouble(elements[i - 1]) * Convert.ToDouble(elements[i + 1]);
                else if (element == "÷")
                    result = Convert.ToDouble(elements[i - 1]) / Convert.ToDouble(elements[i + 1]);
                else continue;
                elements[i] = result.ToString();
                elements.RemoveAt(i + 1);
                elements.RemoveAt(i - 1);
                i = 0;
            }
            for (int i = 0; i < elements.Count; i++)
            {
                double result = 0;
                string element = elements[i];
                if (element == "+")
                    result = Convert.ToDouble(elements[i - 1]) + Convert.ToDouble(elements[i + 1]);
                else if (element == "-")
                    result = Convert.ToDouble(elements[i - 1]) - Convert.ToDouble(elements[i + 1]);
                else continue;
                elements[i] = result.ToString();
                elements.RemoveAt(i + 1);
                elements.RemoveAt(i - 1);
                i = 0;
            }
            return elements[0];
        }
    }
}
