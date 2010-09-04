using System;
using System.Windows;
using Tie.Controllers;
using Tie.Model;

namespace Tie
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                Context.Instance.RefreshPlaylists();
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MainWindow mw = new MainWindow();
            MainWindowViewModel mwc = new MainWindowViewModel();

            mw.DataContext = mwc;

            mw.ShowDialog();

            Context.Instance.Dispose();
        }
    }
}
