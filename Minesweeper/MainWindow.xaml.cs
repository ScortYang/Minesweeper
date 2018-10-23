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
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace Minesweeper
{
    public partial class MainWindow : Window
    {
        private int Theme;
        public MainWindow()
        {
        }
        public static void main(String[] args)
        {
            MainWindow winMain = new MainWindow();
            winMain.Show();

        }
        private void Button_Ez_Click(object sender, RoutedEventArgs e)
        {
            Gamescreen winE = new Gamescreen(1,Theme);
            winE.Show();
        }
        private void Button_Md_Click(object sender, RoutedEventArgs e)
        {
            Gamescreen winM = new Gamescreen(10,Theme);
            winM.Show();
        }
        private void Button_Hd_Click(object sender, RoutedEventArgs e)
        {
            Gamescreen winH = new Gamescreen(20,Theme);
            winH.Show();
        }
        private void Button_Cm_Click(object sender, RoutedEventArgs e)
        {
            Setting setting = new Setting(Theme);
            setting.Show();
        }
        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void MnuAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Minesweeper Version 1.2\n By Scort");
        }

        private void Theme_GD_Click(object sender, RoutedEventArgs e)
        {
            Button_GD.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/img/GD.png", UriKind.RelativeOrAbsolute)));
            Button_CL.Background = new SolidColorBrush(Color.FromRgb(34, 155, 255));
            Background = new SolidColorBrush(Color.FromRgb(18, 103, 72));
            mnu.Background = new SolidColorBrush(Color.FromRgb(27, 124, 89));
            Theme = 0;
        }

        private void Theme_CL_Click(object sender, RoutedEventArgs e)
        {
            Button_GD.Background = new SolidColorBrush(Color.FromRgb(255, 156, 58));
            Button_CL.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/img/CL.png", UriKind.RelativeOrAbsolute)));
            Background = new SolidColorBrush(Color.FromRgb(187, 187, 187));
            mnu.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            Theme = 1;
        }
    }
}
