using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Reflection;
using System.Windows.Media.Animation;
using System.Windows.Threading;
namespace Gunit.View
{
  
    /// <summary>
    /// Interaction logic for GUnitSplash.xaml
    /// </summary>
    //
    public partial class GUnitSplash : MetroWindow
    {
        private MainWindow _mainWindow;
        
        private const string Loading = " ";
        public GUnitSplash()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            lblVersion.Content = Assembly.GetExecutingAssembly().GetName().Version.ToString();
           

            _mainWindow = new MainWindow();
            _mainWindow.ReadyToShow += new MainWindow.ReadyToShowDelegate(_mainWindow_ReadyToShow);

            _mainWindow.Closed += new EventHandler(_mainWindow_Closed);
             
        }
        
        void _mainWindow_ReadyToShow(object sender, EventArgs args)
        {
            // When the main window is done with its time-consuming tasks, 
            // hide the splash screen and show the main window.

            #region Animate the splash screen fading

            Storyboard sb = new Storyboard();
            //
            DoubleAnimation da = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(1))
            };
            //
            Storyboard.SetTarget(da, this);
            Storyboard.SetTargetProperty(da, new PropertyPath(OpacityProperty));
            sb.Children.Add(da);
            //
            sb.Completed += new EventHandler(sb_Completed);
            //
            sb.Begin();

            #endregion // Animate the splash screen fading
        }
        void sb_Completed(object sender, EventArgs e)
        {
            // When the splash screen fades out, we can show the main window:
            this.Visibility = System.Windows.Visibility.Hidden;
            _mainWindow.Visibility = System.Windows.Visibility.Visible;
        }
        void _mainWindow_Closed(object sender, EventArgs e)
        {
            // When the MainWindow is closed, the app does not exit: SplashScreen is its real "main" window.
            // This handler ensures that the MainWindow closing works as expected: exit from teh app.

            this.Close();
        }
    }
  
}
