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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wpf_Motion_emul.Controls {
    /// <summary>
    /// Логика взаимодействия для RobotControl.xaml
    /// </summary>
    public partial class RobotControl : UserControl {
        public RobotControl() {
            InitializeComponent();
            Distance = 50;
        }
        public double X;
        public double Y;
        public double Speed;
        public double Offset;
        public double Distance;
        public Thread thrd_move;

        public double GetY() {
            return Canvas.GetTop(this);
        }
        public double GetX() {
            return Canvas.GetLeft(this);
        }
        public void Init(double x, double y, double speed) {
            X = x;
            Y = y;
            Speed = speed;
        }
        public void StartMove() {
            thrd_move = new Thread(new ThreadStart(move));
            thrd_move.Start();
        }
        private void move() {
            while (true) {
                Dispatcher.Invoke(() => {
                    X += 1 * Speed;
                    if (Math.Abs(Offset) < 1) {
                        Offset = 0;
                    }
                    else {
                        Console.WriteLine($"OffsetRobot = {Offset}");
                        if (Offset < 0) {
                            Y = Y - 1 * Speed;
                            Offset += 1;
                        }
                        else {
                            Y = Y + 1 * Speed;
                            Offset -= 1;
                        }                      
                    }
                });
                Thread.Sleep(30);
            }        
        }
    }
}
