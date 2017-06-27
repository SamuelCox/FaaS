﻿using FaaS.Models;
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

        public virtual DbSet<User> Users { get; set; }
    }
}
