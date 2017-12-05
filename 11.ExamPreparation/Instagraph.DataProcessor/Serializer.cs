using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using AutoMapper.QueryableExtensions;
using Instagraph.Data;
using Instagraph.DataProcessor.Dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Instagraph.DataProcessor
{
    public class Serializer
    {
        public static string ExportUncommentedPosts(InstagraphContext context)
        {
            var posts =
                context
                    .Posts
                    .Include(p => p.Comments)
                    .Where(p => p.Comments.Count == 0)
                    .OrderBy(p => p.Id)
                    .ProjectTo<UncommentedPostDto>()
                    .ToArray();

            var serializePosts = JsonConvert.SerializeObject(posts, Formatting.Indented);

            return serializePosts;
        }

        public static string ExportPopularUsers(InstagraphContext context)
        {

            var users = context.Users
                .Where(u => u.Posts
                    .Any(p => p.Comments
                        .Any(c => u.Followers
                            .Any(f => f.FollowerId == c.UserId))))
                .OrderBy(u => u.Id)
                .ProjectTo<UserFollowerCountDto>()
                .ToArray();

            var result = JsonConvert.SerializeObject(users, Formatting.Indented);

            return result;
        }

        public static string ExportCommentsOnPosts(InstagraphContext context)
        {
            var users =
                context
                    .Users
                    .Include(u => u.Posts)
                    .ThenInclude(p => p.Comments)
                    .Select(u => new
                    {
                        Username = u.Username,
                        MostComments = u.Posts
                            .Select(p => p.Comments.Count)
                            .ToArray()
                    });


            XDocument xDoc = new XDocument();
            xDoc.Add(new XElement("users"));

            var cpDtos = new List<CommentPostDto>();

            foreach (var u in users)
            {
                var username = u.Username;
                var countOfPosts = 0;

                if (u.MostComments.Any())
                {
                    countOfPosts = u.MostComments.OrderByDescending(c => c).FirstOrDefault();
                }

                var dto = new CommentPostDto()
                {
                    Username = username,
                    MostComments = countOfPosts
                };

                cpDtos.Add(dto);
            }

            var orderedCpDtos = cpDtos
                .OrderByDescending(cp => cp.MostComments)
                .ThenBy(cp => cp.Username);

            foreach (var cp in orderedCpDtos)
            {
                xDoc.Root.Add(new XElement("user",
                    new XElement("Username", cp.Username),
                    new XElement("MostComments", cp.MostComments)));
            }

            return xDoc.ToString();
        }
    }
}
