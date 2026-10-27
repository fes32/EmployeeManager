using EmployeeManager.Models;

namespace EmployeeManager.Forms;

public class EmployeeDetailsForm : Form
{
    private readonly Employee _employee;

    public EmployeeDetailsForm(Employee employee)
    {
        _employee = employee;

        Text = "Карточка сотрудника";
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(600, 550);
        MinimumSize = new Size(500, 450);
        AutoScaleMode = AutoScaleMode.Font;

        BuildForm();
    }

    private void BuildForm()
    {
        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            AutoScroll = true,
            Padding = new Padding(16)
        };

        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        AddRow(layout, "ID", _employee.Id.ToString());
        AddRow(layout, "Имя", _employee.FirstName);
        AddRow(layout, "Фамилия", _employee.LastName);
        AddRow(layout, "Отчество", DisplayValue(_employee.MiddleName));
        AddRow(layout, "Дата рождения", _employee.BirthDate.ToString("dd.MM.yyyy"));
        AddRow(layout, "Адрес проживания", _employee.Address);
        AddRow(layout, "Отдел", _employee.Department);
        AddRow(layout, "О себе", DisplayValue(_employee.About), true);

        var closeButton = new Button
        {
            Text = "Закрыть",
            AutoSize = true,
            DialogResult = DialogResult.OK,
            Anchor = AnchorStyles.Right
        };

        layout.Controls.Add(closeButton, 1, 8);
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));

        Controls.Add(layout);

        AcceptButton = closeButton;
    }

    private static void AddRow(TableLayoutPanel layout,string title,string value,bool multiline = false)
    {
        int row = layout.RowCount++;

        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var label = new Label
        {
            Text = title,
            AutoSize = true,
            Anchor = AnchorStyles.Left,
            Margin = new Padding(3, 8, 8, 8)
        };

        Control valueControl;

        if (multiline)
        {
            valueControl = new TextBox
            {
                Text = value,
                ReadOnly = true,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Height = 100,
                Dock = DockStyle.Fill
            };
        }
        else
        {
            valueControl = new Label
            {
                Text = value,
                AutoSize = true,
                MaximumSize = new Size(350, 0),
                Anchor = AnchorStyles.Left,
                Margin = new Padding(3, 8, 3, 8)
            };
        }

        layout.Controls.Add(label, 0, row);
        layout.Controls.Add(valueControl, 1, row);
    }

    private static string DisplayValue(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? "Не указано" : value;
    }
}
