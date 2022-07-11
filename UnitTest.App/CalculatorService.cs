using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.App
{
    public class CalculatorService : ICalculatorService
    {
        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Multip(int a, int b)
        {
            if (a==0)
            {
                throw new Exception("a=0 olamaz");
            }
            return a * b;
        }
    }
}
