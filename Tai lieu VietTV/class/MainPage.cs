using AMobiSDK;
using GoogleAds;
using GoogleAnalytics;
using GoogleAnalytics.Core;
using IcyS.Silverlight.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using RateMyApp.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Threading;
using tiviViet.Control;
using tiviViet.Models;
using tiviViet.ViewModels;
using VideoSDK;
using vn.clevernet.windowsphone.sdk;

namespace tiviViet
{
	public class MainPage : PhoneApplicationPage
	{
		private InterstitialAd _interstitialAd;

		private int _countSelectMenu;

		private AstVideo _videoAd;

		private int _loadFails;

		private bool _loadFv;

		private ObservableCollection<ChannelTv> _channelTvs = new ObservableCollection<ChannelTv>();

		private ObservableCollection<Chanel> _lsChanels;

		private ObservableCollection<Chanel> _lsSaveFav;

		private double _dragDistanceToOpen = 10;

		private double _dragDistanceToClose = 505;

		private double _dragDistanceNegative = -75;

		private ObservableCollection<tiviViet.ViewModels.Ads> _listAdses;

		private bool _isSettingsOpen;

		private FrameworkElement _feContainer;

		private bool _loadfailsAds = true;

		private bool _loadqc = true;

		private int _countChannel;

		internal DataTemplate Template;

		internal Grid LayoutRoot;

		internal Grid SettingsPane;

		internal Button btnRemove;

		internal Grid MenuMain;

		internal ListBox LsMenuTv;

		internal StackPanel GridLoadMenu;

		internal ProgressBar ProgressBarLoading;

		internal Image Menu;

		internal TextBlock TxtHeader;

		internal Image ImgLich;

		internal StackPanel GridThemKenhYeuthich;

		internal Image ImgAdd;

		internal ScrollViewer ScvChannel;

		internal GridView LsChannel;

		internal RateMyApp.Controls.FeedbackOverlay FeedbackOverlay;

		internal ControlProgress ControlLoadding;

		internal Grid PopupFace;

		internal Grid Qc;

		internal ListBox LsFree;

		internal ClevernetView Ads;

		internal MediaVideo Media;

		internal ApplicationBarMenuItem Addfav;

		internal ApplicationBarMenuItem ApplicationbarSendfeedback;

		internal ApplicationBarMenuItem BtnFace;

		private bool _contentLoaded;

		public MainPage()
		{
			this.InitializeComponent();
			this._feContainer = this.LayoutRoot;
			if (!DeviceNetworkInformation.get_IsNetworkAvailable())
			{
				MessageBox.Show("Vui lòng kết nối 3g hoặc wifi");
				Application.get_Current().Terminate();
			}
			else
			{
				this.LoadQc();
			}
			this._lsSaveFav = tiviViet.App.ListFavorite;
			this.FeedbackOverlay.VisibilityChanged += new EventHandler(this, tiviViet.MainPage.FeedbackOverlay_VisibilityChanged);
			if (!IsolatedStorageSettings.get_ApplicationSettings().Contains("Zipcodes"))
			{
				this.CreateTiles();
			}
			if (!tiviViet.App.sttRemove)
			{
				this.ShowVideo();
				this.btnRemove.set_Visibility(0);
			}
			Deployment.get_Current().get_Dispatcher().BeginInvoke(new Action(this, () => {
				this.LoadData();
				GetData.CheckAppVersion();
			}));
		}

		private void AbarQc_OnClick(object sender, EventArgs e)
		{
			this.ShowStore("a2b80625-3d8f-4fa2-9a42-e3759151052f");
		}

		private void ad_LoadFailed300x250(object sender, AdManager e)
		{
		}

		private void Addfav_OnClick(object sender, EventArgs e)
		{
			base.get_NavigationService().Navigate(new Uri("/view/ThemKenhYeuThich.xaml", 0));
		}

		private void adsfull_AdCollapsed(object sender, EventArgs e)
		{
		}

