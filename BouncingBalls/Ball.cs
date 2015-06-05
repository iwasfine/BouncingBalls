using PriorityQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BouncingBalls
{
    public class Ball
    {
        public double px, py;
        private double vx, vy;
        public readonly double radius;
        private readonly double mass;

        //private double width;
        //private double height;

        public Brush Color { get; set; }

        public Ball(double px, double py, double vx, double vy, double radius, Brush brush)
        {
            this.px = px;
            this.py = py;
            this.vx = vx;
            this.vy = vy;
            this.radius = radius;
            this.mass = radius * radius * radius * Math.PI * 4 / 3;
            Color = brush;
        }

        public void Move(double width, double height)
        {
            px = px + vx;
            py = py + vy;
        }

        internal int TimeToHitVerticalWall()
        {
            int time = int.MaxValue;
            if (vx == 0) return int.MaxValue;
            if (vx > 0) time = Math.Min(time, Math.Abs((int)((1 - (px + radius)) / vx)));
            else time = Math.Min(time, Math.Abs((int)(-(px - radius) / vx)));
            return time;
        }

        internal int TimeToHitHorizontalWall()
        {
            int time = int.MaxValue;
            if (vy == 0) return int.MaxValue;
            if (vy > 0) time = Math.Min(time, Math.Abs((int)((1 - (py + radius)) / vy)));
            else time = Math.Min(time, Math.Abs((int)(-(py - radius) / vy)));
            return time;
        }

        internal void BounceOffVerticalWall()
        {
            vx = -vx;
        }

        internal void BounceOffHorizontalWall()
        {
            vy = -vy;
        }

        internal int TimeToHitBall(Ball b)
        {
            Ball a = this;
            if (a == b) return int.MaxValue;
            double dx = b.px - a.px;
            double dy = b.py - a.py;
            double dvx = b.vx - a.vx;
            double dvy = b.vy - a.vy;
            double dvdr = dx * dvx + dy * dvy;
            if (dvdr > 0) return int.MaxValue;
            double dvdv = dvx * dvx + dvy * dvy;
            double drdr = dx * dx + dy * dy;
            double sigma = a.radius + b.radius;
            double d = (dvdr * dvdr) - dvdv * (drdr - sigma * sigma);
            if (d < 0) return int.MaxValue;
            return (int)(-(dvdr + Math.Sqrt(d)) / dvdv);
        }

        internal void BounceOffBall(Ball that)
        {
            double dx = that.px - this.px;
            double dy = that.py - this.py;
            double dvx = that.vx - this.vx;
            double dvy = that.vy - this.vy;
            double dvdr = dx * dvx + dy * dvy;
            double dist = this.radius + that.radius;

            double F = 2 * this.mass * that.mass * dvdr / ((this.mass + that.mass) * dist);
            double fx = F * dx / dist;
            double fy = F * dy / dist;

            this.vx += fx / this.mass;
            this.vy += fy / this.mass;
            that.vx -= fx / that.mass;
            that.vy -= fy / that.mass;

            //double msub = this.mass - that.mass;
            //double msum = this.mass + that.mass;
            //double vxtemp = msub * vx + 2 * that.mass * that.vx;
            //double vytemp = msub * vy + 2 * that.mass * that.vy;
            //that.vx = (-msub * that.vx + 2 * mass * vx) / msum;
            //that.vy = (-msub * that.vy + 2 * mass * vy) / msum;
            //vx = vxtemp / msum;
            //vy = vytemp / msum;
        }
    }

}
