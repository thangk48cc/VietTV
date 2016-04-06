using AMobiSDK;
using Microsoft.Phone.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using tiviViet.Control;
using tiviViet.Models;
using tiviViet.View;
using tiviViet.ViewModels;
using vn.clevernet.windowsphone.sdk;

namespace tiviViet
{
	public class XemLai : PhoneApplicationPage
	{
		private double _dragDistanceToOpen = 10;

		private double _dragDistanceToClose = 505;

		private double _dragDistanceNegative = -75;

		private bool _isSettingsOpen;

		private FrameworkElement _feContainer;

		private bool _statusLoading;

		private int _currentPage;

		private string _kenhDcChon = "";

		private bool _statusclick;

		private string _loaikenh = "vtv";

		private ObservableCollection<Video> _lsVideos = new ObservableCollection<Video>();

		private ObservableCollection<Video> _lsVideosLoadNew = new ObservableCollection<Video>();

		private ObservableCollection<SuggestVideo> _listSuggestVideo;

		private List<MenuVtv> _lsListMenu = new List<MenuVtv>();

		internal Grid LayoutRoot;

		internal Grid SettingsPane;

		internal Grid MenuMain;

		internal ListBox LsMenuTv;

		internal Image Menu;

		internal TextBlock TxtHeader;

		internal Image ImgSearch;

		internal Grid GrContent;

		internal LongListSelector LsChannel;

		internal Grid GrTheoKenh;

		internal Image ImgChannel;

		internal DatePicker PkDate;

		internal ListBox LsTheoKenh;

		internal ControlProgress ControlLoadding;

		internal ClevernetView Ads;

		private bool _contentLoaded;

		public XemLai()
		{
			this.InitializeComponent();
			this._feContainer = this.LayoutRoot;
			this._lsListMenu.Add(new MenuVtv("Mới Nhất", "", "moinhat"));
			this._lsListMenu.Add(new MenuVtv("VTV1", "icon/vtv1.png", "vtv1"));
			this._lsListMenu.Add(new MenuVtv("VTV2", "icon/vtv2.png", "vtv2"));
			this._lsListMenu.Add(new MenuVtv("VTV3", "icon/vtv3.png", "vtv3"));
			this._lsListMenu.Add(new MenuVtv("VTV4", "icon/vtv4.png", "vtv4"));
			this._lsListMenu.Add(new MenuVtv("VTV5", "icon/vtv5.png", "vtv5"));
			this._lsListMenu.Add(new MenuVtv("VTV6", "icon/vtv6.png", "vtv6"));
			this.LsMenuTv.set_ItemsSource(this._lsListMenu);
			this.LoadDataNew();
			if (!App.sttRemove)
			{
				this.LoadQcVaoApp();
			}
		}

		private void ad_LoadFailed320x50(object sender, AdManager e)
		{
			this.Ads.set_Visibility(0);
		}

		private void CloseSettings()
		{
			TranslateTransform transform = this._feContainer.GetHorizontalOffset().Transform;
			double x = transform.get_X();
			DependencyProperty xProperty = TranslateTransform.XProperty;
			CubicEase cubicEase = new CubicEase();
			cubicEase.set_EasingMode(0);
			transform.Animate(x, 0, xProperty, 300, 0, cubicEase, null);
			this._isSettingsOpen = false;
		}

		private void GestureListener_OnDragCompleted(object sender, DragCompletedGestureEventArgs e)
		{
			if (e.Direction == 1 && e.HorizontalChange > 0 && !this._isSettingsOpen)
			{
				if (e.HorizontalChange >= this._dragDistanceToOpen)
				{
					this.OpenSettings();
				}
				else
				{
					this.ResetLayoutRoot();
				}
			}
			if (e.Direction == 1 && e.HorizontalChange < 0 && this._isSettingsOpen)
			{
				if (e.HorizontalChange > this._dragDistanceNegative)
				{
					this.ResetLayoutRoot();
					return;
				}
				this.CloseSettings();
			}
		}

