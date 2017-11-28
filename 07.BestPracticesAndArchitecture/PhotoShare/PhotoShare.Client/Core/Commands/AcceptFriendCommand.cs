namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using PhotoShare.Data;
    using PhotoShare.Models;

    public class AcceptFriendCommand : ICommand
    {
        // AcceptFriend <username1> <username2>
        public string Execute(string[] data)
        {
            var requesterUsername = data[0];
            var accepterUsername = data[1];

            using (PhotoShareContext context = new PhotoShareContext())
            {
                var requesterUser = context.Users
                    .Include(u => u.FriendsAdded)
                    .ThenInclude(af => af.Friend)
                    .FirstOrDefault(u => u.Username == requesterUsername);

                var accepterUser = context.Users
                    .Include(u => u.AddedAsFriendBy)
                    .ThenInclude(fa => fa.Friend)
                    .FirstOrDefault(u => u.Username == accepterUsername);


                if (accepterUser == null)
                {
                    throw new ArgumentException($"{requesterUsername} not found!");
                }

                if (requesterUser == null)
                {
                    throw new ArgumentException($"{accepterUsername} not found!");
                }

                bool requesterAlreadySendRequest = requesterUser.FriendsAdded.Any(u => u.Friend == accepterUser);

                bool accepterAlreadyFriends = accepterUser.AddedAsFriendBy.Any(u => u.Friend == requesterUser);


                //check frienship 12 exist in username1 FriendsAdded and in username2 AddedAsFriendBy

                //check they are already friends

                if (requesterAlreadySendRequest && accepterAlreadyFriends)
                {
                    throw new InvalidOperationException($"{accepterUsername} is already a friend to {requesterUsername}");
                }

                //check is friendship 12 exist in user1 FriendAdded -  was request sent

                if (!requesterAlreadySendRequest)
                {
                    throw new InvalidOperationException($"{requesterUsername} has not added{accepterUsername} as a friend");
                }

                var frienship12 = new Friendship()
                {
                    UserId = accepterUser.Id,
                    FriendId = requesterUser.Id
                };

                accepterUser.AddedAsFriendBy.Add(frienship12);

                context.SaveChanges();

            }

            return $"{accepterUsername} accepted {requesterUsername} as a friend";
        }
    }
}
