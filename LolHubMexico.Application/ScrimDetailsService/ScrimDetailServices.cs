using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Application.Exceptions;
using LolHubMexico.Application.Interfaces;
using LolHubMexico.Application.UserServices;
using LolHubMexico.Domain.DTOs.Players;
using LolHubMexico.Domain.DTOs.Scrims;
using LolHubMexico.Domain.DTOs.Users;
using LolHubMexico.Domain.Entities.DatailsScrims;
using LolHubMexico.Domain.Repositories.ScrimRepository;

namespace LolHubMexico.Application.ScrimDetailsService
{
    public class ScrimDetailServices
    {
        private readonly IDetailsScrimRepository _repository;
        private readonly IPlayerService _playerService;
        private readonly UserService _userService;
        public ScrimDetailServices(IDetailsScrimRepository repository, IPlayerService playerService, UserService userService) { 
            _repository = repository;
            _playerService = playerService;
            _userService = userService;
        }

        public async Task<List<UserLinkDTO>> GetDetailByIdAndTeam(int idScrim, int idTeam)
        {
            if (idScrim == 0)
                throw new AppException("Id scrim mal enviado");
            if (idTeam == 0)
                throw new AppException("Id Team mal enviado");

            var DetailsLst = await _repository.GetDetailsByIdAndTeam(idScrim, idTeam);

            if (DetailsLst == null)
                throw new AppException("No tiene detalles por procesar");

            var lstUserLink = new List<UserLinkDTO>();

            foreach(var detail in DetailsLst)
            {

                var player = await _playerService.GetPlayerByIdUser(detail.idUser);
                var user = await _userService.GetUserById(detail.idUser);
                
                var newUserLink = new UserLinkDTO
                {
                    user = user,
                    player = player
                };

                lstUserLink.Add(newUserLink);
            }

            return lstUserLink;
        }

        public async Task<List<DetailsScrim>> GetDetailsScrimById(int idscrim)
        {
            if(idscrim == null)
                throw new AppException("id null");

            var lstDetails = await _repository.GetDetailsByIdScrim(idscrim);

            if(lstDetails == null)
                throw new AppException("No tiene detalles por procesar");
           

            return lstDetails;
        }

        
    }
}
