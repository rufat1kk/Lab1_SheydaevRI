using Laba7;
using NUnit.Framework;

namespace Laba7.Tests
{
    [TestFixture]
    public class TriangleCalculatorTests
    {
        [Test]
        public void Test01_ValidIntegerInputs_ReturnsScalene()
        {
            var result = TriangleCalculator.ProcessTriangle("3", "4", "5");
            Assert.That(result.TriangleType, Is.EqualTo("разносторонний"));
        }

        [Test]
        public void Test02_Equilateral_ReturnsEquilateral()
        {
            var result = TriangleCalculator.ProcessTriangle("5", "5", "5");
            Assert.That(result.TriangleType, Is.EqualTo("равносторонний"));
        }

        [Test]
        public void Test03_Isosceles_ReturnsIsosceles()
        {
            var result = TriangleCalculator.ProcessTriangle("5", "5", "3");
            Assert.That(result.TriangleType, Is.EqualTo("равнобедренный"));
        }

        [Test]
        public void Test04_NegativeSide_ReturnsNotTriangle()
        {
            var result = TriangleCalculator.ProcessTriangle("-3", "4", "5");
            Assert.That(result.TriangleType, Is.EqualTo("не треугольник"));
        }

        [Test]
        public void Test05_ZeroSide_ReturnsNotTriangle()
        {
            var result = TriangleCalculator.ProcessTriangle("0", "4", "5");
            Assert.That(result.TriangleType, Is.EqualTo("не треугольник"));
        }

        [Test]
        public void Test06_InvalidTriangleInequality_ReturnsNotTriangle()
        {
            var result = TriangleCalculator.ProcessTriangle("1", "1", "3");
            Assert.That(result.TriangleType, Is.EqualTo("не треугольник"));
        }

        [Test]
        public void Test07_NumbersWithDot_ReturnsCorrect()
        {
            var result = TriangleCalculator.ProcessTriangle("3.5", "4.5", "5.5");
            Assert.That(result.TriangleType, Is.EqualTo("разносторонний"));
        }

        [Test]
        public void Test08_NonNumeric_ReturnsEmpty()
        {
            var result = TriangleCalculator.ProcessTriangle("abc", "4", "5");
            Assert.That(result.TriangleType, Is.EqualTo(""));
        }

        [Test]
        public void Test09_EmptyString_ReturnsEmpty()
        {
            var result = TriangleCalculator.ProcessTriangle("", "4", "5");
            Assert.That(result.TriangleType, Is.EqualTo(""));
        }

        [Test]
        public void Test10_CoordinatesInRange()
        {
            var result = TriangleCalculator.ProcessTriangle("3", "4", "5");
            foreach (var coord in result.Coordinates)
            {
                Assert.That(coord.Item1, Is.InRange(0, 100));
                Assert.That(coord.Item2, Is.InRange(0, 100));
            }
        }

        [Test]
        public void Test11_InvalidTriangleReturnsMinusOne()
        {
            var result = TriangleCalculator.ProcessTriangle("1", "1", "3");
            foreach (var coord in result.Coordinates)
            {
                Assert.That(coord, Is.EqualTo((-1, -1)));
            }
        }

        [Test]
        public void Test12_LargeTriangle_CoordinatesInRange()
        {
            var result = TriangleCalculator.ProcessTriangle("100", "100", "150");
            foreach (var coord in result.Coordinates)
            {
                Assert.That(coord.Item1, Is.InRange(0, 100));
                Assert.That(coord.Item2, Is.InRange(0, 100));
            }
        }
    }
}
