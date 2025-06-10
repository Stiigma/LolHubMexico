using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.Repositories.ITonreoRepository;
using LolHubMexico.Domain.Repositories.ScrimRepository;
using LolHubMexico.Application.TorneoServices;

namespace LolHubMexico.Application.dessingPatterns
{
    public class TorneoObserver
    {
        private readonly ITorneoRepository _torneoRepository;
        private readonly IScrimRepository _scrimRepository;

        public TorneoObserver(ITorneoRepository torneoRepository, IScrimRepository scrimRepository)
        {
            _torneoRepository = torneoRepository;
            _scrimRepository = scrimRepository;
        }

        public async Task VerificarEstadoTorneosPediente()
        {
            var torneosActivos = await _torneoRepository.GetTorneosConEstado(0);

            foreach (var torneo in torneosActivos)
            {
                var participantes = await _torneoRepository.GetTorneoEquiposByTorneoIdAsync(torneo.IdTorneo);

                if (participantes.Count > 4)
                {
                    torneo.Estado = 1;
                    await _torneoRepository.EditarTorneoAsync(torneo);
                }                    
               
            }
        }

        public async Task VerificarFechaTorneo()
        {
            var torneosNoCompletos = await _torneoRepository.GetTorneosConEstado(0);
            var torneosCompletos = await _torneoRepository.GetTorneosConEstado(1);

            var todosLosTorneos = torneosNoCompletos.Concat(torneosCompletos).ToList();


            
        }
    }
}
