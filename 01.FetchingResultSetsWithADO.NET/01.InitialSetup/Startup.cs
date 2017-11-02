namespace _01.InitialSetup
{
    using System.Data.SqlClient;
    using System.IO;

    public class Startup
    {
        public static void Main()
        {
            SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=true;");
            connection.Open();

            using (connection)
            {
                string createDatabaseQuery = File.ReadAllText("CreateDatabaseQuery.sql");
                SqlCommand createDatabaseCommand = new SqlCommand(createDatabaseQuery, connection);
                createDatabaseCommand.ExecuteNonQuery();

                string createTablesQuery = File.ReadAllText("CreateTablesQuery.sql");
                SqlCommand createTablesCommand = new SqlCommand(createTablesQuery, connection);
                createTablesCommand.ExecuteNonQuery();
            }
        }
    }
}
