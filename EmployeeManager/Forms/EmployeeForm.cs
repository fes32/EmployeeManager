
using EmployeeManager.Models;
using EmployeeManager.Services;

namespace EmployeeManager.Forms;

public class EmployeeForm : Form
{
    private readonly EmployeeService _employeeService;
    private readonly Employee? _employee;

    private readonly TextBox _firstNameTextBox = new();
    private readonly TextBox _lastNameTextBox = new();
    private readonly TextBox _middleNameTextBox = new();
    private readonly DateTimePicker _birthDatePicker = new();
    private readonly TextBox _addressTextBox = new();
    private readonly TextBox _departmentTextBox = new();
    private readonly TextBox _aboutTextBox = new();

    public EmployeeForm(EmployeeService employeeService, Employee? employee = null)
    {
        _employeeService = employeeService;
        _employee = employee;

        Text = employee is null ? "Добавление сотрудника" : "Редактирование сотрудника";
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(600, 650);
        MinimumSize = new Size(500, 550);
        AutoScaleMode = AutoScaleMode.Font;

        BuildForm();
        FillFields();
    }

    private void BuildForm()
    {
        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 8,
            Padding = new Padding(12),
            AutoScroll = true
        };

        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        AddField(layout, 0, "Имя *", _firstNameTextBox);
        AddField(layout, 1, "Фамилия *", _lastNameTextBox);
        AddField(layout, 2, "Отчество", _middleNameTextBox);

        _birthDatePicker.Format = DateTimePickerFormat.Custom;
        _birthDatePicker.CustomFormat = "dd.MM.yyyy";
        _birthDatePicker.MaxDate = DateTime.Today;

        AddField(layout, 3, "Дата рождения *", _birthDatePicker);
        AddField(layout, 4, "Адрес проживания *", _addressTextBox);
        AddField(layout, 5, "Отдел *", _departmentTextBox);

        _aboutTextBox.Multiline = true;
        _aboutTextBox.ScrollBars = ScrollBars.Vertical;
        _aboutTextBox.MaxLength = 2000;
        _aboutTextBox.Height = 100;

        AddField(layout, 6, "О себе", _aboutTextBox);

        var saveButton = new Button
        {
            Text = "Сохранить",
            AutoSize = true
        };

        var cancelButton = new Button
        {
            Text = "Отмена",
            AutoSize = true,
            DialogResult = DialogResult.Cancel
        };

        var buttonsPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            AutoSize = true
        };

        buttonsPanel.Controls.Add(cancelButton);
        buttonsPanel.Controls.Add(saveButton);

        layout.Controls.Add(buttonsPanel, 1, 7);
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 125));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));

        Controls.Add(layout);

        AcceptButton = saveButton;
        CancelButton = cancelButton;

        saveButton.Click += SaveButton_Click;
    }

    private static void AddField(
        TableLayoutPanel layout,
        int row,
        string labelText,
        Control input)
    {
        var label = new Label
        {
            Text = labelText,
            AutoSize = true,
            Anchor = AnchorStyles.Left,
            Margin = new Padding(3, 8, 3, 3)
        };

        input.Dock = DockStyle.Fill;
        input.Margin = new Padding(3, 5, 3, 5);

        layout.Controls.Add(label, 0, row);
        layout.Controls.Add(input, 1, row);
    }

    private void FillFields()
    {
        if (_employee is null)
        {
            _birthDatePicker.Value = DateTime.Today;
            return;
        }

        _firstNameTextBox.Text = _employee.FirstName;
        _lastNameTextBox.Text = _employee.LastName;
        _middleNameTextBox.Text = _employee.MiddleName ?? string.Empty;
        _birthDatePicker.Value = _employee.BirthDate.Date;
        _addressTextBox.Text = _employee.Address;
        _departmentTextBox.Text = _employee.Department;
        _aboutTextBox.Text = _employee.About ?? string.Empty;
    }

    private void SaveButton_Click(object? sender, EventArgs e)
    {
        var employee = new Employee
        {
            Id = _employee?.Id ?? 0,
            FirstName = _firstNameTextBox.Text.Trim(),
            LastName = _lastNameTextBox.Text.Trim(),
            MiddleName = EmptyToNull(_middleNameTextBox.Text),
            BirthDate = _birthDatePicker.Value.Date,
            Address = _addressTextBox.Text.Trim(),
            Department = _departmentTextBox.Text.Trim(),
            About = EmptyToNull(_aboutTextBox.Text)
        };

        try
        {
            if (_employee is null)
                _employeeService.Create(employee);
            else
            {
                var updated = _employeeService.Update(employee);

                if (!updated)
                {
                    MessageBox.Show("Сотрудник не найден.","Ошибка",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message,"Проверьте введённые данные",MessageBoxButtons.OK,MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Не удалось сохранить сотрудника: {ex.Message}","Ошибка",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }
    }

    private static string? EmptyToNull(string value)
    {
        var trimmed = value.Trim();
        return string.IsNullOrEmpty(trimmed) ? null : trimmed;
    }
}
