using Laba7;
using NUnit.Framework;
using System.Collections.Generic;

namespace Laba7.Tests
{
    [TestFixture]
    public class TriangleCalculatorTests
    {
        [Test(Description = "Тест #1: Корректный парсинг целых чисел")]
        public void Test01_ParseValidIntegers_ReturnsCorrectTriangle()
        {
            string a = "3", b = "4", c = "5";
            var result = TriangleCalculator.ProcessTriangle(a, b, c);
            Assert.That(result.TriangleType, Is.EqualTo("разносторонний"));
        }

        [Test(Description = "Тест #2: Корректный парсинг чисел с плавающей точкой")]
        public void Test02_ParseValidFloats_ReturnsCorrectTriangle()
        {
            string a = "3.5", b = "4.5", c = "5.5";
            var result = TriangleCalculator.ProcessTriangle(a, b, c);
            Assert.That(result.TriangleType, Is.EqualTo("разносторонний"));
        }

        [Test(Description = "Тест #3: Обработка нечисловых данных (буквы)")]
        public void Test03_NonNumericInput_ReturnsErrorCode()
        {
            string a = "abc", b = "4", c = "5";
            var result = TriangleCalculator.ProcessTriangle(a, b, c);
            Assert.That(result.TriangleType, Is.EqualTo(""));
            Assert.That(result.Coordinates[0], Is.EqualTo((-2, -2)));
        }

        [Test(Description = "Тест #4: Обработка пустых строк")]
        public void Test04_EmptyStringInput_ReturnsErrorCode()
        {
            string a = "", b = "4", c = "5";
            var result = TriangleCalculator.ProcessTriangle(a, b, c);
            Assert.That(result.TriangleType, Is.EqualTo(""));
        }

        [Test(Description = "Тест #5: Обработка специальных символов")]
        public void Test05_SpecialCharactersInput_ReturnsErrorCode()
        {
            string a = "!@#", b = "4", c = "5";
            var result = TriangleCalculator.ProcessTriangle(a, b, c);
            Assert.That(result.TriangleType, Is.EqualTo(""));
        }

        [Test(Description = "Тест #6: Отрицательная сторона")]
        public void Test06_NegativeSide_ReturnsNotTriangle()
        {
            string a = "-3", b = "4", c = "5";
            var result = TriangleCalculator.ProcessTriangle(a, b, c);
            Assert.That(result.TriangleType, Is.EqualTo("не треугольник"));
            Assert.That(result.Coordinates[0], Is.EqualTo((-1, -1)));
        }

        [Test(Description = "Тест #7: Нулевая сторона")]
        public void Test07_ZeroSide_ReturnsNotTriangle()
        {
            string a = "0", b = "4", c = "5";
            var result = TriangleCalculator.ProcessTriangle(a, b, c);
            Assert.That(result.TriangleType, Is.EqualTo("не треугольник"));
        }

        [Test(Description = "Тест #8: Все стороны отрицательные")]
        public void Test08_AllSidesNegative_ReturnsNotTriangle()
        {
            string a = "-3", b = "-4", c = "-5";
            var result = TriangleCalculator.ProcessTriangle(a, b, c);
            Assert.That(result.TriangleType, Is.EqualTo("не треугольник"));
        }

        [Test(Description = "Тест #9: Нарушение неравенства треугольника")]
        public void Test09_ViolatesTriangleInequality_ReturnsNotTriangle()
        {
            string a = "1", b = "1", c = "3";
            var result = TriangleCalculator.ProcessTriangle(a, b, c);
            Assert.That(result.TriangleType, Is.EqualTo("не треугольник"));
        }

        [Test(Description = "Тест #10: Сумма двух сторон равна третьей")]
        public void Test10_SumEqualsThirdSide_ReturnsNotTriangle()
        {
            string a = "2", b = "3", c = "5";
            var result = TriangleCalculator.ProcessTriangle(a, b, c);
            Assert.That(result.TriangleType, Is.EqualTo("не треугольник"));
        }

