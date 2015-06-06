using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BouncingBalls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int n;
            if (int.TryParse(inputNum.Text, out n))
            {
                Ball[] balls = new Ball[n];
                Random rnd = new Random();
                for (int i = 0; i < n; i++)
                    balls[i] = new Ball(rnd);
                var sim = new SimulationWindow();
                sim.Show();
                sim.Simulate(balls);
            }
            else MessageBox.Show("Please enter a valid number!");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog()==true)
            {
                FileStream file = new FileStream(dialog.FileName, FileMode.Open);
                StreamReader sr = new StreamReader(file);
                int n = int.Parse(sr.ReadLine().Trim());
                Ball[] balls = new Ball[n];
                try
                {
                    for (int i = 0; i < n; i++)
                    {
                        string[] s = sr.ReadLine().Trim().Split(' ');
                        double px = double.Parse(s[0]);
                        double py = double.Parse(s[1]);
                        double vx = double.Parse(s[2]);
                        double vy = double.Parse(s[3]);
                        double radius = double.Parse(s[4]);
                        double mass = double.Parse(s[5]);
                        byte r = byte.Parse(s[6]);
                        byte g = byte.Parse(s[7]);
                        byte b = byte.Parse(s[8]);
                        Color c = new Color();
                        c.R = r;
                        c.G = g;
                        c.B = b;
                        c.A = 255;
                        balls[i] = new Ball(px, py, vx, vy, radius, mass, new SolidColorBrush(c));
                    }
                }
                catch (Exception)
                {
                    
                    throw new Exception("Wrong file format");
                }
                sr.Close();
                file.Close();
                var sim = new SimulationWindow();
                sim.Show();
                sim.Simulate(balls);
            }
        }
    }
}
