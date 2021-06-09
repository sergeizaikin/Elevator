using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;

namespace Elevator
{
    class Elevator
    {
        public int Position { get; set; } = 0;
        public int PassengersQuantity { get; set; } = 0;
        public Building Building { get; set; }
        public int MaxPassengerCapacity { get; set; } = 15;
        public Screen Screen { get; set; }
        public int Speed { get; set; } = 800;

        public Elevator(Building building, Screen screen)
        {
            Building = building;
            Screen = screen;
        }

        public void ChangePosition(int step)
        {
            if (step + Position <= Building.FloorsQuantity - 1 && step + Position >= 0)
                Position += step;
        }

        public void AddPassengersToElevator(int additionalPassengersQuantity)
        {
            var freeSpace = MaxPassengerCapacity - PassengersQuantity;
            int passengersToStay;
            int passengersToEnter;

            if (PassengersQuantity + additionalPassengersQuantity > MaxPassengerCapacity)
            {
                passengersToStay = additionalPassengersQuantity - freeSpace;
                passengersToEnter = additionalPassengersQuantity - passengersToStay;
            }
            else
            {
                passengersToStay = 0;
                passengersToEnter = additionalPassengersQuantity;
            }

            PassengersQuantity += passengersToEnter;
            Building.WaitingPassengers[Position] = Math.Abs(passengersToStay);
        }

        public void RemovePassengersFromElevator(int quantityOfPassengersToRemove)
        {
            if (PassengersQuantity >= quantityOfPassengersToRemove)
                PassengersQuantity -= quantityOfPassengersToRemove;
        }

        public void GetPassengersFromFloor()
        {
            AddPassengersToElevator(Building.WaitingPassengers[Position]);
        }

        public void LeavePassengersOnFloor(int quantityOfPassengersToLeave)
        {
            if (PassengersQuantity >= quantityOfPassengersToLeave)
            {
                RemovePassengersFromElevator(quantityOfPassengersToLeave);
                Building.WaitingPassengers[Position] += quantityOfPassengersToLeave;
            }

        }

        public void AutomaticMode()
        {
            while (true)
            {

                if (ExistWaitingPassengers()) // If there are passengers waiting
                {
                    var maxWaitingFloor = GetLastWaitingFloor();
                    GoToFloor(maxWaitingFloor);
                    WayDown();
                    LeavePassengersOnFloor(Screen.Elevator.PassengersQuantity);
                    Screen.DrawScreen();
                }
                else
                    Building.GenerateRandomPassengers();

                Thread.Sleep(500);
            }
        }

        private void GoToFloor(int targetFloor)
        {
            if(Position > targetFloor)
            {
                for (int i = Position; i >= targetFloor; i--)
                {
                    Position = i;
                    Screen.DrawScreen();
                    Thread.Sleep(Speed);
                }
            }
            else if (Position < targetFloor)
            {
                for (int i = Position; i <= targetFloor; i++)
                {
                    Position = i;
                    Screen.DrawScreen();
                    Thread.Sleep(Speed);
                }
            }
        }

        private int GetLastWaitingFloor()
        {
            for (int i = Building.FloorsQuantity - 1; i >= 0; i--)
            {
                if (Building.WaitingPassengers[i] > 0)
                    return i;
            }

            return 0;
        }

        private void WayDown()
        {
            while(Position != 0)
            {
                if (Building.WaitingPassengers[Position] > 0)
                {
                    GetPassengersFromFloor();
                    Screen.DrawScreen();
                    Thread.Sleep(Speed);
                }

                Position = GetNextPosition(Direction.Down);
                Screen.DrawScreen();

                Thread.Sleep(Speed);
            }

            if(Position == 0)
            {
                //GetPassengersFromFloor();
                //Screen.DrawScreen();
                //Thread.Sleep(Speed);
            }
        }

        private int GetNextPosition(Direction direction)
        {
            var nextPosition = Position + (int)direction;
            return nextPosition >= 0 ? nextPosition : 0;
        }

        private bool ExistWaitingPassengers()
        {
            for (int i = 1; i < Building.FloorsQuantity; i++)
            {
                if (Building.WaitingPassengers[i] > 0)
                    return true;
            }

            return false;
        }

        private enum Direction
        {
            Up = 1,
            Down = -1
        }
    }
}
