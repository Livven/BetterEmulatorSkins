using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using MahApps.Metro.Controls;

namespace BetterEmulatorSkins
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        ObservableCollection<EmulatorSkin> skins = new ObservableCollection<EmulatorSkin>();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = EmulatorSkin.GetSkins();
        }

        void linkButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start((string)((Button)sender).Tag);
        }
        void skinsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var skin = (EmulatorSkin)((ListBox)sender).SelectedItem;
            skin.Change();
        }
        void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.ToString());
        }
    }
}
