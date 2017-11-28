namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using PhotoShare.Data;
    using PhotoShare.Models;

    public class UploadPictureCommand : ICommand
    {
        // UploadPicture <albumName> <pictureTitle> <pictureFilePath>
        public string Execute(string[] data)
        {
            var albumName = data[0];
            var pictureTitle = data[1];
            var pictureFilePath = data[2];

            using (PhotoShareContext context = new PhotoShareContext())
            {
                if (Session.User == null)
                {
                    throw new InvalidOperationException("Invalid credentials!");
                }

                var album = context.Albums
                    .Include(a => a.AlbumRoles)
                    .ThenInclude(ar => ar.User)
                    .SingleOrDefault(a => a.Name == albumName);

                if (album == null)
                {
                    throw new ArgumentException($"Album {albumName} not found!");
                }

                bool isLoggedUserOwner =
                    album.AlbumRoles.Any(ar => ar.UserId == Session.User.Id && ar.Role == Role.Owner);

                if (!isLoggedUserOwner)
                {
                    throw new InvalidOperationException("Invalid credentials!");
                }

                var picture = new Picture()
                {
                    Title = pictureTitle,
                    Path = pictureFilePath,
                    Album = album
                };

                context.Pictures.Add(picture);

                context.SaveChanges();

                return $"Picture {pictureTitle} added to {albumName}!";
            }
        }
    }
}
