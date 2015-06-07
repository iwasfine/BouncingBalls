using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BouncingBalls
{
    public class Collision : IComparable<Collision>
    {
        public readonly double Time;
        public readonly Ball A, B;
        private readonly int countA, countB;

        public Collision(double time, Ball a, Ball b)
        {
            Time = time;
            A = a;
            B = b;

            if (a != null) countA = a.Count;
            else countA = -1;
            if (b != null) countB = b.Count;
            else countB = -1;
        }

        public int CompareTo(Collision other)
        {
            return Math.Sign(this.Time - other.Time);
        }

        public bool IsValid()
        {
            if (A != null && A.Count != countA) return false;
            if (B != null && B.Count != countB) return false;
            return true;
        }
    }
}
