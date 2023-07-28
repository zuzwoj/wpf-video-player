using Microsoft.Win32;
using System;
using System.Collections;
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
using System.Windows.Threading;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isPlaying = false;
        public delegate void timerTick();
        DispatcherTimer ticks = new DispatcherTimer();
        timerTick tick;
        private TimeSpan skipLength = new TimeSpan(0, 0, 5);
        public MainWindow() 
        {
            InitializeComponent();
            SliderVolume.Value = SliderVolume.Maximum;
            tick = new timerTick(changeStatus);
        }
        void changeStatus()
        {
            SliderTime.Value = Player.Position.TotalMilliseconds;
            Duration.Content = Player.Position;
        }
        private void MenuItem_Open(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if ((bool)openFileDialog.ShowDialog())
            {
                Player.Source = new Uri(openFileDialog.FileName);
            }
        }
        private void SeekToMediaPosition(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            if (!isPlaying)
            {
                int SliderValue = (int)SliderTime.Value;
                TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);
                Player.Position = ts;
            }
        }
        private void MenuItem_Exit(object sender, EventArgs e) { Close(); }
        private void MenuItem_About(object sender, EventArgs e) { MessageBox.Show("A simple video player", "About", MessageBoxButton.OK); }
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
        private void Element_MediaOpened(object sender, EventArgs e) 
        {
            SliderTime.Maximum = Player.NaturalDuration.TimeSpan.TotalMilliseconds;
            SliderVolume.Minimum = 0;
            SliderVolume.Maximum = 100;
            ticks.Interval = TimeSpan.FromMilliseconds(1);
            ticks.Tick += ticks_Tick;
            ticks.Start();
            if (skipLength > Player.NaturalDuration.TimeSpan) skipLength = Player.NaturalDuration.TimeSpan;
        }
        void ticks_Tick(object sender, object e) { Dispatcher.Invoke(tick); }
        void SliderVolume_Click(object sender, MouseButtonEventArgs args)
        {
            Point coord = args.GetPosition(SliderVolume);
            SliderVolume.Value = (coord.X / 70) * 100;
            Player.Volume = (double)SliderVolume.Value / 100;
        }
        private void Element_MediaEnded(object sender, EventArgs e) { Player.Stop(); }
        private void DurationSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            Player.Position = new TimeSpan(0, 0, 0, 0, (int)SliderTime.Value);
        }
        private void DurationSlider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            Player.Stop();
            isPlaying = false;
        }
        void MouseWheelFunc(object sender, MouseWheelEventArgs e) 
        {
            SliderVolume.Value += 0.1 * e.Delta;
            Player.Volume = (double)SliderVolume.Value / 100;
        }
        void OnMouseDownPlayMedia(object sender, RoutedEventArgs args)
        {
            Player.Play();
            isPlaying = true;
        }
        void OnMouseDownPauseMedia(object sender, RoutedEventArgs args)
        {
            Player.Pause();
            isPlaying = false;
        }
        void OnMouseDownStopMedia(object sender, RoutedEventArgs args)
        {
            Player.Stop();
            isPlaying = false;
        }
        void OnMouseDownPrevMedia(object sender, RoutedEventArgs args)
        {
            Player.Pause();
            Player.Position -= skipLength;
            SliderTime.Value -= skipLength.TotalMilliseconds;
            Player.Play();
        }
        void OnMouseDownNextMedia(object sender, RoutedEventArgs args)
        {
            Player.Pause();
            Player.Position += skipLength;
            SliderTime.Value += skipLength.TotalMilliseconds;
            Player.Play();
        }
        void OnMouseDownMuteMedia(object sender, RoutedEventArgs args)
        {

        }
    }
}