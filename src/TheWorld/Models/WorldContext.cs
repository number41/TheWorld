using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using TheWorld.Configs;
using Microsoft.Extensions.OptionsModel;

namespace TheWorld.Models
{
    public class WorldContext : DbContext
    {
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Stop> Stops { get; set; }

        DBSettings Options { get; }

        public WorldContext(IOptions<AppSettings> optionsAccessor)
        {
            Options = optionsAccessor.Value.Data;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Options.WorldContextConnection);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
