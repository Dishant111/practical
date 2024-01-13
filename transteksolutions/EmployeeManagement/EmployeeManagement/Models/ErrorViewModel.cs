using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public class EmployeeCerateModel
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string EmployeeName { get; set; }
        [Range(0.0,double.MaxValue)]
        public decimal Salary { get; set; }
        public int DesignationId { get; set; }

        public List<DesignationModel> Designations { get; set; } = new();
    }

    public class DesignationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class EmployeeListModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public decimal Salary { get; set; }
        public string Designation { get; set; }
    }
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