        [Test(Description = "Тест #11: Равносторонний треугольник")]
        public void Test11_EquilateralSides_ReturnsEquilateral()
        {
            string a = "5", b = "5", c = "5";
            var result = TriangleCalculator.ProcessTriangle(a, b, c);
            Assert.That(result.TriangleType, Is.EqualTo("равносторонний"));
        }

        [Test(Description = "Тест #12: Равнобедренный треугольник")]
        public void Test12_IsoscelesSides_ReturnsIsosceles()
        {
            string a = "5", b = "5", c = "3";
            var result = TriangleCalculator.ProcessTriangle(a, b, c);
            Assert.That(result.TriangleType, Is.EqualTo("равнобедренный"));
        }

        [Test(Description = "Тест #13: Разносторонний треугольник")]
        public void Test13_ScaleneSides_ReturnsScalene()
        {
            string a = "3", b = "4", c = "5";
            var result = TriangleCalculator.ProcessTriangle(a, b, c);
            Assert.That(result.TriangleType, Is.EqualTo("разносторонний"));
        }

        [Test(Description = "Тест #14: Координаты в диапазоне [0,100]")]
        public void Test14_CoordinatesWithinBounds()
        {
            string a = "3", b = "4", c = "5";
            var result = TriangleCalculator.ProcessTriangle(a, b, c);
            foreach (var coord in result.Coordinates)
            {
                Assert.That(coord.Item1, Is.InRange(0, 100));
                Assert.That(coord.Item2, Is.InRange(0, 100));
            }
        }

        [Test(Description = "Тест #15: Невалидный треугольник -> (-1,-1)")]
        public void Test15_InvalidTriangle_ReturnsMinusOne()
        {
            string a = "1", b = "1", c = "3";
            var result = TriangleCalculator.ProcessTriangle(a, b, c);
            foreach (var coord in result.Coordinates)
            {
                Assert.That(coord, Is.EqualTo((-1, -1)));
            }
        }

        [Test(Description = "Тест #16: Большой треугольник")]
        public void Test16_LargeTriangle_CoordinatesWithinBounds()
        {
            string a = "100", b = "100", c = "150";
            var result = TriangleCalculator.ProcessTriangle(a, b, c);
            foreach (var coord in result.Coordinates)
            {
                Assert.That(coord.Item1, Is.InRange(0, 100));
                Assert.That(coord.Item2, Is.InRange(0, 100));
            }
        }

        [Test(Description = "Тест #17: Маленький треугольник")]
        public void Test17_TinyTriangle_CoordinatesWithinBounds()
        {
            string a = "0.1", b = "0.1", c = "0.1";
            var result = TriangleCalculator.ProcessTriangle(a, b, c);
            foreach (var coord in result.Coordinates)
            {
                Assert.That(coord.Item1, Is.InRange(0, 100));
                Assert.That(coord.Item2, Is.InRange(0, 100));
            }
        }

        [Test(Description = "Тест #18: Равнобедренный с целыми числами")]
        public void Test18_IsoscelesWithIntegers()
        {
            string a = "6", b = "6", c = "8";
            var result = TriangleCalculator.ProcessTriangle(a, b, c);
            Assert.That(result.TriangleType, Is.EqualTo("равнобедренный"));
        }

        [Test(Description = "Тест #19: Равносторонний с дробями")]
        public void Test19_EquilateralWithDecimals()
        {
            string a = "4.2", b = "4.2", c = "4.2";
            var result = TriangleCalculator.ProcessTriangle(a, b, c);
            Assert.That(result.TriangleType, Is.EqualTo("равносторонний"));
        }

        [Test(Description = "Тест #20: Проверка количества координат")]
        public void Test20_CoordinatesCount_ReturnsThree()
        {
            string a = "3", b = "4", c = "5";
            var result = TriangleCalculator.ProcessTriangle(a, b, c);
            Assert.That(result.Coordinates.Count, Is.EqualTo(3));
        }
    }
}