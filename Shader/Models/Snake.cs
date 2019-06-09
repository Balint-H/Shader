using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.ComponentModel;
using Terminal.ViewModel;

namespace Terminal.Models
{
    static class Snake
    {
        static public int SnakeLife;
        public static Direction SnakeDir;
        static SnakePos headPos;
        static public List<SnakeBod> SnakeBods;
        static public Crumb targetCrumb;
        static int snakeWait;
        static object crumbLock;
        static Random rnd;
        static int cycles;
        private static int score;
         
        public enum Direction : int
        {
            LEFT = 0,
            UP = 1,
            RIGHT = 2,
            DOWN = 3,
        }

        internal static SnakePos HeadPos
        {
            get => headPos;
            set
            {
                if (headPos.X != value.X || headPos.Y != value.Y)
                {
                    headPos = value;
                    if (headPos.X > 31 || headPos.X < 1 || headPos.Y > 31 || headPos.Y < 1)
                    {
                        SnakeLife = -10;
                        return;
                    }

                    bool exists = false;


                    foreach (SnakeBod curBod in SnakeBods)
                    {
                        if (curBod.BodPos.EqualPos(value))
                        {
                            exists = true;
                            if (curBod.Age > SnakeLife - 1) curBod.Age = 0;
                            else SnakeLife = -10;
                        }
                    }

                    if (!exists) SnakeBods.Add(new SnakeBod(value.X, value.Y));

                    if (headPos.EqualPos(targetCrumb.CrumbPos))
                    {
                        SnakeLife++;
                        Score++;
                        cycles = 0;
                        if (snakeWait > 10) snakeWait -= 5;

                    };

                }
            }
        }

        public static int Score { get => score;

        set
            {
                if (value!=score)
                {
                    score = value;
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        ((TermBind)Application.Current.MainWindow.DataContext).CurTerm = value.ToString();
                    });
                }
            }

        }

        static Snake()
        {

        }



        static Thread UpdateThread;
        static void UpdateSnake()
        {
            Thread.CurrentThread.IsBackground = true;

            while (SnakeLife > 0)
            {
                foreach (SnakeBod curBod in SnakeBods)
                {
                    if (curBod.Age++ < 2)
                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            ((MainWindow)App.Current.MainWindow).SetSnakePix(curBod.BodPos.X * 32 + curBod.BodPos.Y, true);
                        });
                    else if (curBod.Age >= SnakeLife) Application.Current.Dispatcher.Invoke((Action)delegate
                   {
                       ((MainWindow)App.Current.MainWindow).SetSnakePix(curBod.BodPos.X * 32 + curBod.BodPos.Y, false);
                   });
                }

                switch (SnakeDir)
                {
                    case Direction.LEFT:
                        HeadPos = new SnakePos(HeadPos.X - 1, HeadPos.Y);
                        break;
                    case Direction.UP:
                        HeadPos = new SnakePos(HeadPos.X, HeadPos.Y - 1);
                        break;
                    case Direction.RIGHT:
                        HeadPos = new SnakePos(HeadPos.X + 1, HeadPos.Y);
                        break;
                    case Direction.DOWN:
                        HeadPos = new SnakePos(HeadPos.X, HeadPos.Y + 1);
                        break;
                }


                cycles++;
                cycles = cycles % 100;

                if (cycles==1)
                {
                    lock (crumbLock)
                    {
                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {

                             ((MainWindow)App.Current.MainWindow).SetSnakePix(targetCrumb.CrumbPos.X * 32 + targetCrumb.CrumbPos.Y, false);

                        });
                    }
                    targetCrumb = new Crumb(rnd.Next(5, 26), rnd.Next(5, 26));
               
                }

                Application.Current.Dispatcher.Invoke((Action)delegate
                {

                    ((MainWindow)App.Current.MainWindow).SetSnakePix(targetCrumb.CrumbPos.X * 32 + targetCrumb.CrumbPos.Y, true);

                });
                Thread.Sleep(snakeWait);
            }
            Thread.Sleep(2000);
            foreach (SnakeBod curBod in SnakeBods)
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    ((MainWindow)App.Current.MainWindow).SetSnakePix(curBod.BodPos.X * 32 + curBod.BodPos.Y, false);
                });
            }
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                ((MainWindow)App.Current.MainWindow).GameOver(Score, Games.SNAKE);
            });
        }

        static public void Hi()
        {
            Score = 0;
            headPos = new SnakePos(16, 16);
            SnakeBods = new List<SnakeBod>();
            SnakeBods.Add(new SnakeBod(16, 16));
            SnakeLife = 10;
            snakeWait = 150;
            SnakeDir = Direction.LEFT;
            UpdateThread = new Thread(UpdateSnake);
            UpdateThread.IsBackground = true;
            rnd = new Random();
            targetCrumb = new Crumb(0, 0);
            cycles = 0;
            crumbLock = new object();
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                for (int i = 0; i < 32; i++)
                {
                    ((MainWindow)App.Current.MainWindow).SetSnakePix(i, true);
                    ((MainWindow)App.Current.MainWindow).SetSnakePix(i + 992, true);
                    ((MainWindow)App.Current.MainWindow).SetSnakePix(i * 32, true);
                    ((MainWindow)App.Current.MainWindow).SetSnakePix(i * 32 + 31, true);
                }
            });
            UpdateThread.Start();
            
        }
    }

    class Crumb
    {
        SnakePos crumbPos;
        int age;

        public int Age { get => age; set => age = value; }
        internal SnakePos CrumbPos { get => crumbPos; set => crumbPos = value; }

        public Crumb(int xIn, int yIn)
        {
            CrumbPos = new SnakePos(xIn, yIn);
        }
    }

    class SnakeBod
    {
        private SnakePos bodPos;
        private int age;

        internal SnakePos BodPos { get => bodPos; set => bodPos = value; }
        public int Age { get => age; set => age = value; }

        public SnakeBod(int xIn, int yIn)
        {
            BodPos = new SnakePos();
            BodPos = new SnakePos(xIn, yIn);
            Age = 0;
        }
    }



    internal struct SnakePos
    {
        public int X;
        public int Y;

        public SnakePos(int xIn, int yIn)
        {
            X = xIn;
            Y = yIn;
        }

        public bool EqualPos(SnakePos posIn)
        {
            if (X == posIn.X && Y == posIn.Y) return true;
            return false;
        }

    }

    public class Cell : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isVisible;
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                if (isVisible != value)
                {
                    isVisible = value;
                    PropertyChanged?.Invoke(this,
                        new PropertyChangedEventArgs(nameof(IsVisible)));
                    

                }
            }
        }
    }
}
