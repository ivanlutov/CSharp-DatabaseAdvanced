using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

using Newtonsoft.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

using Instagraph.Data;
using Instagraph.DataProcessor.Dtos;
using Instagraph.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Instagraph.DataProcessor
{
    public class Deserializer
    {
        private static string errorMsg = "Error: Invalid data.";
        private static string successMsg = "Successfully imported {0}.";

        public static string ImportPictures(InstagraphContext context, string jsonString)
        {
            var picturesDeserialize = JsonConvert.DeserializeObject<Picture[]>(jsonString);
            var pictures = new List<Picture>();
            var sb = new StringBuilder();

            foreach (var picture in picturesDeserialize)
            {
                var isInvalidPicture = picture.Size <= 0 || string.IsNullOrWhiteSpace(picture.Path);
                var isExistInDatabase = context.Pictures.Any(p => p.Path == picture.Path) ||
                                        pictures.Any(p => p.Path == picture.Path);

                if (isExistInDatabase || isInvalidPicture)
                {
                    sb.AppendLine(errorMsg);
                    continue;
                }

                sb.AppendLine(string.Format(successMsg, $"Picture {picture.Path}"));
                pictures.Add(picture);
            }

            context.Pictures.AddRange(pictures);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportUsers(InstagraphContext context, string jsonString)
        {
            var deserializeUsers = JsonConvert.DeserializeObject<UserDto[]>(jsonString);
            var users = new List<User>();
            var sb = new StringBuilder();

            foreach (var u in deserializeUsers)
            {
                var invalidUsername = string.IsNullOrWhiteSpace(u.Username) || u.Username.Length > 30;
                var invalidPassword = string.IsNullOrWhiteSpace(u.Username) || u.Username.Length > 20;
                var profilePicture = context.Pictures.SingleOrDefault(p => p.Path == u.ProfilePicture);

                if (invalidPassword || invalidUsername || profilePicture == null)
                {
                    sb.AppendLine(errorMsg);
                    continue;
                }

                var user = Mapper.Map<User>(u);
                user.ProfilePicture = profilePicture;
                users.Add(user);
                sb.AppendLine(string.Format(successMsg, $"User {user.Username}"));
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportFollowers(InstagraphContext context, string jsonString)
        {
            var deserializeUserFollower = JsonConvert.DeserializeObject<UserFollowerDto[]>(jsonString);
            var userFollowers = new List<UserFollower>();
            var sb = new StringBuilder();

            foreach (var uf in deserializeUserFollower)
            {
                int? userId = context.Users.SingleOrDefault(u => u.Username == uf.User)?.Id;
                int? followerId = context.Users.SingleOrDefault(u => u.Username == uf.Follower)?.Id;
                if (userId == null || followerId == null)
                {
                    sb.AppendLine(errorMsg);
                    continue;
                }

                bool alreadyFollowed = userFollowers.Any(f => f.UserId == userId && f.FollowerId == followerId);
                if (alreadyFollowed)
                {
                    sb.AppendLine(errorMsg);
                    continue;
                }

                var userFollower = new UserFollower
                {
                    UserId = userId.Value,
                    FollowerId = followerId.Value
                };

                userFollowers.Add(userFollower);
                sb.AppendLine(string.Format(successMsg, $"Follower {uf.Follower} to User {uf.User}"));
            }

            context.UsersFollowers.AddRange(userFollowers);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportPosts(InstagraphContext context, string xmlString)
        {
            var xDoc = XDocument.Parse(xmlString);
            var elements = xDoc.Root.Elements();
            var sb = new StringBuilder();
            var posts = new List<Post>();

            foreach (var e in elements)
            {
                var caption = e.Element("caption")?.Value;
                var user = e.Element("user")?.Value;
                var picture = e.Element("picture")?.Value;

                if (string.IsNullOrWhiteSpace(caption) ||
                    string.IsNullOrWhiteSpace(user) ||
                    string.IsNullOrWhiteSpace(picture))
                {
                    sb.AppendLine(errorMsg);
                    continue;
                }

                int? userId = context.Users.SingleOrDefault(u => u.Username == user)?.Id;
                int? pictureId = context.Pictures.SingleOrDefault(u => u.Path == picture)?.Id;

                if (userId == null || pictureId == null)
                {
                    sb.AppendLine(errorMsg);
                    continue;
                }

                var post = new Post
                {
                    UserId = userId.Value,
                    PictureId = pictureId.Value,
                    Caption = caption
                };

                posts.Add(post);
                sb.AppendLine(string.Format(successMsg, $"Post {post.Caption}"));
            }

            context.Posts.AddRange(posts);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportComments(InstagraphContext context, string xmlString)
        {
            var xDoc = XDocument.Parse(xmlString);
            var elements = xDoc.Root.Elements();
            var sb = new StringBuilder();
            var comments = new List<Comment>();

            foreach (var e in elements)
            {
                var content = e.Element("content")?.Value;
                var user = e.Element("user")?.Value;
                var postIdStr = e.Element("post")?.Attribute("id")?.Value;

                if (string.IsNullOrWhiteSpace(content) ||
                    string.IsNullOrWhiteSpace(user) ||
                    string.IsNullOrWhiteSpace(postIdStr))
                {
                    sb.AppendLine(errorMsg);
                    continue;
                }

                int parsedId;
                bool isParsed = int.TryParse(postIdStr, out parsedId);

                if (!isParsed)
                {
                    sb.AppendLine(errorMsg);
                    continue;
                }

                int? userId = context.Users.SingleOrDefault(u => u.Username == user)?.Id;
                int? postId = context.Posts.SingleOrDefault(p => p.Id == parsedId)?.Id;

                if (userId == null || postId == null)
                {
                    sb.AppendLine(errorMsg);
                    continue;
                }

                var comment = new Comment
                {
                    UserId = userId.Value,
                    PostId = postId.Value,
                    Content = content
                };

                comments.Add(comment);
                sb.AppendLine(string.Format(successMsg, $"Comment {content}"));
            }

            context.Comments.AddRange(comments);
            context.SaveChanges();

            return sb.ToString().Trim();
        }
    }
}
