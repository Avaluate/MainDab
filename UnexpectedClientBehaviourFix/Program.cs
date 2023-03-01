// By Main_EX#3898
// Made this program for myself since ones I found online were broken and did not work
// Also this simplifies the fixing process so you don't have to always manually find the files and delete it
// Decided to release this out because why not be more helpful :D


using System;
using System.IO;
using System.Net;

namespace UnexpectedClientBehaviourFix
{
    class Program
    {
        static void Main(string[] args)
        {
            // Variables
            string localappdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            // Intro and credits and stuff
            Console.Title = "Unexpected Client Behaviour (Error 268) Fix | By Main_EX#3898";
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\nRun this if Roblox is constantly spamming you with 'Unexpected Client Behaviour' :3\n\n");
            Console.Write("Select an option :\n1) Fix error\n2) Revert fix (use this if Roblox isn't opening)\n\nOption : ");
            switch (Console.ReadLine())
            {
                case "1":
                    // Start fix
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("=== Running fix ===\n\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Checking for GlobalBasicSettings_13.xml... ");
                    if (File.Exists(localappdata + "/Roblox/GlobalBasicSettings_13.xml")) // Delete GlobalBasicSettings_13.xml
                    {
                        try
                        {
                            File.Delete(localappdata + "/Roblox/GlobalBasicSettings_13.xml");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("Found and deleted!\n\n");
                        }
                        catch
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write("Not found, most likely already fixed\n\n");
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write("Not found, most likely already fixed\n\n");
                    }

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Checking for GlobalSettings_13.xml... ");
                    if (File.Exists(localappdata + "/Roblox/GlobalSettings_13.xml")) // Possible it still exists somewhere...
                    {
                        try
                        {
                            File.Delete(localappdata + "/Roblox/GlobalSettings_13.xml");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("Found and deleted!\n\n");
                        }
                        catch
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write("Not found, most likely already fixed\n\n");
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write("Not found, most likely already fixed\n\n");
                    }

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Checking for AnalysticsSettings.xml... ");
                    if (File.Exists(localappdata + "/Roblox/AnalysticsSettings.xml")) // Delete AnalysticsSettings.xml
                    {
                        try
                        {
                            File.Delete(localappdata + "/Roblox/AnalysticsSettings.xml");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("Found and deleted!\n\n");
                        }
                        catch
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write("Not found, most likely already fixed\n\n");
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write("Not found, most likely already fixed\n\n");
                    }

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Checking for frm.cfg... ");
                    if (File.Exists(localappdata + "/Roblox/frm.cfg")) // Delete frm.cfg
                    {
                        try
                        {
                            File.Delete(localappdata + "/Roblox/frm.cfg");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("Found and deleted!\n\n");
                        }
                        catch
                        {
                            Console.Write("Not found, most likely already fixed\n\n");
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write("Not found, most likely already fixed\n\n");
                    }

                    // Part 2 : Use DLL fix
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Implementing DLL fix...\n");

                    // I couldn't find a way to get the current version of Roblox online (aka https://setup.rbxcdn.com/version, since it's not the version)
                    // I have to loop though the folders and try find RobloxPlayerBeta.exe
                    
                    string[] Folders = Directory.GetDirectories(localappdata + "\\Roblox\\Versions");
                    foreach (string Sub in Folders)
                    {
                        string[] Files = Directory.GetFiles(Sub);
                        foreach (string FileName in Files)
                        { 
                           if (FileName == Sub + "\\RobloxPlayerBeta.exe")
                            {
                                WebClient Wowie = new WebClient();
                                if (File.Exists(Sub + "\\XInput1_4.dll"))
                                {
                                    File.Delete(Sub + "\\XInput1_4.dll");
                                }
                                Wowie.DownloadFile("https://github.com/gogo9211/Roblox-Woof/releases/download/release/XInput1_4.dll", Sub + "\\XInput1_4.dll");
                            }
                        }
                    }

                    // Finish message
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("\nDLL fix ran and done successfully!"); // All program is literally doing is deleting files... how could it go wrong
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("\nIf you still have any issues, feel free to DM me at Main_EX#3898 (most likely in one of your mutal servers)");
                    Console.Write("\n\nIf Roblox doesn't open after running this fix, rerun this program and select option 2! Press any key to exit...");
                    Console.ReadKey();
                    break;
                
                case "2":

                    // Exact same method to delete the DLL
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("\nDeleting DLL fix...\n");

                    string[] Folders2 = Directory.GetDirectories(localappdata + "\\Roblox\\Versions");
                    foreach (string Subbie in Folders2)
                    {
                        string[] Files = Directory.GetFiles(Subbie);
                        foreach (string FileName in Files)
                        {
                            if (FileName == Subbie + "\\RobloxPlayerBeta.exe")
                            {
                                WebClient Wowie = new WebClient();
                                if (File.Exists(Subbie + "\\XInput1_4.dll"))
                                {
                                    File.Delete(Subbie + "\\XInput1_4.dll");
                                }
                            }
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("\nDLL fix revertion ran and done successfully!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("\nIf you still have any issues, feel free to DM me at Main_EX#3898 (most likely in one of your mutal servers)");
                    Console.Write("\n\nIf Roblox doesn't open after reverting the fix, rerun this program and select option 1! Press any key to exit...");
                    Console.ReadKey();
                    break;
            }
            
        }
    }
}
