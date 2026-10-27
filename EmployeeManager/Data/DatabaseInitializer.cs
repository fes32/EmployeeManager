
using Microsoft.Data.Sqlite;

namespace EmployeeManager.Data;

public static class DatabaseInitializer
{
    private static readonly string DatabaseDirectory =
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "EmployeeManager");

    private static readonly string DatabasePath = Path.Combine(DatabaseDirectory, "employees.db");

    public static string ConnectionString =>$"Data Source={DatabasePath}";

    public static void Initialize()
    {
        Directory.CreateDirectory(DatabaseDirectory);

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText =
            """
            CREATE TABLE IF NOT EXISTS Employees
            (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                FirstName TEXT NOT NULL,
                LastName TEXT NOT NULL,
                MiddleName TEXT NULL,
                BirthDate TEXT NOT NULL,
                Address TEXT NOT NULL,
                Department TEXT NOT NULL,
                About TEXT NULL
            );
            """;

        command.ExecuteNonQuery();
    }
}