		private void adsfull_AdFailed(object sender, ClevernetView.AdFailedEventArgs e)
		{
			MessageBox.Show("sss");
		}

		private void adsfull_AdReceived(object sender, EventArgs e)
		{
		}

		private void AdsFullAdmob()
		{
		}

		private void ApplicationbarSendfeedback_OnClick(object sender, EventArgs e)
		{
			EmailComposeTask emailComposeTask = new EmailComposeTask();
			emailComposeTask.set_To("vietproductionwp@gmail.com");
			emailComposeTask.set_Subject("Tivi Việt Nam");
			emailComposeTask.Show();
		}

		private void BtnFace_OnClick(object sender, EventArgs e)
		{
			WebBrowserTask webBrowserTask = new WebBrowserTask();
			webBrowserTask.set_Uri(new Uri("https://www.facebook.com/CongdongWPVietNam"));
			webBrowserTask.Show();
		}

		private void BtnRemove_OnClick(object sender, RoutedEventArgs e)
		{
			base.get_NavigationService().Navigate(new Uri("/view/RemoveAds.xaml", 0));
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			this.ShowStore(this._listAdses.get_Item(0).Id);
			this.PopupFace.set_Visibility(1);
			IsolatedStorageSettings applicationSettings = IsolatedStorageSettings.get_ApplicationSettings();
			if (applicationSettings.Contains("IdApp"))
			{
				applicationSettings.set_Item("IdApp", this._listAdses.get_Item(0).Id);
			}
			else
			{
				applicationSettings.Add("IdApp", this._listAdses.get_Item(0).Id);
			}
			applicationSettings.Save();
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Application.get_Current().Terminate();
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

		private void CreateTiles()
		{
			IsolatedStorageSettings applicationSettings = IsolatedStorageSettings.get_ApplicationSettings();
			try
			{
				if (Enumerable.FirstOrDefault<ShellTile>(ShellTile.get_ActiveTiles()) != null)
				{
					FlipTileData flipTileDatum = new FlipTileData();
					flipTileDatum.set_BackgroundImage(new Uri("/Assets/Tiles/iconapp21.png", 2));
					flipTileDatum.set_Title("Tivi Việt Free");
					flipTileDatum.set_WideBackgroundImage(new Uri("/Assets/Tiles/logofull.png", 2));
					ShellTile.Create(new Uri("/MainPage.xaml", 2), flipTileDatum, false);
					if (applicationSettings.Contains("Zipcodes"))
					{
						applicationSettings.set_Item("Zipcodes", "titles");
					}
					else
					{
						applicationSettings.Add("Zipcodes", "titles");
					}
					applicationSettings.Save();
				}
			}
			catch (Exception exception)
			{
			}
		}

		private void FeedbackOverlay_VisibilityChanged(object sender, EventArgs e)
		{
			this.FeedbackOverlay.Reset();
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

		private void ImgAdd_OnTap(object sender, GestureEventArgs e)
		{
			Image imgAdd = this.ImgAdd;
			Action u003cu003e9_390 = tiviViet.MainPage.<>c.<>9__39_0;
			if (u003cu003e9_390 == null)
			{
				u003cu003e9_390 = new Action(tiviViet.MainPage.<>c.<>9, () => {
				});
				tiviViet.MainPage.<>c.<>9__39_0 = u003cu003e9_390;
			}
			Animation.Resize(imgAdd, u003cu003e9_390, 1, 0.25, 0.1);
			base.get_NavigationService().Navigate(new Uri("/view/ThemKenhYeuThich.xaml", 0));
		}

		private void ImgLich_OnTap(object sender, GestureEventArgs e)
		{
			Image imgLich = this.ImgLich;
			Action u003cu003e9_460 = tiviViet.MainPage.<>c.<>9__46_0;
			if (u003cu003e9_460 == null)
			{
				u003cu003e9_460 = new Action(tiviViet.MainPage.<>c.<>9, () => {
				});
				tiviViet.MainPage.<>c.<>9__46_0 = u003cu003e9_460;
			}
			Animation.Resize(imgLich, u003cu003e9_460, 1, 0.25, 0.1);
			base.get_NavigationService().Navigate(new Uri("/view/Viewshedule.xaml", 0));
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Application.LoadComponent(this, new Uri("/tiviViet;component/MainPage.xaml", 2));
			this.Template = (DataTemplate)base.FindName("Template");
			this.LayoutRoot = (Grid)base.FindName("LayoutRoot");
			this.SettingsPane = (Grid)base.FindName("SettingsPane");
			this.btnRemove = (Button)base.FindName("btnRemove");
			this.MenuMain = (Grid)base.FindName("MenuMain");
			this.LsMenuTv = (ListBox)base.FindName("LsMenuTv");
			this.GridLoadMenu = (StackPanel)base.FindName("GridLoadMenu");
			this.ProgressBarLoading = (ProgressBar)base.FindName("ProgressBarLoading");
			this.Menu = (Image)base.FindName("Menu");
			this.TxtHeader = (TextBlock)base.FindName("TxtHeader");
			this.ImgLich = (Image)base.FindName("ImgLich");
			this.GridThemKenhYeuthich = (StackPanel)base.FindName("GridThemKenhYeuthich");
			this.ImgAdd = (Image)base.FindName("ImgAdd");
			this.ScvChannel = (ScrollViewer)base.FindName("ScvChannel");
			this.LsChannel = (GridView)base.FindName("LsChannel");
			this.FeedbackOverlay = (RateMyApp.Controls.FeedbackOverlay)base.FindName("FeedbackOverlay");
			this.ControlLoadding = (ControlProgress)base.FindName("ControlLoadding");
			this.PopupFace = (Grid)base.FindName("PopupFace");
			this.Qc = (Grid)base.FindName("Qc");
			this.LsFree = (ListBox)base.FindName("LsFree");
			this.Ads = (ClevernetView)base.FindName("Ads");
			this.Media = (MediaVideo)base.FindName("Media");
			this.Addfav = (ApplicationBarMenuItem)base.FindName("Addfav");
			this.ApplicationbarSendfeedback = (ApplicationBarMenuItem)base.FindName("ApplicationbarSendfeedback");
			this.BtnFace = (ApplicationBarMenuItem)base.FindName("BtnFace");
		}

		public async void LoadData()
		{
			this.ControlLoadding.set_Visibility(0);
			DateTime dateTime = Convert.ToDateTime(DateTime.get_Now());
			string str = string.Concat(dateTime.ToString("dd MM yyyy"), ".txt");
			IsolatedStorageFile.GetUserStoreForApplication();
			this._lsChanels = new ObservableCollection<Chanel>();
			tiviViet.MainPage channelOnline = this;
			ObservableCollection<ChannelTv> observableCollection = channelOnline._channelTvs;
			channelOnline._channelTvs = await GetData.GetChannelOnline(str);
			channelOnline = null;
			foreach (ChannelTv _channelTv in this._channelTvs)
			{
				foreach (Chanel chanel in _channelTv.chanels)
				{
					this._lsChanels.Add(chanel);
				}
			}
			foreach (Chanel _lsChanel in this._lsChanels)
			{
				foreach (Chanel link in this._lsSaveFav)
				{
					if (_lsChanel.ChanelName != link.ChanelName)
					{
						continue;
					}
					link.Link = _lsChanel.Link;
					link.Link2 = _lsChanel.Link2;
					link.link3 = _lsChanel.link3;
					link.link4 = _lsChanel.link4;
					break;
				}
				tiviViet.App.ListFavorite = this._lsSaveFav;
				this.LoadKenhYeuThich();
			}
			this.LsMenuTv.set_ItemsSource(this._channelTvs);
			this.GridLoadMenu.set_Visibility(1);
			this.ControlLoadding.set_Visibility(1);
			this.Ads.set_Visibility(0);
		}

		private async void LoadKenhYeuThich()
		{
			if (this._lsSaveFav.get_Count() <= 0)
			{
				this.GridThemKenhYeuthich.set_Visibility(0);
				this.ScvChannel.set_Visibility(1);
			}
			else
			{
				this.GridThemKenhYeuthich.set_Visibility(1);
				this.ScvChannel.set_Visibility(0);
				this.ScvChannel.ScrollToVerticalOffset(0);
				this.LsChannel.ItemsSource = this._lsSaveFav;
			}
		}

		private async void LoadQc()
		{
			tiviViet.MainPage adsOnline = this;
			ObservableCollection<tiviViet.ViewModels.Ads> observableCollection = adsOnline._listAdses;
			adsOnline._listAdses = await GetData.GetAdsOnline();
			adsOnline = null;
			this.LsFree.set_ItemsSource(this._listAdses);
		}

		private void LoadQcVaoApp()
		{
			Banner300x250 banner300x250 = new Banner300x250();
			((FrameworkElement)banner300x250).set_VerticalAlignment(1);
			((FrameworkElement)banner300x250).set_HorizontalAlignment(1);
			Banner300x250 banner300x2501 = banner300x250;
			this.LayoutRoot.get_Children().Add(banner300x2501);
			AdManager adManager = new AdManager();
			adManager.LoadFailed300x250 += new LoadFailed300x250EventHandle(this.ad_LoadFailed300x250);
			AdManager.setWidgetCode("3ce029a8655b326148224d73b7d7a68b");
			adManager.OnNavigationTo(this, null, banner300x2501, null);
		}

		private void LsChannel_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!(sender is ListBox) || this.LsChannel.SelectedItem == null)
			{
				return;
			}
			Chanel selectedItem = (Chanel)this.LsChannel.SelectedItem;
			PlayVideo.ChanelPlayer = selectedItem;
			EasyTracker.GetTracker().SendEvent("Select channel", selectedItem.chanelName, null, (long)0);
			base.get_NavigationService().Navigate(new Uri("/PlayVideo.xaml", 0));
			this.LsChannel.SelectedIndex = -1;
		}

