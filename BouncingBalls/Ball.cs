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
        public double px { get; private set; }
        public double py { get; private set; }
        private double vx, vy;
        public double radius { get; private set; }
        private readonly double mass;
        public Brush Color { get; set; }
        public int Count { get; private set; }

        public Ball()
        {
            Random rnd = new Random();
            px = rnd.NextDouble();
            py = rnd.NextDouble();
            vx = 0.01 * (rnd.NextDouble() - 0.5);
            vy = 0.01 * (rnd.NextDouble() - 0.5);
            radius = 0.01;
            mass = 0.5;
            Color = Brushes.Black;
        }
        public Ball(double px, double py, double vx, double vy, double radius, double mass, Color color)
        {
            this.px = px;
            this.py = py;
            this.vx = vx;
            this.vy = vy;
            this.radius = radius;
            this.mass = mass;
            Color = new SolidColorBrush(color);
        }

        public void Move(double dt)
        {
            px = px + vx * dt;
            py = py + vy * dt;
        }

        public double TimeToHitVerticalWall()
        {
            if (vx == 0) return double.PositiveInfinity;
            if (vx > 0) return (1 - (px + radius)) / vx;
            return -(px - radius) / vx;
        }

        public double TimeToHitHorizontalWall()
        {
            if (vy == 0) return double.PositiveInfinity;
            if (vy > 0) return (1 - (py + radius)) / vy;
            return -(py - radius) / vy;
        }

        public void BounceOffVerticalWall()
        {
            vx = -vx;
            Count++;
        }

        public void BounceOffHorizontalWall()
        {
            vy = -vy;
            Count++;
        }

        public double TimeToHitBall(Ball b)
        {
            Ball a = this;
            if (a == b) return double.PositiveInfinity;
            double dx = b.px - a.px;
            double dy = b.py - a.py;
            double dvx = b.vx - a.vx;
            double dvy = b.vy - a.vy;
            double dvdr = dx * dvx + dy * dvy;
            if (dvdr > 0) return double.PositiveInfinity;
            double dvdv = dvx * dvx + dvy * dvy;
            double drdr = dx * dx + dy * dy;
            double sigma = a.radius + b.radius;
            double d = (dvdr * dvdr) - dvdv * (drdr - sigma * sigma);
            if (d < 0) return double.PositiveInfinity;
            return -(dvdr + Math.Sqrt(d)) / dvdv;
        }

        public void BounceOffBall(Ball that)
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

            this.Count++;
            that.Count++;
        }
    }

}
