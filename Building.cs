using System;
using System.Collections.Generic;
using System.Text;

namespace Elevator
{
    class Building
    {
        public int FloorsQuantity { get; set; }
        public int[] WaitingPassengers { get; set; }
        public GameEnvironment GameEnvironment { get; set; }

        public Building(int floorsQuantity, GameEnvironment gameEnvironment)
        {
            FloorsQuantity = floorsQuantity;
            WaitingPassengers = new int[floorsQuantity];
            GameEnvironment = gameEnvironment;
        }

        public void AddPassengersToFloor(int floor, int passengersQuantity)
        {
            WaitingPassengers[floor] += passengersQuantity;
        }

        public void GenerateRandomPassengers()
        {
            var quantityOfFloors = new Random().Next(0, FloorsQuantity);

            for (int i = 0; i < quantityOfFloors; i++)
            {
                var passengerQuantity = new Random().Next(0, 6);
                var floor = new Random().Next(0, FloorsQuantity);

                if (WaitingPassengers[floor] != 0)
                    continue;

                AddPassengersToFloor(floor, passengerQuantity);
            }
        }

        public bool ExistWaitingPassengers()
        {
            for (int i = 1; i < FloorsQuantity; i++)
            {
                if (WaitingPassengers[i] > 0)
                    return true;
            }

            return false;
        }

        public int GetLastWaitingFloor()
        {
            for (int i = FloorsQuantity - 1; i >= 0; i--)
            {
                if (WaitingPassengers[i] > 0)
                    return i;
            }

            return 0;
        }
    }
}
