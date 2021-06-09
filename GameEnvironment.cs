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
        private string ConsoleBuffer { get; set; } = "";

        public GameEnvironment(int floorCount, string hotKeyDescription = "", int speed = 600)
        {
            
            Building = new Building(floorCount, this);
            Elevator = new Elevator(Building, this, speed);
            HotKeyDestription = hotKeyDescription;
        }

        public void DrawScreen()
        {
            Console.CursorVisible = false;

            var screenDataNew = "";

            screenDataNew += "---\n";

            for (int i = Building.FloorsQuantity - 1; i >= 0; i--)
            {

                var waitingOnFloor = Building.WaitingPassengers[i] > 0 ? Building.WaitingPassengers[i].ToString() : "";
                var elevatorOrEmpty = i == Elevator.Position ? $"{Elevator.PassengersQuantity}" : "|";
                var floorNum = FormatFloorNumber(i == 0 ? "T:" : $"{i.ToString()}:", Building.FloorsQuantity.ToString());

                screenDataNew += $"{floorNum} {elevatorOrEmpty} {waitingOnFloor}\n";

            }

            screenDataNew += "---\n";
            screenDataNew += $"Max capacity: {Elevator.MaxPassengerCapacity}\n";
            screenDataNew += "\n" + HotKeyDestription;

            if (ConsoleBuffer != "")
            {
                UpdateModifiedLines(ConsoleBuffer, screenDataNew);
                ConsoleBuffer = screenDataNew;
            }
            else
            {
                Console.WriteLine(screenDataNew);
                ConsoleBuffer = screenDataNew;
            }
        }

        private string FormatFloorNumber(string floorNumber, string maxFloorNumber)
        {
            var totalLength = (maxFloorNumber + " ").Length;

            var result = floorNumber + new string(' ', totalLength - floorNumber.Length); // Add spaces

            return result;
        }

        private void ReplaceConsoleLine(int lineNumber, string newText)
        {
            int x = Console.CursorLeft;
            int y = Console.CursorTop;

            Console.SetCursorPosition(0, lineNumber);
            Console.Write(new String(' ', 70));
            Console.SetCursorPosition(0, lineNumber);
            Console.WriteLine(newText);

            Console.SetCursorPosition(x, y); // Restore position

        }

        private void UpdateModifiedLines(string screenDataOld, string screenDataNew)
        {
            string[] screenDataRowsOld = screenDataOld.Split('\n');
            string[] screenDataRowsNew = screenDataNew.Split('\n');

            for (int i = 0; i < screenDataRowsOld.Length; i++)
            {
                if(screenDataRowsOld[i] != screenDataRowsNew[i])
                {
                    ReplaceConsoleLine(i, screenDataRowsNew[i] + "\n");
                }
            }
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
