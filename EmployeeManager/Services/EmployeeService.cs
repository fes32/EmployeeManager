using EmployeeManager.Data;
using EmployeeManager.Models;

namespace EmployeeManager.Services;

public class EmployeeService
{
    private readonly EmployeeRepository _repository;

    public EmployeeService(EmployeeRepository repository)
    {
        _repository = repository;
    }

    public List<Employee> GetAll()
    {
        return _repository.GetAll();
    }

    public Employee? GetById(int id)
    {
        if (id <= 0)
            return null;

        return _repository.GetById(id);
    }

    public int Create(Employee employee)
    {
        ValidateEmployee(employee);

        return _repository.Create(employee);
    }

    public bool Update(Employee employee)
    {
        ValidateEmployee(employee);

        if (employee.Id <= 0)
            throw new ArgumentException("Некорректный идентификатор сотрудника.");

        return _repository.Update(employee);
    }

    public bool Delete(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Некорректный идентификатор сотрудника.");

        return _repository.Delete(id);
    }

    private static void ValidateEmployee(Employee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);

        if (string.IsNullOrWhiteSpace(employee.FirstName))
            throw new ArgumentException("Укажите имя сотрудника.");

        if (string.IsNullOrWhiteSpace(employee.LastName))
            throw new ArgumentException("Укажите фамилию сотрудника.");

        if (employee.FirstName.Length > 100)
            throw new ArgumentException("Имя не должно превышать 100 символов.");

        if (employee.LastName.Length > 100)
            throw new ArgumentException("Фамилия не должна превышать 100 символов.");

        if (employee.MiddleName?.Length > 100)
            throw new ArgumentException("Отчество не должно превышать 100 символов.");

        if (employee.BirthDate.Date > DateTime.Today)
            throw new ArgumentException("Дата рождения не может быть в будущем.");

        if (employee.BirthDate == default)
            throw new ArgumentException("Укажите дату рождения.");

        if (string.IsNullOrWhiteSpace(employee.Address))
            throw new ArgumentException("Укажите адрес проживания.");

        if (employee.Address.Length > 300)
            throw new ArgumentException("Адрес не должен превышать 300 символов.");

        if (string.IsNullOrWhiteSpace(employee.Department))
            throw new ArgumentException("Укажите отдел сотрудника.");

        if (employee.Department.Length > 100)
            throw new ArgumentException("Название отдела не должно превышать 100 символов.");

        if (employee.About?.Length > 2000)
            throw new ArgumentException("Поле «О себе» не должно превышать 2000 символов.");
    }
}
