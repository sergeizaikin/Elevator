using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Elevator
{
    class GameEnvironment
    {
        public Elevator Elevator { get; set; }
        public Building Building { get; set; }
        public string HotKeyDestription { get; set; }

        public GameEnvironment(int floorCount, string hotKeyDescription = "")
        {
            
            Building = new Building(floorCount, this);
            Elevator = new Elevator(Building, this);
            HotKeyDestription = hotKeyDescription;
        }

        public void DrawScreen()
        {
            Console.Clear();
            Console.WriteLine("---");

            for (int i = Building.FloorsQuantity - 1; i >= 0; i--)
            {

                var waitingOnFloor = Building.WaitingPassengers[i] > 0 ? Building.WaitingPassengers[i].ToString() : "";
                var elevatorOrEmpty = i == Elevator.Position ? $"{Elevator.PassengersQuantity}" : "|";
                var floorNum = FormatFloorNumber(i == 0 ? "T:" : $"{i.ToString()}:", Building.FloorsQuantity.ToString());

                Console.WriteLine($"{floorNum} {elevatorOrEmpty} {waitingOnFloor}");

            }

            Console.WriteLine("---");
            Console.WriteLine("\n" + HotKeyDestription);
        }

        private string FormatFloorNumber(string floorNumber, string maxFloorNumber)
        {
            var totalLength = (maxFloorNumber + " ").Length;

            var result = floorNumber + new string(' ', totalLength - floorNumber.Length); // Add spaces

            return result;
        }

        public void AutomaticMode()
        {
            while (true)
            {

                if (Building.ExistWaitingPassengers())
                {
                    var maxWaitingFloor = Building.GetLastWaitingFloor();
                    Elevator.GoToFloor(maxWaitingFloor);
                    Elevator.GoDownCollectingPassengers();
                    Elevator.LeavePassengersOnFloor(Elevator.PassengersQuantity); // Arriving to the first floor leave all passengers
                    DrawScreen();
                }
                else
                    Building.GenerateRandomPassengers();

                Thread.Sleep(500);
            }
        }
    }
}
