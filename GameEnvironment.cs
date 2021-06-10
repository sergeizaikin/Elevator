using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using static Elevator.Entities.Enums;

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

            for (int i = Building.Floors.Count - 1; i >= 0; i--)
            {

                var waitingOnFloor = Building.GetFloorByNumber(i).WaitingPassengers.Count > 0 ? Building.GetFloorByNumber(i).WaitingPassengers.Count.ToString() : "";
                var elevatorOrEmpty = i == Elevator.CurrentFloor ? $"{Elevator.Passengers.Count}" : "|";
                var floorNum = FormatFloorNumber(i == 0 ? "T:" : $"{i.ToString()}:", Building.Floors.Count.ToString());

                screenDataNew += $"{floorNum} {elevatorOrEmpty} {waitingOnFloor}\n";

            }

            screenDataNew += "---\n";
            screenDataNew += $"Max capacity: {Elevator.MaxPassengerCapacity}\n";
            screenDataNew += $"Passengers:\n";
            foreach (var passenger in Elevator.Passengers)
            {
                screenDataNew += $"           {passenger.ToString()}\n";
            }

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

            if (screenDataRowsNew.Length > screenDataOld.Length)
            {
                for (int i = 0; i < screenDataRowsNew.Length; i++)
                {
                    if (i < screenDataOld.Length)
                    {
                        if (screenDataRowsOld[i] != screenDataRowsNew[i])
                        {
                            ReplaceConsoleLine(i, screenDataRowsNew[i] + "\n");
                        }
                    }
                    else
                    {
                        ReplaceConsoleLine(i, screenDataRowsNew[i] + "\n");
                    }

                }
            }
            else
            {
                for (int i = 0; i < screenDataRowsOld.Length; i++)
                {
                    if (i < screenDataRowsNew.Length)
                    {
                        if (screenDataRowsOld[i] != screenDataRowsNew[i])
                        {
                            ReplaceConsoleLine(i, screenDataRowsNew[i] + "\n");
                        }
                    }
                    else
                    {
                        ReplaceConsoleLine(i, "");
                    }

                }
            }

        }

        public void AutomaticMode()
        {
            while (true)
            {

                if (Building.ThereAreWaitingPassengers())
                {
                    if (Elevator.Direction == Direction.Down)
                    {
                        var maxWaitingFloor = Building.GetLastWaitingFloor();
                        Elevator.GoToFloor(maxWaitingFloor.Number);
                        Elevator.GoDownPickingUpPassengers();
                        Elevator.LeavePassengersOnFloor(); // Arriving to the first floor leave all passengers
                        DrawScreen();
                        Elevator.Direction = Direction.Up;
                    }
                    else
                    {

                        Building.ChangePassengersDestination();
                        Elevator.GoUpDeliveringPassengers();
                        Elevator.Direction = Direction.Down;
                    }

                }
                else
                {
                    Building.GenerateRandomPassengers();
                    DrawScreen();
                }

                Thread.Sleep(500);
            }
        }
    }
}
