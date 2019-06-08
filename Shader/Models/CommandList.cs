using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Terminal.ViewModel;

namespace Terminal.Models
{
    static class Highscore
    {
        static public List<string> Names = new List<string>();
        static public List<int> Scores = new List<int>();
        static Highscore()
        {
            Names.Add("MRC");
            Names.Add("MRC");
            Names.Add("MRC");
            Scores.Add(15);
            Scores.Add(13);
            Scores.Add(2);
        }
        public static void Submit(string nameIn, int scoreIn)
        {
            for (int i = 0; i < 3; i++)
            {
                if (scoreIn > Scores[i])
                {
                    Scores.Insert(i, scoreIn);
                    Names.Insert(i, nameIn);
                    break;
                }

            }
        }

    }

    public class CommandList
    {
        private List<Command> _commands;

        public List<Command> Commands
        {
            set
            {
                if (value != _commands) _commands = value;
            }

            get
            {
                return _commands;
            }
        }


        public CommandList()
        {
            _commands = new List<Command>();
            _commands.Add(new Command
            {
                Args = new List<string>() { "D1", "D2", "D3" },
                CmdName = "OPEN",
                CmdAction = CmdOpen

            });
            _commands.Add(new Command
            {
                Args = new List<string>() { "SNAKE", "CURVE" },
                CmdName = "PLAY",
                CmdAction = CmdPlay

            });
            _commands.Add(new Command
            {
                Args = Commands.Select(i=>i.CmdName).ToList(),
                CmdName = "HELP",
                CmdAction = CmdHelp

            });


        }

        public void Hi()
        {
            //hi indeed
        }

        public void CallCommand(string calledCommand)
        {
            foreach (Command curCommand in Commands)
            {
                if (curCommand.CmdName.Equals(calledCommand) || (">" + curCommand.CmdName).Equals(calledCommand))
                {
                    curCommand.Execute();
                    return;
                }
            }

            ((TermBind)Application.Current.MainWindow.DataContext).WriteToDisp("\nCommand " + calledCommand + " not recognised");
            TermBind.IsOpen = false;
        }

