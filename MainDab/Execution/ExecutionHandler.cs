using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MainDabRedo.Execution
{
    class ExecutionHandler
    {
        public static bool Inject()
        {
            if (SelectedAPI.API == "Selected API: WeAreDevs API")
            {
                // kill previous wrappers
                try{ foreach (Process proc in Process.GetProcessesByName("MainDabWRDWrapper")) { proc.Kill();} } catch { }
                try{ foreach (Process proc in Process.GetProcessesByName("WRDFakeServer")) { proc.Kill(); } } catch { }

                Process.Start("MainDabWRDWrapper.exe");
                Thread.Sleep(1000);
                try
                {
                    var Response = SelectedAPI.NewPipe.SendRequest<InjectionRequest>("Inject", new InjectionRequest{AdditionalData = "blank"}); // blank
                    Console.WriteLine($"injection result: {Response}");
                    if (Response.InjectionSuccessful == true)
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show($"WRD injection failed: {Response.AdditionalData}");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"error during injection: {ex.Message}");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static void Execute(string script)
        {
            Process[] pname = Process.GetProcessesByName("RobloxPlayerBeta");
            if (pname.Length < 1) // If Roblox is not running
            {
                MessageBox.Show("Please run Roblox first before attempting to inject");
                return;
            }

            if (Execution.SelectedAPI.API == "Selected API: WeAreDevs API")
            {
                Process[] pname1 = Process.GetProcessesByName("MainDabWRDWrapper");
                if (pname1.Length < 1) 
                {
                    MessageBox.Show("Please begin WRD injection first before attempting to execute a script");
                    return;
                }

                try
                {
                    var Response = SelectedAPI.NewPipe.SendRequest<ExecutionRequest>("Execute", new ExecutionRequest { Script = script });
                    Console.WriteLine($"execution result: {Response}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"error during execution: {ex.Message}");
                }
            }
        }

        public static bool IsInjected()
        {   
            if (Execution.SelectedAPI.API == "Selected API: WeAreDevs API")
            {
                try
                {
                    var Response = SelectedAPI.NewPipe.SendRequest<IsInjectedRequest>("IsInjected", new IsInjectedRequest{ AdditionalData = "blank" }); // blank
                    Console.WriteLine($"isinjected result: {Response}");
                    return Response.IsInjected;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while checking for isinjected res: {ex}");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static void Stop()
        {
            if (Execution.SelectedAPI.API == "Selected API: WeAreDevs API")
            {
                try
                {
                    foreach (Process proc in Process.GetProcessesByName("MainDabWRDWrapper"))
                    {
                        proc.Kill();
                    }
                }
                catch { }
            }    
        }
    }
}
