using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Oxygen;
using EasyExploits;

namespace MainDabRedo.Execution
{
    class ExecutionHandler
    {
        public static void Execute(string script)
        {            
            EasyExploits.Module ezclap = new EasyExploits.Module(); // EasyExploits API
            WeAreDevs.ExploitAPI wrd = new WeAreDevs.ExploitAPI(); // WeAreDevs API

            Process[] pname = Process.GetProcessesByName("RobloxPlayerBeta");
            if (pname.Length > 0)
            {
                if (Execution.SelectedAPI.API == "Selected API: EasyExploits API")
                {
                    try
                    {
                        ezclap.ExecuteScript(script);
                    }
                    catch (Exception sexual)
                    {
                        MessageBox.Show("An error has occured while attempting to execute. Here is the error:\n\n" + sexual + "\n\nTry reopen both Roblox and MainDab. If you need further help, you can join MainDab's Discord at discord.io/maindab", "MainDab");
                    }
                }
                else if (Execution.SelectedAPI.API == "Selected API: WeAreDevs API")
                {
                    wrd.SendLuaScript(script);
                }
                else if (Execution.SelectedAPI.API == "Selected API: Oxygen U API")
                {
                    Oxygen.Execution.Execute(script);
                }
                else if (Execution.SelectedAPI.API == "Selected API: Krnl API")
                {
                    KRNL.MainAPI.Execute(script);
                }
                else if (Execution.SelectedAPI.API == "Selected API: Custom") // Used to be a thing
                {
                    try
                    {
                        CustomInjection.NamedPipes.LuaPipe(script);
                    }
                    catch (Exception sexual)
                    {
                        MessageBox.Show("An error has occured while attempting to execute. Here is the error:\n\n" + sexual + "\n\nTry reopen both Roblox and MainDab. If you need further help, you can join MainDab's Discord at discord.io/maindab", "MainDab");
                    }

                }
                else
                {
                    MessageBox.Show("You must inject before executing a script! Click the inject button to continue.", "MainDab");
                }
            }
            else
            {
                MessageBox.Show("You must inject before executing a script! Click the inject button to continue.", "MainDab");
            }
        }
    }
}
