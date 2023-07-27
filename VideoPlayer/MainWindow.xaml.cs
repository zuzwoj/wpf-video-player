using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

namespace Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isPlaying = false;
        public MainWindow() 
        {
            InitializeComponent(); 
        }
        private void MenuItem_Open(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if ((bool)openFileDialog.ShowDialog())
            {
                Player.Source = new Uri(openFileDialog.FileName);
            }
        }
        private void MenuItem_Exit(object sender, EventArgs e) { Close(); }
        private void MenuItem_About(object sender, EventArgs e) { MessageBox.Show("", "About", MessageBoxButton.OK); }
        void ClickVideo(object sender, RoutedEventArgs args)
        {
            if (isPlaying)
            {
                Player.Pause();
                isPlaying = false;
            }
            else
            {
                Player.Play();
                isPlaying = true;
            }
        }
        private void Element_MediaOpened(object sender, EventArgs e) { }
        private void Element_MediaEnded(object sender, EventArgs e) { Player.Stop(); }
        void MouseWheelFunc(object sender, MouseWheelEventArgs e) { }
    }
}