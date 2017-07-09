using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FaaS.Models
{
    public class User : IdentityUser<string>
    {                
        public virtual ICollection<UserConnection> UserConnections { get; set; }
    }
}
