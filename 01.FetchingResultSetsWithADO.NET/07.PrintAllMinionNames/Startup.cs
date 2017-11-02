namespace _07.PrintAllMinionNames
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public class Startup
    {
        public static void Main()
        {
            SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MinionsDB;Integrated Security=true;");
            connection.Open();

            using (connection)
            {
                var query = @"SELECT [Name] FROM Minions";
                SqlCommand command = new SqlCommand(query, connection);

                var reader = command.ExecuteReader();
                var minions = new List<string>();

                while (reader.Read())
                {
                    minions.Add((string)reader["Name"]);
                }

                var countOfMinions = minions.Count / 2;
                var isOddLine = minions.Count % 2 == 1;

                for (int i = 0; i < countOfMinions; i++)
                {
                    Console.WriteLine(minions[i]);
                    Console.WriteLine(minions[minions.Count - 1 - i]);
                }

                if (isOddLine)
                {
                    Console.WriteLine(minions[countOfMinions]);
                }
            }
        }
    }
}
