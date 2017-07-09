using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FaaS.Models
{
    public class UserConnection
    {
        [Key, Column(Order = 0)]        
        public string Id { get; set; }

        
        public virtual User User { get; set; }

        [Key, Column( Order = 1)]        
        public int AzureConnectionStringID { get; set; }

        
        public virtual AzureConnectionString AzureConnectionString { get; set; }

    }
}
