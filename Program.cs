using System;
using Microsoft.Data.Sqlite;

namespace TriangleLab
{
    // КЛАСС ДЛЯ РАСЧЕТА ТРЕУГОЛЬНИКА
    public class TriangleCalculator
    {
        public static (string TriangleType, string ErrorMessage) DetermineTriangle(double sideA, double sideB, double sideC)
        {
            if (sideA <= 0 || sideB <= 0 || sideC <= 0)
                return ("", "Стороны треугольника должны быть положительными числами");

            if (sideA + sideB <= sideC || sideA + sideC <= sideB || sideB + sideC <= sideA)
                return ("", "Треугольник с такими сторонами не существует");

            if (Math.Abs(sideA - sideB) < 0.0001 && Math.Abs(sideB - sideC) < 0.0001)
                return ("Равносторонний", "");
            if (Math.Abs(sideA - sideB) < 0.0001 || Math.Abs(sideA - sideC) < 0.0001 || Math.Abs(sideB - sideC) < 0.0001)
                return ("Равнобедренный", "");

            return ("Разносторонний", "");
        }
    }

    // КЛАСС ДЛЯ ХРАНЕНИЯ ЗАПИСИ
    public class TriangleRecord
    {
        public double SideA { get; set; }
        public double SideB { get; set; }
        public double SideC { get; set; }
        public string TriangleType { get; set; }
        public string ErrorMessage { get; set; }
    }

    // КЛАСС РАБОТЫ С БАЗОЙ ДАННЫХ
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(string databasePath = "triangles.db")
        {
            _connectionString = $"Data Source={databasePath}";
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Triangles (
                    SideA REAL NOT NULL,
                    SideB REAL NOT NULL,
                    SideC REAL NOT NULL,
                    TriangleType TEXT NOT NULL,
                    ErrorMessage TEXT,
                    PRIMARY KEY (SideA, SideB, SideC)
                )";
            command.ExecuteNonQuery();
        }

        public void AddRecord(double sideA, double sideB, double sideC, string triangleType, string errorMessage)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT OR REPLACE INTO Triangles (SideA, SideB, SideC, TriangleType, ErrorMessage)
                VALUES (@sideA, @sideB, @sideC, @type, @error)";
            command.Parameters.AddWithValue("@sideA", sideA);
            command.Parameters.AddWithValue("@sideB", sideB);
            command.Parameters.AddWithValue("@sideC", sideC);
            command.Parameters.AddWithValue("@type", triangleType);
            command.Parameters.AddWithValue("@error", errorMessage ?? "");
            command.ExecuteNonQuery();
        }

        public TriangleRecord GetRecord(double sideA, double sideB, double sideC)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT SideA, SideB, SideC, TriangleType, ErrorMessage 
                FROM Triangles 
                WHERE SideA = @sideA AND SideB = @sideB AND SideC = @sideC";
            command.Parameters.AddWithValue("@sideA", sideA);
            command.Parameters.AddWithValue("@sideB", sideB);
            command.Parameters.AddWithValue("@sideC", sideC);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new TriangleRecord
                {
                    SideA = reader.GetDouble(0),
                    SideB = reader.GetDouble(1),
                    SideC = reader.GetDouble(2),
                    TriangleType = reader.GetString(3),
                    ErrorMessage = reader.GetString(4)
                };
            }
            return null;
        }

        public void DeleteRecord(double sideA, double sideB, double sideC)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Triangles WHERE SideA = @sideA AND SideB = @sideB AND SideC = @sideC";
            command.Parameters.AddWithValue("@sideA", sideA);
            command.Parameters.AddWithValue("@sideB", sideB);
            command.Parameters.AddWithValue("@sideC", sideC);
            command.ExecuteNonQuery();
        }

        public void ClearAllRecords()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Triangles";
            command.ExecuteNonQuery();
        }
    }

    // ИНТЕРФЕЙС ДЛЯ ВВОДА
    public interface IUserInput
    {
        (double sideA, double sideB, double sideC) GetTriangleSides();
    }

    // ИНТЕРФЕЙС ДЛЯ ВНЕШНЕГО СЕРВИСА
    public interface IExternalService
    {
        bool SendResult(string message);
    }

    // РЕАЛИЗАЦИЯ ВВОДА С КОНСОЛИ
    public class ConsoleUserInput : IUserInput
    {
        public (double sideA, double sideB, double sideC) GetTriangleSides()
        {
            Console.WriteLine("Введите длины сторон треугольника:");
            double sideA = ReadDouble("Сторона A: ");
            double sideB = ReadDouble("Сторона B: ");
            double sideC = ReadDouble("Сторона C: ");
            return (sideA, sideB, sideC);
        }

        private double ReadDouble(string prompt)
        {
            double value;
            while (true)
            {
                Console.Write(prompt);
                if (double.TryParse(Console.ReadLine(), out value))
                    return value;
                Console.WriteLine("Ошибка: введите число");
            }
        }
    }

    // ИМИТАЦИЯ ВНЕШНЕГО СЕРВИСА (EMAIL)
    public class FakeEmailService : IExternalService
    {
        public bool SendResult(string message)
        {
            Console.WriteLine($"[ОТПРАВКА НА EMAIL] Результат: {message}");
            return true;
        }
    }

    // КОНТРОЛЛЕР
    public class TriangleController
    {
        private readonly IUserInput _userInput;
        private readonly DatabaseService _database;
        private readonly IExternalService _externalService;

        public TriangleController(IUserInput userInput, DatabaseService database, IExternalService externalService)
        {
            _userInput = userInput;
            _database = database;
            _externalService = externalService;
        }

        public string ProcessTriangle()
        {
            var (sideA, sideB, sideC) = _userInput.GetTriangleSides();

            string triangleType, errorMessage;

            var existingRecord = _database.GetRecord(sideA, sideB, sideC);

            if (existingRecord != null)
            {
                triangleType = existingRecord.TriangleType;
                errorMessage = existingRecord.ErrorMessage;
                Console.WriteLine($"[КЭШ] Взято из БД: {triangleType}");
            }
            else
            {
                (triangleType, errorMessage) = TriangleCalculator.DetermineTriangle(sideA, sideB, sideC);
                _database.AddRecord(sideA, sideB, sideC, triangleType, errorMessage);
                Console.WriteLine($"[ВЫЧИСЛЕНИЕ] Сохранено в БД");
            }

            string resultMessage = string.IsNullOrEmpty(errorMessage)
                ? $"Треугольник ({sideA},{sideB},{sideC}) - {triangleType}"
                : $"Ошибка: {errorMessage}";

            _externalService.SendResult(resultMessage);
            return resultMessage;
        }
    }

    // ГЛАВНАЯ ПРОГРАММА
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==========================================");
            Console.WriteLine("   ПРОГРАММА ОПРЕДЕЛЕНИЯ ТРЕУГОЛЬНИКА");
            Console.WriteLine("==========================================\n");

            var userInput = new ConsoleUserInput();
            var database = new DatabaseService();
            var externalService = new FakeEmailService();
            var controller = new TriangleController(userInput, database, externalService);

            while (true)
            {
                var result = controller.ProcessTriangle();
                Console.WriteLine($"\nРЕЗУЛЬТАТ: {result}\n");

                Console.Write("Продолжить? (y/n): ");
                var key = Console.ReadKey();
                Console.WriteLine("\n");
                if (key.KeyChar != 'y')
                    break;
            }
        }
    }
}