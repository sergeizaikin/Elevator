using Elevator.Entities;
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
                   "D: Change speed\n" +
                   "A: Enable automatic mode\n";

            var gameEnvironment = new GameEnvironment(18, hotKeys, 700);
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
                        gameEnvironment.Elevator.GetAllPassengersFromFloor(Enums.Direction.Up);
                        gameEnvironment.Elevator.GetAllPassengersFromFloor(Enums.Direction.Down);
                        break;

                    case ConsoleKey.RightArrow:
                        gameEnvironment.Elevator.LeavePassengersOnFloor();
                        break;

                    case ConsoleKey.Q:
                        Console.WriteLine();
                        Console.Write("Enter a floor: ");
                        var floor = int.Parse(Console.ReadLine());

                        Console.Write("Enter a passengers quantity: ");
                        var waitingPassengers = int.Parse(Console.ReadLine());
                        gameEnvironment.Building.AddOnePassengerToFloor(floor, new Passenger(gameEnvironment.Building, floor));
                        break;

                    case ConsoleKey.W:
                        gameEnvironment.Building.AddOnePassengerToFloor(gameEnvironment.Elevator.CurrentFloor, new Passenger(gameEnvironment.Building, gameEnvironment.Elevator.CurrentFloor));
                        break;

                    case ConsoleKey.S:
                        gameEnvironment.Elevator.LeavePassengersOnFloor();
                        break;

                    case ConsoleKey.R:
                        gameEnvironment.Building.GenerateRandomPassengers();
                        break;

                    case ConsoleKey.D:
                        Console.Write("Enter new speed (1 - 10): ");
                        var newSpeed = int.Parse(Console.ReadLine());
                        gameEnvironment.Elevator.ChangeSpeed(newSpeed);
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
