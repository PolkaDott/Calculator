using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calculator
{
    static public class Computer
    {
        public static string Compute (string expression)
        {
            if (expression.Length == 0)
                return null;
            List<string> elements = ParseExpression(expression);
            ComputeList(elements);
            return elements[0];
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
                else if (tempChar == '+' || tempChar == '-' || tempChar == '*' || tempChar == '÷')
                {
                    elements.Add(tempElement);
                    elements.Add(tempChar.ToString());
                    tempElement = "";
                }
            }
            return elements;
        }

        private static void ComputeList(List<string> elements)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                double result;
                if (elements[i] == "*")
                    result = Convert.ToDouble(elements[i - 1]) * Convert.ToDouble(elements[i + 1]);
                else if (elements[i] == "÷")
                    result = Convert.ToDouble(elements[i - 1]) / Convert.ToDouble(elements[i + 1]);
                else if (elements[i] == "+")
                    result = Convert.ToDouble(elements[i - 1]) + Convert.ToDouble(elements[i + 1]);
                else if (elements[i] == "-")
                    result = Convert.ToDouble(elements[i - 1]) - Convert.ToDouble(elements[i + 1]);
                else continue;
                elements[i] = result.ToString();
                elements.RemoveAt(i + 1);
                elements.RemoveAt(i - 1);
                i = 0;
            }
        }
    }
}
