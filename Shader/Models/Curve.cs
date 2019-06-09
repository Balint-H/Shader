using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using static Terminal.Models.BresenhamRedux;

namespace Terminal.Models
{
    static class Curve
    {
        static Point headPos;
        static double stepSize;
        static public bool leftTurn;
        static public bool rightTurn;
        static double curangle = 0;
        static Thread updateThread;
        static bool run;
        static Dictionary<Point, bool> collision;
        static int baselength;
        static int dotlength;
        static int gaplength;
        static SolidColorBrush myBrush;
        static double width=0;
        static double height=0;

        static Curve()
        {
    

        }

        static public void Hi()
        {

            headPos = new Point(20, 20);
            stepSize = 2;
            leftTurn = false;
            rightTurn = false;
            run = true;
            collision = new Dictionary<Point, bool>();
            curangle = 0;
            baselength = 200;
            dotlength = 5;
            gaplength = 20;
            myBrush = new SolidColorBrush(Colors.Lime);

            

            updateThread = new Thread(() => CurveUpdate());
            updateThread.IsBackground = true;
            updateThread.Start();

        }

        static void CurveUpdate()
        {
            
            int round = 0;
            
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                (width, height) = ((MainWindow)Application.Current.MainWindow).GetBoundaries();

            });

            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                ((MainWindow)Application.Current.MainWindow).NewLine();
                ((MainWindow)Application.Current.MainWindow).Expand(new Point(0, 0));
                ((MainWindow)Application.Current.MainWindow).Expand(new Point(0, height));
                ((MainWindow)Application.Current.MainWindow).Expand(new Point(width, height));
                ((MainWindow)Application.Current.MainWindow).Expand(new Point(width, 0));
                ((MainWindow)Application.Current.MainWindow).Expand(new Point(0, 0));
                ((MainWindow)Application.Current.MainWindow).NewLine();

            });
            Thread.Sleep(1000);
            while (run)
            {
                if (leftTurn) curangle -= 0.05;
                else if (rightTurn) curangle += 0.05;
                headPos.X += Math.Cos(curangle) * stepSize;
                headPos.Y += Math.Sin(curangle) * stepSize;

                if (headPos.X < 0) break;
                if (headPos.Y < 0) break;
                if (headPos.X > width) break;
                if (headPos.Y > height) break;       

                    if (round < baselength)
                    {
                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            ((MainWindow)Application.Current.MainWindow).Expand(headPos);
                        });

                        run = !(CollisionUpdate(collision, headPos, curangle, stepSize, 2));
                    }
                    else if (round == baselength)
                    {

                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            ((MainWindow)Application.Current.MainWindow).NewLine();
                            ((MainWindow)Application.Current.MainWindow).Expand(headPos);
                        });

                        run= !(CollisionCheck(collision, headPos, curangle, stepSize, 2));
                    }
                    else if (round < baselength + dotlength)
                    {
                        run= !(CollisionCheck(collision, headPos, curangle, stepSize, 2));
                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            ((MainWindow)Application.Current.MainWindow).Expand(headPos);
                        });

                    }
                    else if (round < baselength + dotlength + gaplength)
                    {
                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            ((MainWindow)Application.Current.MainWindow).ClearAt(dotlength);
                            ((MainWindow)Application.Current.MainWindow).Expand(headPos);
                        });
                        run =  !(CollisionCheck(collision, headPos, curangle, stepSize, 2));
                    }
                    else if (round < baselength + dotlength + gaplength + dotlength)
                    {
                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            ((MainWindow)Application.Current.MainWindow).ClearAt(dotlength);
                            ((MainWindow)Application.Current.MainWindow).Expand(headPos);
                        });
                        run = !(CollisionUpdate(collision, headPos, curangle, stepSize, 2));
                    }
                    else
                    {
                        round = 0;
                    }
               
                Thread.Sleep(20);
                round++;
            }
            Thread.Sleep(1500);
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                ((MainWindow)Application.Current.MainWindow).GameOver(0, Games.CURVE);
            });

        }
    }
}
