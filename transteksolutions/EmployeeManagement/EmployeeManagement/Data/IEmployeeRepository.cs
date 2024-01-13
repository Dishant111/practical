
namespace EmployeeManagement.Data
{
    public interface IEmployeeRepository
    {
        int CreateEmployee(string name, decimal salary, int designationId);
        void DeleteEmployee(int id);
        EmployeeDetail GetEmployeeById(int employeeId);
        List<EmployeeDetail> GetEmployeeListing();
        void UpdateEmployee(int id, string name, decimal salary, int designationId);
    }
}