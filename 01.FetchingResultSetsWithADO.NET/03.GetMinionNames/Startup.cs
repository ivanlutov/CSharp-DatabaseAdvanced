namespace _03.GetMinionNames
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
                var villainId = int.Parse(Console.ReadLine());

                var selectVillainByIdQuery = File.ReadAllText("SelectVillainsById.sql");
                SqlCommand selectVillainCommand = new SqlCommand(selectVillainByIdQuery, connection);
                SqlParameter parameter = new SqlParameter("@villainsId", villainId);
                selectVillainCommand.Parameters.Add(parameter);

                var villainName = (string)selectVillainCommand.ExecuteScalar();

                if (villainName == null)
                {
                    Console.WriteLine($"No villain with ID {villainId} exists in the database.");
                    return;
                }

                Console.WriteLine($"Villain: {villainName}");

                string selectMinionsQuery = File.ReadAllText("SelectMinions.sql");
                SqlCommand selectMinionsCommand = new SqlCommand(selectMinionsQuery, connection);
                SqlParameter villainIdParameter = new SqlParameter("@villainId", villainId);
                selectMinionsCommand.Parameters.Add(villainIdParameter);

                var minionsReader = selectMinionsCommand.ExecuteReader();
                if (!minionsReader.HasRows)
                {
                    Console.WriteLine("No minions");
                    return;
                }

                var index = 1;
                while (minionsReader.Read())
                {
                    var minionName = (string)minionsReader["Name"];
                    var minionAge = (int)minionsReader["Age"];
                    Console.WriteLine($"{index++}. {minionName} {minionAge}");
                }
            }
        }
    }
}
