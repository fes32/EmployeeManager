using EmployeeManager.Data;
using EmployeeManager.Forms;
using EmployeeManager.Services;
namespace EmployeeManager
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            DatabaseInitializer.Initialize();

            var repository = new EmployeeRepository();
            var employeeService = new EmployeeService(repository);

            Application.Run(new MainForm(employeeService));
        }
    }
}