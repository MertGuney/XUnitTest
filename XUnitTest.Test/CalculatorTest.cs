using Moq;
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
        public Mock<ICalculatorService> mymock { get; set; }
        public CalculatorTest()
        {
            mymock = new Mock<ICalculatorService>();
            this.calculator = new Calculator(mymock.Object);
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
        public void Add_SimpleValues_ReturnsTotalValue(int a, int b, int ExpectedTotal)
        {
            mymock.Setup(x => x.Add(a, b)).Returns(ExpectedTotal);

            var actualData = calculator.Add(a, b);

            Assert.Equal(ExpectedTotal, actualData);
            mymock.Verify(x => x.Add(a, b), Times.Once);
        }

        [Theory]
        [InlineData(0, 5, 0)]
        [InlineData(10, 0, 0)]
        public void Add_ZeroValues_ReturnsZeroValue(int a, int b, int ExpectedTotal)
        {
            var actualData = calculator.Add(a, b);
            Assert.Equal(ExpectedTotal, actualData);
        }

        [Theory]
        [InlineData(8, 8, 64)]
        public void Multip_SimpleValues_ReturnsMultipValue(int a, int b, int expectedValue)
        {
            int actualMultip = 0;
            mymock.Setup(x => x.Multip(It.IsAny<int>(), It.IsAny<int>())).Callback<int, int>((x, y) => actualMultip = x * y);
            calculator.Multip(a, b);

            Assert.Equal(expectedValue, calculator.Multip(a, b));
            calculator.Multip(5, 5);
            Assert.Equal(25, actualMultip);
        }

        [Theory]
        [InlineData(0, 5)]
        public void Multip_ZeroValues_ReturnsException(int a, int b)
        {
            mymock.Setup(x => x.Multip(a, b)).Throws(new Exception("a=0 olamaz"));
            Exception exception = Assert.Throws<Exception>(() => calculator.Multip(a, b));
            Assert.Equal("a=0 olamaz", exception.Message);
        }
    }
}
