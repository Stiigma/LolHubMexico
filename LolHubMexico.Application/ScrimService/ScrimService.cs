using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Application;
using LolHubMexico.Application.Exceptions;
using LolHubMexico.Application.Interfaces;
using LolHubMexico.Application.PlayerService;
using LolHubMexico.Application.UserServices;
using LolHubMexico.Domain.DTOs.Scrims;
using LolHubMexico.Domain.Entities.DatailsScrims;
using LolHubMexico.Domain.Entities.Scrims;
using LolHubMexico.Domain.Entities.Users;
using LolHubMexico.Domain.Enums;
using LolHubMexico.Domain.Repositories.PlayerRepository;
using LolHubMexico.Domain.Repositories.ScrimRepository;

namespace LolHubMexico.Application.ScrimServices
{
    public class ScrimService
    {
        private readonly IScrimRepository _scrimRepository;
        private readonly TeamService _teamService;
        private readonly UserService _userService;
        private readonly IPlayerRepository _playerService;
        private readonly IDetailsScrimRepository _detailsScrimRepository;
        public ScrimService(IScrimRepository scrimRepository, TeamService scrimService, IDetailsScrimRepository detailsScrimRepository, UserService userService, IPlayerRepository playerService ) { 
            _scrimRepository = scrimRepository;
            _teamService = scrimService;
            _detailsScrimRepository = detailsScrimRepository;
            _userService = userService;
            _playerService = playerService;
        }

        public async Task<bool> CreateScrim(CreateScrimDTO newDto)
        {
            if(newDto == null) return false;

            if(!IsValidScheduledDate(newDto.scheduled_date))
                throw new AppException("La Fecha es invalida'");

            bool IsCapitan = await _teamService.IsCapitan(newDto.created_by);

            if(!IsCapitan)
                throw new AppException("No es capitan de Equipo");

            var team = await _teamService.GetTeamByUserId(newDto.created_by);

            var isComplete = await _teamService.TeamMemberComplete(team.IdTeam);

            if(!isComplete)
                throw new AppException("El equipo debe de tener 5 miembros");


            var ensenadaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var nowInEnsenada = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, ensenadaTimeZone);
            if (newDto.created_by != team.IdCapitan)
                throw new AppException("No es capitan de Equipo");

            var lstDeatails = new List<DetailsScrim>();
            var scrim = new Scrim
            {
                idScrim = 0
            };
           

            if(newDto.idTeam2 == 0)
            {
                var newScrim = new Scrim
                {
                    idTeam1 = team.IdTeam,
                    scheduled_date = newDto.scheduled_date,
                    created_by = newDto.created_by,
                    created_at = nowInEnsenada,
                    status = 0,
                    description = newDto.description,
                    tittle = newDto.tittle,                    
                    result_verification = "pending",
                };

                scrim = await _scrimRepository.CreateScrim(newScrim);

                

                foreach (var idUser in newDto.idsUsers)
                {
                    var user = await _userService.GetUserById(idUser);
                    var player = await _playerService.GetPlayerByIdUser(idUser);
                    if (user == null || player == null)
                    {
                        await _scrimRepository.DeleteScrim(scrim);
                        throw new AppException($"Error: Jugador {user!.UserName ?? "Desconocido"} No vinculado");

                    }


                    var datails = new DetailsScrim
                    {
                        idScrim = scrim.idScrim,
                        idPlayer = player.IdPlayer,
                        idUser = user.IdUser,
                        puuid = player.Puuid,
                        idTeam = team.IdTeam,
                    };
                    lstDeatails.Add(datails);

                }

            }
            else
            {

                var IsCompleteRival = await _teamService.TeamMemberComplete(newDto.idTeam2);

                if (!IsCompleteRival)
                    throw new AppException("Team Rival no completo");

                var newScrim = new Scrim
                {
                    idTeam1 = team.IdTeam,
                    idTeam2 = newDto.idTeam2,
                    scheduled_date = newDto.scheduled_date,
                    created_by = newDto.created_by,
                    created_at = nowInEnsenada,
                    status = 1,
                    description = newDto.description,
                    tittle = newDto.tittle,
                    result_verification = "pending",
                };

                scrim = await _scrimRepository.CreateScrim(newScrim);



                foreach (var idUser in newDto.idsUsers)
                {
                    var user = await _userService.GetUserById(idUser);
                    var player = await _playerService.GetPlayerByIdUser(idUser);
                    if (user == null || player == null)
                    {
                        await _scrimRepository.DeleteScrim(scrim);
                        throw new AppException($"Error: Jugador {user!.UserName ?? "Desconocido"} No vinculado");

                    }
                        

                    
                    

                    var datails = new DetailsScrim
                    {
                        idScrim = scrim.idScrim,
                        idPlayer = player.IdPlayer,
                        idUser = user.IdUser,
                        puuid = player.Puuid,
                    };

                    lstDeatails.Add(datails);
                }

            }

