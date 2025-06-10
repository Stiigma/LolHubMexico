using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Application.dessingPatterns
{
    public interface IScrimObserver
    {
        Task VerificarScrimsPendientesAsync();

        Task CancelarScrimsInactivasAsync();

        Task VerificarScrimsReportadasAsync();
    }
}
