using LolHubMexico.Domain.Notifications;

namespace LolHubMexico.Notifiers
{
    public class NotifierFactory : INotifierFactory
    {
        private readonly IServiceProvider _provider;

        public NotifierFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public INotifier GetNotifier(string type) =>
            type switch
            {
                "TeamInvitation" => _provider.GetRequiredService<TeamInvitationNotifier>(),
                // "ScrimInvite" => ...
                _ => throw new NotSupportedException($"Tipo de notificación no soportado: {type}")
            };
    }
}
