namespace PhotoShare.Client.Core.Commands
{
    using Models;
    using Data;
    using System;
    using System.Linq;

    public class AddTownCommand : ICommand
    {
        // AddTown <townName> <countryName>
        public string Execute(string[] data)
        {
            string townName = data[0];
            string country = data[1];

            using (PhotoShareContext context = new PhotoShareContext())
            {
                if (Session.User == null)
                {
                    throw new InvalidOperationException("Invalid credentials!");
                }

                if (context.Towns.SingleOrDefault(t => t.Name == townName) != null)
                {
                    throw new ArgumentException($"Town {townName} was already added!");
                }

                Town town = new Town
                {
                    Name = townName,
                    Country = country
                };

                context.Towns.Add(town);
                context.SaveChanges();

                return $"Town {townName} was added successfully!";
            }
        }
    }
}
