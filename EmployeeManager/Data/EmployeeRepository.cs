using EmployeeManager.Models;
using Microsoft.Data.Sqlite;

namespace EmployeeManager.Data;

public class EmployeeRepository
{
    public List<Employee> GetAll()
    {
        var employees = new List<Employee>();

        using var connection = new SqliteConnection(DatabaseInitializer.ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT Id, FirstName, LastName, MiddleName, BirthDate,
                   Address, Department, About
            FROM Employees
            ORDER BY LastName, FirstName;
            """;

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            employees.Add(MapEmployee(reader));
        }

        return employees;
    }

    public Employee? GetById(int id)
    {
        using var connection = new SqliteConnection(DatabaseInitializer.ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT Id, FirstName, LastName, MiddleName, BirthDate,
                   Address, Department, About
            FROM Employees
            WHERE Id = $id;
            """;

        command.Parameters.AddWithValue("$id", id);

        using var reader = command.ExecuteReader();

        return reader.Read() ? MapEmployee(reader) : null;
    }

    public int Create(Employee employee)
    {
        using var connection = new SqliteConnection(DatabaseInitializer.ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            """
            INSERT INTO Employees
                (FirstName, LastName, MiddleName, BirthDate, Address, Department, About)
            VALUES
                ($firstName, $lastName, $middleName, $birthDate, $address, $department, $about);
            SELECT last_insert_rowid();
            """;

        AddEmployeeParameters(command, employee);

        return Convert.ToInt32(command.ExecuteScalar());
    }

    public bool Update(Employee employee)
    {
        using var connection = new SqliteConnection(DatabaseInitializer.ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            """
            UPDATE Employees
            SET FirstName = $firstName,
                LastName = $lastName,
                MiddleName = $middleName,
                BirthDate = $birthDate,
                Address = $address,
                Department = $department,
                About = $about
            WHERE Id = $id;
            """;

        AddEmployeeParameters(command, employee);
        command.Parameters.AddWithValue("$id", employee.Id);

        return command.ExecuteNonQuery() > 0;
    }

    public bool Delete(int id)
    {
        using var connection = new SqliteConnection(DatabaseInitializer.ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Employees WHERE Id = $id;";
        command.Parameters.AddWithValue("$id", id);

        return command.ExecuteNonQuery() > 0;
    }

    private static Employee MapEmployee(SqliteDataReader reader)
    {
        return new Employee
        {
            Id = reader.GetInt32(0),
            FirstName = reader.GetString(1),
            LastName = reader.GetString(2),
            MiddleName = reader.IsDBNull(3) ? null : reader.GetString(3),
            BirthDate = DateTime.Parse(reader.GetString(4)),
            Address = reader.GetString(5),
            Department = reader.GetString(6),
            About = reader.IsDBNull(7) ? null : reader.GetString(7)
        };
    }

    private static void AddEmployeeParameters(SqliteCommand command, Employee employee)
    {
        command.Parameters.AddWithValue("$firstName", employee.FirstName);
        command.Parameters.AddWithValue("$lastName", employee.LastName);
        command.Parameters.AddWithValue("$middleName", (object?)employee.MiddleName ?? DBNull.Value);
        command.Parameters.AddWithValue("$birthDate", employee.BirthDate.ToString("yyyy-MM-dd"));
        command.Parameters.AddWithValue("$address", employee.Address);
        command.Parameters.AddWithValue("$department", employee.Department);
        command.Parameters.AddWithValue("$about", (object?)employee.About ?? DBNull.Value);
    }
}
