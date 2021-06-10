using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using static Elevator.Entities.Enums;

namespace Elevator.Entities
{
    class Passenger
    {
        public int ApartmentFloor { get; set; }
        public int DestinationFloor { get; set; }
        public string Name { get; set; }
        public Building Building { get; set; }
        public Floor Floor { get; set; }
        private int NameMaxLength { get; set; } = 30;

        public Passenger(int apartmentFloor, Building building, int currentFloor = 0)
        {
            Building = building;
            ApartmentFloor = apartmentFloor;
            Floor = building.GetFloorByNumber(currentFloor);
            Name = GetRandomName();
            // If the passenger is on the first floor then go to its apartment
            DestinationFloor = currentFloor == 0 ? ApartmentFloor : 0;
        }

        public Passenger(Building building, int currentFloor = 0)
        {
            Building = building;
            Thread.Sleep(5);
            ApartmentFloor = new Random().Next(1, Building.Floors.Count + 1);
            Floor = building.GetFloorByNumber(currentFloor);
            Name = GetRandomName();

            // If the passenger is on the first floor then go to its apartment
            DestinationFloor = currentFloor == 0 ? ApartmentFloor : 0;
        }

        private string GetRandomName()
        {
            var allNames = File.ReadAllLines(@"Other data\Names.txt");
            var allSurenames = File.ReadAllLines(@"Other data\Surenames.txt");
            var randomNameNumber = new Random().Next(allNames.Length - 1);
            var randomSurenameNumber = new Random().Next(allNames.Length - 1);

            return $"{allNames[randomNameNumber]} {allSurenames[randomSurenameNumber]}";
        }

        public void TryToChangeDestination()
        {
            Thread.Sleep(5);
            bool changeDestination = new Random().Next(0, 2) == 1 ? true : false;

            if (changeDestination)
            {
                if (Floor.Number == 0)
                {
                    DestinationFloor = ApartmentFloor;
                }
                else
                {
                    DestinationFloor = 0;
                }
            }

        }

        public override string ToString()
        {
            return $"Name:{Name}{new String(' ', NameMaxLength - Name.Length)} | Destination: {DestinationFloor} | Apartment: {ApartmentFloor}";
        }

    }
}
