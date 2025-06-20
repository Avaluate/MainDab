using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WpfAnimatedGif;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using System.Diagnostics;
using System.ComponentModel;

namespace MainDab_Bootstrapper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {

        // WebClient Creation
        WebClient WebStuff = new WebClient(); // Create a new generally used WebClient
        bool IsFirstTime = false;

        public MainWindow()
        {
            InitializeComponent();
            Startup.Opacity = 0;
            
        }


        // Newer animation functions
        // StackOverflow saved my ass for this one

        public void Fade(DependencyObject ElementName, double Start, double End, double Time)
        {
            DoubleAnimation Anims = new DoubleAnimation()
            {
                From = Start,
                To = End,
                Duration = TimeSpan.FromSeconds(Time),
                EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut }
            };
            Storyboard.SetTarget(Anims, ElementName);
            Storyboard.SetTargetProperty(Anims, new PropertyPath(OpacityProperty)); // well i don't actually think transparency has this effect
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(Anims);
            storyboard.Begin();
        }

        public void Move(DependencyObject ElementName, Thickness Origin, Thickness Location, double Time)
        {
            ThicknessAnimation Anims = new ThicknessAnimation()
            {
                From = Origin,
                To = Location,
                Duration = TimeSpan.FromSeconds(Time),
                EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut }
            };
            Storyboard.SetTarget(Anims, ElementName);
            Storyboard.SetTargetProperty(Anims, new PropertyPath(MarginProperty));
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(Anims);
            storyboard.Begin();
        }
        public void Scaling(DependencyObject ElementName, double Before, double After, double Time)
        {
            DoubleAnimation ScalingX = new DoubleAnimation()
            {
                From = Before,
                To = After,
                Duration = TimeSpan.FromSeconds(Time),
                EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut }
            };

            Storyboard.SetTarget(ScalingX, ElementName);
            Storyboard.SetTargetProperty(ScalingX, new PropertyPath("RenderTransform.Children[0].ScaleX"));
            Storyboard StoryboardX = new Storyboard();
            StoryboardX.Children.Add(ScalingX);

            DoubleAnimation ScalingY = new DoubleAnimation()
            {
                From = Before,
                To = After,
                Duration = TimeSpan.FromSeconds(Time),
                EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut }
            };

            Storyboard.SetTarget(ScalingY, ElementName);
            Storyboard.SetTargetProperty(ScalingY, new PropertyPath("RenderTransform.Children[0].ScaleY"));
            Storyboard StoryboardY = new Storyboard();
            StoryboardY.Children.Add(ScalingY);

            StoryboardX.Begin();
            StoryboardY.Begin();
        }

        private async Task BackgroundImageAnimationAsync()
        {
            while(true)
            {
                await Task.Delay(1500);
                Fade(DownloadBG, 0.05, 0.01, 1.5);
                await Task.Delay(1500);
                Fade(DownloadBG, 0.01, 0.05, 1.5);
            }
        }

        private async void LoadedAsync(object sender, RoutedEventArgs e)
        {
            Startup.Visibility = Visibility.Visible;
            Startup.Opacity = 0;
            MainDabTitle.Opacity = 0;

            Fade(Startup, 0, 1, 0.5);
            await Task.Delay(1000);
 
            Move(MainDabIcon, MainDabIcon.Margin, new Thickness(127.2, 0, -0.8, 0), 0.5);
            Move(MainDabTitle, MainDabTitle.Margin, new Thickness(0, 189, 0, 0), 0.6);
            Scaling(MainDabIcon, 1, 0.79, 0.5);
            Fade(MainDabTitle, 0, 1, 0.5);

            await Task.Delay(1500);

            Move(MainDabTitle, MainDabTitle.Margin, new Thickness(0, 203, 0, 0), 0.6);
            Move(MainDabIcon, MainDabIcon.Margin, new Thickness(127.2, 0, 53.2, 0), 0.5);
            Fade(MainDabIcon, 1, 0, 0.5);
            Fade(MainDabTitle, 1, 0, 0.5);
            Scaling(MainDabIcon, 0.79, 0.89, 0.5);

            await Task.Delay(1000);

            Startup.Visibility = Visibility.Hidden;
            DownloadMainDab.Visibility = Visibility.Visible;

            string Version = WebStuff.DownloadString("https://raw.githubusercontent.com/Avaluate/MainDabWeb/master/UpdateStuff/Version");
            WebStuff.Dispose(); // Remember to dispose the WebClient! Or someone will scold me for it

            // .FirstOrDefault() is nessesary since GitHub always adds an extra line for some reason
            // If I don't do this, then the string that would return is "MainDab 14.3/n" rather than "MainDab 14.3", so basically an additional unwanted line!
            string OnlineVersion = Version.Split(new[] { '\r', '\n' }).FirstOrDefault();
            LatestUpdate.Content = "Latest MainDab Version: " + OnlineVersion;

            if (File.Exists("MainDab.exe"))
            {
                InstallUpdateText.Content = "MainDab Update Found";
                InstallButton.Content = "Update MainDab";
            }
            else
            {
                IsFirstTime = true;
            }

            Move(InstallUpdateText, InstallUpdateText.Margin, new Thickness(0, 111, -2.2, 0), 0.5);
            Move(InstallButton, InstallButton.Margin, new Thickness(216, 154, 0, 0), 0.6);
            Move(LatestUpdate, LatestUpdate.Margin, new Thickness(-2, 190, -0.2, 0), 0.7);
            Fade(InstallUpdateText, 0, 1, 0.5);
            Fade(InstallButton, 0, 1, 0.6);
            Fade(LatestUpdate, 0, 1, 0.7);
            
            await Task.Delay(1000);

            JoinDiscord.Visibility = Visibility.Visible;
            JoinOurDiscord.Visibility = Visibility.Visible;
            Fade(JoinDiscord, 0, 0.5, 1);
            Fade(JoinOurDiscord, 0, 1, 1);

            Fade(DownloadBG, 0, 0.05, 1.5);

            this.Dispatcher.Invoke(() =>
            {
                _ = BackgroundImageAnimationAsync();
            });
            
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove(); // Make window draggable
            }
            catch
            {
                // Apparently some other random mouse clicks can make this glitch
            }
           
        }

       
        // Download
    
        private async void InstallButton_MouseDown(object sender, RoutedEventArgs e)
        {
            InstallButton.IsEnabled = false;
            Move(InstallUpdateText, new Thickness(0, 111, -2.2, 0), new Thickness(0, 139, -2.2, 0), 0.5);
            Move(InstallButton, new Thickness(216, 154, 0, 0), new Thickness(216, 182, 0, 0), 0.6);
            Move(LatestUpdate, new Thickness(-2, 190, -0.2, 0), new Thickness(-2, 218, -0.2, 0), 0.7);

            Fade(InstallUpdateText, 1, 0, 0.5);
            Fade(InstallButton,1, 0, 0.6);
            Fade(LatestUpdate, 1, 0, 0.7);

            await Task.Delay(700);

            DownloadMainDab.Visibility = Visibility.Hidden;
            InstallMainDab.Visibility = Visibility.Visible;

            Fade(InstallMainDab, 0, 1, 0.5);
            Fade(RequirementCheck, 0, 1, 0.5);


            Fade(CheckingRequirementsLabel, 0, 1, 0.5);
            Fade(Gif1, 0, 0.8, 0.5);
            Move(CheckingRequirementsLabel, new Thickness(35, 101, 0, 0), new Thickness(35, 111, 0, 0), 0.5);
            Move(Gif1, new Thickness(246, 109, 0, 0), new Thickness(246, 119, 0, 0), 0.5);

            Fade(DeletingFilesLabel, 0, 1, 0.6);
            Fade(Gif2, 0, 0.8, 0.6);
            Move(DeletingFilesLabel, new Thickness(35, 130, 0, 0), new Thickness(35, 140, 0, 0), 0.6);
            Move(Gif2, new Thickness(246, 138, 0, 0), new Thickness(246, 148, 0, 0), 0.6);

            Fade(CreatingFoldersLabel, 0, 1, 0.7);
            Fade(Gif3, 0, 0.8, 0.7);
            Move(CreatingFoldersLabel, new Thickness(35, 159, 0, 0), new Thickness(35, 169, 0, 0), 0.7);
            Move(Gif3, new Thickness(246, 167, 0, 0), new Thickness(246, 177, 0, 0), 0.7);

            Fade(DownloadingMainDabLabel, 0, 1, 0.8);
            Fade(Gif4, 0, 0.8, 0.8);
            Move(DownloadingMainDabLabel, new Thickness(35 ,188, 0, 0), new Thickness(35, 198, 0, 0), 0.8);
            Move(Gif4, new Thickness(246, 196, 0, 0), new Thickness(246, 206, 0, 0), 0.8);

            HttpClient fd = new HttpClient();
            var GetWebsite = await fd.GetAsync("https://github.com");
            if (!GetWebsite.IsSuccessStatusCode)
            {
                // GitHub not accessable!
                MessageBox.Show("MainDab's Bootstrapper cannot properly reach GitHub, which it needed in order to download MainDab. Please check your firewall or router settings.\n\nYou can join MainDab's Discord at discord.io/maindab if you need more help.", "Error connecting to GitHub");
                Environment.Exit(0);
            }

            await Task.Delay(2500);

            Fade(RequirementCheck, 1, 0, 0.5);
            Fade(Gif1, 0.8, 0, 0.5);
            await Task.Delay(600);

            Gif1.Visibility = Visibility.Hidden;
            Gif1Completed.Visibility = Visibility.Visible;

            RequirementCheck.Visibility = Visibility.Hidden;
            DeletingFiles.Visibility = Visibility.Visible;
            Fade(Gif1Completed, 0, 0.8, 0.5);
            Fade(DeletingFiles, 0, 1, 0.5);
            await Task.Delay(500);

            // Delete MainDab file
            try
            {
                foreach (Process proc in Process.GetProcessesByName("MainDab")) // Get rid of MainDab
                {
                    proc.Kill();
                }
            }
            catch { }

            await Task.Delay(2000);

            if (File.Exists("MainDab.exe"))
            {
                File.Delete("MainDab.exe");
            }

            Fade(DeletingFiles, 1, 0, 0.5);
            Fade(Gif2, 0.8, 0, 0.5);
            await Task.Delay(600);

            Gif2.Visibility = Visibility.Hidden;
            Gif2Completed.Visibility = Visibility.Visible;

            DeletingFiles.Visibility = Visibility.Hidden;
            CreatingFolders.Visibility = Visibility.Visible;

            Fade(Gif2Completed, 0, 0.8, 0.5);
            Fade(CreatingFolders, 0, 1, 0.5);
            await Task.Delay(500);

            // Get rid of MainDab folder
            try
            {
                DirectoryInfo ruh = new DirectoryInfo("MainDab");
                foreach (FileInfo file in ruh.GetFiles())
                {
                    file.Delete();
                }
                if (Directory.Exists("MainDab"))
                {
                    Directory.Delete("MainDab");
                }
            }
            catch { }

            if (IsFirstTime == true)
            {
                Directory.CreateDirectory("MainDab");
            }

            await Task.Delay(500);

            Fade(CreatingFolders, 1, 0, 0.5);
            Fade(Gif3, 0.8, 0, 0.5);
            await Task.Delay(600);

            Gif3.Visibility = Visibility.Hidden;
            Gif3Completed.Visibility = Visibility.Visible;

            CreatingFolders.Visibility = Visibility.Hidden;
            DownloadingMainDab.Visibility = Visibility.Visible;

            Fade(Gif3Completed, 0, 0.8, 0.5);
            Fade(DownloadingMainDab, 0, 1, 0.5);
            await Task.Delay(500);

            if (IsFirstTime == true)
            {
                WebStuff.DownloadProgressChanged += new DownloadProgressChangedEventHandler(WebStuff_DownloadProgressChanged);
                WebStuff.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(WebStuff_DownloadCompleted);
                WebStuff.DownloadFileAsync(new Uri("https://github.com/Avaluate/MainDabWeb/raw/main/MainDab.exe"), "MainDab\\MainDab.exe");
            }
            else
            {
                WebStuff.DownloadProgressChanged += new DownloadProgressChangedEventHandler(WebStuff_DownloadProgressChanged);
                WebStuff.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(WebStuff_DownloadCompleted);
                WebStuff.DownloadFileAsync(new Uri("https://github.com/Avaluate/MainDabWeb/raw/main/MainDab.exe"), "MainDab.exe");
            }

            
        }

        void WebStuff_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                double bytesIn = double.Parse(e.BytesReceived.ToString());
                double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
                double percentage = bytesIn / totalBytes * 100;
                DownloadBar.Value = int.Parse(Math.Truncate(percentage).ToString());
            });
        }

        void WebStuff_DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.Dispatcher.Invoke(async () =>
            {
                await Task.Delay(500);
                Fade(DownloadingMainDab, 1, 0, 0.5);
                Fade(Gif4, 0.8, 0, 0.5);
                await Task.Delay(600);
                DownloadingMainDab.Visibility = Visibility.Hidden;
                Gif4.Visibility = Visibility.Hidden;
                Gif4Completed.Visibility = Visibility.Visible;
                ContinueToMainDab.Visibility = Visibility.Visible;
                Fade(Gif4Completed, 0, 0.8, 0.5);
                Fade(ContinueToMainDab, 0, 1, 0.5);
                
                if (IsFirstTime == true)
                {
                    Directory.SetCurrentDirectory("MainDab");
                    Process.Start("MainDab.exe");
                }
                else
                {
                    Process.Start("MainDab.exe");
                }

                await Task.Delay(2000);
                Fade(MainGrid, 1, 0, 0.5);
                await Task.Delay(501);
                Environment.Exit(0);
            });
        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
