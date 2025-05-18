using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.Notifications
{
    public interface INotifierFactory
    {
        INotifier GetNotifier(string type);
    }
}