            if(lstDeatails.Count >= 5)
            {
                foreach (var item in lstDeatails)
                {
                    await _detailsScrimRepository.CreateDetailScrim(item);
                }
            }
            else
            {
                await _scrimRepository.DeleteScrim(scrim);
                throw new AppException($"Error: No se completo Los 5 Miembros por Algun Error");
            }



            return true;
        }

        public async Task<List<ScrimPDTO>> GetScrimActiveTeam(int idteam)
        {
            if (idteam == 0)
                throw new AppException($"Error");

            var team = await _teamService.getTeamById(idteam);
            // Obtener scrims manejando posibles nulls
            var scrimsAsTeam1 = await _scrimRepository.GetScrimsByTeam1(team.IdTeam) ?? new List<Scrim>();
            var scrimsAsTeam2 = await _scrimRepository.GetScrimsByTeam2(team.IdTeam) ?? new List<Scrim>();

            // Combinar y filtrar por status = 4
            var filteredScrims = scrimsAsTeam1.Concat(scrimsAsTeam2)
                                              .Where(s => s.status == 4)
                                              .ToList();

            // Mapear a DTO
            var lstScrimDto = new List<ScrimPDTO>();
            foreach (var item in filteredScrims)
            {
                lstScrimDto.Add(new ScrimPDTO
                {
                    idScrim = item.idScrim,
                    idTeam1 = item.idTeam1,
                    idTeam2 = item.idTeam2,
                    description = item.description,
                    scheduled_date = item.scheduled_date,
                    status = item.status,
                    tittle = item.tittle,                  
                });
            }
            return lstScrimDto;
        }


        public async Task<bool> AcceptScrim(RivalDTO rivalDTO)
        {
            if(rivalDTO == null) return false;
            
            var scrim = await _scrimRepository.GetScrimById(rivalDTO.idScrim);

            if (scrim == null)
                throw new AppException("La Scrim no existe");

            if (!rivalDTO.IsAccept)
            {
                
                scrim.idTeam2 = 0;
                scrim.status = (int)ScrimStatus.Open;
                await _scrimRepository.UpdateScrim(scrim);
                return false;
            }
                //return false;

            bool IsCapitan = await _teamService.IsCapitan(rivalDTO.idrival);

            if (!IsCapitan)
                throw new AppException("No eres capitan de Equipo");



            var team = await _teamService.GetTeamByUserId(rivalDTO.idrival);


            var isComplete = await _teamService.TeamMemberComplete(team.IdTeam);

            if (!isComplete)
                throw new AppException("El equipo debe de tener 5 miembros");

           

            

            scrim.idTeam2 = team.IdTeam;
            scrim.status = (int)ScrimStatus.Confirmed;


            await _scrimRepository.UpdateScrim(scrim);
            var lstDeatails = new List<DetailsScrim>();

            foreach (var idUser in rivalDTO.idsUsers)
            {
                var user = await _userService.GetUserById(idUser);
                var player = await _playerService.GetPlayerByIdUser(idUser);
                if (user == null || player == null)
                {
                    throw new AppException($"Error: Jugador {user!.UserName ?? "Desconocido"} No vinculado");

                }





                var datails = new DetailsScrim
                {
                    idScrim = scrim.idScrim,
                    idPlayer = player.IdPlayer,
                    idUser = user.IdUser,
                    puuid = player.Puuid,
                    idTeam = team.IdTeam
                };

                lstDeatails.Add(datails);
            }

            if (lstDeatails.Count >= 5)
            {
                foreach (var item in lstDeatails)
                {
                    await _detailsScrimRepository.CreateDetailScrim(item);
                }
            }
            else
            {
                return false;
            }

                return true;

        }

        public async Task<List<ScrimPDTO>> GetScrimsPending()
        {
            var lstScrims = await _scrimRepository.GetAllScrims();

            var pendingScrims = lstScrims
                .Where(s => s.status == 0)
                .Select(s => new ScrimPDTO
                {
                    idScrim = s.idScrim,
                    idTeam1 = s.idTeam1,
                    idTeam2 = s.idTeam2,
                    scheduled_date = s.scheduled_date,
                    description = s.description,
                    tittle = s.tittle,
                    status = s.status
                })
                .ToList();

            return pendingScrims;
        }

        public async Task<List<ScrimPDTO>> GetScrimsByIdUser(int idUser)
        {
            var teamMember = await _teamService.GetTeamByUserId(idUser);

            var scrimsTeam1 = await _scrimRepository.GetScrimsByTeam1(teamMember.IdTeam);
            var scrimsTeam2 = await _scrimRepository.GetScrimsByTeam2(teamMember.IdTeam);

            var allScrims = scrimsTeam1
                .Concat(scrimsTeam2)
                //.Where(s => s.Status == 0) // opcional si ya lo filtras en el repo
                .DistinctBy(s => s.idScrim) // evitar duplicados si aplica
                .ToList();

            var scrimDTOs = allScrims.Select(s => new ScrimPDTO
            {
                idScrim = s.idScrim,
                idTeam1 = s.idTeam1,
                idTeam2 = s.idTeam2,
                scheduled_date = s.scheduled_date,
                description = s.description,
                tittle = s.tittle,
                status = s.status
            }).ToList();

            return scrimDTOs;
        }

