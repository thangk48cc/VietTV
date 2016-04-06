using AMobiSDK;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Navigation;
using tiviViet;
using tiviViet.Control;
using tiviViet.Models;
using tiviViet.ViewModels;
using vn.clevernet.windowsphone.sdk;

namespace tiviViet.View
{
	public class SearchVideo : PhoneApplicationPage
	{
		private bool _drag;

		private bool _statusLoading;

		private bool _statusLoadingvtv;

		private bool _statusLoadingvtc;

		private string _kenhDcChon = "";

		private int _currentPage;

		private int _currentPagevtc;

		private bool StatusLoadingvtc;

		private ObservableCollection<Video> _lsVideosvtv = new ObservableCollection<Video>();

		private ObservableCollection<Video> _lsVideosLoadNewvtv = new ObservableCollection<Video>();

		private ObservableCollection<Video> _lsVideosvtc = new ObservableCollection<Video>();

		private ObservableCollection<Video> _lsVideosLoadNewvtc = new ObservableCollection<Video>();

		internal Grid LayoutRoot;

		internal TextBox TxtSearch;

		internal Image ImgSearch;

		internal ToggleButton RecentTab;

		internal ToggleButton ArtistsTab;

		internal Pivot Contain;

		internal LongListSelector Lsvtv;

		internal LongListSelector LSvtc;

		internal ControlProgress ControlLoadding;

		internal ClevernetView Ads;

		private bool _contentLoaded;

		public SearchVideo()
		{
			this.InitializeComponent();
			base.add_Loaded(new RoutedEventHandler(this, SearchVideo.SearchVideo_Loaded));
		}

		private void ad_LoadFailed320x50(object sender, AdManager e)
		{
			this.Ads.set_Visibility(0);
		}

		private void ArtistsTab_Click(object sender, RoutedEventArgs e)
		{
			this.CheckTab(sender as ToggleButton);
		}

		private void container_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this._drag = false;
			if (this.Contain.get_SelectedItem() != null)
			{
				this.Uncheck();
				int selectedIndex = this.Contain.get_SelectedIndex();
				if (selectedIndex == 0)
				{
					this.RecentTab.set_IsChecked(new bool?(true));
					this.LoadDataVtv();
					return;
				}
				if (selectedIndex != 1)
				{
					return;
				}
				this.LoadDataVtc();
				this.ArtistsTab.set_IsChecked(new bool?(true));
			}
		}

		private void CheckTab(ToggleButton tab)
		{
			bool? isChecked = tab.get_IsChecked();
			if ((isChecked.GetValueOrDefault() ? isChecked.get_HasValue() : false))
			{
				this.Uncheck();
			}
			tab.set_IsChecked(new bool?(true));
			this.Contain.set_SelectedIndex(int.Parse(tab.get_Tag().ToString()));
		}

		private void ImgSearch_OnTap(object sender, GestureEventArgs e)
		{
			Image imgSearch = this.ImgSearch;
			Action u003cu003e9_270 = SearchVideo.<>c.<>9__27_0;
			if (u003cu003e9_270 == null)
			{
				u003cu003e9_270 = new Action(SearchVideo.<>c.<>9, () => {
				});
				SearchVideo.<>c.<>9__27_0 = u003cu003e9_270;
			}
			Animation.Resize(imgSearch, u003cu003e9_270, 1, 0.25, 0.1);
			this._lsVideosvtv.Clear();
			this._lsVideosvtc.Clear();
			this._currentPagevtc = 0;
			this._currentPage = 0;
			this.StatusLoadingvtc = false;
			this._statusLoading = false;
			bool? isChecked = this.RecentTab.get_IsChecked();
			if ((isChecked.GetValueOrDefault() ? isChecked.get_HasValue() : false))
			{
				this.LoadDataVtv();
				return;
			}
			this.LoadDataVtc();
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Application.LoadComponent(this, new Uri("/tiviViet;component/View/SearchVideo.xaml", 2));
			this.LayoutRoot = (Grid)base.FindName("LayoutRoot");
			this.TxtSearch = (TextBox)base.FindName("TxtSearch");
			this.ImgSearch = (Image)base.FindName("ImgSearch");
			this.RecentTab = (ToggleButton)base.FindName("RecentTab");
			this.ArtistsTab = (ToggleButton)base.FindName("ArtistsTab");
			this.Contain = (Pivot)base.FindName("Contain");
			this.Lsvtv = (LongListSelector)base.FindName("Lsvtv");
			this.LSvtc = (LongListSelector)base.FindName("LSvtc");
			this.ControlLoadding = (ControlProgress)base.FindName("ControlLoadding");
			this.Ads = (ClevernetView)base.FindName("Ads");
		}

		public async void LoadDataVtc()
		{
			this.ControlLoadding.set_Visibility(0);
			this.StatusLoadingvtc = true;
			this._currentPagevtc = this._currentPagevtc + 1;
			SearchVideo searchVideo = this;
			ObservableCollection<Video> observableCollection = searchVideo._lsVideosLoadNewvtc;
			object[] text = new object[] { "http://www1.tvnet.gov.vn/search.html?search_key=", this.TxtSearch.get_Text(), "&page=", this._currentPagevtc };
			ObservableCollection<Video> videoVtc = await GetData.GetVideoVtc(string.Concat(text), "search");
			searchVideo._lsVideosLoadNewvtc = videoVtc;
			searchVideo = null;
			foreach (Video video in this._lsVideosLoadNewvtc)
			{
				this._lsVideosvtc.Add(video);
			}
			this.LSvtc.set_ItemsSource(this._lsVideosvtc);
			this.StatusLoadingvtc = false;
			this.ControlLoadding.set_Visibility(1);
		}

