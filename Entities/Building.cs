using Elevator.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Elevator
{
    class Building
    {
        public GameEnvironment GameEnvironment { get; set; }
        //public List<Passenger> WaitingPassengers { get; set; } = new List<Passenger>();
        public List<Floor> Floors { get; set; } = new List<Floor>();

        public Building(int floorsQuantity, GameEnvironment gameEnvironment)
        {

            for (int i = 0; i < floorsQuantity; i++)
            {
                Floors.Add(new Floor(i));
            }

            GameEnvironment = gameEnvironment;
        }

        public void AddPassengersToFloor(int floorNumber, List<Passenger> passengers)
        {
            Floors.Where(f => f.Number == floorNumber).First().WaitingPassengers.AddRange(passengers);
            passengers.ForEach(x => x.Floor = GetFloorByNumber(floorNumber));
        }

        public void AddOnePassengerToFloor(int floorNumber, Passenger passenger)
        {
            Floors.Where(f => f.Number == floorNumber).First().WaitingPassengers.AddRange(new List<Passenger> { passenger });
        }

        public void RemovePassengersFromFloor(int floorNumber, List<Passenger> passengersToRemove)
        {
            var listOfPassengers = GetFloorByNumber(floorNumber).WaitingPassengers;
            GetFloorByNumber(floorNumber).WaitingPassengers = listOfPassengers.Except(passengersToRemove).ToList();
            passengersToRemove.ForEach(x => x.Floor = null);
        }

        public void GenerateRandomPassengers()
        {
            var quantityOfFloors = new Random().Next(0, Floors.Count());

            // After getting a random quantity of floors we need to get a random number of a floor and a random quantity of passengers
            for (int i = 0; i < quantityOfFloors; i++)
            {
                var randomPassengersQuantity = new Random().Next(0, 6);
                var newPassengers = new List<Passenger>();
                var randomFloorNumber = new Random().Next(0, quantityOfFloors + 1);
                Thread.Sleep(randomFloorNumber);
                var randomApartmentFloorNumber = new Random().Next(1, quantityOfFloors + 1);

                // If there are already waiting passengers then there's no need to create new ones
                if (Floors.Where(f => f.Number == randomFloorNumber).First().WaitingPassengers.Count() > 0)
                    continue;

                for (int p = 0; p < randomPassengersQuantity; p++)
                {
                    Floors.Where(f => f.Number == randomFloorNumber).First().WaitingPassengers.Add(new Passenger(randomApartmentFloorNumber, this, randomFloorNumber));
                }
            }
        }

        public bool ThereAreWaitingPassengers()
        {
            if (Floors.Where(x => x.Number != 0).Any(f => f.WaitingPassengers.Count() > 0))
                return true;

            return false;
        }

        public Floor GetLastWaitingFloor()
        {
            var result = Floors.Where(f => f.WaitingPassengers.Count() > 0).OrderByDescending(x => x.Number).First();

            return result;
        }

        public Floor GetFloorByNumber(int floorNumber)
        {
            return Floors.Where(f => f.Number == floorNumber).First();
        }

        public void ChangePassengersDestination()
        {
            foreach (var floor in Floors)
            {
                foreach (var passenger in floor.WaitingPassengers)
                {
                    passenger.TryToChangeDestination();
                }
            }
        }

    }
}
