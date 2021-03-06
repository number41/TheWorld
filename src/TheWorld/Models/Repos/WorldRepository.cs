﻿using Microsoft.Data.Entity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Models.Repos
{
    public class WorldRepository : IWorldRepository
    {
        private WorldContext dbContext;
        private ILogger<WorldRepository> logger;

        public WorldRepository(WorldContext dbContext, ILogger<WorldRepository> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public void AddStop(string tripName, Stop newStop)
        {
            var theTrip = GetTripByName(tripName);
            newStop.Order = theTrip.Stops.Max(s => s.Order) + 1;
            theTrip.Stops.Add(newStop);
            dbContext.Stops.Add(newStop);
        }

        public void AddTrip(Trip newTrip)
        {

            dbContext.Add(newTrip);
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            try
            {
                return dbContext.Trips.OrderBy(t => t.Name).ToList();
            }
            catch (Exception e)
            {
                logger.LogError("Couldn't fetch the trips!", e);
                throw e;
            }
        }

        public IEnumerable<Trip> GetAllTripsWithStops()
        {
            try
            {
                return dbContext.Trips
                    .Include(t => t.Stops)
                    .OrderBy(t => t.Name)
                    .ToList();
            }
            catch (Exception e)
            {
                logger.LogError("could not fetch all trips with stops");
                throw e;
            }
        }

        public Trip GetTripByName(string tripName)
        {
            return dbContext.Trips
                .Include(t => t.Stops)
                .Where(t => t.Name == tripName)
                .FirstOrDefault();
        }

        public IEnumerable<Trip> GetUserTripsWithStops(string name)
        {
            try
            {
                return dbContext.Trips
                    .Include(t => t.Stops)
                    .OrderBy(t => t.Name)
                    .Where(t => t.UserName == name)
                    .ToList();
            }
            catch (Exception e)
            {
                logger.LogError("could not fetch all trips with stops");
                throw e;
            }
        }

        public bool SaveAll()
        {
            return dbContext.SaveChanges() > 0;
        }
    }
}
