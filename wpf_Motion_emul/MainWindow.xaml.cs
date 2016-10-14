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

namespace wpf_Motion_emul {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            robot.Init(50, 100, 3);
            Start();
        }
        public void Start() {
            Thread thrd_work = new Thread(new ThreadStart(Work));
            robot.Init(50, 100, 1);
            thrd_work.Start();
        }
        public void Work() {
            robot.StartMove();
            while (true) {
                Dispatcher.Invoke(() => {
                    cnv.Children.Clear();
                    Canvas.SetLeft(robot, robot.X);
                    Canvas.SetTop(robot, robot.Y);
                    robot.Offset = laser.Offset;
                    cnv.Children.Add(robot);
                });
                Thread.Sleep(30);
            }
        }

        private void cnv_KeyDown(object sender, KeyEventArgs e) {
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Space) {
                if (robot.thrd_move.ThreadState == ThreadState.Suspended) {
                    robot.thrd_move.Resume();                  
                }
                else {
                    robot.thrd_move.Suspend();
                }
            }
            else if (e.Key == Key.D) {
                robot.X += robot.Speed;
            }
            else if (e.Key == Key.A) {
                robot.X -= robot.Speed;
            }
            else if(e.Key == Key.Enter) {
                robot.X = 50;
                robot.Y = 100;
            }
            

            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            Environment.Exit(0);
        }
    }
}
