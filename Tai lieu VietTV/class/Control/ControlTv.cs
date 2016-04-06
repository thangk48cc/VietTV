using GoogleAnalytics;
using GoogleAnalytics.Core;
using Microsoft.Phone.Controls;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using tiviViet;
using tiviViet.Models;
using tiviViet.ViewModels;

namespace tiviViet.Control
{
	public class ControlTv : UserControl
	{
		internal Grid GridTv;

		private bool _contentLoaded;

		public ControlTv()
		{
			this.InitializeComponent();
		}

		private void GridTv_OnTap(object sender, GestureEventArgs e)
		{
			Grid gridTv = this.GridTv;
			Action u003cu003e9_10 = ControlTv.<>c.<>9__1_0;
			if (u003cu003e9_10 == null)
			{
				u003cu003e9_10 = new Action(ControlTv.<>c.<>9, () => {
				});
				ControlTv.<>c.<>9__1_0 = u003cu003e9_10;
			}
			Animation.Resize(gridTv, u003cu003e9_10, 1, 0.25, 0.1);
			Chanel dataContext = base.get_DataContext() as Chanel;
			if (dataContext != null)
			{
				PlayVideo.ChanelPlayer = dataContext;
				EasyTracker.GetTracker().SendEvent("Select channel", dataContext.chanelName, null, (long)0);
				(Application.get_Current().get_RootVisual() as PhoneApplicationFrame).Navigate(new Uri("/PlayVideo.xaml", 2));
			}
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Application.LoadComponent(this, new Uri("/tiviViet;component/Control/ControlTv.xaml", 2));
			this.GridTv = (Grid)base.FindName("GridTv");
		}
	}
}