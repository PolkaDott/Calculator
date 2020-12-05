using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calculator
{
    class MemoryCell
    {
        private double value = 0;
        public void Zero()
        {
            value = 0;
        }
        public string Add(string addition)
        {
            return (value += Convert.ToDouble(addition)).ToString();
        }
        public string Sub(string subtrahend)
        {
            return (value -= Convert.ToDouble(subtrahend)).ToString();
        }
        public string Get()
        {
            return value.ToString();
        }
    }
}