        public async Task<bool> InsertResultMatchByTeam(ScrimResultReportDTO dto) { 

            if(dto == null)
                throw new AppException("Viene nullo el DTO");

            var scrim = await _scrimRepository.GetScrimById(dto.IdScrim);

            if(scrim == null)
                throw new AppException("Esta Scrim ya no esta disponible");
            var ensenadaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var nowInEnsenada = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, ensenadaTimeZone);

            if (scrim.idTeam1 == dto.IdTeam)
            {
                scrim.imagePath1 = dto.ImagePath ?? "";
                scrim.team1_reported_at = nowInEnsenada;
                scrim.result_verification = "Por Validar";
                scrim.team1_result_reported = dto.Win;
                scrim.idMatch1 = dto.IdMatch;
            }
            else
            {
                scrim.imagePath2 = dto.ImagePath ?? "";
                scrim.team2_reported_at = nowInEnsenada;
                scrim.result_verification = "Por Validar";
                scrim.team2_result_reported = dto.Win;
                scrim.idMatch2 = dto.IdMatch;
            }

          
            return true;
        }

        public async Task<List<ScrimPDTO>> GetScrimsByIdUserActives(int idUser)
        {
            var teamMember = await _teamService.GetTeamByUserId(idUser);

            var scrimsTeam1 = await _scrimRepository.GetScrimsByTeam1(teamMember.IdTeam);
            var scrimsTeam2 = await _scrimRepository.GetScrimsByTeam2(teamMember.IdTeam);

            var allScrims = scrimsTeam1
                .Concat(scrimsTeam2)
                //.Where(s => s.status != 0)
                .DistinctBy(s => s.idScrim) // evitar duplicados si aplica
                .ToList();

            var scrimDTOs = allScrims.Select(s => new ScrimPDTO
            {
                idScrim = s.idScrim,
                idTeam1 = s.idTeam1,
                idTeam2 = s.idTeam2,
                scheduled_date = s.scheduled_date,
                description = s.description,
                tittle = s.tittle,
                status = s.status
            }).ToList();

            return scrimDTOs;
        }

        public async Task<ScrimPDTO> GetScrimById( int idscrim)
        {
            if(idscrim == null)
                throw new AppException("valor nullo");

            var scrim = await _scrimRepository.GetScrimById(idscrim);
            if(scrim == null)
                throw new AppException("No existe id");

            var newScrimdto = new ScrimPDTO
            {
                idScrim = scrim.idScrim,
                idTeam1 = scrim.idTeam1,
                idTeam2 = scrim.idTeam2,
                scheduled_date = scrim.scheduled_date,
                tittle = scrim.tittle,
                description = scrim.description,
                status = scrim.status

            };

            return newScrimdto;

        }

        public async Task<ScrimPDTO> updateScrim(ScrimPDTO scrimPDTO)
        {
            if (scrimPDTO == null)
                throw new AppException("Viene vacio");

            var scrim = await _scrimRepository.GetScrimById(scrimPDTO.idScrim);

            scrim.tittle = scrimPDTO.tittle;
            scrim.scheduled_date = scrimPDTO.scheduled_date;
            scrim.description = scrimPDTO.description;

            var scrimEdit = await _scrimRepository.UpdateScrim(scrim);

            var newScrimdto = new ScrimPDTO
            {
                idScrim = scrimEdit.idScrim,
                idTeam1 = scrimEdit.idTeam1,
                idTeam2 = scrimEdit.idTeam2,
                scheduled_date = scrimEdit.scheduled_date,
                tittle = scrimEdit.tittle,
                description = scrimEdit.description,
                status = scrimEdit.status

            };

            return newScrimdto;
        }
        public async Task<Scrim> updateScrimv2(Scrim scrimPDTO)
        {
            if (scrimPDTO == null)
                throw new AppException("Viene vacio");

            var scrim = await _scrimRepository.GetScrimById(scrimPDTO.idScrim);

            scrim.tittle = scrimPDTO.tittle;
            scrim.scheduled_date = scrimPDTO.scheduled_date;
            scrim.description = scrimPDTO.description;

            var scrimEdit = await _scrimRepository.UpdateScrim(scrim);
            return scrimEdit;            
        }

        private bool IsValidScheduledDate(DateTime scheduledDate)
        {
            var now = DateTime.Now;

            // Verificar que no sea de un día anterior
            if (scheduledDate.Date < now.Date)
                return false;

            // Verificar que sea al menos 10 minutos después de ahora
            if (scheduledDate < now.AddMinutes(10))
                return false;

            return true;
        }

    }
}
