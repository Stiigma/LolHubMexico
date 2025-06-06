using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.Enums
{
    public enum UserRole
    {
        Admin = 1,
        UserCommon = 2,      // Usuario común que aún no ha vinculado su cuenta
        Player = 3 // Usuario que ya vinculó su cuenta de invocador
    }
}
