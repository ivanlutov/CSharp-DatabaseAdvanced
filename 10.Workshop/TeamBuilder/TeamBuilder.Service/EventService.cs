using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TeamBuilder.Service
{
    using System;
    using TeamBuilder.Data;
    using TeamBuilder.Models;
    public class EventService
    {
        private readonly TeamBuilderContext context;

        public EventService(TeamBuilderContext context)
        {
            this.context = context;
        }

        public void Create(string name, string description, DateTime startDate, DateTime endDate, int creatorId)
        {
                var currentEvent = new Event
                {
                    Name = name,
                    Description = description,
                    StartDate = startDate,
                    EndDate = endDate,
                    CreatorId = creatorId
                };

                context.Events.Add(currentEvent);
                context.SaveChanges();
        }

        public bool IsEventExisting(string name)
        {
           return  context
                    .Events
                    .Any(e => e.Name == name);
        }

        public Event GetEventByName(string eventName)
        {
            var eventToPass = context
                .Events
                .Include(e => e.ParticipatingEventTeams)
                .ThenInclude(te => te.Team)
                .OrderByDescending(e => e.StartDate)
                .FirstOrDefault(e => e.Name == eventName);

            return eventToPass;
        }
    }
}