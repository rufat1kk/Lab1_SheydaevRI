using System;

namespace ConverterLab
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ТЕСТИРОВАНИЕ КЛАССА CONVERTER ===\n");

            TestConverter(0f, "0");
            TestConverter(65.7f, "65.7 (положительное)");
            TestConverter(3.14f, "3.14 (положительное)");
            TestConverter(-94.5f, "-94.5 (отрицательное)");
            TestConverter(-10.3f, "-10.3 (отрицательное)");
            TestConverter(-5f, "-5 (особый случай)");

            Console.WriteLine("\n=== ТЕСТ ОТРИЦАТЕЛЬНОГО НУЛЯ ===");
            try
            {
                float negativeZero = BitConverter.ToSingle(new byte[] { 0, 0, 0, 0x80 }, 0);
                int result = Converter.Do(negativeZero);
                Console.WriteLine($"Результат: {result}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Исключение: {ex.Message}");
            }

            Console.WriteLine("\nНажмите любую клавишу для запуска тестов...");
            Console.ReadKey();
        }

        static void TestConverter(float value, string description)
        {
            try
            {
                int result = Converter.Do(value);
                Console.WriteLine($"Converter.Do({value}) = {result} \t ({description})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Converter.Do({value}) -> Исключение: {ex.Message} \t ({description})");
            }
        }
    }
}