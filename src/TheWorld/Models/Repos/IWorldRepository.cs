using System.Collections.Generic;

namespace TheWorld.Models.Repos
{
    public interface IWorldRepository
    {
        IEnumerable<Trip> GetAllTrips();
        IEnumerable<Trip> GetAllTripsWithStops();
        bool SaveAll();
        void AddTrip(Trip newTrip);
        Trip GetTripByName(string tripName);
        void AddStop(string tripName, Stop newStop);
        IEnumerable<Trip> GetUserTripsWithStops(string name);
    }
}