namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using System.Text;
    using Microsoft.EntityFrameworkCore;
    using PhotoShare.Data;

    public class ListFriendsCommand : ICommand
    {
        public string Execute(string[] data)
        {
            var username = data[0];

            using (PhotoShareContext context = new PhotoShareContext())
            {
                var user = context
                    .Users
                    .Include(u => u.FriendsAdded)
                        .ThenInclude(fa => fa.Friend)
                    .SingleOrDefault(u => u.Username == username);

                if (user == null)
                {
                    throw new ArgumentException($"User {username} not found!");
                }

                var friends = user.FriendsAdded.ToList();

                var result = string.Empty;

                if (friends.Count == 0)
                {
                    result = "No friends for this user. :(";
                }
                else
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("Friends:");
                    friends.ForEach(f => sb.AppendLine(f.Friend.Username));

                    result = sb.ToString().Trim();
                }

                return result;
            }
        }
    }
}