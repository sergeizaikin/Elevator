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

            var gameEnvironment = new GameEnvironment(18, hotKeys, 10);
            gameEnvironment.DrawScreen();

            while (true)
            {
                var insertedkey = Console.ReadKey().Key;

                switch (insertedkey)
                {
                    case ConsoleKey.UpArrow:
                        gameEnvironment.Elevator.ChangePosition(1);
                        break;

                    case ConsoleKey.DownArrow:
                        gameEnvironment.Elevator.ChangePosition(-1);
                        break;

                    case ConsoleKey.LeftArrow:
                        gameEnvironment.Elevator.GetPassengersFromFloor();
                        break;

                    case ConsoleKey.RightArrow:
                        gameEnvironment.Elevator.LeavePassengersOnFloor(1);
                        break;

                    case ConsoleKey.Q:
                        Console.WriteLine();
                        Console.Write("Enter a floor: ");
                        var floor = int.Parse(Console.ReadLine());

                        Console.Write("Enter a passengers quantity: ");
                        var waitingPassengers = int.Parse(Console.ReadLine());
                        gameEnvironment.Building.AddPassengersToFloor(floor, waitingPassengers);
                        break;

                    case ConsoleKey.W:
                        gameEnvironment.Building.AddPassengersToFloor(gameEnvironment.Elevator.Position, 1);
                        break;

                    case ConsoleKey.S:
                        gameEnvironment.Elevator.LeavePassengersOnFloor(gameEnvironment.Elevator.PassengersQuantity);
                        break;

                    case ConsoleKey.R:
                        gameEnvironment.Building.GenerateRandomPassengers();
                        break;

                    case ConsoleKey.A:
                        gameEnvironment.AutomaticMode();
                        break;

                    default:
                        break;
                }

                gameEnvironment.DrawScreen();
            }
        }
    }
}
