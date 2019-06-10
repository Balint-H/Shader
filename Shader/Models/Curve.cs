
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
        static Point [] headPos;
        static double stepSize;
        static public bool [] leftTurn;
        static public bool [] rightTurn;
        static double[] curangle;
        static Thread updateThread;
        static bool run;
        static Dictionary<Point, bool> collision;
        static int baselength;
        static int dotlength;
        static int gaplength;
        static SolidColorBrush myBrush;
        static double width=0;
        static double height=0;
        static int prec = 2;
        public static int playerCount;

        static Curve()
        {
    

        }

        static public void Hi()
        {

            headPos = new Point[] { new Point(20, 20), new Point(width - 20, height - 20) };
            stepSize = 2;
            leftTurn = new bool[] { false, false };
            rightTurn = new bool[] { false, false };
            run = true;
            collision = new Dictionary<Point, bool>();
            curangle = new double[] { 0, Math.PI };
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
                for (int i = 0; i < playerCount; i++)
                {
                    if (leftTurn[i]) curangle[i] -= 0.05;
                    else if (rightTurn[i]) curangle[i] += 0.05;
                    headPos[i].X += Math.Cos(curangle[i]) * stepSize;
                    headPos[i].Y += Math.Sin(curangle[i]) * stepSize;

                    if (headPos[i].X < 0) break;
                    if (headPos[i].Y < 0) break;
                    if (headPos[i].X > width) break;
                    if (headPos[i].Y > height) break;

                    if (round < baselength)
                    {
                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            ((MainWindow)Application.Current.MainWindow).Expand(headPos);
                        });

                        run = !(CollisionUpdate(collision, headPos, curangle, stepSize, prec));
                    }
                    else if (round == baselength)
                    {

                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            ((MainWindow)Application.Current.MainWindow).NewLine();
                            ((MainWindow)Application.Current.MainWindow).Expand(headPos);
                        });

                        run = !(CollisionCheck(collision, headPos, curangle, stepSize, prec));
                    }
                    else if (round < baselength + dotlength)
                    {
                        run = !(CollisionCheck(collision, headPos, curangle, stepSize, prec));
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
                        run = !(CollisionCheck(collision, headPos, curangle, stepSize, prec));
                    }
                    else if (round < baselength + dotlength + gaplength + dotlength)
                    {
                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            ((MainWindow)Application.Current.MainWindow).ClearAt(dotlength);
                            ((MainWindow)Application.Current.MainWindow).Expand(headPos);
                        });
                        run = !(CollisionUpdate(collision, headPos, curangle, stepSize, prec));
                    }
                    else
                    {
                        round = 0;
                    }
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
