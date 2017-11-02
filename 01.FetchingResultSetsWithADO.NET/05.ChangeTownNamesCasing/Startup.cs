namespace _05.ChangeTownNamesCasing
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IO;

    public class Startup
    {
        public static void Main()
        {
            SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MinionsDB;Integrated Security=true;");
            connection.Open();

            var countryName = Console.ReadLine();

            using (connection)
            {
                var affectedRows = UpperCaseTowns(connection, countryName);

                if (affectedRows > 0)
                {
                    PrintUpdatedTowns(connection, countryName);
                }
                else
                {
                    Console.WriteLine("No town names were affected.");
                }
            }
        }
        private static void PrintUpdatedTowns(SqlConnection connection, string countryName)
        {
            var query = File.ReadAllText("SelectTownsByCountryName.sql");
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@countryName", countryName);
            var reader = command.ExecuteReader();

            var townNames = new List<string>();
            while (reader.Read())
            {
                townNames.Add((string)reader["TownName"]);
            }

            Console.WriteLine($"{townNames.Count} town names were affected. ");
            Console.WriteLine($"[{string.Join(", ", townNames)}]");
        }
        private static int UpperCaseTowns(SqlConnection connection, string countryName)
        {
            var query = File.ReadAllText("UpperCaseTownsNames.sql");
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@countryName", countryName);

            var result = command.ExecuteNonQuery();
            return result;
        }
    }
}
