namespace _09.IncreaseAgeStoredProcedure
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;

    public class Startup
    {
        public static void Main()
        {
            SqlConnection connection =
                new SqlConnection(
                    @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MinionsDB;Integrated Security=true;");
            connection.Open();

            using (connection)
            {
                var minionId = int.Parse(Console.ReadLine());

                string connString = "";
                string queryProc = @"SELECT * 
                                       FROM SYSOBJECTS 
                                      WHERE TYPE='P' 
                                        AND name='usp_GetOlder'";
                bool spExists = false;

                using (SqlCommand commandProc = new SqlCommand(queryProc, connection))
                {
                    using (SqlDataReader readerProc = commandProc.ExecuteReader())
                    {
                        while (readerProc.Read())
                        {
                            spExists = true;
                            break;
                        }
                    }
                }

                if (!spExists)
                {
                    string procQuery = File.ReadAllText("Procedure_GetOlder.sql");
                    SqlCommand selectMinionsCommand = new SqlCommand(procQuery, connection);
                    selectMinionsCommand.ExecuteNonQuery();
                }

                SqlCommand command =
                    new SqlCommand("usp_GetOlder", connection) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddWithValue("@minionId", minionId);

                var affectedRows = command.ExecuteNonQuery();

                if (affectedRows > 0)
                {
                    var query = @"SELECT Name, Age 
                                    FROM Minions
                                   WHERE Id = @minionId";
                    SqlCommand getMinionsData = new SqlCommand(query, connection);
                    getMinionsData.Parameters.AddWithValue("@minionId", minionId);

                    var reader = getMinionsData.ExecuteReader();
                    reader.Read();
                    Console.WriteLine($"{reader["Name"]} - {reader["Age"]} years old");
                }
            }
        }
    }
}
