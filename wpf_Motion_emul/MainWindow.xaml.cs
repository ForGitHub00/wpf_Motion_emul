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
            Map = new double[2, 1000];
            Map2 = new List<double[]>();
            robot.Init(50, 100, 3);
            Start();
        }

        public double[,] Map;
        public List<double[]> Map2;
        public void Start() {
            Thread thrd_work = new Thread(new ThreadStart(Work));
            robot.Init(50, 100, 1);
            thrd_work.Start();
        }
        public void Work() {
            robot.StartMove();
            int c = 0;
            while (true) {
                Dispatcher.Invoke(() => {
                    cnv.Children.Clear();
                    Canvas.SetLeft(robot, robot.X);
                    Canvas.SetTop(robot, robot.Y);
                    //robot.Offset = laser.Offset;
                    cnv.Children.Add(robot);
                    cnv.Children.Add(line);
                    CombinedGeometry g = RenderedIntersect(cnv, robot.laser, line);
                    if (!g.Bounds.IsEmpty) {
                        double offs = g.Bounds.Y - robot.GetY() - 50;
                        double[] temp = new double[2] { robot.X + robot.Distance, robot.Y + offs };
                        if (c != 0) {
                        if (Map2[c - 1][0] != temp[0]) {
                                Map2.Add(temp);
                                c++;
                            }
                        }
                        else {
                            Map2.Add(temp);
                            c++;                            
                        }
                        for (int i = 0; i < Map2.Count; i++) {
                            if (robot.X  < Map2[i][0]) {
                                robot.Offset = Map2[i][1] - robot.Y;
                                Console.WriteLine($"NEW!!! {i}  Map1 = {Map2[i][0]}; Map2 = {Map2[i][1]}; Offs = {Map2[i][1] - robot.Y}; robotX = {robot.X}");
                                break;
                            }
                        }
                        
                        Polyline pol = new Polyline() {
                            Fill = new SolidColorBrush(Colors.Pink),
                            StrokeThickness = 5,
                            Stroke = new SolidColorBrush(Colors.Pink)                           
                            };
                        cnv.Children.Remove(pol);
                        foreach (var item in Map2) {
                            pol.Points.Add(new Point(item[0], item[1]));
                        }
                        cnv.Children.Add(pol);
                        laser.SetOffset(offs);
                    }
                    else {
                        laser.SetOffset(0);
                    }

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
                if (Math.Abs(robot.Offset) < 1) {
                    robot.Offset = 0;
                }
                else {
                    if (robot.Offset < 0) {
                        robot.Y = robot.Y - 1 * robot.Speed;
                        robot.Offset += 1;
                    }
                    else {
                        robot.Y = robot.Y + 1 * robot.Speed;
                        robot.Offset -= 1;
                    }
                }
            }
            else if (e.Key == Key.A) {
                robot.X -= robot.Speed;
                if (Math.Abs(robot.Offset) < 1) {
                    robot.Offset = 0;
                }
                else {
                    if (robot.Offset < 0) {
                        robot.Y = robot.Y - 1 * robot.Speed;
                        robot.Offset += 1;
                    }
                    else {
                        robot.Y = robot.Y + 1 * robot.Speed;
                        robot.Offset -= 1;
                    }
                }
            }
            else if(e.Key == Key.Enter) {
                robot.X = 50;
                robot.Y = 100;
            }
            

            
        }

        static CombinedGeometry RenderedIntersect(Visual ctx, Shape s1, Shape s2) {
            var p1 = new Pen(Brushes.Transparent, 0.01);
            var t1 = s1.TransformToAncestor(ctx) as Transform;
            var t2 = s2.TransformToAncestor(ctx) as Transform;
            var g1 = s1.RenderedGeometry;
            var g2 = s2.RenderedGeometry;
            if (s1 is Line)
                g1 = g1.GetWidenedPathGeometry(p1);
            if (s2 is Line)
                g2 = g2.GetWidenedPathGeometry(p1);
            g1.Transform = t1;
            g2.Transform = t2;
            return new CombinedGeometry(GeometryCombineMode.Intersect, g1, g2);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            Environment.Exit(0);
        }

        private void line_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                Console.WriteLine($"Top = {Canvas.GetTop(line)}; Left = {Canvas.GetLeft(line)}");
                Console.WriteLine($"Y = {e.GetPosition(cnv).Y.ToString()}; X = {e.GetPosition(cnv).X.ToString()}");
                Canvas.SetTop(line, e.GetPosition(cnv).X);
            }
        }

        private void line_MouseUp(object sender, MouseButtonEventArgs e) {
            Console.WriteLine("UP!");
        }

        private void cnv_MouseMove(object sender, MouseEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                Canvas.SetTop(line, e.GetPosition(cnv).Y);
                Canvas.SetLeft(line, e.GetPosition(cnv).X - line.Width / 2);
            }
            else if (e.RightButton == MouseButtonState.Pressed) {
                RotateTransform r = new RotateTransform();
                r.Angle = e.GetPosition(cnv).Y / 5;
                line.RenderTransform = r;
            }
        }
    }
}
