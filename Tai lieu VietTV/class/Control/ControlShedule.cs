using Microsoft.Phone.Controls;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using tiviViet;
using tiviViet.Models;
using tiviViet.ViewModels;

namespace tiviViet.Control
{
	public class ControlShedule : UserControl
	{
		internal Grid LayoutRoot;

		internal TextBlock TxtLoaiTran;

		internal TextBlock TxtTenTran;

		internal TextBlock TxtNgay;

		internal TextBlock TxtGio;

		internal EventTrigger LoadEventTrigger;

		internal Storyboard FadeIn;

		private bool _contentLoaded;

		public ControlShedule()
		{
			this.InitializeComponent();
			base.add_Loaded(new RoutedEventHandler(this, ControlShedule.ControlShedule_Loaded));
		}

		private void ControlShedule_Loaded(object sender, RoutedEventArgs e)
		{
			DetailsChannel dataContext = base.get_DataContext() as DetailsChannel;
			if (dataContext != null)
			{
				try
				{
					this.TxtLoaiTran.set_Text(dataContext.Name.Substring(0, dataContext.Name.IndexOf(":")));
					this.TxtTenTran.set_Text(dataContext.Name.Substring(dataContext.Name.IndexOf(":") + 1, dataContext.Name.IndexOf("(") - dataContext.Name.IndexOf(":") - 1));
					this.TxtNgay.set_Text(ConverTime.FooballCell(dataContext.Starttime));
					this.TxtGio.set_Text(dataContext.Name.Substring(dataContext.Name.IndexOf("(") - 1));
				}
				catch (Exception exception)
				{
				}
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
			Application.LoadComponent(this, new Uri("/tiviViet;component/Control/ControlShedule.xaml", 2));
			this.LayoutRoot = (Grid)base.FindName("LayoutRoot");
			this.TxtLoaiTran = (TextBlock)base.FindName("TxtLoaiTran");
			this.TxtTenTran = (TextBlock)base.FindName("TxtTenTran");
			this.TxtNgay = (TextBlock)base.FindName("TxtNgay");
			this.TxtGio = (TextBlock)base.FindName("TxtGio");
			this.LoadEventTrigger = (EventTrigger)base.FindName("LoadEventTrigger");
			this.FadeIn = (Storyboard)base.FindName("FadeIn");
		}

		private async void LayoutRoot_OnTap(object sender, GestureEventArgs e)
		{
			Chanel chanel = new Chanel();
			Chanel chanel1 = chanel;
			string linkShedule = await GetData.GetLinkShedule((this.get_DataContext() as DetailsChannel).Channelid);
			chanel1.Link = linkShedule;
			chanel1 = null;
			chanel.Link2 = "";
			chanel.ChanelName = (this.get_DataContext() as DetailsChannel).Name;
			chanel.TypeChannel = "TrucTiep";
			PlayVideo.ChanelPlayer = chanel;
			(Application.get_Current().get_RootVisual() as PhoneApplicationFrame).Navigate(new Uri("/PlayVideo.xaml", 2));
		}
	}
}