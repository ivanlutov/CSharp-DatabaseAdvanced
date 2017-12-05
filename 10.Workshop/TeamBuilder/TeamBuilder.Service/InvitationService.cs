namespace TeamBuilder.Service
{
    using System.Linq;
    using TeamBuilder.Data;
    using TeamBuilder.Models;
    public class InvitationService
    {
        private readonly TeamBuilderContext context;

        public InvitationService(TeamBuilderContext context)
        {
            this.context = context;
        }

        public bool IsInviteExisting(string teamName, User user)
        {
            return context
                .Invitations
                .Any(i => i.Team.Name == teamName && i.InvitedUserId == user.Id && i.IsActive);
        }
    }
}