		private void GestureListener_OnDragDelta(object sender, DragDeltaGestureEventArgs e)
		{
			Offset offset;
			if (e.Direction == 1 && e.HorizontalChange > 0 && !this._isSettingsOpen)
			{
				offset = this._feContainer.GetHorizontalOffset();
				double value = offset.Value + e.HorizontalChange;
				if (value <= this._dragDistanceToOpen)
				{
					this._feContainer.SetHorizontalOffset(value);
				}
				else
				{
					this.OpenSettings();
				}
			}
			if (e.Direction == 1 && e.HorizontalChange < 0 && this._isSettingsOpen)
			{
				offset = this._feContainer.GetHorizontalOffset();
				double num = offset.Value + e.HorizontalChange;
				if (num < this._dragDistanceToClose)
				{
					this.CloseSettings();
					return;
				}
				this._feContainer.SetHorizontalOffset(num);
			}
		}

		public async void GetDatasXemlai(string date)
		{
			this.ControlLoadding.set_Visibility(0);
			ListBox lsTheoKenh = this.LsTheoKenh;
			ObservableCollection<LichXemLai> lichXemLai = await GetData.GetLichXemLai(this._kenhDcChon, date);
			lsTheoKenh.set_ItemsSource(lichXemLai);
			lsTheoKenh = null;
			this.ControlLoadding.set_Visibility(1);
		}

