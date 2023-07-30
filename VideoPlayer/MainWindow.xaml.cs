using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
using System.Xml;
using System.Windows.Markup;
using System.Reflection;
using System.Resources;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isPlaying = false;
        private bool isMuted = false;
        private string title = "About";
        private string desc = "A simple video player";
        private string path = Directory.GetCurrentDirectory();
        public delegate void timerTick();
        DispatcherTimer ticks = new DispatcherTimer();
        timerTick tick;
        private TimeSpan skipLength = new TimeSpan(0, 0, 5);
        public MainWindow() 
        {
            InitializeComponent();
            SliderVolume.Value = SliderVolume.Maximum;
            tick = new timerTick(changeStatus);
            path = path.Substring(0, path.Length - 25);
        }
        void changeStatus()
        {
            SliderTime.Value = Player.Position.TotalMilliseconds;
            Duration.Content = Player.Position;
        }
        private void MenuItem_Open(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Video files (*.webm;*.gif;*.gifv;*.avi;*.amv;*.mp4;*.m4p;*.m4v;*.mpg;*.mp2;*.mpeg;*.mpe;*.mpv;*.mpg;*.mpeg;*.m2v)|" +
                "*.webm;*.gif;*.gifv;*.avi;*.amv;*.mp4;*.m4p;*.m4v;*.mpg;*.mp2;*.mpeg;*.mpe;*.mpv;*.mpg;*.mpeg;*.m2v";
            bool? status = openFileDialog.ShowDialog();
            if (status is not null && status is true) Player.Source = new Uri(openFileDialog.FileName);
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
        private void LightThemeSelected(object sender, EventArgs e) 
        {
            var stream = new FileStream(path + @"/Themes/Light/Light.xaml", FileMode.Open);
            foreach (DictionaryEntry dictionaryEntry in (ResourceDictionary)XamlReader.Load(stream)) Application.Current.Resources[dictionaryEntry.Key] = dictionaryEntry.Value;
            MenuItem itemChecked = (MenuItem)sender;
            foreach (MenuItem item in ((MenuItem)itemChecked.Parent).Items)
            {
                if (item == itemChecked) continue;
                item.IsChecked = false;
            }
        }
        private void DarkThemeSelected(object sender, EventArgs e)
        {
            var stream = new FileStream(path + @"/Themes/Dark/Dark.xaml", FileMode.Open);
            foreach (DictionaryEntry dictionaryEntry in (ResourceDictionary)XamlReader.Load(stream)) Application.Current.Resources[dictionaryEntry.Key] = dictionaryEntry.Value;
            MenuItem itemChecked = (MenuItem)sender;
            foreach (MenuItem item in ((MenuItem)itemChecked.Parent).Items)
            {
                if (item == itemChecked) continue;
                item.IsChecked = false;
            }
        }
        private void OtterThemeSelected(object sender, EventArgs e)
        {
            var stream = new FileStream(path + @"/Themes/Otter/Otter.xaml", FileMode.Open);
            foreach (DictionaryEntry dictionaryEntry in (ResourceDictionary)XamlReader.Load(stream)) Application.Current.Resources[dictionaryEntry.Key] = dictionaryEntry.Value;
            MenuItem itemChecked = (MenuItem)sender;
            foreach (MenuItem item in ((MenuItem)itemChecked.Parent).Items)
            {
                if (item == itemChecked) continue;
                item.IsChecked = false;
            }
        }
        private void EnglishSelected(object sender, EventArgs e)
        {
            title = "About";
            desc = "A simple video editor";
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
            MainWindow newWindow = new MainWindow();
            Application.Current.MainWindow = newWindow;
            newWindow.Show();
            this.Close();
            ((MenuItem)newWindow.testname.Items[0]).IsChecked = true;
            ((MenuItem)newWindow.testname.Items[1]).IsChecked = false;
            ((MenuItem)newWindow.testname.Items[2]).IsChecked = false;
        }
        private void GermanSelected(object sender, EventArgs e)
        {
            title = "Über die App";
            desc = "Ein einfacher Videoplayer";
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de-DE");
            MainWindow newWindow = new MainWindow();
            Application.Current.MainWindow = newWindow;
            newWindow.Show();
            this.Close();
            ((MenuItem)newWindow.testname.Items[0]).IsChecked = false;
            ((MenuItem)newWindow.testname.Items[1]).IsChecked = true;
            ((MenuItem)newWindow.testname.Items[2]).IsChecked = false;
        }
        private void PolishSelected(object sender, EventArgs e)
        {
            title = "O programie";
            desc = "Prosty odtwarzacz filmów";
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pl-PL");
            MainWindow newWindow = new MainWindow();
            Application.Current.MainWindow = newWindow;
            newWindow.Show();
            this.Close();
            ((MenuItem)newWindow.testname.Items[0]).IsChecked = false;
            ((MenuItem)newWindow.testname.Items[1]).IsChecked = false;
            ((MenuItem)newWindow.testname.Items[2]).IsChecked = true;
        }

        private void MenuItem_About(object sender, EventArgs e) { MessageBox.Show(desc, title, MessageBoxButton.OK); } 
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
            ticks.Tick += ticks_Tick!;
            ticks.Start();
            if (skipLength > Player.NaturalDuration.TimeSpan) skipLength = Player.NaturalDuration.TimeSpan;
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((ComboBox)sender).SelectedIndex)
            {
                case 0:
                    Player.SpeedRatio = 0.1;
                    break;
                case 1:
                    Player.SpeedRatio = 0.25;
                    break;
                case 2:
                    Player.SpeedRatio = 0.5;
                    break;
                case 3:
                    Player.SpeedRatio = 1;
                    break;
                case 4:
                    Player.SpeedRatio = 1.5;
                    break;
                case 5:
                    Player.SpeedRatio = 2;
                    break;
                case 6:
                    Player.SpeedRatio = 4;
                    break;
            }
        }
        void ticks_Tick(object sender, object e) { Dispatcher.Invoke(tick); }
        void SliderVolume_Click(object sender, MouseButtonEventArgs args)
        {
            Point coord = args.GetPosition(SliderVolume);
            SliderVolume.Value = (coord.X / 70) * 100;
            if(!isMuted) Player.Volume = (double)SliderVolume.Value / 100;
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
            if (isMuted)
            {
                Img.Source = new BitmapImage(new Uri(@"/VideoPlayer;component/Resources/Mute.png", UriKind.Relative));
                Player.Volume = (double)SliderVolume.Value / 100;
            }
            else 
            {
                Img.Source = new BitmapImage(new Uri(@"/VideoPlayer;component/Resources/Unmute.png", UriKind.Relative));
                Player.Volume = 0;
            }
            isMuted = !isMuted;
        }
    }
}