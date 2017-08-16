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
using MahApps.Metro;
namespace Gunit.View
{
    /// <summary>
    /// Interaction logic for ChangeTheme.xaml
    /// </summary>
    public partial class ChangeTheme : MetroWindow
    {
        public ChangeTheme()
        {
            InitializeComponent();
        }
        private void AccentSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var selectedAccent = AccentSelector.SelectedItem as Accent;
            if (selectedAccent != null)
            {
                var theme = ThemeManager.DetectAppStyle(System.Windows.Application.Current);
                ThemeManager.ChangeAppStyle(System.Windows.Application.Current, selectedAccent, theme.Item1);
                System.Windows.Application.Current.MainWindow.Activate();

            }

        }
    }
}
