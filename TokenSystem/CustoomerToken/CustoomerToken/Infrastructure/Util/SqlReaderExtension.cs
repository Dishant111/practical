using System.Data.SqlClient;

namespace CustoomerToken.Infrastructure.Util
{
    public static class SqlReaderExtension
    {
        public static DateTime? GetNullableDateTime(this SqlDataReader reader, string name)
        {
            var col = reader.GetOrdinal(name);
            return reader.IsDBNull(col) ?
                        (DateTime?)null :
                        (DateTime?)reader.GetDateTime(col);
        }

    }
}
