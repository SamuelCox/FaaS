using FaaS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaaS.Data
{
    public class FaaSContext : DbContext
    {

        public FaaSContext(DbContextOptions<FaaSContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserConnection>().HasKey(table => new {
                table.UserName,
                table.AzureConnectionStringID
            });
        }

        public List<string> GetConnectionStrings(string username)
        {
            List<string> connections = (from u in Users
                                        join uc in UserConnections on u.UserName equals uc.UserName
                                        join azcs in AzureConnectionStrings on
                                        uc.AzureConnectionStringID equals azcs.AzureConnectionStringID                                        
                                        into joined
                                        where u.UserName == username
                                        from x in joined.DefaultIfEmpty()
                                        select x.ConnectionString).ToList();
            return connections;
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<UserConnection> UserConnections { get; set; }

        public virtual DbSet<AzureConnectionString> AzureConnectionStrings { get; set; }
    }
}
