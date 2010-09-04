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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace Tie
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Left = Screen.PrimaryScreen.WorkingArea.Width - Width;
            Top = Screen.PrimaryScreen.WorkingArea.Height - Height;
        }

        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.AutoReverse = true;
            da.Duration = TimeSpan.FromSeconds(5);
            da.RepeatBehavior = RepeatBehavior.Forever;
            da.From = 3.0f;
            da.To = 15.0f;
            da.By = 0.1f;

            this.HeaderDropShadowEffect.BeginAnimation(DropShadowEffect.BlurRadiusProperty, da);
        }
    }
}