        #region Cmds
        public static void CmdOpen(Command myCommand)
        {
            Thread.CurrentThread.IsBackground = true;

            foreach (string curArg in myCommand.Args)
            {
                if (curArg.Equals(Command.Arguement))
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        ((TermBind)Application.Current.MainWindow.DataContext).WriteToDisp("\nOpening " + Command.Arguement + "\n");
                    });
                    TermBind.IsOpen = false;
                    return;
                }

            }
            StringBuilder doors = new StringBuilder();

            foreach (string curArg in myCommand.Args)
            {
                doors.Append("\n-" + curArg);
            }
            doors.Append("\n");
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                ((TermBind)Application.Current.MainWindow.DataContext).WriteToDisp("\nUnable to open " + Command.Arguement + ".\n\nThe following objects can be opened:" + doors);
            });
            TermBind.IsOpen = false;

        }

        public static void CmdName()
        {
            Thread.CurrentThread.IsBackground = true;
            TermBind.IsOpen = true;
            Command.Arguement = "poop";
            while (true)
            {
                if (!Command.Arguement.Equals("poop"))
                {
                    Highscore.Submit(Command.Arguement.Substring(1), Snake.Score);
                    break;
                }
            }
            Application.Current.Dispatcher.Invoke((Action)delegate
            {

                ((TermBind)Application.Current.MainWindow.DataContext).WriteToDisp("\nScore submitted");

            });
            TermBind.IsOpen = false;
        }

        public static void CmdHelp(Command myCommand)
        {

            Application.Current.Dispatcher.Invoke((Action)delegate
            {

                foreach (Command curCmd in ((MainWindow)Application.Current.MainWindow).CurList.Commands)
                {
                    if (curCmd.CmdName.Equals(Command.Arguement))
                    {
                        ((TermBind)Application.Current.MainWindow.DataContext).WriteToDisp(curCmd.CmdDescription);
                        return;
                    }
                }
                ((TermBind)Application.Current.MainWindow.DataContext).WriteToDisp("\nCan't help you with that");
            });

            TermBind.IsOpen = false;
        }

        public static void CmdPlay(Command myCommand)
        {

            Thread.CurrentThread.IsBackground = true;

            switch (Command.Arguement)
            {
                case "SNAKE":
                    CmdSnake(myCommand);
                    break;
                case "CURVE":
                    CmdCurve(myCommand);
                    break;
                default:
                    StringBuilder games = new StringBuilder();

                    foreach (string curArg in myCommand.Args)
                    {
                        games.Append("\n-" + curArg);
                    }
                    games.Append("\n");
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        ((TermBind)Application.Current.MainWindow.DataContext).WriteToDisp("\nNo installed game called " + Command.Arguement + ".\n\nThe following games are installed:" + games);
                    });
                    TermBind.IsOpen = false;
                    break;
            }
        }


        public static void CmdCurve(Command myCommand)
        {
            Thread.CurrentThread.IsBackground = true;

            Application.Current.Dispatcher.Invoke((Action)delegate
            {

                ((TermBind)Application.Current.MainWindow.DataContext).WriteToDisp(@"
   ____   __    __   ______    __    __    _____  
  / ___)  ) )  ( (  (   __ \   ) )  ( (   / ___/  
 / /     ( (    ) )  ) (__) ) ( (    ) ) ( (__    
( (       ) )  ( (  (    __/   \ \  / /   ) __)   
( (      ( (    ) )  ) \ \  _   \ \/ /   ( (      
 \ \___   ) \__/ (  ( ( \ \_))   \  /     \ \___  
  \____)  \______/   )_) \__/     \/       \____\ 
                                                      

Enter >START to begin, or >SCORE to see highscores");

            });
            Thread.Sleep(250);
            string origArg = Command.Arguement;
            while (origArg == Command.Arguement) Thread.Sleep(100);
            while (true)
            {
                if (Command.Arguement == ">START")
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {

                        ((MainWindow)Application.Current.MainWindow).GameOn();

                    });
                    Snake.Hi();
                    break;
                }
                if (Command.Arguement == ">SCORE")
                {
                    StringBuilder myScore = new StringBuilder();
                    myScore.Append("\n");
                    for (int i = 0; i < 3; i++)
                    {
                        myScore.Append("\n" + Highscore.Names[i] + "\t\t" + Highscore.Scores[i].ToString());
                    }
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {

                        ((TermBind)Application.Current.MainWindow.DataContext).WriteToDisp(myScore.ToString());

                    });
                    Command.Arguement = "none";
                }
            }
        }


        public static void CmdSnake(Command myCommand)
        {
            Thread.CurrentThread.IsBackground = true;

            Application.Current.Dispatcher.Invoke((Action)delegate
            {

                ((TermBind)Application.Current.MainWindow.DataContext).WriteToDisp("\n     ██████ ███▄    █ ▄▄▄      ██ ▄█▓█████     \n   ▒██    ▒ ██ ▀█   █▒████▄    ██▄█▒▓█   ▀     \n   ░ ▓██▄  ▓██  ▀█ ██▒██  ▀█▄ ▓███▄░▒███       \n     ▒   ██▓██▒  ▐▌██░██▄▄▄▄██▓██ █▄▒▓█  ▄     \n   ▒██████▒▒██░   ▓██░▓█   ▓██▒██▒ █░▒████▒    \n   ▒ ▒▓▒ ▒ ░ ▒░   ▒ ▒ ▒▒   ▓▒█▒ ▒▒ ▓░░ ▒░ ░    \n   ░ ░▒  ░ ░ ░░   ░ ▒░ ▒   ▒▒ ░ ░▒ ▒░░ ░  ░    \n   ░  ░  ░    ░   ░ ░  ░   ▒  ░ ░░ ░   ░       \n         ░          ░      ░  ░  ░     ░  ░    \n\n\nEnter >START to begin, or >SCORE to see highscores");

            });
            Thread.Sleep(250);
            string origArg = Command.Arguement;
            while (origArg == Command.Arguement) Thread.Sleep(100);
            while (true)
            {
                if (Command.Arguement == ">START")
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {

                        ((MainWindow)Application.Current.MainWindow).GameOn();

                    });
                    Snake.Hi();
                    break;
                }
                if (Command.Arguement == ">SCORE")
                {
                    StringBuilder myScore = new StringBuilder();
                    myScore.Append("\n");
                    for (int i = 0; i < 3; i++)
                    {
                        myScore.Append("\n" + Highscore.Names[i] + "\t\t" + Highscore.Scores[i].ToString());
                    }
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {

                        ((TermBind)Application.Current.MainWindow.DataContext).WriteToDisp(myScore.ToString());

                    });
                    Command.Arguement = "none";
                }
            }
        }

        #endregion
    }
}
