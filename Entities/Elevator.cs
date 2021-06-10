using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using Elevator.Entities;
using static Elevator.Entities.Enums;

namespace Elevator
{
    class Elevator
    {
        public int CurrentFloor { get; set; } = 0;
        public Building Building { get; set; }
        public int MaxPassengerCapacity { get; set; } = 10;
        public GameEnvironment Screen { get; set; }
        public int Speed { get; set; }
        public List<Passenger> Passengers { get; set; } = new List<Passenger>();
        public Direction Direction { get; set; } = Direction.Up;
        public bool isInAutomaticMode { get; set; } = false;

        public Elevator(Building building, GameEnvironment screen, int speed)
        {
            Building = building;
            Screen = screen;
            Speed = speed;
        }

        public void ChangePosition(int step)
        {
            if (step + CurrentFloor <= Building.Floors.Count() - 1 && step + CurrentFloor >= 0)
                CurrentFloor += step;
        }

        public void ChangeSpeed(int speed)
        {
            Speed = 1000 / speed;
        }

        public void AddPassengersToElevator(List<Passenger> additionalPassengers)
        {
            var passengersQuantity = Passengers.Count;
            var additionalPassengersQuantity = additionalPassengers.Count;

            var freeSpace = MaxPassengerCapacity - passengersQuantity;
            int quantityOfPassengersToStay;
            int quantityOfPassengersToEnter;

            // If the lift capacity has been exceeded then we need to count how many can get in and how many will stay
            // If no then we can take everybody
            if (passengersQuantity + additionalPassengersQuantity > MaxPassengerCapacity)
            {
                quantityOfPassengersToStay = additionalPassengersQuantity - freeSpace;
                quantityOfPassengersToEnter = additionalPassengersQuantity - quantityOfPassengersToStay;
            }
            else
            {
                quantityOfPassengersToEnter = additionalPassengersQuantity;
            }

            var passengersToEnter = additionalPassengers.Take(quantityOfPassengersToEnter).ToList();

            Passengers.AddRange(passengersToEnter);
            Building.RemovePassengersFromFloor(CurrentFloor, passengersToEnter);
        }

        public void GetAllPassengersFromFloor(Direction direction)
        {
            if (direction == Direction.Up)
            {
                AddPassengersToElevator(Building.GetFloorByNumber(CurrentFloor).WaitingPassengers.Where(x => x.DestinationFloor > CurrentFloor).ToList());
            }
            else
            {
                AddPassengersToElevator(Building.GetFloorByNumber(CurrentFloor).WaitingPassengers.Where(x => x.DestinationFloor < CurrentFloor).ToList());
            }
        }

        public void LeavePassengersOnFloor()
        {
            // We remove only passengers which destination is current floor
            var passengersToLeave = Passengers.Where(p => p.DestinationFloor == CurrentFloor).ToList();
            RemovePassengersFromElevator(passengersToLeave);

            Building.AddPassengersToFloor(CurrentFloor, passengersToLeave);
        }

        private void RemovePassengersFromElevator(List<Passenger> passengersToLeave)
        {
            Passengers = Passengers.Except(passengersToLeave).ToList();
        }

        public void GoToFloor(int targetFloor)
        {
            if (CurrentFloor > targetFloor)
            {
                for (int i = CurrentFloor; i >= targetFloor; i--)
                {
                    CurrentFloor = i;
                    Screen.DrawScreen();
                    Thread.Sleep(Speed);
                }
            }
            else if (CurrentFloor < targetFloor)
            {
                for (int i = CurrentFloor; i <= targetFloor; i++)
                {
                    CurrentFloor = i;
                    Screen.DrawScreen();
                    Thread.Sleep(Speed);
                }
            }
        }

        public void GoUpDeliveringPassengers()
        {
            var lastFloor = Building.Floors.Last().Number;

            while (CurrentFloor <= lastFloor)
            {
                if (Building.GetFloorByNumber(CurrentFloor).WaitingPassengers.Count > 0)
                {
                    GetAllPassengersFromFloor(Direction.Up);
                    if (Passengers.Count > 0)
                        lastFloor = Passengers.OrderByDescending(p => p.DestinationFloor).First().DestinationFloor; // Update destination floor of the elevator to the highest floor of passengers
                    Screen.DrawScreen();
                    Thread.Sleep(Speed);
                }
                else
                {
                    if (CurrentFloor == 0)
                        return;
                }

                LeavePassengersOnFloor();

                if (CurrentFloor != lastFloor)
                    CurrentFloor = GetNextPosition(Direction.Up);
                else
                    break;

                Screen.DrawScreen();

                Thread.Sleep(Speed);
            }
        }

        public void GoDownPickingUpPassengers()
        {
            while (CurrentFloor != 0)
            {
                if (Building.GetFloorByNumber(CurrentFloor).WaitingPassengers.Count > 0)
                {
                    GetAllPassengersFromFloor(Direction.Down);
                    Screen.DrawScreen();
                    Thread.Sleep(Speed);
                }

                CurrentFloor = GetNextPosition(Direction.Down);
                Screen.DrawScreen();

                Thread.Sleep(Speed);
            }
        }

        private int GetNextPosition(Direction direction)
        {
            var nextPosition = CurrentFloor + (int)direction;
            return nextPosition >= 0 ? nextPosition : 0;
        }
    }
}
