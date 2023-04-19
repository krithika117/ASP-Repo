using Microsoft.Data.SqlClient;

namespace JobPortal
{
    public class Connection
    {
        private static readonly SqlConnection conn = new();
        public static void Init(string connectionString)
        {
            conn.ConnectionString = connectionString;
            conn.Open();
        }
        public static SqlCommand CreateCommand(string query = "")
        {
            return new(query, conn);
        }
    }
}
