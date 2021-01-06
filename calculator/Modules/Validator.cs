using System;

namespace calculator
{
    public static class Validator
    {
        public static bool Validate(string expression)
        {
            string result;
            try
            {
                result = Computer.Compute(expression);
            }
            catch (Exception)
            {
                return false;
            }
            if (result == null || result.Length == 0)
                return false;
            return true;
        }
    }
}
