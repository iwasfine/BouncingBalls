using PriorityQueue;
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
    enum Wall
    {
        VerticalWall = -1,
        HorizontalWall = -2
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
            border.Height = border.Width;
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
            collision = new int[n + 2, n + 2];
            for (int i = 0; i < n; i++)
            {
                int cTime = balls[i].TimeToHitVerticalWall();
                //while (cTime == 0)
                //{
                //    balls[i].BounceOffVerticalWall();
                //    cTime = balls[i].TimeToHitVerticalWall();
                //}
                if (cTime > 6000000) continue;
                cTime += time;
                collision[i, n] = cTime;
                Collision c = new Collision();
                c.Time = cTime;
                c.IsBall = false;
                c.Ball1 = i;
                c.Ball2 = (int)Wall.VerticalWall;
                pq.Add(c);

                cTime = balls[i].TimeToHitHorizontalWall();
                //while (cTime == 0)
                //{
                //    balls[i].BounceOffVerticalWall();
                //    cTime = balls[i].TimeToHitVerticalWall();
                //}
                if (cTime > 6000000) continue;
                cTime += time;
                collision[i, n + 1] = cTime;
                c = new Collision();
                c.Time = cTime;
                c.IsBall = false;
                c.Ball1 = i;
                c.Ball2 = (int)Wall.HorizontalWall;
                pq.Add(c);
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    int cTime = balls[i].TimeToHitBall(balls[j]);
                    if (cTime == int.MaxValue) continue;
                    cTime += time;
                    collision[i, j] = cTime;
                    collision[j, i] = cTime;
                    Collision c = new Collision();
                    c.Time = cTime;
                    c.IsBall = true;
                    c.Ball1 = i;
                    c.Ball2 = j;
                    pq.Add(c);
                }
            }
        }

        private void ballMoveTime_Tick(object sender, EventArgs e)
        {
            time++;
            foreach (var item in balls)
            {
                item.Move(border.Width, border.Height);
            }
            while (pq.Peek().Time < time)
            {
                pq.Poll();
            }
            while (pq.Peek().Time == time)
            {
                int n = balls.Count;
                Collision c = pq.Poll();
                if (c.IsBall)
                {
                    Ball ball = balls[c.Ball1];
                    if (time != collision[c.Ball1, c.Ball2]) return;
                    ball.BounceOffBall(balls[c.Ball2]);
                    updateCollision(ball,  n, c.Ball1);
                    updateCollision(balls[c.Ball2],  n, c.Ball2);
                }
                else
                {
                    Ball ball = balls[c.Ball1];
                    if (c.Ball2 == (int)Wall.VerticalWall)
                    {
                        if (time != collision[c.Ball1, n]) return;
                        ball.BounceOffVerticalWall();
                        updateCollision(ball,  n, c.Ball1);
                        //int temp = ball.TimeToHitVerticalWall();
                        //if (temp >= int.MaxValue - time) temp = int.MaxValue;
                        //else temp += time;
                        //collision[c.Ball1, n] = temp;
                        //Collision newC = new Collision();
                        //newC.Time = temp;
                        //newC.IsBall = false;
                        //newC.Ball1 = c.Ball1;
                        //newC.Ball2 = (int)Wall.VerticalWall;
                        //pq.Add(newC);
                    }
                    else
                    {
                        if (time != collision[c.Ball1, n + 1]) return;
                        ball.BounceOffHorizontalWall();
                        updateCollision(ball,  n, c.Ball1);
                        //int temp = ball.TimeToHitHorizontalWall();
                        //if (temp >= int.MaxValue - time) temp = int.MaxValue;
                        //else temp += time;
                        //collision[c.Ball1, n + 1] = temp;
                        //Collision newC = new Collision();
                        //newC.Time = temp;
                        //newC.IsBall = false;
                        //newC.Ball1 = c.Ball1;
                        //newC.Ball2 = (int)Wall.HorizontalWall;
                        //pq.Add(newC);
                    }
                }
            }
        }

        private void updateCollision(Ball ball,  int n, int i)
        {
            int temp = ball.TimeToHitVerticalWall();
            if (temp >= int.MaxValue - time) temp = int.MaxValue;
            else temp += time;
            if (temp <= time)
            {
                ;
            }
            collision[i, n] = temp;
            Collision newC = new Collision();
            newC.Time = temp;
            newC.IsBall = false;
            newC.Ball1 = i;
            newC.Ball2 = (int)Wall.VerticalWall;
            pq.Add(newC);

            temp = ball.TimeToHitHorizontalWall();
            if (temp >= int.MaxValue - time) temp = int.MaxValue;
            else temp += time;
            if (temp <= time)
            {
                ;
            }
            collision[i, n + 1] = temp;
            newC = new Collision();
            newC.Time = temp;
            newC.IsBall = false;
            newC.Ball1 = i;
            newC.Ball2 = (int)Wall.HorizontalWall;
            pq.Add(newC);

            for (int j = 0; j < n; j++)
            {
                int cTime = balls[i].TimeToHitBall(balls[j]);
                if (cTime == int.MaxValue) continue;
                cTime += time;
                if (cTime <= time)
                {
                    ;
                }
                collision[i, j] = cTime;
                collision[j, i] = cTime;
                Collision c = new Collision();
                c.Time = cTime;
                c.IsBall = true;
                c.Ball1 = i;
                c.Ball2 = j;
                pq.Add(c);
            }
        }
        private void addEllipses()
        {
            ellipses = new List<Ellipse>();
            foreach (var item in balls)
            {
                Ellipse e = new Ellipse();
                e.Width = 2 * item.radius * (border.Width - 30);
                e.Height = 2 * item.radius * (border.Width - 30);
                Canvas.SetTop(e, item.py * (border.Width - 30) + 15 - item.radius * (border.Width - 30) + 10);
                Canvas.SetLeft(e, item.px * (border.Width - 30) + 15 - item.radius * (border.Width - 30) + 10);
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
                double px = rnd.NextDouble();
                double py = rnd.NextDouble();
                double vx = (rnd.NextDouble() - 0.5) * 5 / (border.Width - 30);
                double vy = (rnd.NextDouble() - 0.5) * 5 / (border.Width - 30);
                double radius = 6 / (border.Width - 30);
                //if (i < 1) radius = 15;
                balls.Add(new Ball(px, py, vx, vy, radius, Brushes.Blue));
            }
        }

        private void showTimer_Tick(Object sender, EventArgs e)
        {
            for (int i = 0; i < balls.Count; i++)
            {
                Canvas.SetTop(ellipses[i], balls[i].py * (border.Width - 30) + 15 - balls[i].radius * (border.Width - 30) + 10);
                Canvas.SetLeft(ellipses[i], balls[i].px * (border.Width - 30) + 15 - balls[i].radius * (border.Width - 30) + 10);
            }
        }
    }
}
