namespace _02.GetVillainsNames
{
    using System;
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
                string selectVillainsQuery = File.ReadAllText("SelectVillains.sql");
                SqlCommand selectCommand = new SqlCommand(selectVillainsQuery, connection);
                var reader = selectCommand.ExecuteReader();

                while (reader.Read())
                {
                    var villainsName = (string)reader["Name"];
                    var minionsCount = (int)reader["MinionsCount"];

                    Console.WriteLine($"{villainsName} {minionsCount}");
                }
            }
        }
    }
}
