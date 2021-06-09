using System;

namespace Elevator
{
    class Program
    {
        static void Main(string[] args)
        {
            var hotKeys = "Up Arrow: Go up\n" +
                   "Down Arrow: Go down \n" +
                   "Left Arrow: Get passengers from the elevator's floor\n" +
                   "Right Arrow: Leave one pasenger on the elevator's floor\n" +
                   "Q: Add passengers to a floor\n" +
                   "W: Add passengers to the elevator's floor\n" +
                   "S: Leave all passengers on the elevator's floor\n" +
                   "R: Generate random passengers on random floors\n" +
                   "A: Enable automatic mode\n";

            var Screen = new Screen(18, hotKeys);
            Screen.DrawScreen();

            while (true)
            {
                var insertedkey = Console.ReadKey().Key;

                switch (insertedkey)
                {
                    case ConsoleKey.UpArrow:
                        Screen.Elevator.ChangePosition(1);
                        break;

                    case ConsoleKey.DownArrow:
                        Screen.Elevator.ChangePosition(-1);
                        break;

                    case ConsoleKey.LeftArrow:
                        Screen.Elevator.GetPassengersFromFloor();
                        break;

                    case ConsoleKey.RightArrow:
                        Screen.Elevator.LeavePassengersOnFloor(1);
                        break;

                    case ConsoleKey.Q:
                        Console.WriteLine();
                        Console.Write("Enter a floor: ");
                        var floor = int.Parse(Console.ReadLine());

                        Console.Write("Enter a passengers quantity: ");
                        var waitingPassengers = int.Parse(Console.ReadLine());
                        Screen.Building.AddPassengersToFloor(floor, waitingPassengers);
                        break;

                    case ConsoleKey.W:
                        Screen.Building.AddPassengersToFloor(Screen.Elevator.Position, 1);
                        break;

                    case ConsoleKey.S:
                        Screen.Elevator.LeavePassengersOnFloor(Screen.Elevator.PassengersQuantity);
                        break;

                    case ConsoleKey.R:
                        Screen.Building.GenerateRandomPassengers();
                        break;

                    case ConsoleKey.A:
                        Screen.Elevator.AutomaticMode();
                        break;

                    default:
                        break;
                }

                Screen.DrawScreen();
            }
        }
    }
}
