using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace tiviViet.Control
{
	public class ControlProgress : UserControl
	{
		internal Grid LayoutRoot;

		internal System.Windows.Controls.StackPanel StackPanel;

		internal Image CandleImage;

		private bool _contentLoaded;

		public ControlProgress()
		{
			this.InitializeComponent();
			base.add_Loaded(new RoutedEventHandler(this, ControlProgress.ControlProgress_Loaded));
		}

		private void ControlProgress_Loaded(object sender, RoutedEventArgs e)
		{
			Storyboard storyboard = new Storyboard();
			storyboard.set_RepeatBehavior(RepeatBehavior.get_Forever());
			Storyboard storyboard1 = storyboard;
			ObjectAnimationUsingKeyFrames objectAnimationUsingKeyFrame = new ObjectAnimationUsingKeyFrames();
			Storyboard.SetTarget(objectAnimationUsingKeyFrame, this.CandleImage);
			Storyboard.SetTargetProperty(objectAnimationUsingKeyFrame, new PropertyPath("Source", new object[0]));
			storyboard1.get_Children().Add(objectAnimationUsingKeyFrame);
			for (int i = 0; i <= 15; i++)
			{
				DiscreteObjectKeyFrame discreteObjectKeyFrame = new DiscreteObjectKeyFrame();
				discreteObjectKeyFrame.set_KeyTime(KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds((double)(70 * i))));
				discreteObjectKeyFrame.set_Value(string.Format("/icon/load/{0}.png", i));
				objectAnimationUsingKeyFrame.get_KeyFrames().Add(discreteObjectKeyFrame);
			}
			if (!base.get_Resources().Contains("CandleStoryboard"))
			{
				base.get_Resources().Add("CandleStoryboard", storyboard1);
			}
			storyboard1.Begin();
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Application.LoadComponent(this, new Uri("/tiviViet;component/Control/ControlProgress.xaml", 2));
			this.LayoutRoot = (Grid)base.FindName("LayoutRoot");
			this.StackPanel = (System.Windows.Controls.StackPanel)base.FindName("StackPanel");
			this.CandleImage = (Image)base.FindName("CandleImage");
		}
	}
}