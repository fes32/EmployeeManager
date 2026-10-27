namespace EmployeeManager.Models;

public class Employee
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string? MiddleName { get; set; }

    public DateTime BirthDate { get; set; }

    public string Address { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;


    public string? About { get; set; }
}
