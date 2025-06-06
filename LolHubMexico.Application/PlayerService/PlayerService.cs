using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Application.Exceptions;
using LolHubMexico.Application.Interfaces;
using LolHubMexico.Application.UserServices;
using LolHubMexico.Domain.DTOs.Players;
using LolHubMexico.Domain.Entities.Players;
using LolHubMexico.Domain.Repositories.PlayerRepository;
using LolHubMexico.Domain.Enums;
using LolHubMexico.Domain.Repositories.UserRepository;
using static System.Net.WebRequestMethods;

namespace LolHubMexico.Application.PlayerService
{
    public class PlayerService : IPlayerService
    {
        private readonly IRiotService _riotService;
        private readonly UserService _userService;
        private readonly IPlayerRepository _playerRepository;
        //private readonly IPlayerRepository _playerRepository;

        public PlayerService(IRiotService riotService, IPlayerRepository playerRepository, UserService userService)
        {
            _riotService = riotService;
            _userService = userService;
            _playerRepository = playerRepository;
        }

        public async Task<RspPlayerLinkDTO> LinkSummonerAsync(int userId, string summonerName, string region, string MainRole)
        {

            var user = await _userService.ExistUser(userId);

            if (!user)
                throw new AppException("El id del Usuario no Existe");
            
            var accountRiot = await _riotService.GetSummonerByNameAsync(region, summonerName);
            if (accountRiot == null)
                throw new AppException("Summoner not found.");

            var summoners = await _riotService.GetSummonerByPuiid(accountRiot.Puuid);

            if (summoners == null)
                throw new AppException("Error Interno: Puuid No valido");

            var newPlayer = new Player
            {
                IdUser = userId,
                SummonerId = summoners.SummonerId,
                SummonerName = summonerName,
                Level = summoners.SummonerLevel,
                MainRole = MainRole,
                ProfilePicture = $"https://ddragon.leagueoflegends.com/cdn/14.10.1/img/profileicon/{summoners.ProfileIconId}.png",
                Puuid = summoners.Puuid,
                Status = 1,
                Region = "LAN",
                Verified = true,
            };

            var IsChangeRole = await _userService.ChangeRoleByUserId(userId, UserRole.Player);
            
            if(!IsChangeRole)
                throw new AppException("Error Interno: Error con Base de datos");

            var createdPlayer = await _playerRepository.CreatePlayer(newPlayer);

            //var IsChangeRole = await _userService.ChangeRoleByUserId(userId, UserRole.Player);

            if (createdPlayer == null)
                throw new AppException("Error Interno: Algo paso al registrarlo");





            return new RspPlayerLinkDTO
            {
                IdPlayer = createdPlayer.IdPlayer,
                IdUser = userId,
                SummonerName = createdPlayer.SummonerName,
                Region = region,
            };


        }

        public async Task<PlayerDTO> GetPlayerByIdUser(int idUser)
        {
            if(idUser == null)
                throw new AppException("El id es nulo");

            var player = await _playerRepository.GetPlayerByIdUser(idUser);

            if(player == null)
                throw new AppException("Error 320: Player No Vinculado");

            var playerOut = new PlayerDTO
            {
                IdPlayer = player.IdPlayer,
                IdUser = player.IdUser,
                Level = player.Level,
                MainRole = player.MainRole,
                ProfilePicture = player.ProfilePicture,
                Puuid = player.Puuid,
                SummonerName = player.SummonerName,
            };

            return playerOut;
        }
    }

}
