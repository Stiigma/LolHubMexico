using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.DTOs.Players;
using LolHubMexico.Domain.DTOs.Users;

namespace LolHubMexico.Domain.DTOs.Scrims
{
    public class UserLinkDTO
    {
        public UserDTO user { get; set; }

        public PlayerDTO player { get; set; }
    }
}
