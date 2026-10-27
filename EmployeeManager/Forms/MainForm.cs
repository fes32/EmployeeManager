
using EmployeeManager.Models;
using EmployeeManager.Services;

namespace EmployeeManager.Forms;

public partial class MainForm : Form
{
    private readonly EmployeeService _employeeService;
    private readonly DataGridView _employeesGrid;
    private readonly Button _addButton;
    private readonly Button _detailsButton;
    private readonly Button _editButton;
    private readonly Button _deleteButton;

    public MainForm(EmployeeService employeeService)
    {
        _employeeService = employeeService;

        Text = "Employee Manager — Управление сотрудниками";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(1100, 600);
        MinimumSize = new Size(800, 400);

        _employeesGrid = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AutoGenerateColumns = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            RowHeadersVisible = false
        };

        _employeesGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Id",
            HeaderText = "ID",
            DataPropertyName = nameof(Employee.Id),
            FillWeight = 40
        });

        _employeesGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "LastName",
            HeaderText = "Фамилия",
            DataPropertyName = nameof(Employee.LastName)
        });

        _employeesGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "FirstName",
            HeaderText = "Имя",
            DataPropertyName = nameof(Employee.FirstName)
        });

        _employeesGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "MiddleName",
            HeaderText = "Отчество",
            DataPropertyName = nameof(Employee.MiddleName)
        });

        _employeesGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "BirthDate",
            HeaderText = "Дата рождения",
            DataPropertyName = nameof(Employee.BirthDate),
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Format = "dd.MM.yyyy"
            }
        });

        _employeesGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Department",
            HeaderText = "Отдел",
            DataPropertyName = nameof(Employee.Department)
        });

        _addButton = new Button
        {
            Text = "Добавить",
            AutoSize = true
        };

        _detailsButton = new Button
        {
            Text = "Подробнее",
            AutoSize = true
        };

        _editButton = new Button
        {
            Text = "Редактировать",
            AutoSize = true
        };

        _deleteButton = new Button
        {
            Text = "Удалить",
            AutoSize = true
        };

        var buttonsPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            AutoSize = true,
            Padding = new Padding(8),
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = true
        };

        buttonsPanel.Controls.AddRange(
        [
            _addButton,
            _detailsButton,
            _editButton,
            _deleteButton
        ]);

        Controls.Add(_employeesGrid);
        Controls.Add(buttonsPanel);

        _addButton.Click += AddButton_Click;
        _detailsButton.Click += DetailsButton_Click;
        _editButton.Click += EditButton_Click;
        _deleteButton.Click += DeleteButton_Click;
        _employeesGrid.CellDoubleClick += EmployeesGrid_CellDoubleClick;

        LoadEmployees();
    }
    private void MainForm_Load(object sender, EventArgs e)
    {

    }

    private void LoadEmployees()
    {
        try
        {
            _employeesGrid.DataSource = _employeeService.GetAll();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Не удалось загрузить сотрудников: {ex.Message}","Ошибка",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }
    }

    private Employee? GetSelectedEmployee()
    {
        if (_employeesGrid.CurrentRow?.DataBoundItem is Employee employee)
            return employee;

        MessageBox.Show("Сначала выберите сотрудника.","Внимание",MessageBoxButtons.OK,MessageBoxIcon.Information);

        return null;
    }

    private void AddButton_Click(object? sender, EventArgs e)
    {
        using var form = new EmployeeForm(_employeeService);

        if (form.ShowDialog(this) == DialogResult.OK)
            LoadEmployees();
    }

    private void DetailsButton_Click(object? sender, EventArgs e)
    {
        var employee = GetSelectedEmployee();

        if (employee is null)
            return;

        using var form = new EmployeeDetailsForm(employee);
        form.ShowDialog(this);
    }

    private void EditButton_Click(object? sender, EventArgs e)
    {
        var employee = GetSelectedEmployee();

        if (employee is null)
            return;

        using var form = new EmployeeForm(_employeeService, employee);

        if (form.ShowDialog(this) == DialogResult.OK)
            LoadEmployees();
    }

    private void DeleteButton_Click(object? sender, EventArgs e)
    {
        var employee = GetSelectedEmployee();

        if (employee is null)
            return;

        var result = MessageBox.Show($"Удалить сотрудника {employee.LastName} {employee.FirstName}?","Подтверждение удаления",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);

        if (result != DialogResult.Yes)
            return;

        try
        {
            if (_employeeService.Delete(employee.Id))
            {
                LoadEmployees();
                MessageBox.Show("Сотрудник удалён.");
            }
            else
                MessageBox.Show("Сотрудник не найден.");
        }
        catch (Exception ex)
        {
            MessageBox.Show( $"Не удалось удалить сотрудника: {ex.Message}","Ошибка",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }
    }

    private void EmployeesGrid_CellDoubleClick(object? sender,DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
            DetailsButton_Click(sender, EventArgs.Empty);
    }
}
