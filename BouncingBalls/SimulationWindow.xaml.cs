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
            border.Margin = new Thickness(10, 10, 10, 10);
            border.StrokeThickness = 2;
            border.Width = this.Width - 40;
            border.Height = this.Height - 50;
            canvas.Children.Add(border);
        }
        private DispatcherTimer showTimer = new DispatcherTimer();
        private DispatcherTimer ballMoveTimer = new DispatcherTimer();
        private Rectangle border;

        public List<Ball> balls { get; set; }
        private List<Ellipse> ellipses;
        public void Simulate(int numOfBalls)
        {
            balls = new List<Ball>();
            initializeBalls(numOfBalls);
            addEllipses();
            showTimer.Tick += showTimer_Tick;
            showTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            ballMoveTimer.Tick += ballMoveTime_Tick;
            ballMoveTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            showTimer.Start();
            ballMoveTimer.Start();
        }

        private void ballMoveTime_Tick(object sender, EventArgs e)
        {
            foreach (var item in balls)
            {
                item.Move();
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
                Canvas.SetTop(e, item.py - item.radius);
                Canvas.SetLeft(e, item.px - item.radius);
                e.Fill = item.Color;
                ellipses.Add(e);
                canvas.Children.Add(e);
            }
        }

        private void initializeBalls(int numOfBalls)
        {
            double x = border.Width;
            double y = border.Height;
            Random rnd = new Random();
            for (int i = 0; i < numOfBalls; i++)
            {
                double px = rnd.NextDouble() * (x - 20);
                double py = rnd.NextDouble() * (y - 20);
                double vx = (rnd.NextDouble() - 0.5) * 5;
                double vy = (rnd.NextDouble() - 0.5) * 5;
                double radius = 5;
                balls.Add(new Ball(px, py, vx, vy, radius, Brushes.Blue));
            }
        }

        private void showTimer_Tick(Object sender, EventArgs e)
        {
            for (int i = 0; i < balls.Count; i++)
            {
                Canvas.SetTop(ellipses[i], balls[i].py-balls[i].radius);
                Canvas.SetLeft(ellipses[i], balls[i].px-balls[i].radius);
            }
        }
    }
}
