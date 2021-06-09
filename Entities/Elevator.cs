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
        public GameEnvironment Screen { get; set; }
        public int Speed { get; set; } = 800;

        public Elevator(Building building, GameEnvironment screen)
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

        public void GoToFloor(int targetFloor)
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

        public void GoDownCollectingPassengers()
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

        private enum Direction
        {
            Up = 1,
            Down = -1
        }
    }
}