		public async void LoadDataVtv()
		{
			this.ControlLoadding.set_Visibility(0);
			this._statusLoading = true;
			this._currentPage = this._currentPage + 1;
			SearchVideo searchVideo = this;
			ObservableCollection<Video> observableCollection = searchVideo._lsVideosLoadNewvtv;
			object[] text = new object[] { "http://vtv.vn/video/tim-kiem.htm?zoneVideoId=0&keywords=", this.TxtSearch.get_Text(), "&page=", this._currentPage };
			ObservableCollection<Video> videoXemLai = await GetData.GetVideoXemLai(string.Concat(text));
			searchVideo._lsVideosLoadNewvtv = videoXemLai;
			searchVideo = null;
			foreach (Video video in this._lsVideosLoadNewvtv)
			{
				this._lsVideosvtv.Add(video);
			}
			this.Lsvtv.set_ItemsSource(this._lsVideosvtv);
			this._statusLoading = false;
			this.ControlLoadding.set_Visibility(1);
		}

		private void LoadQc()
		{
			Banner320x50 banner320x50 = new Banner320x50();
			((FrameworkElement)banner320x50).set_VerticalAlignment(2);
			((FrameworkElement)banner320x50).set_HorizontalAlignment(1);
			Banner320x50 banner320x501 = banner320x50;
			this.LayoutRoot.get_Children().Add(banner320x501);
			AdManager adManager = new AdManager();
			adManager.LoadFailed320x50 += new LoadFailed320x50EventHandle(this.ad_LoadFailed320x50);
			AdManager.setWidgetCode("3ce029a8655b326148224d73b7d7a68b");
			adManager.OnNavigationTo(this, null, null, banner320x501);
		}

		private void LSvtc_OnItemRealized(object sender, ItemRealizationEventArgs e)
		{
			Video content = e.get_Container().get_Content() as Video;
			if (content != null && !this._statusLoadingvtc && this._lsVideosvtc.get_Count() - this._lsVideosvtc.IndexOf(content) <= 2)
			{
				this.LoadDataVtc();
			}
		}

		private async void LSvtc_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			LongListSelector longListSelector = sender as LongListSelector;
			if (longListSelector != null)
			{
				Video selectedItem = longListSelector.get_SelectedItem() as Video;
				if (selectedItem != null)
				{
					this.ControlLoadding.set_Visibility(0);
					Xemvideo.Link = await GetData.GetLinkMp4Vtc(selectedItem.Url);
					Xemvideo.NameVideo = selectedItem.Title;
					this.ControlLoadding.set_Visibility(1);
					this.get_NavigationService().Navigate(new Uri("/View/XemVideo.xaml", 0));
					this.LSvtc.set_SelectedItem(null);
				}
			}
		}

		private void LSVTV_OnItemRealized(object sender, ItemRealizationEventArgs e)
		{
			Video content = e.get_Container().get_Content() as Video;
			if (content != null && !this._statusLoadingvtv && this._lsVideosvtv.get_Count() - this._lsVideosvtv.IndexOf(content) <= 2)
			{
				this.LoadDataVtv();
			}
		}

		private async void LSVTV_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			LongListSelector longListSelector = sender as LongListSelector;
			if (longListSelector != null)
			{
				Video selectedItem = longListSelector.get_SelectedItem() as Video;
				if (selectedItem != null)
				{
					this.ControlLoadding.set_Visibility(0);
					Xemvideo.Link = await GetData.GetLinkMp4Vtv(selectedItem.Url);
					Xemvideo.NameVideo = selectedItem.Title;
					this.ControlLoadding.set_Visibility(1);
					this.get_NavigationService().Navigate(new Uri("/View/XemVideo.xaml", 0));
					this.Lsvtv.set_SelectedItem(null);
				}
			}
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			if (!App.sttRemove)
			{
				this.Ads.LoadAd();
			}
		}

		private void RecentTab_Click(object sender, RoutedEventArgs e)
		{
			this.CheckTab(sender as ToggleButton);
		}

		private void SearchVideo_Loaded(object sender, RoutedEventArgs e)
		{
			this.TxtSearch.Focus();
		}

		private void TextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.get_Key() == 3)
			{
				base.Focus();
				this._lsVideosvtv.Clear();
				this._currentPagevtc = 0;
				this._currentPage = 0;
				this.StatusLoadingvtc = false;
				this._statusLoading = false;
				this._lsVideosvtc.Clear();
				bool? isChecked = this.RecentTab.get_IsChecked();
				if ((isChecked.GetValueOrDefault() ? isChecked.get_HasValue() : false))
				{
					this.LoadDataVtv();
					return;
				}
				this.LoadDataVtc();
			}
		}

		private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
		{
		}

		private void Uncheck()
		{
			this.RecentTab.set_IsChecked(new bool?(false));
			this.ArtistsTab.set_IsChecked(new bool?(false));
		}
	}
}