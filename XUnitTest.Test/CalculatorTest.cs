using System;
using System.Collections.Generic;
using System.Text;
using UnitTest.App;
using Xunit;

namespace XUnitTest.Test
{
    public class CalculatorTest
    {
        [Fact]//test metodu olucağını belirtir ve parametre almayan bir metot olduğu belirtilir.
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
        [Theory]
        [InlineData(5, 5, 10)]
        [InlineData(10, 10, 20)]
        public void AddTestParams(int a, int b, int ExpectedTotal)
        {
            var calculator = new Calculator();

            var actualData = calculator.Add(a, b);

            Assert.Equal(ExpectedTotal, actualData);
        }
    }
}