		private void GetSuggestVideo()
		{
			try
			{
				WebClient webClient = new WebClient();
				webClient.add_DownloadStringCompleted(new DownloadStringCompletedEventHandler(this, XemLai.webClient_DownloadStringCompleted));
				webClient.DownloadStringAsync(new Uri("http://online.vtv.vn/Ajax/SuggestSearchVideo.aspx", 0));
			}
			catch (Exception exception)
			{
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
			Application.LoadComponent(this, new Uri("/tiviViet;component/XemLai.xaml", 2));
			this.LayoutRoot = (Grid)base.FindName("LayoutRoot");
			this.SettingsPane = (Grid)base.FindName("SettingsPane");
			this.MenuMain = (Grid)base.FindName("MenuMain");
			this.LsMenuTv = (ListBox)base.FindName("LsMenuTv");
			this.Menu = (Image)base.FindName("Menu");
			this.TxtHeader = (TextBlock)base.FindName("TxtHeader");
			this.ImgSearch = (Image)base.FindName("ImgSearch");
			this.GrContent = (Grid)base.FindName("GrContent");
			this.LsChannel = (LongListSelector)base.FindName("LsChannel");
			this.GrTheoKenh = (Grid)base.FindName("GrTheoKenh");
			this.ImgChannel = (Image)base.FindName("ImgChannel");
			this.PkDate = (DatePicker)base.FindName("PkDate");
			this.LsTheoKenh = (ListBox)base.FindName("LsTheoKenh");
			this.ControlLoadding = (ControlProgress)base.FindName("ControlLoadding");
			this.Ads = (ClevernetView)base.FindName("Ads");
		}

		private async void LoadDataNew()
		{
			this.ControlLoadding.set_Visibility(0);
			this._statusLoading = true;
			this._currentPage = this._currentPage + 1;
			XemLai xemLai = this;
			ObservableCollection<Video> observableCollection = xemLai._lsVideosLoadNew;
			ObservableCollection<Video> videoXemLai = await GetData.GetVideoXemLai(string.Concat("http://vtvapi.channelvn.net/Handlers/VideoByZone.aspx?zonevideoid=0&page=", this._currentPage, "&pagesize=18&isnewstyle=1&topExclusion=0"));
			xemLai._lsVideosLoadNew = videoXemLai;
			xemLai = null;
			foreach (Video video in this._lsVideosLoadNew)
			{
				this._lsVideos.Add(video);
			}
			this.LsChannel.set_ItemsSource(this._lsVideos);
			this._statusLoading = false;
			this.ControlLoadding.set_Visibility(1);
		}

		private async void LoadDataVtc()
		{
			this.ControlLoadding.set_Visibility(0);
			this._statusLoading = true;
			this._currentPage = this._currentPage + 1;
			XemLai xemLai = this;
			ObservableCollection<Video> observableCollection = xemLai._lsVideosLoadNew;
			object[] objArray = new object[] { "http://www1.tvnet.gov.vn/modules/process.php?option=vod&cat_id=0&page=", this._currentPage, "&channel_id=", this._kenhDcChon };
			ObservableCollection<Video> videoVtc = await GetData.GetVideoVtc(string.Concat(objArray), "theokenh");
			xemLai._lsVideosLoadNew = videoVtc;
			xemLai = null;
			foreach (Video video in this._lsVideosLoadNew)
			{
				this._lsVideos.Add(video);
			}
			this.LsChannel.set_ItemsSource(this._lsVideos);
			this._statusLoading = false;
			this.ControlLoadding.set_Visibility(1);
		}

		private void LoadQcVaoApp()
		{
			Banner300x250 banner300x250 = new Banner300x250();
			((FrameworkElement)banner300x250).set_VerticalAlignment(1);
			((FrameworkElement)banner300x250).set_HorizontalAlignment(1);
			Banner300x250 banner300x2501 = banner300x250;
			this.LayoutRoot.get_Children().Add(banner300x2501);
			AdManager adManager = new AdManager();
			AdManager.setWidgetCode("3ce029a8655b326148224d73b7d7a68b");
			adManager.OnNavigationTo(this, null, banner300x2501, null);
		}

		private void LsChannel_OnItemRealized(object sender, ItemRealizationEventArgs e)
		{
			Video content = e.get_Container().get_Content() as Video;
			if (content != null && !this._statusLoading && this._lsVideos.get_Count() - this._lsVideos.IndexOf(content) <= 2)
			{
				if (this._loaikenh.Contains("vtv"))
				{
					this.LoadDataNew();
					return;
				}
				this.LoadDataVtc();
			}
		}

		private async void LsChannel_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			LongListSelector longListSelector = sender as LongListSelector;
			if (longListSelector != null)
			{
				Video selectedItem = longListSelector.get_SelectedItem() as Video;
				if (selectedItem != null)
				{
					if (selectedItem.Date != null)
					{
						this.ControlLoadding.set_Visibility(0);
						Xemvideo.Link = await GetData.GetLinkMp4Vtv(selectedItem.Url);
						this.ControlLoadding.set_Visibility(1);
					}
					else
					{
						Xemvideo.Link = selectedItem.Url;
					}
					Xemvideo.NameVideo = selectedItem.Title;
					this.get_NavigationService().Navigate(new Uri("/View/XemVideo.xaml", 0));
					this.LsChannel.set_SelectedItem(null);
				}
			}
		}

		private void LsMenuTV_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ListBox listBox = sender as ListBox;
			if (listBox == null)
			{
				return;
			}
			MenuVtv selectedItem = listBox.get_SelectedItem() as MenuVtv;
			if (selectedItem == null)
			{
				return;
			}
			if (selectedItem.Url == "moinhat")
			{
				this._loaikenh = "vtv";
				this._statusLoading = false;
				this._currentPage = 0;
				if (this.LsChannel.get_ItemsSource().get_Count() > 0)
				{
					this.LsChannel.get_ItemsSource().Clear();
				}
				this._lsVideos = new ObservableCollection<Video>();
				this.LsChannel.set_Visibility(0);
				this.GrTheoKenh.set_Visibility(1);
				this.LoadDataNew();
			}
			else if (!selectedItem.Url.Contains("vtv"))
			{
				this._loaikenh = "vtc";
				this._statusLoading = false;
				this._currentPage = 0;
				this.LsChannel.get_ItemsSource().Clear();
				this._lsVideos = new ObservableCollection<Video>();
				this.LsChannel.set_Visibility(0);
				this.GrTheoKenh.set_Visibility(1);
				this._kenhDcChon = selectedItem.Url;
				this.LoadDataVtc();
			}
			else
			{
				this.PkDate.Value = new DateTime?(DateTime.get_Now());
				this.LsChannel.set_Visibility(1);
				this.GrTheoKenh.set_Visibility(0);
				this.ImgChannel.set_Source(new BitmapImage(new Uri(selectedItem.ImageChannel, 0)));
				DateTime dateTime = Convert.ToDateTime(this.PkDate.ValueString);
				string str = dateTime.ToString("dd/MM/yyyy");
				this._kenhDcChon = selectedItem.Url;
				this.GetDatasXemlai(str);
			}
			this.LsMenuTv.set_SelectedIndex(-1);
			this.CloseSettings();
		}

