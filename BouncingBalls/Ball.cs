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
            if (Math.Abs(px + radius - width) < 1 || Math.Abs(px - radius) < 1) vx = -vx;
            py = py + vy;
            if (Math.Abs(py + radius - height) < 1 || Math.Abs(py - radius) < 1) vy = -vy;
        }
    }

}
