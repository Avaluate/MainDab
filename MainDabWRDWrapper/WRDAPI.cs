using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

namespace MainDabWRDWrapper
{
    public class WRDAPI
    {

        // MainDab already allocates a console

        
        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();
        

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow); // show = 5, hide = 0


        // import exports from dlls
        [DllImport("wearedevs_exploit_api.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte initialize();

        [DllImport("wearedevs_exploit_api.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool isAttached();

        [DllImport("wearedevs_exploit_api.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void execute([MarshalAs(UnmanagedType.LPStr)] string script);


        public static string WRDLink = "https://wrdcdn.net/r/2/exploit%20api/wearedevs_exploit_api.dll";

        public static Thread WRDInit()
        {
            if (!File.Exists("wearedevs_exploit_api.dll"))
            {
                WebClient WC = new WebClient();
                try
                {
                    WC.DownloadFile(WRDLink, "wearedevs_exploit_api.dll");
                    WC.Dispose();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            AllocConsole();
            Thread initthread = new Thread(delegate ()
            {
                initialize();
                ShowWindow(GetConsoleWindow(), 5);
                Console.Title = "MainDab WeAreDevs Wrapper";
            });
            initthread.Start();
            return initthread;

        }

        public static void Execute(String Script)
        {
            execute(Script);
            ShowWindow(GetConsoleWindow(), 5);
            Console.Title = "MainDab WeAreDevs Wrapper";
        }

        public static bool IsInjected()
        {
            ShowWindow(GetConsoleWindow(), 5);
            Console.Title = "MainDab WeAreDevs Wrapper";
            if (isAttached()) { Console.WriteLine("attached");  return true; }
            else { Console.WriteLine("not attached"); return false; }
        }
    }
}
