using System;
using System.Collections.Generic;
using System.Text;

namespace Elevator
{
    class Building
    {
        public int FloorsQuantity { get; set; }
        public int[] WaitingPassengers { get; set; }
        public Screen Screen { get; set; }

        public Building(int floorsQuantity, Screen screen)
        {
            FloorsQuantity = floorsQuantity;
            WaitingPassengers = new int[floorsQuantity];
            Screen = screen;
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
    }
}