		private void LsTheoKenh_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ListBox listBox = sender as ListBox;
			if (listBox == null)
			{
				return;
			}
			LichXemLai selectedItem = listBox.get_SelectedItem() as LichXemLai;
			if (selectedItem == null)
			{
				return;
			}
			if (selectedItem.Url != "")
			{
				TimKiem.NguonTrang = "VTV";
				DateTime dateTime = Convert.ToDateTime(this.PkDate.ValueString);
				TimKiem.Date = dateTime.ToString("dd/MM/yyyy");
				TimKiem.TitleVideo = selectedItem.Title;
				base.get_NavigationService().Navigate(new Uri("/TimKiem.xaml", 0));
			}
			else
			{
				TimKiem.NguonTrang = "VTC";
				TimKiem.TitleVideo = selectedItem.Title;
				base.get_NavigationService().Navigate(new Uri("/TimKiem.xaml", 0));
			}
			this.LsTheoKenh.set_SelectedIndex(-1);
		}

		private void Menu_OnTap(object sender, GestureEventArgs e)
		{
			if (this._isSettingsOpen)
			{
				this.CloseSettings();
				return;
			}
			this.OpenSettings();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			if (!App.sttRemove)
			{
				this.Ads.LoadAd();
			}
		}

		private void OpenSettings()
		{
			TranslateTransform transform = this._feContainer.GetHorizontalOffset().Transform;
			double x = transform.get_X();
			DependencyProperty xProperty = TranslateTransform.XProperty;
			CubicEase cubicEase = new CubicEase();
			cubicEase.set_EasingMode(0);
			transform.Animate(x, 380, xProperty, 300, 0, cubicEase, null);
			this._isSettingsOpen = true;
		}

		private void PkDate_OnTap(object sender, GestureEventArgs e)
		{
			this._statusclick = true;
		}

		private void PkDate_OnValueChanged(object sender, DateTimeValueChangedEventArgs e)
		{
			DateTime dateTime = Convert.ToDateTime(this.PkDate.ValueString);
			string str = dateTime.ToString("dd/MM/yyyy");
			if (this._statusclick)
			{
				this.GetDatasXemlai(str);
				this._statusclick = false;
			}
		}

		private void ResetLayoutRoot()
		{
			if (!this._isSettingsOpen)
			{
				this._feContainer.SetHorizontalOffset(0);
				return;
			}
			this._feContainer.SetHorizontalOffset(380);
		}

		private void UIElement_OnTap(object sender, GestureEventArgs e)
		{
			base.get_NavigationService().Navigate(new Uri("/view/SearchVideo.xaml", 0));
		}

		private void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			try
			{
				this._listSuggestVideo = new ObservableCollection<SuggestVideo>();
				if (!string.IsNullOrEmpty(e.get_Result()))
				{
					JToken first = JArray.Parse(e.get_Result()).First;
					while (first != null)
					{
						SuggestVideo suggestVideo = new SuggestVideo()
						{
							Title = first.Value<string>("Title"),
							ZoneParentId = first.Value<int>("ZoneVideoId"),
							ZoneVideoId = first.Value<int>("ZoneVideoId"),
							ChannelId = first.Value<int>("ChannelId")
						};
						first = first.Next;
						this._listSuggestVideo.Add(suggestVideo);
					}
				}
			}
			catch (Exception exception)
			{
			}
		}
	}
}