using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TriangleLab
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string template = "{Timestamp:HH:mm:ss} | [{Level:u3}] | {Message:lj}{NewLine}{Exception}";
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate: template)
                .WriteTo.File("logs/log_.txt", outputTemplate: template, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("Программа запущена");
            Log.Information("Введите стороны треугольника (через Enter)");
            Log.Information("Для выхода введите 'exit'");

            while (true)
            {
                Console.WriteLine("\n=== Определение типа треугольника ===");
                Console.Write("Введите сторону A: ");
                string inputA = Console.ReadLine();
                if (inputA.ToLower() == "exit") break;

                Console.Write("Введите сторону B: ");
                string inputB = Console.ReadLine();
                if (inputB.ToLower() == "exit") break;

                Console.Write("Введите сторону C: ");
                string inputC = Console.ReadLine();
                if (inputC.ToLower() == "exit") break;

               
                var result = TriangleCalculator.ProcessTriangle(inputA, inputB, inputC);

                Console.WriteLine($"\nРезультат: {result.TriangleType}");
                Console.WriteLine($"Координаты вершин:");
                Console.WriteLine($"A: ({result.Coordinates[0].Item1}, {result.Coordinates[0].Item2})");
                Console.WriteLine($"B: ({result.Coordinates[1].Item1}, {result.Coordinates[1].Item2})");
                Console.WriteLine($"C: ({result.Coordinates[2].Item1}, {result.Coordinates[2].Item2})");
            }

            Log.Information("Программа завершена");
            Log.CloseAndFlush();
        }
    }

    public class TriangleResult
    {
        public string TriangleType { get; set; }
        public List<(int, int)> Coordinates { get; set; }
    }

    public static class TriangleCalculator
    {
        public static TriangleResult ProcessTriangle(string sideAStr, string sideBStr, string sideCStr)
        {
            Log.Debug($"Обработка запроса: A={sideAStr}, B={sideBStr}, C={sideCStr}");

            
            if (!float.TryParse(sideAStr, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out float a) ||
                !float.TryParse(sideBStr, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out float b) ||
                !float.TryParse(sideCStr, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out float c))
            {
                Log.Warning($"Нечисловые входные данные: A={sideAStr}, B={sideBStr}, C={sideCStr}");
                return new TriangleResult
                {
                    TriangleType = "",
                    Coordinates = new List<(int, int)> { (-2, -2), (-2, -2), (-2, -2) }
                };
            }

           
            if (a <= 0 || b <= 0 || c <= 0)
            {
                Log.Warning($"Отрицательные или нулевые стороны: A={a}, B={b}, C={c}");
                return new TriangleResult
                {
                    TriangleType = "не треугольник",
                    Coordinates = new List<(int, int)> { (-1, -1), (-1, -1), (-1, -1) }
                };
            }

            
            if (a + b <= c || a + c <= b || b + c <= a)
            {
                Log.Warning($"Стороны не образуют треугольник: A={a}, B={b}, C={c}");
                return new TriangleResult
                {
                    TriangleType = "не треугольник",
                    Coordinates = new List<(int, int)> { (-1, -1), (-1, -1), (-1, -1) }
                };
            }

            
            string triangleType;
            const float epsilon = 0.0001f;

            if (Math.Abs(a - b) < epsilon && Math.Abs(b - c) < epsilon)
                triangleType = "равносторонний";
            else if (Math.Abs(a - b) < epsilon || Math.Abs(a - c) < epsilon || Math.Abs(b - c) < epsilon)
                triangleType = "равнобедренный";
            else
                triangleType = "разносторонний";

            
            var coordinates = CalculateCoordinates(a, b, c);

            Log.Information($"УСПЕШНЫЙ ЗАПРОС: {triangleType} треугольник | Стороны: {a:F2}, {b:F2}, {c:F2} | Координаты: A({coordinates[0].Item1},{coordinates[0].Item2}) B({coordinates[1].Item1},{coordinates[1].Item2}) C({coordinates[2].Item1},{coordinates[2].Item2})");

            return new TriangleResult
            {
                TriangleType = triangleType,
                Coordinates = coordinates
            };
        }

        private static List<(int, int)> CalculateCoordinates(float a, float b, float c)
        {
            

            float Ax = 0, Ay = 0;  
            float Bx = c, By = 0;  

            
            float Cx = (b * b - a * a + c * c) / (2 * c);
            float Cy = (float)Math.Sqrt(b * b - Cx * Cx);

            
            float minX = Math.Min(0, Math.Min(c, Cx));
            float maxX = Math.Max(0, Math.Max(c, Cx));
            float minY = Math.Min(0, Math.Min(0, Cy));
            float maxY = Math.Max(0, Math.Max(0, Cy));

            float scaleX = 90f / (maxX - minX);  
            float scaleY = 90f / (maxY - minY);
            float scale = Math.Min(scaleX, scaleY);

            
            float offsetX = (100f - scale * (maxX - minX)) / 2f - minX * scale;
            float offsetY = (100f - scale * (maxY - minY)) / 2f - minY * scale;

            
            int AxPx = (int)Math.Round(Ax * scale + offsetX);
            int AyPx = (int)Math.Round(Ay * scale + offsetY);
            int BxPx = (int)Math.Round(Bx * scale + offsetX);
            int ByPx = (int)Math.Round(By * scale + offsetY);
            int CxPx = (int)Math.Round(Cx * scale + offsetX);
            int CyPx = (int)Math.Round(Cy * scale + offsetY);

            
            AxPx = Math.Clamp(AxPx, 0, 100);
            AyPx = Math.Clamp(AyPx, 0, 100);
            BxPx = Math.Clamp(BxPx, 0, 100);
            ByPx = Math.Clamp(ByPx, 0, 100);
            CxPx = Math.Clamp(CxPx, 0, 100);
            CyPx = Math.Clamp(CyPx, 0, 100);

            return new List<(int, int)> { (AxPx, AyPx), (BxPx, ByPx), (CxPx, CyPx) };
        }
    }
}