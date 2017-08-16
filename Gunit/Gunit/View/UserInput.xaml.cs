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
using Gunit.Model;
using MahApps.Metro.Controls;
namespace Gunit.View
{
    /// <summary>
    /// Interaction logic for UserInput.xaml
    /// </summary>
    public partial class UserInput : MetroWindow
    {
        
        public UserInput(string text = "")
        {
            InitializeComponent();
            this.Title = text;
        }
        public string Value { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Value = txtInput.Text;
            this.Close();
        }
    }
}
