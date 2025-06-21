using MainDabWRDWrapper;
using Microsoft.Win32;
using System;
using System.Drawing;

namespace MainDabWRDWrapper
{
    internal class Program
    {
        static string WrapperVersion = "1.0";
        static async Task Main(string[] args)
        {
            Console.Title = "MainDab WeAreDevs Wrapper";

            RegistryKey SettingReg = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\MainDabWRDWrapper"); // From the settings we saved
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\MainDabWRDWrapper");
            key.SetValue("WrapperVersion", WrapperVersion);
            key.Close();

            var server = new PipeProcess("MainDabWRDWrapper");
            Console.WriteLine("Starting pipe server, please don't close this window (literally don't)...");
            await server.StartAsync();
        }
    }
}