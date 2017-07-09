using FaaS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaaS.Data
{
    public class FaaSContext : IdentityDbContext<User, IdentityRole<string>,string>
    {

        public FaaSContext(DbContextOptions<FaaSContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUser<string>>(i =>
            {                
                i.HasKey(x => x.Id);
            });

            builder.Entity<IdentityUserLogin<string>>(i =>
            {
                i.HasKey(x => x.UserId);

            });

            builder.Entity<IdentityUserRole<string>>(i =>
            {
                i.HasKey(x => x.UserId);

            });

            builder.Entity<IdentityUserToken<string>>(i =>
            {
                i.HasKey(x => x.UserId);
            });

            builder.Entity<UserConnection>().HasKey(table => new {
                table.Id,
                table.AzureConnectionStringID
            });
        }

        public List<string> GetConnectionStrings(string username)
        {
            List<string> connections = (from u in Users
                                        join uc in UserConnections on u.Id equals uc.Id
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
