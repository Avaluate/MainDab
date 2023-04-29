/*
  __  __       _       _____        _     
 |  \/  |     (_)     |  __ \      | |    
 | \  / | __ _ _ _ __ | |  | | __ _| |__  
 | |\/| |/ _` | | '_ \| |  | |/ _` | '_ \ 
 | |  | | (_| | | | | | |__| | (_| | |_) |
 |_|  |_|\__,_|_|_| |_|_____/ \__,_|_.__/ 
                 
 You can join MainDab's Discord server at discord.io/maindab
 This is the source code for MainDab, which can be found at https://github.com/MainDabRblx/MainDabUISource
                                          
 MainDab by Main_EX#3898 (on Discord)

*/

/*
 A few notes:
 - JUST BECAUSE IT'S OPEN SOURCE DOESN'T MEAN THE CODE IS GOOD! Some code here may be written in a very horrible fashion.
 - NOT EVERYTHING HERE HAS BEEN ANNOTATED! Things here aren't fully organised in order.
 - YOU ARE EXPECTED TO KNOW BASIC C#!

 Also as a reminder:
 - Just because MainDab is "open source"
 - You may be missing quite a few dependencies. The majority of these dependencies are restored via NuGet.
 - The Pastebin dependency can be downloaded here : https://github.com/MainDabRblx/ProjectDab/raw/master/PastebinAPIs.dll
 - Exploit API dependencies can be found by downloading them from their proper pages, which should should be able to find online
*/


// References
using DiscordRPC;
using DiscordRPC.Logging;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using Newtonsoft.Json;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Newtonsoft.Json.Linq;

namespace MainDabRedo
{

    public partial class MainWindow : Window
    {
        // VARIABLES //
        string CurrentVersion = "MainDab 15.1 SP8"; // The version of MainDab for this specific build

        // The default text editor text
        string DefaultTextEditorText = "--[[\r\nWelcome to MainDab!\r\nMake sure to join MainDab's Discord at discord.io/maindab\r\nIf you need help, join our Discord!\r\n--]]\r\n-- Paste in your text below this comment.\r\n\r\nprint(\"MainDab Moment\")";

        // Variables relating to injection
        bool InjectionInProgress = false; // When injection is in progress, self explanatory
        bool OxygenInjected = false; // This function is here just so the status text shows whether Oxygen is injecting or not

        // Animation variables (I'm still not good at WPF)
        private bool CloseCompleted = false; // Window fade-in

        // Scripthub stuff
        bool IsScriptHubOpened = false;
        bool IsGameHubOpened = false;

        // Theme
        bool IsDefaultTheme = true; // So it won't apply on startup smh
        bool IsAvalonLoaded = false; // Prevent more errors from occuring

        // Variables for the theming, you'll see it later on
        string CurrentLuaXSHDLocation = ""; // The path for the AvalonEdit syntax highlighting. Will be needed for future theming

        string LeftRGB = ""; // Top left gradient
        string RightRGB = ""; // Bottom right gradient
        string BGImageURL = ""; // Background image URL
        string BGTransparency = ""; // Background image transparency, on a 0 to 1 scale

        // Variables for the AvalonEdit background colours, there's probably a much better way
        int AvalonEditBGA = 6; // A (Transparency)
        int AvalonEditBGR = 47; // R
        int AvalonEditBGG = 47; // G
        int AvalonEditBGB = 49; // B

        string AvalonEditFont = "Consolas"; // Font to be used for editor, this is for future customisation options

        // Variables for custom icons (to be implemented in the future)
        Array scripts = null;
        Array gamescripts = null;

        // WebClient Creation
        WebClient WebStuff = new WebClient(); // Create a new generally used WebClient

        // Execution
        // Note : Both EasyExploits API and OxygenAPI are added as a reference and not actually copied into the execution folder!
        EasyExploits.Module EasyExploitsModule = new EasyExploits.Module();
        Execution.WeAreDevs.ExploitAPI WeAreDevsModule = new Execution.WeAreDevs.ExploitAPI(); // WeAreDevs API

        // DETECTION //
        private void Detection()
        {
            string Check = WebStuff.DownloadString("https://clientsettings.roblox.com/v2/user-channel?binaryType=WindowsPlayer");
            if (Check != "{\"channelName\":\"LIVE\"}")
            {
                foreach (Process proc in Process.GetProcessesByName("RobloxPlayerBeta"))
                {
                    proc.Kill();
                }
                MessageBox.Show("MainDab Warning", "Byfron has been detected in your Roblox installation. MainDab will not work with Byfron.\n\nJoin discord.io/maindab for more help. A document with more information about Byfron will open up in your browser.\n\nCode: " + Check);
                Process.Start("https://docs.google.com/document/d/1rITy7pHJz8VaiG5U6x3XFmx8LzG0YKH4peebo7r5pwE/edit");
                Environment.Exit(0);
            }
        }

        // WINDOW INITILISATION //
        public MainWindow()
        {
            InitializeComponent();
            MainWin.WindowStartupLocation = WindowStartupLocation.CenterScreen; // Center MainDab to the middle of the screen
            Detection();
            // UPDATE SYSTEM //

            // First, we want to check and see if the updater is still there

            if (File.Exists("MainDabUpdater.exe"))
            {
                File.Delete("MainDabUpdater.exe"); // If it is, we should delete it
            }

            string Version = WebStuff.DownloadString("https://raw.githubusercontent.com/MainDabRblx/ProjectDab/master/UpdateStuff/Version");
            WebStuff.Dispose(); // Remember to dispose the WebClient! Or someone will scold me for it

            // .FirstOrDefault() is nessesary since GitHub always adds an extra line for some reason
            // If I don't do this, then the string that would return is "MainDab 14.3/n" rather than "MainDab 14.3", so basically an additional unwanted line!
            string OnlineVersion = Version.Split(new[] { '\r', '\n' }).FirstOrDefault();
            
            if (CurrentVersion != OnlineVersion) // If the current version is not equal to the value online
            {
                // Downloading MainDab's Updater

                WebStuff.DownloadFile("https://github.com/MainDabRblx/ProjectDab/raw/main/MainDab%20Updater.exe", "MainDabUpdater.exe");
                WebStuff.Dispose();

                // Downloading MainDab's Updater

                // We have to set it like this since the updater needs the right startup path to run correctly
                Directory.SetCurrentDirectory(Directory.GetCurrentDirectory());
                Process.Start("MainDabUpdater.exe"); // Run the updater
                Environment.Exit(0);
                // Note : The updater automatically deletes MainDab.exe


            }

            // SETUP //
            // So basically, what I want to do is to organise the folders and stuff

            // All these folders are needed for MainDab to run
            // This can be written in a shorter way, but I'll just leave it like this
            if (!Directory.Exists("Applications")) // Tools
            {
                Directory.CreateDirectory("Applications");
            }
            if (!Directory.Exists("EditorThemes"))
            {
                Directory.CreateDirectory("EditorThemes");
            }
            if (!Directory.Exists("Scripts"))
            {
                Directory.CreateDirectory("Scripts");
            }
            if (!Directory.Exists("Themes"))
            {
                Directory.CreateDirectory("Themes");
            }
            if (!Directory.Exists("Workspace"))
            {
                Directory.CreateDirectory("Workspace");
            }

            // Theme checking for Avalon stuff, etc
            if (File.Exists("EditorThemes\\lua_md_default.xshd"))
            {
                File.Delete("EditorThemes\\lua_md_default.xshd"); // We want to update default theme regardless lol
                string penis = WebStuff.DownloadString("https://raw.githubusercontent.com/MainDabRblx/ProjectDab/master/Themes/lua_md_default.xshd");
                File.WriteAllText("EditorThemes\\lua_md_default.xshd", penis);
            }
            else
            {
                // The Avalon syntax thing doesn't exist, so we'll just download it
                string penis = WebStuff.DownloadString("https://raw.githubusercontent.com/MainDabRblx/ProjectDab/master/Themes/lua_md_default.xshd");
                File.WriteAllText("EditorThemes\\lua_md_default.xshd", penis);
            }
            CurrentLuaXSHDLocation = "EditorThemes\\lua_md_default.xshd";
            IsAvalonLoaded = true;
            // Set location
            // CurrentLuaXSHDLocation = "Default.xshd";


            // Finally, load scripthub data
            this.Dispatcher.Invoke(async () => // Prevent error from this being done on "another thread"
            {
                scripts = await ScriptHub.MainDabSC.GetSCData(); // Extract data from json file
                gamescripts = await ScriptHub.MainDabGSC.GetGSCData(); // Extract data from json file
            });

            // Now load KRNL API
            Execution.KRNL.MainAPI.Load();

        }

        // Make MainDab actually draggable
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Draggable top window
            DragMove();
        }

        // MainDab's counter, loads in a browser then deletes itself
        // I didn't know how to make an actual proper counter so here we go
        private void WebBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            Counter.Visibility = Visibility.Visible;
            dynamic activeX = this.Counter.GetType().InvokeMember("ActiveXInstance",
                    BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                    null, this.Counter, new object[] { }); // We want to hide errors
            activeX.Silent = true; // There we go

