using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
namespace Minesweeper
{
    public partial class Setting : Window
    {
        private int Theme;
        public Setting(int a)
        {
            Theme = a;
            InitializeComponent();
            Background = (a == 1) ? new SolidColorBrush(Color.FromRgb(187, 187, 187)) : new SolidColorBrush(Color.FromRgb(18, 103, 72));
        }
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Gamescreen winc = new Gamescreen(Convert.ToInt32(slider.Value),Theme);
            winc.Show();
            this.Close();
        }
        private void label_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            label.Content = slider.Value;
        }
    }
}
