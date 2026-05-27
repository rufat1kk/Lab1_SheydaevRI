using Xunit;
using System;

namespace ConverterLab
{
    public class ConverterTests
    {
        // Тест 1: x = 0 возвращает 1000
        [Fact]
        public void Do_ZeroInput_Returns1000()
        {
            int result = Converter.Do(0f);
            Assert.Equal(1000, result);
        }

        // Тест 2: положительное число возвращает целую часть
        [Fact]
        public void Do_PositiveNumber_ReturnsIntegerPart()
        {
            // 65.7 -> целая часть 65
            int result1 = Converter.Do(65.7f);
            Assert.Equal(65, result1);

            // 3.14 -> целая часть 3
            int result2 = Converter.Do(3.14f);
            Assert.Equal(3, result2);

            // 123.9 -> целая часть 123
            int result3 = Converter.Do(123.9f);
            Assert.Equal(123, result3);
        }

        // Тест 3: отрицательный ноль вызывает исключение
        [Fact]
        public void Do_NegativeZero_ThrowsArgumentException()
        {
            // В C# нельзя напрямую создать -0.0f из литерала, нужно через биты
            float negativeZero = BitConverter.ToSingle(new byte[] { 0, 0, 0, 0x80 }, 0);

            Assert.Throws<ArgumentException>(() => Converter.Do(negativeZero));
        }

        // Тест 4: отрицательное число возвращает целую часть минус 5
        [Fact]
        public void Do_NegativeNumber_ReturnsIntegerPartMinus5()
        {
            // -94.5 -> целая часть -94, минус 5 = -99
            int result1 = Converter.Do(-94.5f);
            Assert.Equal(-99, result1);

            // -10.3 -> целая часть -10, минус 5 = -15
            int result2 = Converter.Do(-10.3f);
            Assert.Equal(-15, result2);

            // -1.1 -> целая часть -1, минус 5 = -6
            int result3 = Converter.Do(-1.1f);
            Assert.Equal(-6, result3);
        }

        // Тест 5: x = -5 возвращает -2000
        [Fact]
        public void Do_Minus5_ReturnsMinus2000()
        {
            int result = Converter.Do(-5f);
            Assert.Equal(-2000, result);
        }

        // Дополнительные тесты для граничных случаев

        [Fact]
        public void Do_SmallPositive_ReturnsZero()
        {
            // 0.9 -> целая часть 0
            int result = Converter.Do(0.9f);
            Assert.Equal(0, result);
        }

        [Fact]
        public void Do_LargePositive_ReturnsIntegerPart()
        {
            int result = Converter.Do(9999.9f);
            Assert.Equal(9999, result);
        }

        [Fact]
        public void Do_NegativeWithoutFraction_ReturnsValueMinus5()
        {
            // -10.0 -> целая часть -10, минус 5 = -15
            int result = Converter.Do(-10.0f);
            Assert.Equal(-15, result);
        }

        [Fact]
        public void Do_MaxNegative_WorksCorrectly()
        {
            int result = Converter.Do(-1000.5f);
            Assert.Equal(-1005, result); // -1000 - 5 = -1005
        }
    }
}