using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.DTOs.Torneo;
using LolHubMexico.Domain.Entities.Torneos;

namespace LolHubMexico.Application.Interfaces
{
    public interface ITorneoService
    {
        Task<TorneoDTO> CrearTorneoAsync(TorneoDTO torneo);
    }
}
