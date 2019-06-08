using MyEffects;
using ShaderEffectLibrary;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using Terminal.ViewModel;
using Terminal.Models;
using System.Windows.Threading;
using System.Media;

namespace Terminal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// https://stackoverflow.com/questions/14948171/how-to-emulate-a-console-in-wpf
    /// </summary>
    public partial class MainWindow : Window
    {
        public CommandList CurList = new CommandList();
        public SoundPlayer EntSound = new SoundPlayer(AppDomain.CurrentDomain.BaseDirectory + "Sounds/enter.wav");
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new TermBind();
            Disp.Effect = new SmoothMagnifyEffect();
            PixelGrid.Effect = new PixelateEffect();
            BloomGrid.Effect = new BloomEffect();
            Scan.Effect = new SmoothMagnifyEffect();
            ((PixelateEffect)PixelGrid.Effect).HorizontalPixelCounts = 1200;
            ((PixelateEffect)PixelGrid.Effect).VerticalPixelCounts = 800;
            ((SmoothMagnifyEffect)Disp.Effect).InnerRadius -= 0.7;
            ((SmoothMagnifyEffect)Disp.Effect).OuterRadius += 0.5;
            ((SmoothMagnifyEffect)Disp.Effect).Magnification -= 0.7;
            ((BloomEffect)BloomGrid.Effect).BaseIntensity++;
            ((SmoothMagnifyEffect)Scan.Effect).InnerRadius -= 0.7;
            ((SmoothMagnifyEffect)Scan.Effect).OuterRadius += 0.5;
            ((SmoothMagnifyEffect)Scan.Effect).Magnification -= 0.7;
            ((TermBind)DataContext).CurTerm = "Hypothalamic Terminal";
            InpScroll.Visibility = Visibility.Hidden;
            Inp.Focus();
            Closing += TermBind.OnWindowClosing;
            //
            ((TermBind)DataContext).WriteToDisp("\n          ║                      __  ________ __  __\n     ╔═══╝║╚═══╗                 | \\ | |_   _|  \\/  |\n     ╚══╗▒╠═╔══╝                 |  \\| | | | | \\  / | \n   ╔════╩╗║░╠════╗               | . ` | | | | |\\/| | \n   ╚══╗░╔╩╬═╝░╔══╝               | |\\  |_| |_| |  | | \n╔═════╩═╗▒▓░╬═╩═════╗            |_|_\\_|_____|_|__|_| \n╚════╗░╗╚░▓▒╝░░╔════╝            |  _ \\| |  | |/ ____|\n ╔═══╝░╚══╩╗╔╗▒╚═══╗             | |_) | |  | | (___  \n ╚═════╗░▒░╠╝╠═════╝             |  _ <| |  | |\\___ \\ \n     ╔═╝▒░╔╝░╚═╗                 | |_) | |__| |____) |\n     ╚═══╗║╔═══╝                 |____/ \\____/|_____/\n          ║		");
            ((TermBind)DataContext).Wait(2000);
            ((TermBind)DataContext).ClearScroll(Inp);
            ((TermBind)DataContext).WriteToDisp("\n███████╗██╗   ██╗███╗   ██╗ █████╗ ██████╗ ███████╗███████╗\n██╔════╝╚██╗ ██╔╝████╗  ██║██╔══██╗██╔══██╗██╔════╝██╔════╝\n███████╗ ╚████╔╝ ██╔██╗ ██║███████║██████╔╝███████╗█████╗  \n╚════██║  ╚██╔╝  ██║╚██╗██║██╔══██║██╔═══╝ ╚════██║██╔══╝  \n███████║   ██║   ██║ ╚████║██║  ██║██║     ███████║███████╗\n╚══════╝   ╚═╝   ╚═╝  ╚═══╝╚═╝  ╚═╝╚═╝     ╚══════╝╚══════╝\n\t\t\t\t\t\t\tv3.4");
            ((TermBind)DataContext).WriteToDisp("Welcome Amber!\nSynapse System v3.4 (TM) booting up");
            ((TermBind)DataContext).WriteToDisp("...\n", 1000);
            ((TermBind)DataContext).WriteToDisp("Performing primary diagnosis");
            ((TermBind)DataContext).Wait(2000);
            ((TermBind)DataContext).WriteToDisp("Exit Status 74\n302 Errors and 5888 Warnings\nShutting d");
            ((TermBind)DataContext).Wait(2000);
            ((TermBind)DataContext).WriteToDisp("status:override");
            ((TermBind)DataContext).WriteToDisp("value:'Force'\n");
            ((TermBind)DataContext).WriteToDisp("Manual Override succesful. Booting up in Basic Mode.\n");
            ((TermBind)DataContext).WriteToDisp("status:accesLevel");
            ((TermBind)DataContext).WriteToDisp("value:0\n");
            ((TermBind)DataContext).WriteToDisp("Flagging functions");
            ((TermBind)DataContext).WriteToDisp("...\n", 1000);
            ((TermBind)DataContext).WriteToDisp("Boot complete");
            ((TermBind)DataContext).Wait(2000);
            ((TermBind)DataContext).ClearScroll(Inp);
            ((TermBind)DataContext).WriteToDisp(" "); 
            
            // ((TermBind)DataContext).WriteToDisp("\n          ║                      __  ________ __  __\n     ╔═══╝║╚═══╗                 | \\ | |_   _|  \\/  |\n     ╚══╗▒╠═╔══╝                 |  \\| | | | | \\  / | \n   ╔════╩╗║░╠════╗               | . ` | | | | |\\/| | \n   ╚══╗░╔╩╬═╝░╔══╝               | |\\  |_| |_| |  | | \n╔═════╩═╗▒▓░╬═╩═════╗            |_|_\\_|_____|_|__|_| \n╚════╗░╗╚░▓▒╝░░╔════╝            |  _ \\| |  | |/ ____|\n ╔═══╝░╚══╩╗╔╗▒╚═══╗             | |_) | |  | | (___  \n ╚═════╗░▒░╠╝╠═════╝             |  _ <| |  | |\\___ \\ \n     ╔═╝▒░╔╝░╚═╗                 | |_) | |__| |____) |\n     ╚═══╗║╔═══╝                 |____/ \\____/|_____/\n          ║		");

        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.D1:
                    if (ToneGrid.Effect == null)
                    {
                        ToneGrid.Effect = new ColorToneEffect();
                        ((ColorToneEffect)ToneGrid.Effect).DarkColor = Colors.Black;
                    }
                    ((ColorToneEffect)ToneGrid.Effect).Toned++;
                    break;
                case Key.D2:
                    if (ToneGrid.Effect == null)
                    {
                        ToneGrid.Effect = new ColorToneEffect();
                        ((ColorToneEffect)ToneGrid.Effect).DarkColor = Colors.Black;
                    }
                    ((ColorToneEffect)ToneGrid.Effect).Toned--;
                    break;
                case Key.D3:
                    ((PixelateEffect)PixelGrid.Effect).HorizontalPixelCounts++;
                    ((PixelateEffect)PixelGrid.Effect).VerticalPixelCounts++;
                    break;
                case Key.D4:
                    ((PixelateEffect)PixelGrid.Effect).HorizontalPixelCounts--;
                    ((PixelateEffect)PixelGrid.Effect).VerticalPixelCounts--;
                    break;
                case Key.D5:
                    ((BloomEffect)BloomGrid.Effect).BloomIntensity++;
                    //     ((PixelateEffect)BloomGrid.Effect);
                    break;
                case Key.D6:
                    ((BloomEffect)BloomGrid.Effect).BloomIntensity--;
                    //     ((PixelateEffect)BloomGrid.Effect);
                    break;
                case Key.D7:
                    ((BloomEffect)BloomGrid.Effect).BaseIntensity++;
                    //     ((PixelateEffect)BloomGrid.Effect);
                    break;
                case Key.D8:
                    ((BloomEffect)BloomGrid.Effect).BaseIntensity--;
                    //     ((PixelateEffect)BloomGrid.Effect);
                    break;
                case Key.Return:
                    EntSound.Play();
                    string upperText = ((TermBind)DataContext).DispText.ToUpper();
                    string[] lines = upperText.Split(new char[1] { '\n' });
                    string[] words = lines[lines.Length - 1].Split(new char[1] { ' ' });
                    if (!TermBind.IsOpen)
                    {

                        if (words.Length >= 2)
                            Command.Arguement = words[1];
                        else
                            Command.Arguement = "None";
                        TermBind.IsOpen = true;
                        CurList.CallCommand(words[0]);
                    }
                    else
                    {
                        Command.Arguement = words[0];
                    }
                    break;

                case Key.Left:
                    if (!TermBind.IsOpen) break;
                    Snake.SnakeDir = (Snake.Direction)((int)(++Snake.SnakeDir) % 4);
                    break;
                case Key.Right:
                    if (!TermBind.IsOpen) break;
                    if (Snake.SnakeDir == Snake.Direction.LEFT) Snake.SnakeDir = Snake.Direction.DOWN;
                    else Snake.SnakeDir--;

                    break;
            }

        }



        #region scroll functions
        public void Scroll()
        {
            Inp.CaretIndex = Inp.Text.Length;
            Inp.ScrollToEnd();
            InpScroll.ScrollToEnd();
        }

        public void ToggleScroll()
        {
            if (InpScroll.Visibility == Visibility.Visible)
            {
                InpScroll.Visibility = Visibility.Hidden;
                Inp.Visibility = Visibility.Visible;
                Inp.Focus();
                return;
            }
            InpScroll.Visibility = Visibility.Visible;
            Inp.Visibility = Visibility.Hidden;
            Inp.Focus();
        }

        #endregion

        #region game functions
        object SnakeLock = new object();

        public void SetSnakePix(int indexIn, bool stateIn)
        {
            lock (SnakeLock)
            {
                ((TermBind)DataContext).Cells[indexIn].IsVisible = stateIn;
            }
        }


        public void GameOver(int score)
        {
            SnakeGrid.Visibility = Visibility.Hidden;
            Inp.Visibility = Visibility.Visible;
            InpScroll.Visibility = Visibility.Hidden;
            Inp.Focus();
            ((TermBind)DataContext).CurTerm = "Hypothalamic Terminal";

            ((TermBind)DataContext).WriteToDisp("\n\nFinal score of "+Snake.Score.ToString()+"\n\nEnter Name:");
            Thread nameThread = new Thread(CommandList.CmdName);
            nameThread.IsBackground = true;
            nameThread.Start();

        }

        public void GameOn()
        {
            InpScroll.Visibility = Visibility.Hidden;
            Inp.Visibility = Visibility.Hidden;
            SnakeGrid.Visibility = Visibility.Visible;
        }
        #endregion
    }
}
