using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

using System.Threading;
using Microsoft.Win32;
using System.Windows;

namespace MainDabRedo.Execution.CustomInjection
{
    class Functions
    {
        public static OpenFileDialog openfiledialog = new OpenFileDialog
        {
            Filter = "Script File|*.txt;*.lua|All files (*.*)|*.*",
            FilterIndex = 1,
            RestoreDirectory = true,
            Title = "Open File"
        };
        public static void Inject()
        {
            new Thread(delegate ()
            {
                bool flag = NamedPipes.NamedPipeExist(NamedPipes.luapipename);
                if (flag)
                {
                    MessageBox.Show("Already injected!", "Error");
                }
                else
                {
                    bool flag2 = !NamedPipes.NamedPipeExist(NamedPipes.luapipename);
                    if (flag2)
                    {
                        switch (Injector.DllInjector.GetInstance.Inject("RobloxPlayerBeta", AppDomain.CurrentDomain.BaseDirectory + Functions.exploitdllname))
                        {
                            case Injector.DllInjectionResult.DllNotFound:
                                MessageBox.Show("MainDab.dll was not found! Please restart the application!");
                                break;

                            case Injector.DllInjectionResult.GameProcessNotFound:
                                MessageBox.Show("Couldn't find RobloxPlayerBeta.exe!", "Roblox isn't started!");
                                break;

                            case Injector.DllInjectionResult.InjectionFailed:
                                MessageBox.Show("Injection Failed!", "Failed for whatever reason (try kill roblox or restart ur pc)");
                                break;
                        }
                    }
                }
            }).Start();
        }
        public static string exploitdllname = "MainDab.dll";
    }
}