		private void LsChannel_Ontap(object sender, GestureEventArgs e)
		{
		}

		private void LsFree_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!(sender is ListBox) || this.LsFree.get_SelectedItem() == null)
			{
				return;
			}
			tiviViet.ViewModels.Ads selectedItem = (tiviViet.ViewModels.Ads)this.LsFree.get_SelectedItem();
			MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask();
			marketplaceDetailTask.set_ContentIdentifier(selectedItem.Id);
			marketplaceDetailTask.set_ContentType(1);
			marketplaceDetailTask.Show();
			this.LsFree.set_SelectedItem(null);
		}

		private async void LsMenuTV_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (sender is ListBox && this.LsMenuTv.get_SelectedItem() != null)
			{
				ChannelTv selectedItem = (ChannelTv)this.LsMenuTv.get_SelectedItem();
				this.GridThemKenhYeuthich.set_Visibility(1);
				this.ScvChannel.set_Visibility(0);
				EasyTracker.GetTracker().SendEvent("Select Category", selectedItem.GroupName, null, (long)0);
				if (selectedItem.groupName == "Xem Lại")
				{
					this.CloseSettings();
					this.get_NavigationService().Navigate(new Uri("/Xemlai.xaml", 0));
				}
				else if (selectedItem.groupName != "Bóng đá live")
				{
					this.CloseSettings();
					this.ScvChannel.ScrollToVerticalOffset(0);
					this.TxtHeader.set_Text(selectedItem.groupName);
					this.LsChannel.ItemsSource = selectedItem.chanels;
				}
				else if (!selectedItem.Chanels.get_Item(0).ChanelName.EndsWith("appnew"))
				{
					this.CloseSettings();
					this.get_NavigationService().Navigate(new Uri("/view/viewdetalsshedule.xaml", 0));
				}
				else
				{
					this.ShowStore(selectedItem.Chanels.get_Item(0).link);
				}
				this.LsMenuTv.set_SelectedIndex(-1);
			}
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

		protected override void OnBackKeyPress(CancelEventArgs e)
		{
			if (this._isSettingsOpen)
			{
				this.CloseSettings();
				e.set_Cancel(true);
				return;
			}
			if (this.TxtHeader.get_Text() != "Kênh Yêu Thích")
			{
				this.TxtHeader.set_Text("Kênh Yêu Thích");
				this.LoadKenhYeuThich();
				e.set_Cancel(true);
				return;
			}
			if (!IsolatedStorageSettings.get_ApplicationSettings().Contains("IdApp"))
			{
				e.set_Cancel(true);
				this.PopupFace.set_Visibility(0);
			}
			else
			{
				if (!IsolatedStorageSettings.get_ApplicationSettings().get_Item("IdApp").ToString().EndsWith(this._listAdses.get_Item(0).Id))
				{
					e.set_Cancel(true);
					this.PopupFace.set_Visibility(0);
					return;
				}
				if (MessageBox.Show("Bạn có thực sự muốn thoát không", "Thoát", 1) != 1)
				{
					e.set_Cancel(true);
					return;
				}
			}
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			if (this.TxtHeader.get_Text().Equals("Kênh Yêu Thích"))
			{
				this.LoadKenhYeuThich();
			}
			if (!tiviViet.App.sttRemove)
			{
				this.Ads.LoadAd();
			}
		}

		protected override void OnOrientationChanged(OrientationChangedEventArgs e)
		{
			if (base.get_Orientation() != 2 && base.get_Orientation() != 18 && base.get_Orientation() != 34)
			{
				this.Media.Orientation = TypeOrientations.Portrait;
				return;
			}
			this.Media.Orientation = TypeOrientations.Landscape;
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

		private void ResetLayoutRoot()
		{
			if (!this._isSettingsOpen)
			{
				this._feContainer.SetHorizontalOffset(0);
				return;
			}
			this._feContainer.SetHorizontalOffset(380);
		}

		private void ShowStore(string id)
		{
			MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask();
			marketplaceDetailTask.set_ContentIdentifier(id);
			marketplaceDetailTask.set_ContentType(1);
			marketplaceDetailTask.Show();
		}

		private void ShowVideo()
		{
			base.get_Dispatcher().BeginInvoke(new Action(this, () => {
				this.Media.set_Visibility(0);
				this._videoAd = new AstVideo();
				this._videoAd.TestVideo(false);
				this._videoAd.SetWidgetCode("3ce029a8655b326148224d73b7d7a68b");
				this._videoAd.SetVolume(false);
				this._videoAd.LoadVideo(this, this.Media);
				this._videoAd.Loadcompleted += new LoadCompleteHandle(this.VideoAd_Loadcompleted);
				this._videoAd.LoadFailed += new LoadFailHandle(this.VideoAd_LoadFailed);
				this._videoAd.ClosedVideo += new AstEventHandle(this.VideoAd_ClosedVideo);
			}));
		}

		private void UIElement_OnTap(object sender, GestureEventArgs e)
		{
		}

		private void VideoAd_ClosedVideo(object sender, AstVideo e)
		{
			this.LoadQcVaoApp();
			this.Ads.LoadAd();
		}

		private void VideoAd_Loadcompleted(object sender, AstVideo e)
		{
			this._videoAd.PlayVideo();
		}

		private void VideoAd_LoadFailed(object sender, AstVideo e)
		{
			this.LoadQcVaoApp();
			this.Ads.LoadAd();
		}

		private void View_AdReceived(object sender, EventArgs e)
		{
		}
	}
}