﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.Entities.DatailsScrims;
using LolHubMexico.Domain.Entities.Scrims;
using LolHubMexico.Domain.Repositories.ScrimRepository;
using LolHubMexico.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LolHubMexico.Infrastructure.Repositories.ScrimRepository
{
    public class DetailsScrimRepository : IDetailsScrimRepository
    {
        private readonly ContextDB _context;
        public DetailsScrimRepository(ContextDB context) {
            _context = context;
        }

        public async Task<DetailsScrim> CreateDetailScrim(DetailsScrim detailsScrim)
        {
            var newDetails =  _context.DetailsScrim.Add(detailsScrim);

            await _context.SaveChangesAsync();

            return newDetails.Entity;

        }

        public async Task<List<DetailsScrim>> GetDetailsById(int idDetails)
        {
           
            return await _context.DetailsScrim
                    .Where(d => d.idDetailsScrim == idDetails ).ToListAsync();
        }

        public async Task<List<DetailsScrim>> GetDetailsByIdScrim(int idscrim)
        {
            return await _context.DetailsScrim
                    .Where(d => d.idScrim == idscrim).ToListAsync();
        }

        public async Task<List<DetailsScrim>> GetDetailsByIdAndTeam(int idScrim, int idTeam)
        {

            return await _context.DetailsScrim
                    .Where(d => d.idScrim == idScrim && d.idTeam == idTeam).ToListAsync();
        }

        public async Task<DetailsScrim> UpdateDetailsScrimAsync(DetailsScrim updatedDetail)
        {
            var existing = await _context.DetailsScrim
                .FirstOrDefaultAsync(d => d.idDetailsScrim == updatedDetail.idDetailsScrim);

            if (existing == null)
                return null;

            // Actualización campo por campo
            existing.idMatch = updatedDetail.idMatch;
            existing.carril = updatedDetail.carril;
            existing.teamDamagePercentage = updatedDetail.teamDamagePercentage;
            existing.kills = updatedDetail.kills;
            existing.deaths = updatedDetail.deaths;
            existing.assists = updatedDetail.assists;
            existing.goldEarned = updatedDetail.goldEarned;
            existing.farm = updatedDetail.farm;
            existing.visionScore = updatedDetail.visionScore;
            existing.championName = updatedDetail.championName;
            existing.nivel = updatedDetail.nivel;
            existing.items = updatedDetail.items;
            existing.idTeam = updatedDetail.idTeam;

            await _context.SaveChangesAsync();

            return existing;
        }
    }
}
