using System;
using System.Collections.Generic;
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

namespace wpf_Motion_emul.Controls {
    /// <summary>
    /// Логика взаимодействия для Laser_Viewer.xaml
    /// </summary>
    public partial class Laser_Viewer : UserControl {
        public Laser_Viewer() {
            InitializeComponent();
            slider.ValueChanged += Slider_ValueChanged;
            Offset = 0;
        }
        public double Offset;
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            Offset = slider.Value - 40;
            Offset -= 200;
            Offset /= 4;
            lb_offset.Content = Offset.ToString();
        }

        public double GetOffset() {
            return Offset;
        }
    
    }
}
