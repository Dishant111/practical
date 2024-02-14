using CustoomerToken.Domain.Employees;
using CustoomerToken.Domain.Tokens;
using CustoomerToken.Infrastructure.Util;
using CustoomerToken.Models;
using System.Data;
using System.Data.SqlClient;

namespace CustoomerToken.Infrastructure.Repositories
{
    public class EmployeeRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;
        public EmployeeRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString"];
        }

        public Employee? GetById(int id)
        {
            Employee result = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetEmployeeById", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                var sqlreader = cmd.ExecuteReader();
                while (sqlreader.Read())
                {
                    result = new Employee()
                    {
                        Id = Convert.ToInt32(sqlreader["Id"]),
                        QueryId = (QueryType)Convert.ToInt32(sqlreader["QueryId"]),
                        UserName = Convert.ToString(sqlreader["UserName"]),
                        CreatedOn = Convert.ToDateTime(sqlreader["CreatedOn"]),
                        UpdatedOn = sqlreader.GetNullableDateTime("UpdatedOn"),
                        DeletedOn = sqlreader.GetNullableDateTime("DeletedOn"),
                        IsDeleted = Convert.ToBoolean(sqlreader["IsDeleted"]),
                    };

                }
                con.Close();
                return result;
            }

        }

        public EmployeeCredential? GetUserCredential(string userName)
        {
            EmployeeCredential result = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetEmployeeCredentialByUserName", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserName", userName);
                con.Open();
                var sqlreader = cmd.ExecuteReader();
                while (sqlreader.Read())
                {
                    
                    result = new EmployeeCredential()
                    {
                        Id = Convert.ToInt32(sqlreader["Id"]),
                        QueryId = (QueryType)Convert.ToInt32(sqlreader["QueryId"]),
                        UserName = Convert.ToString(sqlreader["UserName"]),
                        Password = Convert.ToString(sqlreader["Password"]),
                        CreatedOn = Convert.ToDateTime(sqlreader["CreatedOn"]),
                        UpdatedOn = sqlreader.GetNullableDateTime("UpdatedOn"),
                        DeletedOn = sqlreader.GetNullableDateTime("DeletedOn"),
                        IsDeleted = Convert.ToBoolean(sqlreader["IsDeleted"]),
                    };

                }
                con.Close();
                return result;
            }
        }

        public Employee? Create(EmployeeCredential employee)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("CreateEmployees", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@QueryId", employee.QueryId);
                cmd.Parameters.AddWithValue("@UserName", employee.UserName);
                cmd.Parameters.AddWithValue("@Password", employee.Password);

                con.Open();
                var value = cmd.ExecuteScalar();
                employee.Id = Convert.ToInt32(value);
                con.Close();
                return employee;
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteEmployees", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
