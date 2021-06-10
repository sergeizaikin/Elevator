using System;
using System.Collections.Generic;
using System.Text;

namespace Elevator.Entities
{
    class Floor
    {
        public int Number { get; set; }
        public List<Passenger> WaitingPassengers { get; set; } = new List<Passenger>();

        public Floor(int number)
        {
            Number = number;
        }

        public override string ToString()
        {
            return Number.ToString();
        }
    }
}
