﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using Terminal.Models;

namespace Terminal.ViewModel
{
    class TermBind : INotifyPropertyChanged
    {
        static private bool isOpen; //If any view (i.e. SettingsView or HelpView) is visible, this is true
        static public bool IsOpen
        {
            get { return isOpen; }
            set
            {
                if (value != isOpen)
                {
                    isOpen = value;
                }
            }
        }

        private string dispText;
        public string DispText
        {
            get { return dispText; }
            set
            {
                if (!value.Equals(dispText))
                {
                    dispText = value;
                    OnPropertyChanged();
                }
            }

        }

        private string curTerm;
        public string CurTerm
        {
            get { return curTerm; }
            set
            {
                if (!value.Equals(curTerm))
                {
                    curTerm = value;
                    OnPropertyChanged();
                }
            }

        }

        private double shadowAngle;
        public double ShadowAngle
        {
            get { return shadowAngle; }
            set
            {
                if (value != shadowAngle)
                {
                    shadowAngle = value;
                    OnPropertyChanged();
                }
            }

        }

        public MediaPlayer BackgroundPlayer = new MediaPlayer();
        public SoundPlayer player = new SoundPlayer(AppDomain.CurrentDomain.BaseDirectory + "Sounds/print2.wav");



        public List<Cell> Cells { get; } =
        Enumerable.Range(0, 1024).Select(i => new Cell()).ToList();

        Thread flickerThread;
        Thread writeThread;
        //private static object writeLock = new object();
        private static Semaphore _pool;

        public TermBind()
        {
            _pool = new Semaphore(1, 1);
            flickerThread = new Thread(FlickerThread);
            flickerThread.IsBackground = true;
            flickerThread.Start();

            Uri soundPath =  new Uri(AppDomain.CurrentDomain.BaseDirectory + "Sounds/whirr.mp3", UriKind.RelativeOrAbsolute);
            BackgroundPlayer.MediaFailed += (o, args) =>
            {
                MessageBox.Show("Media Failed!!");
            };
            BackgroundPlayer.Open(soundPath);
            
            BackgroundPlayer.MediaEnded += new EventHandler(Media_Ended);
            BackgroundPlayer.Play();

        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Thread Functions
        public void WriteThread(string stringIn)
        {
                _pool.WaitOne();
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    ((MainWindow)App.Current.MainWindow).ToggleScroll();
                });

                for (int i = 0; i < stringIn.Length; i++)
                {
                    DispText += stringIn[i];
                    
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        ((MainWindow)App.Current.MainWindow).Scroll();
                    });
                    Thread.Sleep(3);
                    if (i % 31 == 0 || i % 41 == 0)
                    {
                        Thread.Sleep(40);
                        player.Play();
                    }
                }

                Thread.Sleep(50);
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    DispText += "\n>";
                    ((MainWindow)App.Current.MainWindow).Scroll();
                    ((MainWindow)App.Current.MainWindow).ToggleScroll();
                });
            _pool.Release();
        }

        public void WriteThread(string stringIn, int waitTime)
        {
            _pool.WaitOne();
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                ((MainWindow)App.Current.MainWindow).ToggleScroll();
            });

            for (int i = 0; i < stringIn.Length; i++)
            {
                DispText += stringIn[i];
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    ((MainWindow)App.Current.MainWindow).Scroll();
                });
                Thread.Sleep(waitTime);
            }

            Thread.Sleep(50);
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                DispText += "\n>";
                ((MainWindow)App.Current.MainWindow).Scroll();
                ((MainWindow)App.Current.MainWindow).ToggleScroll();
            });
            _pool.Release();
        }

        public void WaitThread(int waitTime)
        {
            _pool.WaitOne();
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                ((MainWindow)App.Current.MainWindow).ToggleScroll();
            });


            Thread.Sleep(waitTime);


            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                ((MainWindow)App.Current.MainWindow).Scroll();
                ((MainWindow)App.Current.MainWindow).ToggleScroll();
            });
            _pool.Release();
        }


        public void ClearThread(TextBox box)
        {
            _pool.WaitOne(0);
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                ((MainWindow)App.Current.MainWindow).ToggleScroll();
            });

            double lineNumber=5;
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                lineNumber = (box.ActualHeight / box.FontSize) + 5;
            });

            Thread.Sleep(200);

            for (int i = 0; i < (int)lineNumber; i++)
            {
                DispText += "\n";
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    ((MainWindow)App.Current.MainWindow).Scroll();
                });
                Thread.Sleep(100);
            }

            Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        ((MainWindow)App.Current.MainWindow).Scroll();
                        ((MainWindow)App.Current.MainWindow).ToggleScroll();
                    });
            _pool.Release();
        }


        public void FlickerThread()
        {
            Thread.CurrentThread.IsBackground = true;
            Thread.Sleep(100);
            while (true)
            {
                ShadowAngle = 330;
                Thread.Sleep(50);
                ShadowAngle = 320;
                Thread.Sleep(50);
                ShadowAngle = 310;
                Thread.Sleep(50);
            }
        }
        #endregion

        #region Write Functions
        public void WriteToDisp(string stringIn)
        {
            _pool.WaitOne(0);
            writeThread = new Thread(() => WriteThread(stringIn));
            writeThread.IsBackground = true;
            writeThread.Start();
        }

        public void WriteToDisp(string stringIn, int waitTime)
        {
            _pool.WaitOne(0);
            writeThread = new Thread(() => WriteThread(stringIn, waitTime));
            writeThread.IsBackground = true;
            writeThread.Start();
        }

        public void Wait(int waitTime)
        {
            _pool.WaitOne(0);
            writeThread = new Thread(() => WaitThread(waitTime));
            writeThread.IsBackground = true;
            writeThread.Start();
        }

        public void ClearScroll(TextBox box)
        {
            _pool.WaitOne(0);
            writeThread = new Thread(() => ClearThread(box));
            writeThread.IsBackground = true;
            writeThread.Start();
        }
        #endregion

        private void Media_Ended(object sender, EventArgs e)
        {
            MediaPlayer media = sender as MediaPlayer;
            media.Position = new TimeSpan(0, 0, 8); 
            media.Play();
        }


    }
}
