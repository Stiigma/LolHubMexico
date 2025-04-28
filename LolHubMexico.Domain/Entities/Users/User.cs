using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.Entities.Users
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }
        public string UserName { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash {  get; set; }
        public string Nacionality { get; set; }
        public int Role {  get; set; }
        public DateTime Registration_date { get; set; }
        public int Status { get; set; }





    }
}
