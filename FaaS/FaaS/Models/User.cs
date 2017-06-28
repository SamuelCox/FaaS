using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FaaS.Models
{
    public class User
    {
        [Key]
        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public string Salt { get; set; }

        public virtual ICollection<UserConnection> UserConnections { get; set; }
    }
}
