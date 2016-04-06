using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using tiviViet.Control;
using tiviViet.Models;
using tiviViet.View;
using tiviViet.ViewModels;

namespace tiviViet
{
	public class TimKiem : PhoneApplicationPage
	{
		public static string Date;

		public static string NguonTrang;

		public static string TitleVideo;

		private bool _statusLoading;

		private int _currentPage;

		private string _linkVideo;

		private ObservableCollection<Video> _lsVideosLoadNew = new ObservableCollection<Video>();

		private ObservableCollection<Video> _lsVideos = new ObservableCollection<Video>();

		internal Grid LayoutRoot;

		internal Image ImgPhim;

		internal Image ImagePlay;

		internal Grid GridHeader;

		internal Image Menu;

		internal TextBlock TxtNameFilm;

		internal LongListSelector LsChannel;

		internal ControlProgress ControlLoadding;

		private bool _contentLoaded;

		public TimKiem()
		{
			this.InitializeComponent();
			this.LoadDataChitet();
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Application.LoadComponent(this, new Uri("/tiviViet;component/TimKiem.xaml", 2));
			this.LayoutRoot = (Grid)base.FindName("LayoutRoot");
			this.ImgPhim = (Image)base.FindName("ImgPhim");
			this.ImagePlay = (Image)base.FindName("ImagePlay");
			this.GridHeader = (Grid)base.FindName("GridHeader");
			this.Menu = (Image)base.FindName("Menu");
			this.TxtNameFilm = (TextBlock)base.FindName("TxtNameFilm");
			this.LsChannel = (LongListSelector)base.FindName("LsChannel");
			this.ControlLoadding = (ControlProgress)base.FindName("ControlLoadding");
		}

		public async void LoadDataChitet()
		{
			if (TimKiem.NguonTrang != "VTV")
			{
				this.LoadDataNew();
			}
			else
			{
				TimKiem timKiem = this;
				ObservableCollection<Video> observableCollection = timKiem._lsVideosLoadNew;
				string[] titleVideo = new string[] { "http://vtv.vn/video/tim-kiem.htm?zoneVideoId=0&keywords=", TimKiem.TitleVideo, " - ", TimKiem.Date, "&page=1" };
				ObservableCollection<Video> videoXemLai = await GetData.GetVideoXemLai(string.Concat(titleVideo));
				timKiem._lsVideosLoadNew = videoXemLai;
				timKiem = null;
				if (this._lsVideosLoadNew.get_Count() <= 0)
				{
					MessageBox.Show("Không hỗ trợ xem trương trình này");
					this.get_NavigationService().GoBack();
				}
				else
				{
					this.ImgPhim.set_Source(new BitmapImage(new Uri(this._lsVideosLoadNew.get_Item(0).UrlImage)));
					this._linkVideo = this._lsVideosLoadNew.get_Item(0).Url;
					this.TxtNameFilm.set_Text(this._lsVideosLoadNew.get_Item(0).Title);
				}
				this.LoadDataNew();
			}
		}

		public async void LoadDataNew()
		{
			TimKiem timKiem;
			if (TimKiem.NguonTrang != "VTV")
			{
				this.ControlLoadding.set_Visibility(0);
				this._statusLoading = true;
				this._currentPage = this._currentPage + 1;
				timKiem = this;
				ObservableCollection<Video> observableCollection = timKiem._lsVideosLoadNew;
				object[] titleVideo = new object[] { "http://www1.tvnet.gov.vn/search.html?search_key=", TimKiem.TitleVideo, "&page=", this._currentPage };
				ObservableCollection<Video> videoVtc = await GetData.GetVideoVtc(string.Concat(titleVideo), "search");
				timKiem._lsVideosLoadNew = videoVtc;
				timKiem = null;
				foreach (Video video in this._lsVideosLoadNew)
				{
					this._lsVideos.Add(video);
				}
				this.LsChannel.set_ItemsSource(this._lsVideos);
				if (this._lsVideos.get_Count() <= 0)
				{
					MessageBox.Show("Không hỗ trợ xem trương trình này");
					this.get_NavigationService().GoBack();
				}
				else
				{
					this.ImgPhim.set_Source(new BitmapImage(new Uri(this._lsVideos.get_Item(0).UrlImage)));
					this._linkVideo = this._lsVideos.get_Item(0).Url;
					this.TxtNameFilm.set_Text(this._lsVideos.get_Item(0).Title);
				}
				this._statusLoading = false;
				this.ControlLoadding.set_Visibility(1);
			}
			else
			{
				this.ControlLoadding.set_Visibility(0);
				this._statusLoading = true;
				this._currentPage = this._currentPage + 1;
				timKiem = this;
				ObservableCollection<Video> observableCollection1 = timKiem._lsVideosLoadNew;
				object[] objArray = new object[] { "http://vtv.vn/video/tim-kiem.htm?zoneVideoId=0&keywords=", TimKiem.TitleVideo, "&page=", this._currentPage };
				ObservableCollection<Video> videoXemLai = await GetData.GetVideoXemLai(string.Concat(objArray));
				timKiem._lsVideosLoadNew = videoXemLai;
				timKiem = null;
				foreach (Video video1 in this._lsVideosLoadNew)
				{
					this._lsVideos.Add(video1);
				}
				this.LsChannel.set_ItemsSource(this._lsVideos);
				this._statusLoading = false;
				this.ControlLoadding.set_Visibility(1);
			}
		}

		private void LsChannel_OnItemRealized(object sender, ItemRealizationEventArgs e)
		{
			Video content = e.get_Container().get_Content() as Video;
			if (content != null && !this._statusLoading && this._lsVideos.get_Count() - this._lsVideos.IndexOf(content) <= 2)
			{
				this.LoadDataNew();
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
					this.ControlLoadding.set_Visibility(0);
					if (selectedItem.Date != null)
					{
						Xemvideo.Link = await GetData.GetLinkMp4Vtv(selectedItem.Url);
					}
					else
					{
						Xemvideo.Link = await GetData.GetLinkMp4Vtc(selectedItem.Url);
					}
					Xemvideo.NameVideo = selectedItem.Title;
					this.ControlLoadding.set_Visibility(1);
					this.get_NavigationService().Navigate(new Uri("/View/XemVideo.xaml", 0));
					this.LsChannel.set_SelectedItem(null);
				}
			}
		}

		private void Menu_OnTap(object sender, GestureEventArgs e)
		{
			base.get_NavigationService().GoBack();
		}

		private async void UIElement_OnTap(object sender, GestureEventArgs e)
		{
			if (!this._linkVideo.Contains("vtv"))
			{
				Xemvideo.Link = await GetData.GetLinkMp4Vtc(this._linkVideo);
			}
			else
			{
				Xemvideo.Link = await GetData.GetLinkMp4Vtv(this._linkVideo);
			}
			Xemvideo.NameVideo = this.TxtNameFilm.get_Text();
			this.get_NavigationService().Navigate(new Uri("/View/XemVideo.xaml", 0));
		}
	}
}