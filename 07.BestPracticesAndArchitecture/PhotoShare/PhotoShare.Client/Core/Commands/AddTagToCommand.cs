using System.Linq;
using PhotoShare.Client.Utilities;
using PhotoShare.Data;
using PhotoShare.Models;

namespace PhotoShare.Client.Core.Commands
{
    using System;

    public class AddTagToCommand : ICommand
    {
        // AddTagTo <albumName> <tag>

        public string Execute(string[] data)
        {
            var albumName = data[0];
            var tagName = data[1];

            using (PhotoShareContext context = new PhotoShareContext())
            {
                if (Session.User == null)
                {
                    throw new InvalidOperationException("Invalid credentials!");
                }

                var album = context.Albums.SingleOrDefault(a => a.Name == albumName);
                var tag = context.Tags.SingleOrDefault(t => t.Name == TagUtilities.ValidateOrTransform(tagName));

                if (album == null || tag == null)
                {
                    throw new ArgumentException("Either tag or album do not exist!");
                }

                bool isLoggedUserOwner =
                    context
                        .AlbumRoles
                        .Any(ar => ar.UserId == Session.User.Id && ar.AlbumId == album.Id && ar.Role == Role.Owner);

                if (!isLoggedUserOwner)
                {
                    throw new InvalidOperationException("Invalid credentials!");
                }

                context.AlbumTags.Add(new AlbumTag
                {
                    Album = album,
                    Tag = tag
                });

                context.SaveChanges();

                return $"Tag {tagName} added to {albumName}!";
            }
        }
    }
}