            // Now load the counter up
            Counter.Navigate("https://maindabrblx.github.io/avaluate.github.io/");
            new Thread(() =>
            {
                // We are gonna delete it later so less memory usage in the background
                Thread.CurrentThread.IsBackground = true;
                Thread.Sleep(25000); // It can take this long to load :sob:
                this.Dispatcher.Invoke(() =>
                {
                    Counter.Dispose();
                    MainGrid.Children.Remove(Counter); // we will "try" to remove it
                    GC.Collect();
                });
            })
            {

            }.Start();

        }

        // Theme settings
        private void ThemeLoading(object sender, RoutedEventArgs e)
        {
            try
            {
                RegistryKey SettingReg = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\MainDabTheme");
                if (SettingReg != null)
                {
                    string Thingy = SettingReg.GetValue("ISDEFAULT").ToString();
                    if (Thingy == "No")
                    {
                        string LeftRGBVal = SettingReg.GetValue("LEFTRGB").ToString();
                        string RightRGBVal = SettingReg.GetValue("RIGHTRGB").ToString();
                        string BGImageURLVal = SettingReg.GetValue("BGIMAGEURL").ToString();
                        string BGImageTransparencyVal = SettingReg.GetValue("BGTRANSPARENCY").ToString();
                        float transparencyval = float.Parse(BGImageTransparencyVal);
                        transparencyval = transparencyval / 100;

                        // Border
                        var conv = new ColorConverter();
                        LinearGradientBrush brushy = new LinearGradientBrush();
                        brushy.StartPoint = new Point(0, 0);
                        brushy.EndPoint = new Point(1, 1);
                        brushy.GradientStops.Add(new GradientStop((Color)conv.ConvertFrom(LeftRGBVal), 0.0));
                        brushy.GradientStops.Add(new GradientStop((Color)conv.ConvertFrom(RightRGBVal), 0.0));
                        WindowBorder.BorderBrush = brushy;

                        ImageBrush bb = new ImageBrush();
                        Image image = new Image();
                        image.Source = new BitmapImage(new Uri(BGImageURLVal));
                        image.Opacity = transparencyval;
                        bb.ImageSource = image.Source;
                        bb.Opacity = transparencyval;
                        MainGrid.Background = bb;
                        MainGrid.Background.Opacity = transparencyval;

                        IsDefaultTheme = false;
                    }
                }
            }
            catch
            {
                // Prevent themes from breaking MainDab on startup
            }

        }


        // TAB CREATION FUNCTIONS //
        // Honestly these functions were actually pasted off other sources A LONG TIME AGO
        // http://avalonedit.net/documentation/ for any other references
        // Note that these are also "Sentinel" tabs

        public TextEditor CreateNewTab()
        {
            TextEditor textEditor = new TextEditor // Here, we make a new Avalon editor
            {
                // Setup some settings
                LineNumbersForeground = new SolidColorBrush(Color.FromRgb(199, 197, 197)),
                ShowLineNumbers = true,
                Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                Background = new SolidColorBrush(Color.FromArgb((byte)AvalonEditBGA, (byte)AvalonEditBGR, (byte)AvalonEditBGB, (byte)AvalonEditBGG)),
                FontFamily = new FontFamily(AvalonEditFont),
                HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden,
                VerticalScrollBarVisibility = ScrollBarVisibility.Visible,
                WordWrap = true
            };

            // Set some settings regarding the editor
            textEditor.Options.EnableEmailHyperlinks = false;
            textEditor.Options.EnableHyperlinks = false; // The URL looks ugly
            textEditor.Options.AllowScrollBelowDocument = false;

            // Loop until Avalon has loaded
            while (IsAvalonLoaded == false)
            {
                Thread.Sleep(500);
            }

            // This one is for the theming for Avalon
            Stream xshd_stream = File.OpenRead(CurrentLuaXSHDLocation);
            XmlTextReader xshd_reader = new XmlTextReader(xshd_stream);
            textEditor.SyntaxHighlighting = HighlightingLoader.Load(xshd_reader, HighlightingManager.Instance); // Now finally set it

            xshd_reader.Close();
            xshd_stream.Close();
            return textEditor;
        }

        // Get the text from the current texteditor
        // You can call this using CurrentTabWithStuff()
        // For example, to set the texteditor text, CurrentTabWithStuff().Text = "Text here";
        public TextEditor CurrentTabWithStuff() 
        {
            return this.TabControl.SelectedContent as TextEditor;
        }
        
        // Create a new tab
        public TabItem CreateTab(string text = "", string title = "Tab")
        {
            title = title + " " + TabControl.Items.Count.ToString(); // Counts the amount of tabs
            bool loaded = false; // Some weird bugs have been occuring without this here

            // Calls the function CreateNewTab, which is found above
            TextEditor textEditor = CreateNewTab();
            textEditor.Text = text;
            TabItem tab = new TabItem
            {
                Content = textEditor,
                Style = TryFindResource("Tab2") as Style, // Declared in the XAML stuff, https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement.tryfindresource?view=net-5.0
                AllowDrop = true,
                Header = title // From the function

            };

            tab.Loaded += delegate (object source, RoutedEventArgs e) // Function for when the tab is loaded
            {
                if (loaded)
                {
                    return;
                }

                loaded = true; // Prevents some weird bug from occuring, though I doubt this is needed
            };

            // This is the function for the "X" icon, basically close tab button
            tab.MouseDown += delegate (object sender, MouseButtonEventArgs farded)
            {
                if (farded.OriginalSource is Border) // First of all actually check it
                {
                    if (farded.MiddleButton == MouseButtonState.Pressed) // MiddleButton = Left click here
                    {
                        this.TabControl.Items.Remove(tab); // Remove the tab
                        return;
                    }
                }
            };

            // A second loaded function, in order to actually make the magic work
            tab.Loaded += delegate (object seggs, RoutedEventArgs daddy)
            {
                tab.GetTemplateItem<System.Windows.Controls.Button>("CloseButton").Click += delegate (object sjdfaskdfasklf, RoutedEventArgs efdsn)
                {
                    this.TabControl.Items.Remove(tab);
                };

                loaded = true;
            };

            // Now finally set the title
            string oldHeader = title;
            this.TabControl.SelectedIndex = this.TabControl.Items.Add(tab);

            // This is the text that we start off with
            CurrentTabWithStuff().Text = DefaultTextEditorText;
            return tab;

        }

        // Now when the texteditor is loaded
        private void TextEditorLoad(object sender, RoutedEventArgs e)
        {
            // Right, now let's do some editor theming

            if (!Directory.Exists("EditorThemes"))
            {
                Directory.CreateDirectory("EditorThemes"); // We already checked for this in the beginning, this is just a double check
            }

            // Loop until Avalon has loaded
            while (IsAvalonLoaded == false)
            {
                Thread.Sleep(500);
            }

            // Now, let's load up the theme
            Stream input = File.OpenRead(CurrentLuaXSHDLocation);
            XmlTextReader xmlTextReader = new XmlTextReader(input);
            TextEditor.SyntaxHighlighting = HighlightingLoader.Load(xmlTextReader, HighlightingManager.Instance);
            
            // The template is defined in the xaml code
            this.TabControl.GetTemplateItem<System.Windows.Controls.Button>("AddTabButton").Click += delegate (object s, RoutedEventArgs f)
            {
                this.CreateTab("", "Tab");
            };

            // More theming
            foreach (TabItem tab in TabControl.Items)
            {
                tab.GetTemplateItem<System.Windows.Controls.Button>("CloseButton").Width = 0;
            }
            
            // Now actually set it
            Stream nya = File.OpenRead(CurrentLuaXSHDLocation);
            XmlTextReader xml = new XmlTextReader(nya);
            TextEditor.SyntaxHighlighting = HighlightingLoader.Load(xml, HighlightingManager.Instance); 

            CurrentTabWithStuff().Text = DefaultTextEditorText; // Scroll all the way up to the top of this source code to set it
        }

        // This is similar to the function above, but it's actually for the texteditor that first spawns in
        private void TextEditor_Loaded(object sender, RoutedEventArgs e)
        {
            // Set some settings regarding the editor
            TextEditor.Options.EnableEmailHyperlinks = false;
            TextEditor.Options.EnableHyperlinks = false; // The URL looks ugly
            TextEditor.Options.AllowScrollBelowDocument = false;

            // Set some stuff first
            MainWin.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

            // Loop until Avalon has loaded
            while (IsAvalonLoaded == false)
            {
                Thread.Sleep(500);
            }

            // Load file
            Stream input = File.OpenRead(CurrentLuaXSHDLocation);
            XmlTextReader xmlTextReader = new XmlTextReader(input);
            CurrentTabWithStuff().SyntaxHighlighting = HighlightingLoader.Load(xmlTextReader, HighlightingManager.Instance);
        }

        // ADDITIONAL FUNCTIONS //
        // These are where additional functions go

        // Run Discord's RPC, this will reflect in setting options later
        public void DiscordRPC()
        {
            // For usage examples, look at https://github.com/Lachee/discord-rpc-csharp/blob/master/DiscordRPC.Example/Program.cs

            DiscordRpcClient client;
            client = new DiscordRpcClient("795935176873213982") // The ID of the client
            {
                Logger = new ConsoleLogger
                {
                    Level = LogLevel.Warning
                }
            };

            client.OnReady += delegate { };
            client.OnPresenceUpdate += delegate { };
            client.Initialize();
            client.SetPresence(new RichPresence()
            {
                Details = "Using " + CurrentVersion, // Status
                State = "Keyless Roblox Exploit",

                Timestamps = new Timestamps
                {
                    Start = DateTime.UtcNow,
                },

                Assets = new Assets
                {
                    LargeImageKey = "icon_maindab_v3",
                    LargeImageText = "MainDab Roblox Exploit",
                    SmallImageKey = "icon_maindab_v3_side"
                },

                // These show the buttons in the Discord RPC status
                Buttons = new DiscordRPC.Button[]
                {
                    new DiscordRPC.Button() { Label = "Join MainDab's Discord", Url = "https://discord.io/maindab" },
                    new DiscordRPC.Button() { Label = "Get MainDab", Url = "https://github.com/MainDabRblx/ProjectDab/blob/master/MainDabBootstrapper.exe?raw=true" }
                },

            });
        }

        public void Inject()
        {
            // So first, lets check and see if Roblox is already running
            Process[] pname = Process.GetProcessesByName("RobloxPlayerBeta");

            // So if Roblox is indeed running
            if (pname.Length > 0)
            {
                InjectionInProgress = true;

                // The rest here is self explanatory
                if (Execution.SelectedAPI.API == "Selected API: EasyExploits API")
                {
                    string Version = WebStuff.DownloadString("https://raw.githubusercontent.com/MainDabRblx/ProjectDab/master/UpdateStuff/EasyExploitsAPIUse");
                    WebStuff.Dispose(); // Remember to dispose the WebClient! Or someone will scold me for it

                    // .FirstOrDefault() is nessesary since GitHub always adds an extra line for some reason
                    // If I don't do this, then the string that would return is "MainDab 14.3/n" rather than "MainDab 14.3", so basically an additional unwanted line!
                    string OnlineVersion = Version.Split(new[] { '\r', '\n' }).FirstOrDefault();

                    if (OnlineVersion == "No")
                    {
                        MessageBox.Show("The EasyExploits API function is currently disabled manually by MainDab Developers. Please use another API, such as WeAreDevs API. You can join MainDab at discord.io/maindab if you have further questions.", "MainDab");
                    }
                    else
                    {
                        EasyExploitsModule.LaunchExploit();
                    }
                }
                else if (Execution.SelectedAPI.API == "Selected API: WeAreDevs API")
                {
                    WeAreDevsModule.LaunchExploit();
                }
                else if (Execution.SelectedAPI.API == "Selected API: Oxygen U API")
                {
                    Oxygen.API.Inject();
                }
                else if (Execution.SelectedAPI.API == "Selected API: Krnl API")
                {
                    InjectionInProgress = true;
                    Thread.Sleep(100);
                    Execution.KRNL.MainAPI.Inject();
                }
                // Custom API option disabled for now
                /*else if (Execution.SelectedAPI.API == "Selected API: Custom")
                {
                    Execution.CustomInjection.Functions.Inject();
                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;

                        this.Dispatcher.Invoke(() =>
                        {
                            // Status.Content = "Injecting Custom API...";

                        });
                        Thread.Sleep(6000);
                        this.Dispatcher.Invoke(() =>
                        {
                            //Status.Content = "";
                        });
                    }).Start();
                }*/

                else // API not selected yet
                {
                    MessageBox.Show("Please select an API to use first in settings. The option you have selected may be invalid due to MainDab updates.", "MainDab");
                }
            }
            else // Best to tell the user first!
            {
                MessageBox.Show("Please open Roblox first before injecting!", "MainDab");
            }
        }

        // ANIMATIONS //
        // All of these animations are just basic fade in/out anims
        // Oh and these are found in the XAML code

        // Sidebar animations
        // Home icon animation
        private void HomePage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Storyboard sb = TryFindResource("HomeOpen") as Storyboard;
            sb.Begin();

            // This is pretty damn stupid to do but oh well
            HomeGrid.Visibility = Visibility.Visible;
            ExecutorGrid.Visibility = Visibility.Hidden;
            ScriptHubGrid.Visibility = Visibility.Hidden;
            GameHubGrid.Visibility = Visibility.Hidden;
            ToolsGrid.Visibility = Visibility.Hidden;
            SettingsGrid.Visibility = Visibility.Hidden;

           
        }

        // Execution icon animation
        private void ExecutorPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Storyboard sb = TryFindResource("ExecutionOpen") as Storyboard;
            sb.Begin();

            HomeGrid.Visibility = Visibility.Hidden;
            ExecutorGrid.Visibility = Visibility.Visible;
            ScriptHubGrid.Visibility = Visibility.Hidden;
            GameHubGrid.Visibility = Visibility.Hidden;
            ToolsGrid.Visibility = Visibility.Hidden;
            SettingsGrid.Visibility = Visibility.Hidden;
           
        }

        // Scripthub page icon
        private void ScriptHubPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Storyboard sb = TryFindResource("ScriptHubOpen") as Storyboard;
            sb.Begin();

            HomeGrid.Visibility = Visibility.Hidden;
            ExecutorGrid.Visibility = Visibility.Hidden;
            ScriptHubGrid.Visibility = Visibility.Visible;
            GameHubGrid.Visibility = Visibility.Hidden;
            ToolsGrid.Visibility = Visibility.Hidden;
            SettingsGrid.Visibility = Visibility.Hidden;
           
        }

        // Utilities page icon
        private void UtilitiesPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Storyboard sb = TryFindResource("ToolsOpen") as Storyboard;
            sb.Begin();

            HomeGrid.Visibility = Visibility.Hidden;
            ExecutorGrid.Visibility = Visibility.Hidden;
            ScriptHubGrid.Visibility = Visibility.Hidden;
            GameHubGrid.Visibility = Visibility.Hidden;
            ToolsGrid.Visibility = Visibility.Visible;
            SettingsGrid.Visibility = Visibility.Hidden;
          
        }

        // Settings page icon
        private void SettingsPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Storyboard sb = TryFindResource("SettingsOpen") as Storyboard;
            sb.Begin();

            HomeGrid.Visibility = Visibility.Hidden;
            ExecutorGrid.Visibility = Visibility.Hidden;
            ScriptHubGrid.Visibility = Visibility.Hidden;
            GameHubGrid.Visibility = Visibility.Hidden;
            ToolsGrid.Visibility = Visibility.Hidden;
            SettingsGrid.Visibility = Visibility.Visible;
           
        }

        // Settings animations

        // API Selection button
        private void Border_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            Storyboard sb = TryFindResource("APISelection1") as Storyboard;
            sb.Begin();

            APISelection.Visibility = Visibility.Visible;
            GeneralOptions.Visibility = Visibility.Hidden;
            ThemeSelection.Visibility = Visibility.Hidden;
        }

        // General options button
        private void Border_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            Storyboard sb = TryFindResource("GeneralOptions1") as Storyboard;
            sb.Begin();
            APISelection.Visibility = Visibility.Hidden;
            GeneralOptions.Visibility = Visibility.Visible;
            ThemeSelection.Visibility = Visibility.Hidden;
        }

        // Theme selection button
        private void Border_MouseDown_3(object sender, MouseButtonEventArgs e)
        {
            Storyboard sb = TryFindResource("ThemeSelection1") as Storyboard;
            sb.Begin();
            APISelection.Visibility = Visibility.Hidden;
            GeneralOptions.Visibility = Visibility.Hidden;
            ThemeSelection.Visibility = Visibility.Visible;
        }

        // TOP BAR FUNCTIONS //

        // Minimise MainDab
        private void Mini(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        // Exit functions
        // Now, since we want to play a fade out animation, we will need something extra
        private void ExitMD(object sender, MouseButtonEventArgs e)
        {
            this.Close(); // There will be a function that will run when the window closes, MainWin_Closing
        }
        // Closing function
        private void MainWin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!CloseCompleted)
            {
                FormFadeOut.Begin(); // Start the animation storyboard FormFadeOut
                e.Cancel = true;
            }
        }
        // Now when it's actually done, kill off MainDab process
        private void CloseWindow(object sender, EventArgs e)
        {
            CloseCompleted = true;
            Environment.Exit(0);
        }

        // Draggable top window
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Draggable top window
            DragMove();
        }

        // HOME GRID FUNCTIONS //

        // Join Discord button
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            byte[] succ = WebStuff.DownloadData("https://raw.githubusercontent.com/MainDabRblx/ProjectDab/master/UpdateStuff/DiscordLink.txt");
            WebStuff.Dispose();
            string discord = Encoding.UTF8.GetString(succ);
            Process.Start(discord);
        }

        // Get help button
        private void Help(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://maindab.gitbook.io/maindabdocs/"); // MainDab help website
        }

        // Set notice board text
        private void Notice(object sender, RoutedEventArgs e)
        {
            string penis = WebStuff.DownloadString("https://raw.githubusercontent.com/MainDabRblx/ProjectDab/master/UpdateStuff/Notice");
            NoticeBoard.Text = penis;
        }

        // Set changelog board text
        private void ChangelogBoard(object sender, RoutedEventArgs e)
        {
            string penis = WebStuff.DownloadString("https://raw.githubusercontent.com/MainDabRblx/ProjectDab/master/UpdateStuff/Changelog");
            Changelog.Text = penis;
        }

        // EXECUTION GRID FUNCTIONS //

        // Execute script icon
        private void Execute(object sender, MouseButtonEventArgs e)
        {
            Execution.ExecutionHandler.Execute(this.CurrentTabWithStuff().Text); // We have our own execution handler class
            // You can find this in the Execution folder
        }

        // Injection icon
        private void Inject(object sender, MouseButtonEventArgs e)
        {
            Inject(); // Scroll up to find this function or right click and go to definition
        }

        // Clear icon
        private void Clear(object sender, MouseButtonEventArgs e)
        {
            CurrentTabWithStuff().Text = ""; // Clear the currently selected textbox
        }

        // Kill Roblox icon (so end process)
        private void KillRoblox(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to kill Roblox?", "MainDab", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                // Just a basic YesNo thing
            }
            else
            {
                try
                {
                    foreach (Process proc in Process.GetProcessesByName("RobloxPlayerBeta")) // We will loop though each process just to find it
                    // Not sure if there's a more efficient way but this does the job quickly
                    {
                        proc.Kill();
                        MessageBox.Show("Roblox process killed", "MainDab");
                    }

                }
                catch
                {
                    // Just in case the user is stupid or something
                    MessageBox.Show("Roblox process has already been killed, or Roblox isn't running.", "MainDab");
                }
            }
        }

        // Open file function itself
        public static OpenFileDialog openfiledialog = new OpenFileDialog // New OpenFileDialog thing
        {
            Filter = "Text Files and Lua Files (*.txt *.lua)|*.txt;*.lua|All files (*.*)|*.*", // Either any of those will work
            FilterIndex = 1,
            RestoreDirectory = true,
            Title = "Open File"
        };

        // Open file icon
        private void OpenFile(object sender, MouseButtonEventArgs e)
        {
           OpenFileDialog FL = new OpenFileDialog()
            {
                CheckFileExists = true,
                Filter = "Text Files and Lua Files (*.txt *.lua)|*.txt;*.lua|All files (*.*)|*.*" // Same filter
            };

            if (FL.ShowDialog() == true)
            {
                CurrentTabWithStuff().Text = File.ReadAllText(FL.FileName);
            }
        }
        
        // Save file icon
        private void SaveFile(object sender, MouseButtonEventArgs e)
        {
            var saveDialog = new SaveFileDialog
            {
                Title = "Save File",
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                Filter = "Text Files and Lua Files(*.txt *.lua) | *.txt; *.lua | All files(*.*) | *.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (saveDialog.ShowDialog() == true) File.WriteAllText(saveDialog.FileName, CurrentTabWithStuff().Text);
        }

       

        // Sets the selected API
        private void SetAPI(object sender, RoutedEventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\MainDabData");
            if (key != null)
            {
                string apishouldbe = (key.GetValue("DLL").ToString());
                CurrentAPILabel.Content = apishouldbe;
                Execution.SelectedAPI.API = apishouldbe;
            }
            else
            {
                CurrentAPILabel.Content = "Using WeAreDevs API";
                Execution.SelectedAPI.API = "Selected API: WeAreDevs API";
            }
        }

        // Code for injection status
        // This is the function that we are going to run for checking whether it's injected or not
        // There is probably a better way to code this, but it does the job
        private void StatusCheck(Object o)
        {
            Process[] pname = Process.GetProcessesByName("RobloxPlayerBeta");
            if (pname.Length > 0) // If Roblox is running
            {
                if (InjectionInProgress == true)
                {
                    // EasyExploits API check
                    if (Execution.SelectedAPI.API == "Selected API: EasyExploits API")
                    {

                        if (EasyExploitsModule.isInjected())
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                InjectionStatus.Content = "EasyExploits injected";
                                InjectionStatus.Foreground = new SolidColorBrush(Color.FromRgb(0, 192, 140));
                            });
                        }
                        else
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                InjectionStatus.Content = "EasyExploits injection in progress";
                                InjectionStatus.Foreground = new SolidColorBrush(Color.FromRgb(170, 192, 0));
                            });
                        }
                    }

                    // WeAreDevs API check
                    if (Execution.SelectedAPI.API == "Selected API: WeAreDevs API")
                    {
                        if (WeAreDevsModule.isAPIAttached())
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                InjectionStatus.Content = "WeAreDevs injected";
                                InjectionStatus.Foreground = new SolidColorBrush(Color.FromRgb(0, 192, 140));
                            });
                        }
                        else
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                InjectionStatus.Content = "WeAreDevs injection in progress";
                                InjectionStatus.Foreground = new SolidColorBrush(Color.FromRgb(170, 192, 0));
                            });
                        }
                    }

                    // OxygenU API check
                    if (Execution.SelectedAPI.API == "Selected API: Oxygen U API")
                    {

                        if (Oxygen.Execution.Exists() == true)
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                InjectionStatus.Content = "Oxygen U injected";
                                InjectionStatus.Foreground = new SolidColorBrush(Color.FromRgb(0, 192, 140));
                            });
                        }
                        else
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                InjectionStatus.Content = "Oxygen U injection in progress";
                                InjectionStatus.Foreground = new SolidColorBrush(Color.FromRgb(170, 192, 0));
                            });
                        }
                       
                        }
                    // Krnl API check
                    if (Execution.SelectedAPI.API == "Selected API: Krnl API")
                    {

                        if (Execution.KRNL.MainAPI.IsAttached() == true)
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                InjectionStatus.Content = " Krnl injected";
                                InjectionStatus.Foreground = new SolidColorBrush(Color.FromRgb(0, 192, 140));
                            });
                        }
                        else
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                InjectionStatus.Content = "Krnl injection in progress";
                                InjectionStatus.Foreground = new SolidColorBrush(Color.FromRgb(170, 192, 0));
                            });
                        }
                    }
                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {

                        InjectionStatus.Content = "Awaiting injection";
                        InjectionStatus.Foreground = new SolidColorBrush(Color.FromRgb(192, 110, 0));

                    });
                }
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {

                    InjectionStatus.Content = "Roblox not opened";
                    InjectionStatus.Foreground = new SolidColorBrush(Color.FromRgb(192, 0, 0));
                    InjectionInProgress = false;
                });
            }
        }

        private System.Threading.Timer StatusCheckTimer; // Create timer

        private void StartStatusCheck(object sender, RoutedEventArgs e) // Actual function
        { 
            StatusCheckTimer = new System.Threading.Timer(StatusCheck, null, 1000, 1000); // Run the check every 10 seconds or so
        }


        // SCRIPTHUB GRID FUNCTIONS //

        // https://learn.microsoft.com/en-us/dotnet/desktop/wpf/properties/dependency-properties-overview?view=netdesktop-6.0
        public static readonly DependencyProperty ScriptStringVal = DependencyProperty.Register("ScriptString", typeof(ScriptDetails), typeof(TabThingy));

        public struct ScriptDetails // String for the script itself
        {
            public string ScriptExecute;
        }

        // Get details of script
        public ScriptDetails SetStringValue
        {
            get => (ScriptDetails)GetValue(ScriptStringVal);
            set => SetValue(ScriptStringVal, value);
        }

        
        // When the scripthub panel loads
        private void WrapPanel_LoadedAsync(object sender, RoutedEventArgs e)
        {
            
        }

        // TOOLS GRID FUNCTIONS //

        // Multi Roblox Instance
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (File.Exists("Applications\\multirblx.exe"))
            {
                Process.Start("Applications\\multirblx.exe");
            }
            else
            {
                MessageBox.Show("Downloading Multiple Roblox. Click OK to continue.", "MainDab");
                // Taken from one of my personal projects at https://github.com/MainDabRblx/MultipleRobloxInstances
                WebStuff.DownloadFile("https://github.com/MainDabRblx/MultipleRobloxInstances/releases/download/v1.0/MultipleRobloxInstances.exe", "Applications\\multirblx.exe");
                WebStuff.Dispose();
                Process.Start("Applications\\multirblx.exe");
                MessageBox.Show("Multiple Roblox downloaded and started!", "MainDab");
            }
        }

        // FPS unlocker
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (File.Exists("Applications\\fpsunlocker.exe"))
            {
                Process.Start("Applications\\fpsunlocker.exe");
            }
            else
            {
                MessageBox.Show("Downloading FPS Unlocker. Click OK to continue.", "MainDab");
                // Taken from https://github.com/axstin/rbxfpsunlocker
                WebStuff.DownloadFile("https://github.com/MainDabRblx/ProjectDab/blob/master/fpsunlocker.exe?raw=true", "Applications\\fpsunlocker.exe");
                WebStuff.Dispose();
                Process.Start("Applications\\fpsunlocker.exe");
                MessageBox.Show("FPS unlocker downloaded and started!", "MainDab");
            }
        }

        // SETTINGS GRID FUNCTIONS //
        // There are multiple tabs on this grid //

        // API selection //
        // Basically, what we are going to do here is to check WeAreDevs API status
      
       

        // I'm not too sure how they check anymore, so I removed it for now
        private void EasyExploitAPICheck(object sender, RoutedEventArgs e)
        {
            // This was stupid for me to think it'd work
            /* ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
             string[] array = this.HITLER.DownloadString("https://easyexploits.com/Module").Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
             bool flag = !(array[2] == "true");
             bool flag2 = Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Roblox\\Versions\\" + array[3]);
             if (flag2)
             {
                 EasyExploitsStatus.Fill = new SolidColorBrush(Color.FromRgb(40, 195, 126));
             }
             else
             {
                 EasyExploitsStatus.Fill = new SolidColorBrush(Color.FromRgb(206, 24, 24));
             }*/
        }
        
        // There is no status check for OxygenU API, since they update relatively quickly

        // EasyExploits API selection
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string Version = WebStuff.DownloadString("https://raw.githubusercontent.com/MainDabRblx/ProjectDab/master/UpdateStuff/EasyExploitsAPIUse");
            WebStuff.Dispose(); // Remember to dispose the WebClient! Or someone will scold me for it

            // .FirstOrDefault() is nessesary since GitHub always adds an extra line for some reason
            // If I don't do this, then the string that would return is "MainDab 14.3/n" rather than "MainDab 14.3", so basically an additional unwanted line!
            string OnlineVersion = Version.Split(new[] { '\r', '\n' }).FirstOrDefault();

            if(OnlineVersion == "No")
            {
                MessageBox.Show("The EasyExploits API function is currently disabled manually by MainDab Devlopers. Please use another API, such as WeAreDevs API. You can join MainDab at discord.io/maindab if you have further questions.", "MainDab");
            }
            else
            {
                Execution.SelectedAPI.API = "Selected API: EasyExploits API";
                CurrentAPILabel.Content = "Using EasyExploits API";
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\MainDabData"); // We save the user's selection
                key.SetValue("DLL", "Selected API: EasyExploits API");
                key.Close();
                MessageBox.Show("API set to EasyExploits", "MainDab");
            }
           
        }

        // WeAreDevs API selection
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Execution.SelectedAPI.API = "Selected API: WeAreDevs API";
            CurrentAPILabel.Content = "Using WeAreDevs API";
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\MainDabData");
            key.SetValue("DLL", "Selected API: WeAreDevs API");
            key.Close();
            MessageBox.Show("API set to WeAreDevs", "MainDab");
        }

        // Oxygen API selection
        private void OxygenAPI(object sender, RoutedEventArgs e)
        {
            Execution.SelectedAPI.API = "Selected API: Oxygen U API";
            CurrentAPILabel.Content = "Using Oxygen U API";
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\MainDabData");
            key.SetValue("DLL", "Selected API: Oxygen U API");
            key.Close();
            MessageBox.Show("API set to Oxygen U API", "MainDab");
        }

        // Join Discord for help button
        private void DiscordLinkie(object sender, RoutedEventArgs e)
        {
            Process.Start("https://discord.io/maindab");
        }

        // General options //
        // Most of the code here is pretty much self explanatory
        // Topmost enable/disable
        private void TopmostFunc(object sender, RoutedEventArgs e)
        {
            RegistryKey SettingReg = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\MainDabSettings"); // From the settings we saved
            if (SettingReg != null)
            {
                string TopMostSetting = SettingReg.GetValue("TOPMOST").ToString();
                if (TopMostSetting == "Yes")
                {
                    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\MainDabSettings");
                    key.SetValue("TOPMOST", "No");
                    key.Close();
                    TopMostButton.Background = new SolidColorBrush(Color.FromArgb(20, 33, 33, 33));
                    TopMostButton.Content = "Disabled";
                    Topmost = false;
                }
                else
                {
                    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\MainDabSettings");
                    key.SetValue("TOPMOST", "Yes");
                    key.Close();
                    TopMostButton.Background = new SolidColorBrush(Color.FromArgb(20, 11, 47, 199));
                    TopMostButton.Content = "Enabled";
                    Topmost = true;
                }
            }
            else
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\MainDabSettings");
                key.SetValue("TOPMOST", "Yes");
                key.Close();
                TopMostButton.Background = new SolidColorBrush(Color.FromArgb(20, 11, 47, 199));
                TopMostButton.Content = "Enabled";
                Topmost = true;
            }

        }

        // Load the setting for it
        private void TopmostFuncSetting(object sender, RoutedEventArgs e)
        {
            RegistryKey SettingReg = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\MainDabSettings");
            if (SettingReg != null)
            {
                string TopMostSetting = SettingReg.GetValue("TOPMOST").ToString();
                if (TopMostSetting == "Yes")
                {
                    TopMostButton.Content = "Enabled";
                    TopMostButton.Background = new SolidColorBrush(Color.FromArgb(20, 11, 47, 199));
                    Topmost = true;
                }
            }

        }

        // Discord RPC enable/disable function
        private void DiscordRPCFunc(object sender, RoutedEventArgs e)
        {
            RegistryKey SettingReg = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\MainDabRPC");
            if (SettingReg != null)
            {
                string TopMostSetting = SettingReg.GetValue("DISCORDRPC").ToString();
                if (TopMostSetting == "Yes")
                {
                    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\MainDabRPC");
                    key.SetValue("DISCORDRPC", "No");
                    key.Close();
                    DiscordRPCButton.Background = new SolidColorBrush(Color.FromArgb(20, 33, 33, 33));
                    DiscordRPCButton.Content = "Disabled";
                    MessageBox.Show("Discord RPC disabled, MainDab will now close. Please reopen MainDab to apply settings.", "MainDab");
                    Environment.Exit(0);
                }
                else
                {
                    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\MainDabRPC");
                    key.SetValue("DISCORDRPC", "Yes");
                    key.Close();
                    DiscordRPCButton.Content = "Enabled";
                    DiscordRPCButton.Background = new SolidColorBrush(Color.FromArgb(20, 11, 47, 199));
                    MessageBox.Show("Discord RPC enabled, MainDab will now close. Please reopen MainDab to apply settings.", "MainDab");
                    Environment.Exit(0);
                }
            }
            else
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\MainDabRPC");
                key.SetValue("DISCORDRPC", "Yes");
                key.Close();
                DiscordRPCButton.Content = "Enabled";
                DiscordRPCButton.Background = new SolidColorBrush(Color.FromArgb(30, 11, 47, 199));
                MessageBox.Show("Discord RPC enabled, MainDab will now close. Please reopen MainDab to apply settings.", "MainDab");
                Environment.Exit(0);
            }
        }

        // Load setting for Discord RPC
        private void DiscordRPCSetting(object sender, RoutedEventArgs e)
        {
            if (Registry.CurrentUser.OpenSubKey(@"SOFTWARE\MainDabRPC") != null)
            {
                RegistryKey SettingReg = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\MainDabRPC");
                if (SettingReg != null)
                {
                    if (SettingReg.GetValue("DISCORDRPC").ToString() != null)
                    {
                        string TopMostSetting = SettingReg.GetValue("DISCORDRPC").ToString();
                        if (TopMostSetting == "Yes")
                        {
                            DiscordRPCButton.Content = "Enabled";
                            DiscordRPCButton.Background = new SolidColorBrush(Color.FromArgb(20, 11, 47, 199));
                            DiscordRPC();
                        }
                    }  
                }
                else
                {
                    DiscordRPCButton.Background = new SolidColorBrush(Color.FromRgb(40, 195, 126));
                    DiscordRPCButton.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    DiscordRPC();
                }
            }
            else
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\MainDabRPC");
                key.SetValue("DISCORDRPC", "Yes");
                key.Close();
                DiscordRPCButton.Background = new SolidColorBrush(Color.FromRgb(40, 195, 126));
                DiscordRPCButton.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                DiscordRPC();
            }
        }

        // Use custom theme function
        private void UseCustomTheme(object sender, RoutedEventArgs e)
        {
            RegistryKey SettingReg = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\MainDabTheme");
            if (SettingReg != null)
            {
                if (IsDefaultTheme == false)
                {
                    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\MainDabTheme"); // Save the settings in registry
                    key.SetValue("ISDEFAULT", "No");
                    key.SetValue("LEFTRGB", LeftRGB);
                    key.SetValue("RIGHTRGB", RightRGB);
                    key.SetValue("BGIMAGEURL", BGImageURL);
                    key.SetValue("BGTRANSPARENCY", BGTransparency);
                    key.Close();
                    MessageBox.Show("Theme settings saved!", "MainDab");
                }
                else
                {
                    // Do nothing lol
                    MessageBox.Show("Theme settings saved!", "MainDab");
                }
            }
            else
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\MainDabTheme");
                key.SetValue("ISDEFAULT", "No");
                key.SetValue("LEFTRGB", LeftRGB);
                key.SetValue("RIGHTRGB", RightRGB);
                key.SetValue("BGIMAGEURL", BGImageURL);
                key.SetValue("BGTRANSPARENCY", BGTransparency);
                key.Close();
                MessageBox.Show("Theme settings saved!", "MainDab");
            }
        }

        // Reset to default theme function
        private void ResetTheme(object sender, RoutedEventArgs e)
        {
            RegistryKey SettingReg = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\MainDabTheme");
            if (SettingReg != null)
            {
                if (IsDefaultTheme == false)
                {
                    IsDefaultTheme = true;
                    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\MainDabTheme");
                    key.SetValue("ISDEFAULT", "Yes");
                    key.Close();
                    MessageBox.Show("Theme settings reset! Please reopen MainDab.", "MainDab");
                }
                else
                {
                    // Do nothing lol
                    MessageBox.Show("Theme settings reset! Please reopen MainDab.", "MainDab");
                }
            }
            else
            {
                IsDefaultTheme = true;
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\MainDabTheme");
                key.SetValue("ISDEFAULT", "Yes");
                key.Close();
                MessageBox.Show("Theme settings reset! Please reopen MainDab.", "MainDab");
            }
        }

        // Theme options //
        // This part took time to make as well btw

        // Apply theme button
        private void ThemeApply(object sender, RoutedEventArgs e)
        {
            try
            {
                // Border first
                var conv = new ColorConverter();
                var left = (Color)conv.ConvertFrom(LeftGradient.Text);
                var right = (Color)conv.ConvertFrom(RightGradient.Text);
                LinearGradientBrush brushy = new LinearGradientBrush();
                brushy.StartPoint = new Point(0, 0);
                brushy.EndPoint = new Point(1, 1);
                brushy.GradientStops.Add(new GradientStop(left, 0.0));
                brushy.GradientStops.Add(new GradientStop(right, 1.0));
                WindowBorder.BorderBrush = brushy;

                // Image now alongside transparency
                float transparencyval = float.Parse(ImageTransparency.Text);
                transparencyval = transparencyval / 100;
                ImageBrush bb = new ImageBrush();
                Image image = new Image();
                image.Source = new BitmapImage(
                    new Uri(
                       ImageLink.Text));
                image.Opacity = transparencyval;
                bb.ImageSource = image.Source;
                bb.Opacity = transparencyval;
                MainGrid.Background = bb;
                MainGrid.Background.Opacity = transparencyval;

                // If user wants to set it as default theme
                IsDefaultTheme = false;
                LeftRGB = LeftGradient.Text;
                RightRGB = RightGradient.Text;
                BGImageURL = ImageLink.Text;
                BGTransparency = ImageTransparency.Text;

                MessageBox.Show("Theme set!", "MainDab");
            }
            catch
            { 
                MessageBox.Show("Did you put in the right hex code format and image url?", "MainDab");
            }
            
        }

        // To be removed soon
        private void ThemeDemo1(object sender, RoutedEventArgs e)
        {
            LeftGradient.Text = "#4C464646";
            RightGradient.Text = "#4C464646";
            ImageTransparency.Text = "20";
            CreatorName.Text = "Main_EX";
            ImageLink.Text = "https://art.pixilart.com/b7875a3999e9a79.gif";
            var conv = new BrushConverter();
            WindowBorder.BorderBrush = (Brush)conv.ConvertFrom("#4C464646");
            ImageBrush bb = new ImageBrush();
            Image image = new Image();
            image.Source = new BitmapImage(
                new Uri(
                   "https://art.pixilart.com/b7875a3999e9a79.gif"));
            image.Opacity = 0.2;
            bb.ImageSource = image.Source;
            bb.Opacity = 0.2;
            MainGrid.Background = bb;
            MainGrid.Background.Opacity = 0.2;

            IsDefaultTheme = false;
            LeftRGB = "#4C464646";
            RightRGB = "#4C464646";
            BGImageURL = "https://art.pixilart.com/b7875a3999e9a79.gif";
            BGTransparency = "20";
        }

        // I'm just keeping this function here for now
        private void JSON(object sender, RoutedEventArgs e)
        {    
            try
            {
                // Deserialise first
                // Might be a bad way but works for such a small purpose
                var jsonfile = File.ReadAllText("Themes\\theme.json");
                var stuff = JObject.Parse(jsonfile);

                // Declare
                var conv = new ColorConverter();

                // Border setting first
                var left = (Color)conv.ConvertFrom(stuff.SelectToken("TopLeftBorderColour").Value<string>());
                var right = (Color)conv.ConvertFrom(stuff.SelectToken("BottomRightBorderColour").Value<string>());

                // Actual setting
                LinearGradientBrush brushy = new LinearGradientBrush();
                brushy.StartPoint = new Point(0, 0);
                brushy.EndPoint = new Point(1, 1);
                brushy.GradientStops.Add(new GradientStop(left, 0.0));
                brushy.GradientStops.Add(new GradientStop(right, 1.0));
                WindowBorder.BorderBrush = brushy;

                // Image transparency
                float transparencyval = float.Parse(stuff.SelectToken("BackgroundImageTransparency").Value<string>());
                transparencyval = transparencyval / 100;

                // Image stuff
                ImageBrush bb = new ImageBrush();
                Image image = new Image();

                image.Source = new BitmapImage(new Uri(stuff.SelectToken("BackgroundImageURL").Value<string>()));
                image.Opacity = transparencyval; // Just in case

                bb.ImageSource = image.Source;
                bb.Opacity = transparencyval; // Just in case

                MainGrid.Background = bb;
                MainGrid.Background.Opacity = transparencyval; // Just in case

                //MessageBox.Show("Theme set!", "MainDab");
            }
            catch
            {
                MessageBox.Show("Did you put in the right hex code format and image url?", "MainDab");
            }
        }

        // When listbox loaded
        private void ThemeListBox_Loaded(object sender, RoutedEventArgs e)
        {
            string[] fileEntries = Directory.GetFiles("Themes"); // Get stuff from folder
            foreach (string fileName in fileEntries)
                ThemeListBox.Items.Add(fileName.Remove(0, 7));
        }

        private void ListBoxSelect(object sender, SelectionChangedEventArgs e) // When something is selected on the list
        {
            object item = ThemeListBox.SelectedItem;

            if (item == null)
            {

            }
            else
            {
                string filepath = item.ToString();
                filepath = "Themes\\" + filepath;
                try
                {

                    // Deserialise first
                    // Might be a bad way but works for such a small purpose
                    // MessageBox.Show(filepath);

                    var jsonfile = File.ReadAllText(filepath);
                    var stuff = JObject.Parse(jsonfile);

                    // Declare
                    var conv = new ColorConverter();

                    // Name
                    CreatorName.Text = stuff.SelectToken("MadeBy").Value<string>();
                    

                    // Border setting first
                    var left = (Color)conv.ConvertFrom(stuff.SelectToken("TopLeftBorderColour").Value<string>());
                    var right = (Color)conv.ConvertFrom(stuff.SelectToken("BottomRightBorderColour").Value<string>());

                    LeftGradient.Text = stuff.SelectToken("TopLeftBorderColour").Value<string>();
                    RightGradient.Text = stuff.SelectToken("BottomRightBorderColour").Value<string>();

                    // Actual setting
                    LinearGradientBrush brushy = new LinearGradientBrush();
                    brushy.StartPoint = new Point(0, 0);
                    brushy.EndPoint = new Point(1, 1);
                    brushy.GradientStops.Add(new GradientStop(left, 0.0));
                    brushy.GradientStops.Add(new GradientStop(right, 1.0));
                    WindowBorder.BorderBrush = brushy;

                    // Image transparency
                    float transparencyval = float.Parse(stuff.SelectToken("BackgroundImageTransparency").Value<string>());
                    ImageTransparency.Text = transparencyval.ToString();

                    transparencyval = transparencyval / 100;

                    // Image stuff
                    ImageBrush bb = new ImageBrush();
                    Image image = new Image();

                    image.Source = new BitmapImage(new Uri(stuff.SelectToken("BackgroundImageURL").Value<string>()));
                    image.Opacity = transparencyval; // Just in case
                    ImageLink.Text = stuff.SelectToken("BackgroundImageURL").Value<string>();

                    bb.ImageSource = image.Source;
                    bb.Opacity = transparencyval; // Just in case

                    MainGrid.Background = bb;
                    MainGrid.Background.Opacity = transparencyval; // Just in case

                    // Default theme
                    IsDefaultTheme = false;
                    LeftRGB = stuff.SelectToken("TopLeftBorderColour").Value<string>();
                    RightRGB = stuff.SelectToken("BottomRightBorderColour").Value<string>();
                    BGImageURL = stuff.SelectToken("BackgroundImageURL").Value<string>();
                    BGTransparency = stuff.SelectToken("BackgroundImageTransparency").Value<string>();

                    //MessageBox.Show("Theme set!", "MainDab");
                }
                catch
                {
                   // MessageBox.Show("The theme file is invalid!", "MainDab");
                }
            }
        }

        private void ThemeSave(object sender, RoutedEventArgs e)
        {
            // Instead of properly sterialising the json file properly, we can manually write it ourselves
            string FinalJson = ("{\n" +
                "  \"MadeBy\" : \"" + CreatorName.Text + "\",\n" +
                "  \"TopLeftBorderColour\" : \"" + LeftGradient.Text + "\",\n" +
                "  \"BottomRightBorderColour\" : \"" + RightGradient.Text + "\",\n" +
                "  \"BackgroundImageURL\" : \"" + ImageLink.Text + "\",\n" +
                "  \"BackgroundImageTransparency\" : \"" + ImageTransparency.Text + "\"\n" +
                "}"); // I didn't want to go though so much trouble figuring out steralisation, bear with this

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON file (*.json)|*.json";
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, FinalJson); // Save file

        }

        // Download themes button
        private void ThemeDownload(object sender, RoutedEventArgs e)
        {
            ThemeListBox.Items.Clear();
            DownloadMessage.Visibility = Visibility.Visible;
            if (!Directory.Exists("Themes"))
            {
                Directory.CreateDirectory("Themes"); // just checking, just checking.
            }
            string[] fileEntriesasdasd = Directory.GetFiles("Themes");
            foreach (string fileNameb in fileEntriesasdasd)
                ThemeListBox.Items.Add(fileNameb.Remove(0, 7));

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                this.Dispatcher.Invoke(() =>
                {
                    DownloadMessageTextBox.Text = "Downloading themes from theme repository...";
                    
                    Storyboard sb = TryFindResource("FadeInListBox") as Storyboard;
                    sb.Begin();

                });
                var json = WebStuff.DownloadString("https://raw.githubusercontent.com/MainDabRblx/ProjectDab/master/UpdateStuff/ThemeList.json");
                dynamic dsfadfasdf = JsonConvert.DeserializeObject(json);
                foreach (var item in dsfadfasdf)
                {
                    string FileName = item.filename;
                    string URL = item.themeurl;
                    if (File.Exists("Themes\\" + FileName))
                    {
                        File.Delete("Themes\\" + FileName); // Update themes
                    }
                    WebStuff.DownloadFile(URL, "Themes\\" + FileName);
                    this.Dispatcher.Invoke(() =>
                    {
                        if (ThemeListBox.Items.Contains(FileName))
                        {
                            // Do nothing
                        }
                        else
                        {
                            ThemeListBox.Items.Add(FileName);
                        }
                    });
                }
                this.Dispatcher.Invoke(() =>
                {
                    DownloadMessageTextBox.Text = "Download completed!";
                    Storyboard sb = TryFindResource("FadeOutListBox") as Storyboard;
                    sb.Completed += new EventHandler(Done);
                    sb.Begin();
                });
            }).Start();
        }

        private void Done(object sender, EventArgs e)
        {
            DownloadMessage.Visibility = Visibility.Hidden; // For the animation
        }

        // OTHER FUNCTIONS //

        // Originally we thought WeAreDevs would sponsor MainDab. Turns out not. I'm still salty about that.
        private void TextBlock_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://wearedevs.net/"); // As asked, the link is correct
        }

        private void LoadingScreenLoaded(object sender, RoutedEventArgs e)
        {
           
        }

       
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChangedAsync(object sender, TextChangedEventArgs e)
        {
            // This is likely a very inefficient method of searching, if there are better ways, I would like to know
            // The CPU usage rises while searching
            if (IsScriptHubOpened == true)
            {
                new Thread(() =>
                {
                    this.Dispatcher.Invoke(() => // Prevent error from this being done on "another thread"
                    {
                        WP.Children.Clear();

                        foreach (var scriptData in scripts)
                        {
                            var obj = new TabThingy
                            {
                                Script = (ScriptHub.ScriptData)scriptData
                            };

                            if (GeneralScriptSearch.Text == "" || GeneralScriptSearch.Text == "Search for a script here" || GeneralScriptSearch.IsFocused == false || GeneralScriptSearch.IsKeyboardFocused == false)
                            {
                                // Functions for buttons
                                obj.Executed += (_, _) => Execution.ExecutionHandler.Execute(obj.Script.Script);
                                obj.CopyScript += (_, _) => Clipboard.SetText(obj.Script.Script);
                                WP.Children.Add(obj); // Add objects into scripthub panel
                            }

                            else if (obj.ScriptTitle.Content.ToString().ToLower().Contains(GeneralScriptSearch.Text.ToLower()) == true || obj.Description.Text.ToString().ToLower().Contains(GeneralScriptSearch.Text.ToLower()) == true || obj.Credit.Content.ToString().ToLower().Contains(GeneralScriptSearch.Text.ToLower()) == true)
                            {
                                // Functions for buttons
                                obj.Executed += (_, _) => Execution.ExecutionHandler.Execute(obj.Script.Script);
                                obj.CopyScript += (_, _) => Clipboard.SetText(obj.Script.Script);
                                WP.Children.Add(obj); // Add objects into scripthub panel
                            }

                        }
                        GC.Collect();

                    });

                })

                { }.Start();
            }
        }

        private void GeneralScriptSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (GeneralScriptSearch.Text == "Search for a script here")
            {
                GeneralScriptSearch.Text = "";
                GeneralScriptSearch.CaretBrush = new SolidColorBrush(Color.FromArgb(100, 137, 137, 137));
            }
        }
        private void GeneralScriptSearch_GotFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (GeneralScriptSearch.Text == "Search for a script here")
            {
                GeneralScriptSearch.Text = "";
                GeneralScriptSearch.CaretBrush = new SolidColorBrush(Color.FromArgb(100, 137, 137, 137));
            }
        }

        private void GeneralScriptSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (GeneralScriptSearch.Text == "")
            {
                GeneralScriptSearch.Text = "Search for a script here";
                GeneralScriptSearch.CaretBrush = new SolidColorBrush(Color.FromArgb(0, 137, 137, 137));
            }
        }

        private void GeneralScriptSearch_LostFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (GeneralScriptSearch.Text == "")
            {
                GeneralScriptSearch.Text = "Search for a script here";
                GeneralScriptSearch.CaretBrush = new SolidColorBrush(Color.FromArgb(0, 137, 137, 137));
            }
        }

        private void WP1_LoadedAsync(object sender, RoutedEventArgs e)
        {
           
        
        }

        private void GameScriptTextChanged(object sender, TextChangedEventArgs e)
        {
            
            
        }

        private void GameScriptSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (GameScriptSearch.Text == "Search for a script here")
            {
                GameScriptSearch.Text = "";
                GameScriptSearch.CaretBrush = new SolidColorBrush(Color.FromArgb(100, 137, 137, 137));
            }
        }

        private void GameScriptSearch_GotFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (GameScriptSearch.Text == "Search for a script here")
            {
                GameScriptSearch.Text = "";
                GameScriptSearch.CaretBrush = new SolidColorBrush(Color.FromArgb(100, 137, 137, 137));
            }
        }

        private void GameScriptSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (GameScriptSearch.Text == "")
            {
                GameScriptSearch.Text = "Search for a script here";
                GameScriptSearch.CaretBrush = new SolidColorBrush(Color.FromArgb(0, 137, 137, 137));
            }
        }

        private void GameScriptSearch_LostFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (GameScriptSearch.Text == "")
            {
                GameScriptSearch.Text = "Search for a script here";
                GameScriptSearch.CaretBrush = new SolidColorBrush(Color.FromArgb(0, 137, 137, 137));
            }
        }

        private void GameHubOpen(object sender, MouseButtonEventArgs e)
        {
            Storyboard sb = TryFindResource("GameHubOpen") as Storyboard;
            sb.Begin();

            HomeGrid.Visibility = Visibility.Hidden;
            ExecutorGrid.Visibility = Visibility.Hidden;
            ScriptHubGrid.Visibility = Visibility.Hidden;
            GameHubGrid.Visibility = Visibility.Visible;
            ToolsGrid.Visibility = Visibility.Hidden;
            CustomisationGrid.Visibility = Visibility.Hidden;
            SettingsGrid.Visibility = Visibility.Hidden;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void HomeRadioButtonClick(object sender, RoutedEventArgs e)
        {
            // This is pretty damn stupid to do but oh well
            HomeGrid.Visibility = Visibility.Visible;
            ExecutorGrid.Visibility = Visibility.Hidden;
            ScriptHubGrid.Visibility = Visibility.Hidden;
            GameHubGrid.Visibility = Visibility.Hidden;
            ToolsGrid.Visibility = Visibility.Hidden;
            CustomisationGrid.Visibility = Visibility.Hidden;
            SettingsGrid.Visibility = Visibility.Hidden;
        }

        private void ExecutorRadioButtonClick(object sender, RoutedEventArgs e)
        {
            // This is pretty damn stupid to do but oh well
            HomeGrid.Visibility = Visibility.Hidden;
            ExecutorGrid.Visibility = Visibility.Visible;
            ScriptHubGrid.Visibility = Visibility.Hidden;
            GameHubGrid.Visibility = Visibility.Hidden;
            ToolsGrid.Visibility = Visibility.Hidden;
            CustomisationGrid.Visibility = Visibility.Hidden;
            SettingsGrid.Visibility = Visibility.Hidden;
        }

        private void ScriptHubRadioButtonClick(object sender, RoutedEventArgs e)
        {
            if (IsScriptHubOpened == false)
            {
                IsScriptHubOpened = true;
                WP.Children.Clear();
                foreach (var scriptData in scripts)
                {
                    var obj = new TabThingy
                    {
                        Script = (ScriptHub.ScriptData)scriptData
                    };
                    // Functions for buttons
                    obj.Executed += (_, _) => Execution.ExecutionHandler.Execute(obj.Script.Script);
                    obj.CopyScript += (_, _) => Clipboard.SetText(obj.Script.Script);
                    WP.Children.Add(obj); // Add objects into scripthub panel
                }
            }

          
            // This is pretty damn stupid to do but oh well
            HomeGrid.Visibility = Visibility.Hidden;
            ExecutorGrid.Visibility = Visibility.Hidden;
            ScriptHubGrid.Visibility = Visibility.Visible;
            GameHubGrid.Visibility = Visibility.Hidden;
            ToolsGrid.Visibility = Visibility.Hidden;
            CustomisationGrid.Visibility = Visibility.Hidden;
            SettingsGrid.Visibility = Visibility.Hidden;
        }

        private void GameHubRadioButtonClick(object sender, RoutedEventArgs e)
        {
            if (IsGameHubOpened == false)
            {
                IsGameHubOpened = true;
                WP1.Children.Clear();
                foreach (var scriptData in gamescripts)
                {
                    var obj = new GameTab()
                    {
                        Script = (ScriptHub.GameScriptData)scriptData
                    };
                    // Functions for buttons
                    obj.Executed += (_, _) => Execution.ExecutionHandler.Execute(obj.Script.Script);
                    obj.CopyScript += (_, _) => Clipboard.SetText(obj.Script.Script);
                    WP1.Children.Add(obj); // Add objects into scripthub panel
                }
            }
            

            // This is pretty damn stupid to do but oh well
            HomeGrid.Visibility = Visibility.Hidden;
            ExecutorGrid.Visibility = Visibility.Hidden;
            ScriptHubGrid.Visibility = Visibility.Hidden;
            GameHubGrid.Visibility = Visibility.Visible;
            ToolsGrid.Visibility = Visibility.Hidden;
            CustomisationGrid.Visibility = Visibility.Hidden;
            SettingsGrid.Visibility = Visibility.Hidden;
        }

        private void ToolsRadioButtonClick(object sender, RoutedEventArgs e)
        {
            // This is pretty damn stupid to do but oh well
            HomeGrid.Visibility = Visibility.Hidden;
            ExecutorGrid.Visibility = Visibility.Hidden;
            ScriptHubGrid.Visibility = Visibility.Hidden;
            GameHubGrid.Visibility = Visibility.Hidden;
            ToolsGrid.Visibility = Visibility.Visible;
            CustomisationGrid.Visibility = Visibility.Hidden;
            SettingsGrid.Visibility = Visibility.Hidden;
        }

        private void SettingsRadioButtonClick(object sender, RoutedEventArgs e)
        {
            // This is pretty damn stupid to do but oh well
            HomeGrid.Visibility = Visibility.Hidden;
            ExecutorGrid.Visibility = Visibility.Hidden;
            ScriptHubGrid.Visibility = Visibility.Hidden;
            GameHubGrid.Visibility = Visibility.Hidden;
            ToolsGrid.Visibility = Visibility.Hidden;
            CustomisationGrid.Visibility = Visibility.Hidden;
            SettingsGrid.Visibility = Visibility.Visible;
        }

        private void HomeRadioButtonLoaded(object sender, RoutedEventArgs e)
        {
            HomeRadioButton.IsChecked = true;
        }

        private void SearchGameHub(object sender, RoutedEventArgs e)
        {
            // This is likely a very inefficient method of searching, if there are better ways, I would like to know
            // The CPU usage rises while searching
            
                new Thread(() =>
                {
                    this.Dispatcher.Invoke(() => // Prevent error from this being done on "another thread"
                    {
                        WP1.Children.Clear();

                        foreach (var scriptData in gamescripts)
                        {
                            var obj = new GameTab
                            {
                                Script = (ScriptHub.GameScriptData)scriptData
                            };

                            if (GameScriptSearch.Text == "" | GameScriptSearch.Text == "Search for a script here" | string.IsNullOrEmpty(GameScriptSearch.Text))
                            {
                                // Functions for buttons
                                obj.Executed += (_, _) => Execution.ExecutionHandler.Execute(obj.Script.Script);
                                obj.CopyScript += (_, _) => Clipboard.SetText(obj.Script.Script);
                                WP1.Children.Add(obj); // Add objects into scripthub panel
                            }

                            else if (obj.ScriptTitle.Content.ToString().ToLower().Contains(GameScriptSearch.Text.ToLower()) == true | obj.Description.Text.ToString().ToLower().Contains(GameScriptSearch.Text.ToLower()) == true | obj.Credit.Content.ToString().ToLower().Contains(GameScriptSearch.Text.ToLower()) == true)
                            {
                                // Functions for buttons
                                obj.Executed += (_, _) => Execution.ExecutionHandler.Execute(obj.Script.Script);
                                obj.CopyScript += (_, _) => Clipboard.SetText(obj.Script.Script);
                                WP1.Children.Add(obj); // Add objects into scripthub panel
                            }
                        }
                    });
                })
                { }.Start();
        }

        private void KrnlAPI(object sender, RoutedEventArgs e)
        {
            Execution.SelectedAPI.API = "Selected API: Krnl API";
            CurrentAPILabel.Content = "Using Krnl API";
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\MainDabData");
            key.SetValue("DLL", "Selected API: Krnl API");
            key.Close();
            MessageBox.Show("API set to Krnl", "MainDab");
        }

        private void MainWin_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.F5))
             {
                if(BlurTextbox.Visibility == Visibility.Visible)
                {
                    BlurTextbox.Visibility = Visibility.Hidden;
                }
                else
                {
                    BlurTextbox.Visibility = Visibility.Visible;
                }
            }
        }

        private void SearchScriptHub(object sender, RoutedEventArgs e)
        {
            // This is likely a very inefficient method of searching, if there are better ways, I would like to know
            // The CPU usage rises while searching
            if (IsScriptHubOpened == true)
            {
                new Thread(() =>
                {
                    this.Dispatcher.Invoke(() => // Prevent error from this being done on "another thread"
                    {
                        WP.Children.Clear();

                        foreach (var scriptData in scripts)
                        {
                            var obj = new TabThingy
                            {
                                Script = (ScriptHub.ScriptData)scriptData
                            };

                            if (GeneralScriptSearch.Text == "" | GeneralScriptSearch.Text == "Search for a script here" | string.IsNullOrEmpty(GeneralScriptSearch.Text))
                            {
                                // Functions for buttons
                                obj.Executed += (_, _) => Execution.ExecutionHandler.Execute(obj.Script.Script);
                                obj.CopyScript += (_, _) => Clipboard.SetText(obj.Script.Script);
                                WP.Children.Add(obj); // Add objects into scripthub panel
                            }

                            else if (obj.ScriptTitle.Content.ToString().ToLower().Contains(GeneralScriptSearch.Text.ToLower()) == true || obj.Description.Text.ToString().ToLower().Contains(GeneralScriptSearch.Text.ToLower()) == true || obj.Credit.Content.ToString().ToLower().Contains(GeneralScriptSearch.Text.ToLower()) == true)
                            {
                                // Functions for buttons
                                obj.Executed += (_, _) => Execution.ExecutionHandler.Execute(obj.Script.Script);
                                obj.CopyScript += (_, _) => Clipboard.SetText(obj.Script.Script);
                                WP.Children.Add(obj); // Add objects into scripthub panel
                            }

                        }
                        GC.Collect();

                    });

                })

                { }.Start();
            }
         }

        private void CustomisationRadioButtonClick(object sender, RoutedEventArgs e)
        {
            HomeGrid.Visibility = Visibility.Hidden;
            ExecutorGrid.Visibility = Visibility.Hidden;
            ScriptHubGrid.Visibility = Visibility.Hidden;
            GameHubGrid.Visibility = Visibility.Hidden;
            ToolsGrid.Visibility = Visibility.Hidden;
            CustomisationGrid.Visibility = Visibility.Visible;
            SettingsGrid.Visibility = Visibility.Hidden;
        }
    }
}