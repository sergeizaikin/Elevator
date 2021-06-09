using System;
using System.Collections.Generic;
using System.Text;

namespace Elevator
{
    class Screen
    {
        public Elevator Elevator { get; set; }
        public Building Building { get; set; }
        public string HotKeyDestription { get; set; }

        public Screen(int floorCount, string hotKeyDescription = "")
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

    }
}
