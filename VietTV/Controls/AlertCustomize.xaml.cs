using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using System.Windows.Threading;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace VietTV.Controls
{
    public partial class AlertCustomize : UserControl
    {
        public AlertCustomize()
        {
            InitializeComponent();
            stbOpen.Begin();
            Height = txtContent.ActualHeight + 25;
            Width = txtContent.ActualWidth + 25;
            this.VerticalAlignment = VerticalAlignment.Bottom + 20;
            this.HorizontalAlignment = HorizontalAlignment.Center + 20;
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Start();
            timer.Tick += timer_Tick;
        }
        public AlertCustomize(string content)
        {
            InitializeComponent();
            stbOpen.Stop();
            stbOpen.Begin();
            txtContent.Text = content;
            Height = txtContent.ActualHeight + 150;
            Width = txtContent.ActualWidth + 25;
            this.VerticalAlignment = VerticalAlignment.Bottom + 20;
            this.HorizontalAlignment = HorizontalAlignment.Center + 20;
            timer.Stop();
            timer.Interval = TimeSpan.FromSeconds(3);

            timer.Start();
            timer.Tick += timer_Tick;


        }

        void timer_Tick(object sender, EventArgs e)
        {
            stbClose.Begin();
            stbClose.Completed += (d, f) =>
            {
                var Popclaim = this.Parent as Popup;
                if (Popclaim != null)
                    Popclaim.IsOpen = false;
                timer.Stop();
            };

        }
        DispatcherTimer timer = new DispatcherTimer();

    }
}
