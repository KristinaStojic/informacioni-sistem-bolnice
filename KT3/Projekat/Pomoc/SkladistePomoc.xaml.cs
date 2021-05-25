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
using System.Windows.Threading;

namespace Projekat.Pomoc
{
    /// <summary>
    /// Interaction logic for SkladistePomoc.xaml
    /// </summary>
    public partial class SkladistePomoc : Window
    {
        DispatcherTimer timer;
        bool isDraging = false;
        public SkladistePomoc()
        {
            InitializeComponent();
            mediaElement.Play();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += new EventHandler(timer_Tick);
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Play();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Pause();
        }

        private void mediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            TimeSpan ts = mediaElement.NaturalDuration.TimeSpan;
            timelineSlider.Maximum = ts.TotalSeconds;
            timelineSlider.SmallChange = 1;
            timelineSlider.LargeChange = Math.Min(10, ts.Seconds / 10);
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (!isDraging)
            {
                timelineSlider.Value = mediaElement.Position.TotalSeconds;
            }
        }

        private void timelineSlider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            isDraging = true;
        }

        private void timelineSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            isDraging = false;
            mediaElement.Position = TimeSpan.FromSeconds(timelineSlider.Value); 
        }
    }
}
