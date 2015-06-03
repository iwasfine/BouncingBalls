﻿using PriorityQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BouncingBalls
{
    public struct Collision
    {
        public bool IsBall;
        public int Ball1;
        public int Ball2;
        public int Time;
    }
    /// <summary>
    /// Interaction logic for SimulationWindow.xaml
    /// </summary>
    public partial class SimulationWindow : Window
    {
        public SimulationWindow()
        {
            InitializeComponent();
            border = new Rectangle();
            border.Stroke = Brushes.Black;
            border.StrokeThickness = 2;
            border.Width = this.Width - 40;
            border.Height = this.Height - 60;
            Canvas.SetLeft(border, 10);
            Canvas.SetTop(border, 10);
            canvas.Children.Add(border);
        }
        private DispatcherTimer showTimer = new DispatcherTimer();
        private DispatcherTimer ballMoveTimer = new DispatcherTimer();
        private Rectangle border;
        private PriorityQueue<Collision> pq = new PriorityQueue<Collision>((c1, c2) => c1.Time - c2.Time);
        private int[,] collision;
        private int time = 0;
        private int pretime = 0;

        public List<Ball> balls { get; set; }
        private List<Ellipse> ellipses;
        public void Simulate(int numOfBalls)
        {
            initializeBalls(numOfBalls);
            addEllipses();
            initializeCollision();
            showTimer.Tick += showTimer_Tick;
            showTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            ballMoveTimer.Tick += ballMoveTime_Tick;
            ballMoveTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            showTimer.Start();
            ballMoveTimer.Start();
        }

        private void initializeCollision()
        {
            int n = balls.Count;
            collision = new int[n + 1, n + 1];
            for (int i = 0; i < n; i++)
            {
                int cTime = balls[i].TimeToHitWall(border.Width, border.Height) ;
                if (cTime > 6000000) continue;
                cTime += time;
                collision[i, n] = cTime;
                Collision c = new Collision();
                c.Time = cTime;
                c.IsBall = false;
                c.Ball1 = i;
                c.Ball2 = -1;
                pq.Add(c);
            }
            //for (int i = 0; i < n; i++)
            //{
            //    for (int j = i + 1; j < n; j++)
            //    {
            //        int cTime = balls[i].TimeToHitBall(balls[j]) + time;
            //        collision[i, j] = cTime;
            //        collision[j, i] = cTime;
            //        Collision c = new Collision();
            //        c.Time = cTime;
            //        c.IsBall = true;
            //        c.Ball1 = i;
            //        c.Ball2 = j;
            //        pq.Add(c);
            //    }
            //}
        }

        private void ballMoveTime_Tick(object sender, EventArgs e)
        {
            time++;
            foreach (var item in balls)
            {
                item.Move(border.Width, border.Height);
            }
            while(pq.Peek().Time == time)
            {
                int n = balls.Count;
                Collision c = pq.Poll();
                if (c.IsBall)
                {

                }
                else
                {
                    Ball ball = balls[c.Ball1];
                    if(time != collision[c.Ball1,n]) return;
                    ball.BounceOffWall(border.Width,border.Height);
                    int temp = ball.TimeToHitWall(border.Width, border.Height);
                    if (temp > 6000000) return;
                    temp += time;
                    collision[c.Ball1, n] = temp;
                    Collision newC = new Collision();
                    newC.Time = temp;
                    newC.IsBall = false;
                    newC.Ball1 = c.Ball1;
                    newC.Ball2 = -1;
                    pq.Add(newC);
                }
            }
        }

        private void addEllipses()
        {
            ellipses = new List<Ellipse>();
            foreach (var item in balls)
            {
                Ellipse e = new Ellipse();
                e.Width = 2 * item.radius;
                e.Height = 2 * item.radius;
                Canvas.SetTop(e, item.py - item.radius + 10);
                Canvas.SetLeft(e, item.px - item.radius + 10);
                e.Fill = item.Color;
                ellipses.Add(e);
                canvas.Children.Add(e);
            }
        }

        private void initializeBalls(int numOfBalls)
        {
            balls = new List<Ball>();
            double x = border.Width;
            double y = border.Height;
            Random rnd = new Random();
            for (int i = 0; i < numOfBalls; i++)
            {
                double px = rnd.NextDouble() * (x - 20);
                double py = rnd.NextDouble() * (y - 20);
                double vx = (rnd.NextDouble() - 0.5) * 5;
                double vy = (rnd.NextDouble() - 0.5) * 5;
                //double vx = 5;
                //double vy = 5;
                double radius = 8;
                balls.Add(new Ball(px, py, vx, vy, radius, Brushes.Blue));
            }
        }

        private void showTimer_Tick(Object sender, EventArgs e)
        {
            for (int i = 0; i < balls.Count; i++)
            {
                Canvas.SetTop(ellipses[i], balls[i].py - balls[i].radius + 10);
                Canvas.SetLeft(ellipses[i], balls[i].px - balls[i].radius + 10);
            }
        }
    }
}
