using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Models.Repos
{
    public class WorldRepository : IWorldRepository
    {
        private WorldContext dbContext;

        public WorldRepository(WorldContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            return dbContext.Trips.OrderBy(t => t.Name).ToList();
        }

        public IEnumerable<Trip> GetAllTripsWithStops()
        {
            return dbContext.Trips
                .Include(t => t.Stops)
                .OrderBy(t => t.Name)
                .ToList();
        }
    }
}
