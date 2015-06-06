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
        private Rectangle border;
        private CollisionSystem sys;

        public async void Simulate(Ball[] balls)
        {
            sys = new CollisionSystem(balls, canvas, border.Width, 10);
            await sys.SimulateAsync(6000000);

        }

    }
}
