namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using PhotoShare.Data;
    using PhotoShare.Models;


    public class AddFriendCommand : ICommand
    {
        // AddFriend <username1> <username2>
        public string Execute(string[] data)
        {
            var sendingRequestUsername = data[0];
            var receivedRequestUsername = data[1];

            using (PhotoShareContext context = new PhotoShareContext())
            {
                if (Session.User == null || Session.User.Username != sendingRequestUsername)
                {
                    throw new InvalidOperationException("Invalid credentials!");
                }

                var sendingRequestUser = context.Users
                    .Include(u => u.FriendsAdded)
                    .ThenInclude(af => af.Friend)
                    .FirstOrDefault(u => u.Username == sendingRequestUsername);

                var receivedRequestUser = context.Users
                    .Include(u => u.AddedAsFriendBy)
                    .ThenInclude(u => u.Friend)
                    .FirstOrDefault(u => u.Username == receivedRequestUsername);


                if (sendingRequestUser == null)
                {
                    throw new ArgumentException($"{sendingRequestUsername} not found!");
                }

                if (receivedRequestUser == null)
                {
                    throw new ArgumentException($"{receivedRequestUsername} not found!");
                }

                bool senderAlreadySendRequest = sendingRequestUser.FriendsAdded.Any(u => u.Friend == receivedRequestUser);


                bool receiverAlreadyAcceptRequest = receivedRequestUser.AddedAsFriendBy.Any(u => u.Friend == sendingRequestUser);

                //check frienship 12 exist in username1 FriendsAdded and in username2 AddedAsFriendBy
                if (senderAlreadySendRequest && receiverAlreadyAcceptRequest)
                {
                    throw new InvalidOperationException($"{receivedRequestUsername} is already a friend to {sendingRequestUsername}");
                }

                if (!receiverAlreadyAcceptRequest && senderAlreadySendRequest)
                {
                    throw new InvalidOperationException($"{sendingRequestUsername} already send a request to {receivedRequestUsername}");
                }

                var frienship12 = new Friendship()
                {
                    User = sendingRequestUser,
                    Friend = receivedRequestUser
                };


                sendingRequestUser.FriendsAdded.Add(frienship12);

                context.SaveChanges();

            }

            return $"Friend {receivedRequestUsername} added to {sendingRequestUsername}";
        }
    }
}
