using System.Data;
using System.Data.SqlClient;

namespace EmployeeManagement.Data
{
    public class EmployeeDetail
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public decimal Salary { get; set; }
        public int DesignationId { get; set; }
        public string Designation { get; set; }
    }

    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string connectionString;

        public EmployeeRepository(IConfiguration configuration)
        {
            this.connectionString = configuration["ConnectionStrings"];
        }

        public int CreateEmployee(string name, decimal salary, int designationId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("CreateEmployee", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Salary", salary);
                    command.Parameters.AddWithValue("@Designation_Id", designationId);

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int employeeId))
                    {
                        return employeeId;
                    }

                    return 0;
                }
            }
        }

        public void UpdateEmployee(int id, string name, decimal salary, int designationId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("UpdateEmployee", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Salary", salary);
                    command.Parameters.AddWithValue("@Designation_Id", designationId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteEmployee(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("DeleteEmployee", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<EmployeeDetail> GetEmployeeListing()
        {
            List<EmployeeDetail> employeeList = new List<EmployeeDetail>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("GetEmployeeListing", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EmployeeDetail employee = new EmployeeDetail
                            {
                                EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                                EmployeeName = reader["EmployeeName"]?.ToString(),
                                Salary = Convert.ToDecimal(reader["Salary"]),
                                DesignationId = Convert.ToInt32(reader["DesignationId"]),
                                Designation = reader["Designation"]?.ToString()
                            };

                            employeeList.Add(employee);
                        }
                    }
                }
            }

            return employeeList;
        }



        public EmployeeDetail GetEmployeeById(int employeeId)
        {
            EmployeeDetail employee = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("GetEmployeeById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@EmployeeId", employeeId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            employee = new EmployeeDetail
                            {
                                EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                                EmployeeName = reader["EmployeeName"]?.ToString(),
                                Salary = Convert.ToDecimal(reader["Salary"]),
                                DesignationId = Convert.ToInt32(reader["DesignationId"]),
                                Designation = reader["Designation"]?.ToString()
                            };
                        }
                    }
                }
            }

            return employee;
        }


    }
}
