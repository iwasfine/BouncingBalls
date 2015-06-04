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

        private double width;
        private double height;

        public Brush Color { get; set; }

        public Ball(double px, double py, double vx, double vy, double radius, Brush brush, double width, double height)
        {
            this.px = px;
            this.py = py;
            this.vx = vx;
            this.vy = vy;
            this.radius = radius;
            this.mass = radius * radius * radius * Math.PI * 4 / 3;
            Color = brush;
            this.width = width;
            this.height = height;
        }

        public void Move(double width, double height)
        {
            px = px + vx;
            py = py + vy;
        }

        //public int TimeToHitWall(double width, double height)
        //{
        //    int time = int.MaxValue;
        //    if (vx > 0) time = Math.Min(time, Math.Abs((int)((width - (px + radius)) / vx)));
        //    else time = Math.Min(time, Math.Abs((int)(-(px - radius) / vx)));
        //    if (vy > 0) time = Math.Min(time, Math.Abs((int)((height - (py + radius)) / vy)));
        //    else time = Math.Min(time, Math.Abs((int)(-(py - radius) / vy)));
        //    return time;
        //}

        //public void BounceOffWall(double width, double height)
        //{
        //    //if (Math.Abs(px + radius - width) < 1 || Math.Abs(px - radius) < 1 || px + radius > width || px - radius < 0) vx = -vx;
        //    //if (Math.Abs(py + radius - height) < 1 || Math.Abs(py - radius) < 1 || py + radius > height || py - radius < 0) vy = -vy;
        //    double x = Math.Min(Math.Abs(px + radius - width), Math.Abs(px - radius));
        //    double y = Math.Min(Math.Abs(py + radius - height), Math.Abs(py - radius));
        //    if (x < y) vx = -vx;
        //    else vy = -vy;
        //}

        internal int TimeToHitVerticalWall()
        {
            int time = int.MaxValue;
            if (vx == 0) return int.MaxValue;
            if (vx > 0) time = Math.Min(time, Math.Abs((int)((width - (px + radius)) / vx)));
            else time = Math.Min(time, Math.Abs((int)(-(px - radius) / vx)));
            return time;
        }

        internal int TimeToHitHorizontalWall()
        {
            int time = int.MaxValue;
            if (vy == 0) return int.MaxValue;
            if (vy > 0) time = Math.Min(time, Math.Abs((int)((height - (py + radius)) / vy)));
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
    }

}
