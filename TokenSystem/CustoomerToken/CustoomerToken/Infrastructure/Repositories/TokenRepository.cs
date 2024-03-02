using CustoomerToken.Domain.Tokens;
using CustoomerToken.Infrastructure.Util;
using CustoomerToken.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Data.SqlClient;

namespace CustoomerToken.Infrastructure.Repositories
{
    public class TokenRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;
        public TokenRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString"];
        }

        public Token? GetById(int id)
        {
            Token result = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetTokenById", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                var sqlreader = cmd.ExecuteReader();
                while (sqlreader.Read())
                {
                    result = new Token()
                    {
                        Id = Convert.ToInt32(sqlreader["Id"]),
                        QueryId = (QueryType)Convert.ToInt32(sqlreader["QueryId"]),
                        StatusId = (QueryStatus)Convert.ToInt32(sqlreader["StatusId"]),
                        Phone = Convert.ToString(sqlreader["Phone"]),
                        Address = Convert.ToString(sqlreader["Address"]),
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

        public Token? Create(Token token)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("CreateToken", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@QueryId", token.QueryId);
                cmd.Parameters.AddWithValue("@StatusId", token.StatusId);
                if (!string.IsNullOrEmpty(token.Phone))
                    cmd.Parameters.AddWithValue("@Phone", token.Phone);
                if (!string.IsNullOrEmpty(token.Address))
                    cmd.Parameters.AddWithValue("@Address", token.Address);
                con.Open();
                var value = cmd.ExecuteScalar();
                token.Id = Convert.ToInt32(value);
                con.Close();
                return token;
            }
        }

        public Token? Update(Token token)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UpdateToken", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", token.Id);
                cmd.Parameters.AddWithValue("@QueryId", token.QueryId);
                cmd.Parameters.AddWithValue("@StatusId", token.StatusId);
                if (!string.IsNullOrEmpty(token.Phone))
                    cmd.Parameters.AddWithValue("@Phone", token.Phone);
                if (!string.IsNullOrEmpty(token.Address))
                    cmd.Parameters.AddWithValue("@Address", token.Address);
                
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                return token;
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteToken", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

            }
        }

        public PaginationResult<Token>? GetUnResoved(int pageSize, int pageNo)
        {
            int totalRecords = 0;
            List<Token> list = new List<Token>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("GetUnresolvedTokens", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@pageNo", pageNo);

                var sqlreader = cmd.ExecuteReader();
                while (sqlreader.Read())
                {
                    list.Add(new Token()
                    {
                        Id = Convert.ToInt32(sqlreader["Id"]),
                        QueryId = (QueryType)Convert.ToInt32(sqlreader["QueryId"]),
                        StatusId = (QueryStatus)Convert.ToInt32(sqlreader["StatusId"]),
                        Phone = Convert.ToString(sqlreader["Phone"]),
                        Address = Convert.ToString(sqlreader["Address"]),
                        CreatedOn = Convert.ToDateTime(sqlreader["CreatedOn"]),
                        UpdatedOn = sqlreader.GetNullableDateTime("UpdatedOn"),
                        DeletedOn = sqlreader.GetNullableDateTime("DeletedOn"),
                        IsDeleted = Convert.ToBoolean(sqlreader["IsDeleted"]),
                    });
                }

                sqlreader.NextResult();

                while (sqlreader.Read())
                {
                    totalRecords = Convert.ToInt32(sqlreader["TotalRecords"]);
                }

                return new PaginationResult<Token>(totalRecords, pageSize, pageNo, list);
            }
        }

        public PaginationResult<Token>? GetUnResoved(QueryType queryType, int pageSize, int pageNo)
        {
            int totalRecords = 0;
            List<Token> list = new List<Token>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("GetUnresolvedTokensByQuryId", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@QueryId", (int)queryType);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@pageNo", pageNo);

                var sqlreader = cmd.ExecuteReader();
                while (sqlreader.Read())
                {
                    list.Add(new Token()
                    {
                        Id = Convert.ToInt32(sqlreader["Id"]),
                        QueryId = (QueryType)Convert.ToInt32(sqlreader["QueryId"]),
                        StatusId = (QueryStatus)Convert.ToInt32(sqlreader["StatusId"]),
                        Phone = Convert.ToString(sqlreader["Phone"]),
                        Address = Convert.ToString(sqlreader["Address"]),
                        CreatedOn = Convert.ToDateTime(sqlreader["CreatedOn"]),
                        UpdatedOn = sqlreader.GetNullableDateTime("UpdatedOn"),
                        DeletedOn = sqlreader.GetNullableDateTime("DeletedOn"),
                        IsDeleted = Convert.ToBoolean(sqlreader["IsDeleted"]),
                    });
                }

                sqlreader.NextResult();

                while (sqlreader.Read())
                {
                    totalRecords = Convert.ToInt32(sqlreader["TotalRecords"]);
                }

                return new PaginationResult<Token>(totalRecords, pageSize, pageNo, list);
            }
        }

    }
}
