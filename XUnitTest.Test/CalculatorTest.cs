using System;
using System.Collections.Generic;
using System.Text;
using UnitTest.App;
using Xunit;

namespace XUnitTest.Test
{
    public class CalculatorTest
    {
        public Calculator calculator { get; set; }
        public CalculatorTest()
        {
            this.calculator = new Calculator();
        }

        #region Old
        //[Fact]//test metodu olucağını belirtir ve parametre almayan bir metot olduğu belirtilir.
        //public void AddTest()
        //{
        //    //Bir test oluşturma 3 aşamadan oluşur
        //    //Arrange (değişkenlerimizi initialize ettiğimiz yerdir)
        //    int a = 5;
        //    int b = 20;

        //    //Act (parametreler verip test edip çalıştırdığımız yerdir)
        //    var total = calculator.Add(a, b);
        //    //Assert (doğrulama evresidir act evresinden çıkan sonucun doğruluğunu test ederiz)
        //    Assert.Equal(25, total);

        //    #region AssertMethodlar
        //    //Contains - DoesNotContain (Beklenen değer gerçek değerin içerisinde geçiyorsa test metodu başarılı olur (contains) beklenen değer gerçek değerin içerisinde geçmiyorsa test netodu başarılı olur (doesnotcontain))
        //    //var names = new List<string>() { "mert", "elif", "melek" };
        //    //Assert.Contains(names, x => x == "mert");
        //    //Assert.Contains("mert", "mert guney");
        //    //Assert.DoesNotContain("elif", "mert guney");

        //    //True - False
        //    //Assert.True(5 > 2);
        //    //Assert.False("".GetType() != typeof(string));

        //    //Matches - DoesNotMatch (regex code ifadesini kontrol eder)
        //    //Assert.Matches("^The", "the dog");
        //    //Assert.DoesNotMatch("^The", "dog the");
        //    #endregion
        //}
        #endregion

        [Theory]
        [InlineData(5, 5, 10)]
        [InlineData(10, 10, 20)]
        public void Add_SimpleValues_ReturnTotalValue(int a, int b, int ExpectedTotal)
        {
            var actualData = calculator.Add(a, b);

            Assert.Equal(ExpectedTotal, actualData);
        }
        [Theory]
        [InlineData(0, 5, 0)]
        [InlineData(10, 0, 0)]
        public void Add_ZeroValues_ReturnZeroValue(int a, int b, int ExpectedTotal)
        {
            var actualData = calculator.Add(a, b);

            Assert.Equal(ExpectedTotal, actualData);
        }
    }
}
