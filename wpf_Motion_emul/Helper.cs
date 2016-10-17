using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_Motion_emul {
    class Helper {
        public struct MyPoint {
            public double X;
            public double Y;
            public int countOfPackets;
        }
        public List<MyPoint> Map = new List<MyPoint>();
        public double MaxCur = 0.3; // максимальная корекция за один пакет
        public double Distance = 50; // растояние между головкой и лазером

        int laserCor = 1;
        double robotX;
        double robotY;
        public void Work() {
            int counter = 0;
            while (true) {
                if (laserCor != 0) { // данные с лазера
                    if (Map.Count == 0) {
                        Map.Add(new MyPoint() {
                            X = (int)robotX + Distance,
                            Y = robotY + laserCor,
                            countOfPackets = counter
                        });
                        counter = 0;
                    }
                    else {
                        if (Map[Map.Count - 1].X != (int)robotX) {
                            Map.Add(new MyPoint() {
                                X = (int)robotX + Distance,
                                Y = robotY + laserCor,
                                countOfPackets = counter
                            });
                            counter = 0;
                        }
                        else {
                            counter++;
                        }
                    }
                }

                for (int i = 0; i < Map.Count; i++) {
                    if (robotX > Map[i].X) {
                        if (true) {

                        }
                    }
                }
            }

        }
    }
}

    

