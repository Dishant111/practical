using System.Data.SqlClient;
using System.Data;

namespace EmployeeManagement.Data
{
    public class Designation
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string Name { get; set; }
    }
    public class DesignationRepository : IDesignationRepository
    {
        private readonly string connectionString;

        public DesignationRepository(IConfiguration configuration)
        {
            this.connectionString = configuration["ConnectionStrings"];
        }

        public List<Designation> GetAllDesignations()
        {
            List<Designation> designations = new List<Designation>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("GetAllDesignations", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Designation designation = new Designation
                            {
                                Id = (int)reader["Id"],
                                CreatedOn = (DateTime)reader["CreatedOn"],
                                UpdatedOn = reader["UpdatedOn"] as DateTime?,
                                Name = reader["Name"].ToString()
                            };

                            designations.Add(designation);
                        }
                    }
                }
            }

            return designations;
        }
    }
}
