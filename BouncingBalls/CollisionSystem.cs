using PriorityQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace BouncingBalls
{
    class CollisionSystem
    {
        private PriorityQueue<Collision> pq;
        private double t = 0.0;
        private double hz = 0.5;
        private Ball[] balls;
        private Ellipse[] ellipses;
        private Canvas canvas;
        private double size;
        private double offset;

        public CollisionSystem(Ball[] balls, Canvas canvas, double size, double offset)
        {
            this.balls = (Ball[])balls.Clone();
            this.canvas = canvas;
            this.size = size;
            this.offset = offset;
            addEllipses();
        }

        private void addEllipses()
        {
            ellipses = new Ellipse[balls.Length];
            for (int i = 0; i < balls.Length; i++)
            {
                Ellipse e = new Ellipse();
                e.Width = 2 * balls[i].radius * size;
                e.Height = 2 * balls[i].radius * size;
                Canvas.SetTop(e, (balls[i].py - balls[i].radius) * size + offset);
                Canvas.SetLeft(e, (balls[i].px - balls[i].radius) * size + offset);
                e.Fill = balls[i].Color;
                ellipses[i] = e;
                canvas.Children.Add(e);
            }
        }

        private void predict(Ball a, double limit)
        {
            if (a == null) return;

            for (int i = 0; i < balls.Length; i++)
            {
                double dt = a.TimeToHitBall(balls[i]);
                if (t + dt <= limit)
                    pq.Add(new Collision(t + dt, a, balls[i]));
            }

            double dtX = a.TimeToHitVerticalWall();
            double dtY = a.TimeToHitHorizontalWall();
            if (t + dtX <= limit) pq.Add(new Collision(t + dtX, a, null));
            if (t + dtY <= limit) pq.Add(new Collision(t + dtY, null, a));
        }

        private void redraw(double limit)
        {
            for (int i = 0; i < ellipses.Length; i++)
            {
                try
                {
                    ellipses[i].Dispatcher.Invoke(() =>
                            {
                                Canvas.SetTop(ellipses[i], (balls[i].py - balls[i].radius) * size + offset);
                                Canvas.SetLeft(ellipses[i], (balls[i].px - balls[i].radius) * size + offset);
                            });
                }
                catch (Exception)
                {
                    
                    return;
                }
            }
            Thread.Sleep(30);
            if (t < limit)
            {
                pq.Add(new Collision(t + 1.0 / hz, null, null));
            }
        }

        public Task SimulateAsync(double limit)
        {
            return Task.Run(() => Simulate(limit));
        }

        private void Simulate(double limit)
        {
            pq = new PriorityQueue<Collision>((c1, c2) => Math.Sign(c1.Time - c2.Time));
            for (int i = 0; i < balls.Length; i++)
            {
                predict(balls[i], limit);
            }
            pq.Add(new Collision(0, null, null));


            while (pq.Count != 0)
            {

                Collision e = pq.Poll();
                if (!e.IsValid()) continue;
                Ball a = e.A;
                Ball b = e.B;

                for (int i = 0; i < balls.Length; i++)
                    balls[i].Move(e.Time - t);
                t = e.Time;

                if (a != null && b != null) a.BounceOffBall(b);
                else if (a != null && b == null) a.BounceOffVerticalWall();
                else if (a == null && b != null) b.BounceOffHorizontalWall();
                else if (a == null && b == null) redraw(limit);

                predict(a, limit);
                predict(b, limit);
            }
        }
    }
}
