using CustoomerToken.Domain.Tokens;

namespace CustoomerToken.Domain.Employees
{
    public class Employee
    {
        public int Id { get; set; }
        public QueryType QueryId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool IsDeleted { get; set; }
    }


    public class EmployeeCredential : Employee
    {
        public string Password { get; set; }
    }
}
