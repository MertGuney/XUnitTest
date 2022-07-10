using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.App
{
    public class Calculator
    {
        public int Add(int a, int b)
        {
            if (a == 0 || b == 0)
            {
                return 0;
            }
            return a + b;
        }
    }
}
