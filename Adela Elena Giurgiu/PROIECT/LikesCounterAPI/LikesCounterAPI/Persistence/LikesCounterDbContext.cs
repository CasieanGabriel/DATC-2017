using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LikesCounterAPI.Models;

namespace LikesCounterAPI.Persistence
{
    public class LikesCounterDbContext : DbContext
    {
        public LikesCounterDbContext(DbContextOptions<LikesCounterDbContext> options) : base(options)
        {
        }
        public DbSet<Account> Account { get; set; }
    }
}
