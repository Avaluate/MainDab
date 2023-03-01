using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainDabRedo
{
    /// <summary>
    /// Interaction logic for GameTab.xaml
    /// </summary>
    public partial class GameTab : UserControl
    {
        /*
        SCRIPT HUB FRAMES
        Here are the codes relating ot the script hub. I don't know why I named it tab thingy
         */
        // Right, so we wanna get all these data needed first
        public static readonly DependencyProperty ScriptProperty = DependencyProperty.Register("Script", typeof(ScriptHub.GameScriptData), typeof(GameTab), new PropertyMetadata(default(ScriptHub.GameScriptData)));
        
        


        // Self explanatory, get the data
        public ScriptHub.GameScriptData Script
        {
            get => (ScriptHub.GameScriptData)GetValue(ScriptProperty);
            set
            {
                // Also self explanatory
                SetValue(ScriptProperty, value);

                // Set the content
                ScriptTitle.Content = value.Title;
                Credit.Content = value.Credits;
                Description.Text = value.Desc;

                // Attempt #1 at reducing image quality to improve memory usage
                new Thread(() =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        // Background img
                        var imageBrush = (ImageBrush)BorderImg.Background;
                        var filename = value.ImageURL;
                            
                        BitmapImage img = new BitmapImage();
                        img.DownloadCompleted += (object sender, EventArgs e) =>
                        {
                            imageBrush.Freeze();
                            img.Freeze();
                        };

                        img.BeginInit();
                        img.DecodePixelWidth = 200;
                        img.DecodePixelHeight = 100;
                        img.UriSource = new Uri(filename, UriKind.RelativeOrAbsolute);
                        imageBrush.ImageSource = img;
                        img.CacheOption = BitmapCacheOption.None;
                        img.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                        img.EndInit();
                    });
                })
                { }.Start(); 
            }
        }

      

        public GameTab()
        {
            InitializeComponent();
        }

        // So now we want to handle the functions for when the execute and copy script button is clicked
        public event RoutedEventHandler Executed;
        public event RoutedEventHandler CopyScript;

        // This is done seperately, somewhere else (just some lamda expressions later)
        private void Execute(object sender, RoutedEventArgs e) => Executed?.Invoke(this, e);

        // Copy to clipboard
        private void Copy(object sender, RoutedEventArgs e)
        {

            CopyScript?.Invoke(this, e); // We need to also do this, since we actually need to copy it to the clipboard
            new Thread(() =>
            {
                // We need to create a new thread to avoid freezing and stuff
                Thread.CurrentThread.IsBackground = true;
                this.Dispatcher.Invoke(() => // Or it shouts an error at us for using same thread or something
                {
                    CopiedMsg.Visibility = Visibility.Visible;
                    // Just some animations
                    Storyboard sb = TryFindResource("FadeIn") as Storyboard;
                    sb.Begin();
                });

                Thread.Sleep(2000); // Wait

                // More animations again
                this.Dispatcher.Invoke(() =>
                {
                    Storyboard sb = TryFindResource("FadeOut") as Storyboard;
                    sb.Completed += new EventHandler(Done); // Prevent things being too buggy
                    sb.Begin();
                });
            }).Start();
        }

        // Self explanatory
        private void Done(object sender, EventArgs e)
        {
            CopiedMsg.Visibility = Visibility.Hidden;
        }
    }
}
