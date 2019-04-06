using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminal.Models
{
    class TermScreen
    {
        CommandList ScreenCommands = new CommandList();
        string name;
        static List<TermScreen> Screens= new List<TermScreen>();

        static TermScreen()
        {
            Screens.Add(new TermScreen
            {
                ScreenCommands = new CommandList(),

            });
        }
    }
}
