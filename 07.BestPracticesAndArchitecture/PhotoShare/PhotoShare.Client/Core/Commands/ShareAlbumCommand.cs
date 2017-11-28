namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using PhotoShare.Data;
    using PhotoShare.Models;

    public class ShareAlbumCommand : ICommand
    {
        // ShareAlbum <albumId> <username> <permission>
        // For example:
        // ShareAlbum 4 dragon321 Owner
        // ShareAlbum 4 dragon11 Viewer

        public string Execute(string[] data)
        {
            var albumId = int.Parse(data[0]);
            var username = data[1];
            var permissionToString = data[2];

            using (PhotoShareContext context = new PhotoShareContext())
            {
                if (Session.User == null || Session.User.Username != username)
                {
                    throw new InvalidOperationException("Invalid credentials!");
                }

                var album = context
                    .Albums
                    .Include(a => a.AlbumRoles)
                    .SingleOrDefault(a => a.Id == albumId);

                if (album == null)
                {
                    throw new ArgumentException($"Album {albumId} not found!");
                }

                var user = context.Users.SingleOrDefault(u => u.Username == username);
                if (user == null)
                {
                    throw new ArgumentException($"User {username} not found!");
                }

                Role role;
                switch (permissionToString)
                {
                    case "Owner":
                        role = Role.Owner;
                        break;
                    case "Viewer":
                        role = Role.Viewer;
                        break;
                        default:
                            throw new ArgumentException(@"Permission must be either ""Owner"" or ""Viewer""!");

                }

                bool isRoleExist = context
                    .AlbumRoles
                    .Any(a => a.User == user && a.Album == album && a.Role == role);

                if (isRoleExist)
                {
                    throw new InvalidOperationException($"Username {username} already added to album {album.Name} ({permissionToString})");
                }

                var albumRole = new AlbumRole()
                {
                    AlbumId = album.Id,
                    UserId = user.Id,
                    Role = role
                };

                album.AlbumRoles.Add(albumRole);

                context.SaveChanges();

                return $"Username {username} added to album {album.Name} ({permissionToString})";
            }
        }
    }
}
