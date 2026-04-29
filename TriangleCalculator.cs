using System;
using System.Collections.Generic;

namespace Laba7
{
    public class TriangleResult
    {
        public string TriangleType { get; set; }
        public List<(int, int)> Coordinates { get; set; }
    }

    public static class TriangleCalculator
    {
        public static TriangleResult ProcessTriangle(string sideAStr, string sideBStr, string sideCStr)
        {
            if (!float.TryParse(sideAStr, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out float a) ||
                !float.TryParse(sideBStr, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out float b) ||
                !float.TryParse(sideCStr, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out float c))
            {
                return new TriangleResult
                {
                    TriangleType = "",
                    Coordinates = new List<(int, int)> { (-2, -2), (-2, -2), (-2, -2) }
                };
            }

            if (a <= 0 || b <= 0 || c <= 0)
            {
                return new TriangleResult
                {
                    TriangleType = "не треугольник",
                    Coordinates = new List<(int, int)> { (-1, -1), (-1, -1), (-1, -1) }
                };
            }

            if (a + b <= c || a + c <= b || b + c <= a)
            {
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
            
            float discriminant = b * b - Cx * Cx;
            if (discriminant < 0) discriminant = 0;
            float Cy = (float)Math.Sqrt(discriminant);

            float minX = Math.Min(0, Math.Min(c, Cx));
            float maxX = Math.Max(0, Math.Max(c, Cx));
            float minY = Math.Min(0, Math.Min(0, Cy));
            float maxY = Math.Max(0, Math.Max(0, Cy));

            float scaleX = (maxX - minX) != 0 ? 90f / (maxX - minX) : 1f;
            float scaleY = (maxY - minY) != 0 ? 90f / (maxY - minY) : 1f;
            if (float.IsInfinity(scaleX) || float.IsNaN(scaleX)) scaleX = 1f;
            if (float.IsInfinity(scaleY) || float.IsNaN(scaleY)) scaleY = 1f;
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
