using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.Entities.Scrims;

namespace LolHubMexico.Application.ScrimProcessing
{
    public interface IScrimProcessor
    {
        Task ProcessAsync(Scrim scrim);
    }
}
