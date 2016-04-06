using Microsoft.Phone.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using tiviViet.Models;
using tiviViet.View;

namespace tiviViet.Control
{
	public class ControlBao : UserControl
	{
		internal Grid LayoutRoot;

		internal TextBlock TxtTime;

		internal TextBlock TxtNguon;

		internal EventTrigger LoadEventTrigger;

		internal Storyboard FadeIn;

		private bool _contentLoaded;

		public ControlBao()
		{
			this.InitializeComponent();
		}

		private void ControlBao_Loaded(object sender, RoutedEventArgs e)
		{
			Articlelist dataContext = base.get_DataContext() as Articlelist;
			if (dataContext != null)
			{
				this.TxtTime.set_Text(dataContext.Converdate);
				this.TxtNguon.set_Text(string.Concat("(", dataContext.SourceName, ")"));
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
			Application.LoadComponent(this, new Uri("/tiviViet;component/Control/ControlBao.xaml", 2));
			this.LayoutRoot = (Grid)base.FindName("LayoutRoot");
			this.TxtTime = (TextBlock)base.FindName("TxtTime");
			this.TxtNguon = (TextBlock)base.FindName("TxtNguon");
			this.LoadEventTrigger = (EventTrigger)base.FindName("LoadEventTrigger");
			this.FadeIn = (Storyboard)base.FindName("FadeIn");
		}

		private void LayoutRoot_OnTap(object sender, GestureEventArgs e)
		{
			Articlelist dataContext = base.get_DataContext() as Articlelist;
			if (dataContext != null)
			{
				ViewDetailsBao.ArticleId = dataContext.ContentId;
				ViewDetailsBao.ListId = dataContext.ListId;
				ViewDetailsBao.title = dataContext.Title;
			}
			(Application.get_Current().get_RootVisual() as PhoneApplicationFrame).Navigate(new Uri("/view/ViewDetailsBao.xaml", 2));
		}
	}
}