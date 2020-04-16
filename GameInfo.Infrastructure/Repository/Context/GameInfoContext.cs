using GameInfo.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameInfo.Infrastructure.Repository.Context
{
    public class GameInfoContext : DbContext
    {
        public GameInfoContext(DbContextOptions<GameInfoContext> options) : base(options)
        {

        }
        public DbSet<User> User { get; set; }
        public DbSet<Game> Game { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().HasQueryFilter(p => p.Deleted == false);
        }
    }
}
