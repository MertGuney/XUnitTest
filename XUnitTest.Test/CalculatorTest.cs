using System;
using System.Collections.Generic;
using System.Text;
using UnitTest.App;
using Xunit;

namespace XUnitTest.Test
{
    public class CalculatorTest
    {
        [Fact]
        public void AddTest()
        {
            //Bir test oluşturma 3 aşamadan oluşur
            //Arrange (değişkenlerimizi initialize ettiğimiz yerdir)
            int a = 5;
            int b = 20;
            var calculator = new Calculator();
            //Act (parametreler verip test edip çalıştırdığımız yerdir)
            var total = calculator.Add(a, b);
            //Assert (doğrulama evresidir act evresinden çıkan sonucun doğruluğunu test ederiz)
            Assert.Equal(25, total);
        }
    }
}
