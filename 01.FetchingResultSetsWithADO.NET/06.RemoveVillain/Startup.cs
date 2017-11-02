namespace _06.RemoveVillain
{
    using System;
    using System.Data.SqlClient;

    public class Startup
    {
        public static void Main()
        {
            SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MinionsDB;Integrated Security=true;");
            connection.Open();

            var villainId = int.Parse(Console.ReadLine());
            using (connection)
            {
                if (IsVillianExist(connection, villainId))
                {
                    var numberOfMinions = GetNumberOfMinions(connection, villainId);
                    var nameOfVillian = GetNameOfVillian(connection, villainId);

                    if (numberOfMinions > 0)
                    {
                        ReleaseMinionsFromVillian(connection, villainId);
                    }

                    DeleteVillianFromDatabase(connection, villainId);

                    Console.WriteLine($"{nameOfVillian} was deleted");
                    Console.WriteLine($"{numberOfMinions} minions released");
                }
                else
                {
                    Console.WriteLine("No such villain was found");
                }
            }
        }

        private static string GetNameOfVillian(SqlConnection connection, int villainId)
        {
            var query = @"SELECT Name FROM Villains
                           WHERE Id = @villianId";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@villianId", villainId);
            return (string)command.ExecuteScalar();
        }

        private static void DeleteVillianFromDatabase(SqlConnection connection, int villainId)
        {
            var query = @"DELETE FROM Villains
                           WHERE Id = @villianId";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@villianId", villainId);
            command.ExecuteScalar();
        }

        private static void ReleaseMinionsFromVillian(SqlConnection connection, int villainId)
        {
            var query = @"DELETE FROM VillainsMinions
                           WHERE VillainId = @villianId";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@villianId", villainId);
            command.ExecuteScalar();
        }

        private static int GetNumberOfMinions(SqlConnection connection, int villainId)
        {
            var query = @"SELECT COUNT(*) FROM VillainsMinions
                           WHERE VillainId = @villianId";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@villianId", villainId);
            return (int)command.ExecuteScalar();
        }

        private static bool IsVillianExist(SqlConnection connection, int villainId)
        {
            var query = @"SELECT COUNT(*) FROM Villains
                           WHERE Id = @villianId";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@villianId", villainId);
            var result = (int)command.ExecuteScalar();

            if (result == 0)
            {
                return false;
            }

            return true;
        }
    }
}
