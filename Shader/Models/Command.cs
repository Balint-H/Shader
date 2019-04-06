using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using Terminal.ViewModel;

namespace Terminal.Models
{
    public class Command
    {
        private static string arguement;
        public static string Arguement { get => arguement; set => arguement = value; }
        private string cmdName;
        public string CmdName { get => cmdName; set => cmdName = value; }
        private string cmdDescription;
        public string CmdDescription { get => cmdDescription; set => cmdDescription = value; }
        public List<string> Args;
        public Thread CmdThread;
        public delegate void OnCmd(Command myCommand);
        private OnCmd cmdAction;
        public OnCmd CmdAction
        {
            get { return cmdAction; }
            set
            {
                if (cmdAction != value)
                {
                    cmdAction = value;
                    CmdThread = new Thread(() => cmdAction(this));
                    CmdThread.IsBackground = true;
                    StringBuilder descBuilder = new StringBuilder();
                    descBuilder.Append("\nCommand ");
                    descBuilder.Append(this.CmdName);
                    descBuilder.Append(" can be used with the following arguements:");
                    foreach(string curArg in Args)
                    {
                        descBuilder.Append("\n-");
                        descBuilder.Append(curArg);
                    }
                    descBuilder.Append("\n");
                    CmdDescription = descBuilder.ToString();
                }
            }
        }



        public void Execute()
        {
            CmdThread.Start();
            CmdThread = new Thread(() => cmdAction(this));
            CmdThread.IsBackground = true;

        }
       



    }
}
