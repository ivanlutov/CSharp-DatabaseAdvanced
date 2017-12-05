using System;
using System.Linq;
using System.Text;
using TeamBuilder.Service;

namespace TeamBuilder.App.Core.Commands
{
    using TeamBuilder.App.Utilities;

    public class ShowTeamCommand : ICommand
    {
        private readonly TeamService teamService;

        public ShowTeamCommand(TeamService teamService)
        {
            this.teamService = teamService;
        }

        public string Execute(params string[] args)
        {
            Check.CheckLength(1, args);

            var teamName = args[0];
            if (!teamService.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            var team = teamService.GetTeamByName(teamName);

            var sb = new StringBuilder();

            sb.AppendLine($"{team.Name} {team.Acronym}");
            sb.AppendLine("Members:");

            if (team.UserTeams.Count > 0)
            {
                foreach (var user in team.UserTeams)
                {
                    sb.AppendLine($"--{user.User.Username}");
                }
            }
            else
            {
                sb.AppendLine("No existing members in this team!");
            }

            return sb.ToString().Trim();
        }
    }
}