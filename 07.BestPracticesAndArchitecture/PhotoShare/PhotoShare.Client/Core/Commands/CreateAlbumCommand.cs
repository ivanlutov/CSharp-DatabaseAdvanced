using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using PhotoShare.Client.Utilities;
using PhotoShare.Data;
using PhotoShare.Models;

namespace PhotoShare.Client.Core.Commands
{
    using System;

    public class CreateAlbumCommand : ICommand
    {
        // CreateAlbum <username> <albumTitle> <BgColor> <tag1> <tag2>...<tagN>      

        public string Execute(string[] data)
        {
            var username = data[0];
            var title = data[1];
            var colorToString = data[2];
            var tags = data.Skip(3).Select(tag => TagUtilities.ValidateOrTransform(tag)).ToArray();

            using (PhotoShareContext context = new PhotoShareContext())
            {
                if (Session.User == null || Session.User.Username != username)
                {
                    throw new InvalidOperationException("Invalid credentials!");
                }

                var user = context.Users.SingleOrDefault(u => u.Username == username);
                if (user == null)
                {
                    throw new ArgumentException($"User {username} not found!");
                }

                if (context.Albums.SingleOrDefault(a => a.Name == title) != null)
                {
                    throw new ArgumentException($"Album {title} exists!");
                }

                object color;
                Enum.TryParse(typeof(Color), colorToString, out color);

                if (color == null)
                {
                    throw new ArgumentException($"Color {colorToString} not found!");
                }

                foreach (var tag in tags)
                {
                    if (context.Tags.SingleOrDefault(t => t.Name == tag) == null)
                    {
                        throw new ArgumentException("Invalid tags!");
                    }
                }

                Album album = new Album
                {
                    Name = title,
                    BackgroundColor = (Color) color

                };

                context.AlbumRoles.Add(new AlbumRole
                {
                    User = user,
                    Album = album,
                    Role = Role.Owner
                });

                foreach (var tag in tags)
                {
                    var currentTag = context.Tags.SingleOrDefault(t => t.Name == tag);

                    context.AlbumTags.Add(new AlbumTag
                    {
                        Tag = currentTag,
                        Album = album
                    });
                }

                context.SaveChanges();

                return $"Album {title} successfully created!";
            }
        }
    }
}
