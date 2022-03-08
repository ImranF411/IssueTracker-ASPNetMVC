﻿#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IssueTracker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace IssueTracker.Data
{
    public class IssueTrackerContext : IdentityDbContext
    {
        public IssueTrackerContext (DbContextOptions<IssueTrackerContext> options)
            : base(options)
        {
        }

        public DbSet<IssueTracker.Models.Ticket> Tickets { get; set; }

        public DbSet<IssueTracker.Models.User> Users { get; set; }
    }
}
