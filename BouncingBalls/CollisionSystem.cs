using PriorityQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BouncingBalls
{
    class CollisionSystem
    {
        private PriorityQueue<Collision> pq;
        private double t = 0.0;
        private double hz = 0.5;
        private Ball[] balls;

        public CollisionSystem(Ball[] balls)
        {
            this.balls = (Ball[]) balls.Clone();   
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
            StdDraw.clear();
            for (int i = 0; i < particles.length; i++)
            {
                particles[i].draw();
            }
            StdDraw.show(20);
            if (t < limit)
            {
                pq.insert(new Event(t + 1.0 / hz, null, null));
            }
        }

        public void simulate(double limit)
        {

            pq = new PriorityQueue<Collision>();
            for (int i = 0; i < balls.Length; i++)
            {
                predict(balls[i], limit);
            }
            pq.Add(new Collision(0, null, null));      


            while (pq.Count!=0)
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
