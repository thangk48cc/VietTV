using AMobiSDK;
using GoogleAds;
using GoogleAnalytics;
using GoogleAnalytics.Core;
using Microsoft.PlayerFramework;
using Microsoft.Phone.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using tiviViet.Models;
using tiviViet.Models.plus;
using tiviViet.ViewModels;
using tiviViet.ViewModels.Mytv;

namespace tiviViet
{
	public class PlayVideo : PhoneApplicationPage
	{
		public static Chanel ChanelPlayer;

		private DispatcherTimer _timer;

		private AddFav _addFav;

		private int _countAds;

		private int _timeShowMenu;

		private int _countLoadChanel;

		private bool _clickfav;

		private List<string> _lichtv = new List<string>();

		public bool Sttplay;

		private string _str = "";

		private bool _sttPlay;

		private bool _sttLoadLich;

		private int _countLink;

		internal Grid LayoutRoot;

		internal MediaPlayer Player;

		internal Grid GridMenu;

		internal TextBlock TxtChannelName;

		internal StackPanel GridServer;

		internal Button BtnServer1;

		internal Button BtnServer2;

		internal Button BtnServer3;

		internal Button BtnServer4;

		internal StackPanel GridMenuLich;

		internal Image ImgFav;

		internal Grid GridQc;

		internal Image ImgCloseQc;

		internal Grid Qc320X50;

		internal AdView Ads;

		internal Grid GridLichChieu;

		internal ListBox ListBoxLichChieu;

		internal Image ImgClose;

		internal ProgressBar Load;

		private bool _contentLoaded;

		public PlayVideo()
		{
			this.InitializeComponent();
			this._addFav = new AddFav();
			this.TxtChannelName.set_Text(PlayVideo.ChanelPlayer.ChanelName);
			this.LoadImageFav();
			this.LoadLichChieu();
		}

		private void ad_LoadFailed320x50(object sender, AdManager e)
		{
			this.ImgCloseQc.set_Visibility(1);
			this.Qc320X50.set_Visibility(1);
			this.Ads.set_Visibility(0);
		}

		private void ads_ReceivedAd(object sender, AdEventArgs e)
		{
			this.ImgCloseQc.set_Visibility(0);
		}

		private void BtnServer2_OnClick(object sender, RoutedEventArgs e)
		{
			this._countLoadChanel = 1;
			this.GetDirectLink(PlayVideo.ChanelPlayer.Link2);
			this.ChangeColorButton(2);
		}

		private void BtnServer3_OnClick(object sender, RoutedEventArgs e)
		{
			this._countLoadChanel = 2;
			this.GetDirectLink(PlayVideo.ChanelPlayer.Link3);
			this.ChangeColorButton(3);
		}

		private void BtnServer4_OnClick(object sender, RoutedEventArgs e)
		{
			this._countLoadChanel = 3;
			this.GetDirectLink(PlayVideo.ChanelPlayer.Link4);
			this.ChangeColorButton(4);
		}

		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			this._countLoadChanel = 0;
			this.GetDirectLink(PlayVideo.ChanelPlayer.Link);
			this.ChangeColorButton(1);
		}

		public void ChangeColorButton(int btn)
		{
			switch (btn)
			{
				case 1:
				{
					this.BtnServer1.set_Background(new SolidColorBrush(Colors.get_Gray()));
					this.BtnServer2.set_Background(new SolidColorBrush(Colors.get_Transparent()));
					this.BtnServer3.set_Background(new SolidColorBrush(Colors.get_Transparent()));
					this.BtnServer4.set_Background(new SolidColorBrush(Colors.get_Transparent()));
					return;
				}
				case 2:
				{
					this.BtnServer2.set_Background(new SolidColorBrush(Colors.get_Gray()));
					this.BtnServer1.set_Background(new SolidColorBrush(Colors.get_Transparent()));
					this.BtnServer3.set_Background(new SolidColorBrush(Colors.get_Transparent()));
					this.BtnServer4.set_Background(new SolidColorBrush(Colors.get_Transparent()));
					return;
				}
				case 3:
				{
					this.BtnServer3.set_Background(new SolidColorBrush(Colors.get_Gray()));
					this.BtnServer2.set_Background(new SolidColorBrush(Colors.get_Transparent()));
					this.BtnServer1.set_Background(new SolidColorBrush(Colors.get_Transparent()));
					this.BtnServer4.set_Background(new SolidColorBrush(Colors.get_Transparent()));
					return;
				}
				case 4:
				{
					this.BtnServer4.set_Background(new SolidColorBrush(Colors.get_Gray()));
					this.BtnServer2.set_Background(new SolidColorBrush(Colors.get_Transparent()));
					this.BtnServer1.set_Background(new SolidColorBrush(Colors.get_Transparent()));
					this.BtnServer3.set_Background(new SolidColorBrush(Colors.get_Transparent()));
					return;
				}
				default:
				{
					return;
				}
			}
		}

		public static string DecodeEncodedNonAsciiCharacters(string value)
		{
			string str = value;
			MatchEvaluator u003cu003e9_220 = PlayVideo.<>c.<>9__22_0;
			if (u003cu003e9_220 == null)
			{
				u003cu003e9_220 = new MatchEvaluator(PlayVideo.<>c.<>9, (Match m) => ((char)int.Parse(m.get_Groups().get_Item("Value").get_Value(), 515)).ToString());
				PlayVideo.<>c.<>9__22_0 = u003cu003e9_220;
			}
			return Regex.Replace(str, "\\\\u(?<Value>[a-zA-Z0-9]{4})", u003cu003e9_220);
		}

		public static string EncodeNonAsciiCharacters(string value)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string str = value;
			for (int i = 0; i < str.get_Length(); i++)
			{
				char chr = str[i];
				if (chr <= '\u007F')
				{
					stringBuilder.Append(chr);
				}
				else
				{
					int num = chr;
					string str1 = string.Concat("\\u", num.ToString("x4"));
					stringBuilder.Append(str1);
				}
			}
			return stringBuilder.ToString();
		}

		private async void GetDirectLink(string link)
		{
			HttpClient httpClient = new HttpClient();
			httpClient.DefaultRequestHeaders.IfModifiedSince = new DateTimeOffset?(DateTimeOffset.get_Now());
			if (PlayVideo.ChanelPlayer.typeChannel.EndsWith("TrucTiep"))
			{
				if (this._countLink > 0 && !link.Contains("m3u8"))
				{
					string stringAsync = await httpClient.GetStringAsync(string.Concat("http://winphonevn.com/server/getlinkbongda.php?url=", link));
					link = stringAsync;
				}
				this._countLink = this._countLink + 1;
			}
			this.Player.Source = null;
			string[] strArray = null;
			string str = "";
			this.Load.set_Visibility(0);
			if (link.Contains("!"))
			{
				strArray = link.Split(new char[] { '!' });
			}
			try
			{
				if (strArray != null)
				{
					str = await httpClient.GetStringAsync(strArray[0]);
				}
				if (link.Contains("fptlive"))
				{
					string str1 = link;
					char[] chrArray = new char[] { '/' };
					string str2 = str1.Split(chrArray)[1];
					httpClient = new HttpClient();
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					dictionary.Add("id", str2);
					dictionary.Add("quality", "3");
					dictionary.Add("mobile", "web");
					dictionary.Add("type", "newchannel");
					FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(dictionary);
					HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
					{
						Method = HttpMethod.Post,
						RequestUri = new Uri("http://fptplay.net/show/getlinklivetv", 1),
						Content = formUrlEncodedContent
					};
					HttpRequestMessage uri = httpRequestMessage;
					uri.Headers.Referrer = new Uri("http://fptplay.net/");
					uri.Headers.Add("X-Requested-With", "XMLHttpRequest");
					HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(uri, HttpCompletionOption.ResponseContentRead);
					string str3 = await httpResponseMessage.Content.ReadAsStringAsync();
					if (str3 != null && str3.get_Length() > 0)
					{
						string item = (string)JObject.Parse(str3)["stream"];
						if (item.get_Length() > 0)
						{
							this._str = item;
						}
					}
				}
				else if (strArray != null)
				{
					Match match = (new Regex(strArray[1], 9)).Match(str);
					if (match.get_Success() && match.get_Groups().get_Item(1).get_Value().StartsWith("http"))
					{
						this._str = HttpUtility.UrlDecode(match.get_Groups().get_Item(1).get_Value());
					}
				}
				else if (link.Contains("app.101vn"))
				{
					Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
					dictionary1.Add("talachua", "101vn");
					dictionary1.Add("package", "duahau3");
					dictionary1.Add("version", "10");
					FormUrlEncodedContent formUrlEncodedContent1 = new FormUrlEncodedContent(dictionary1);
					HttpResponseMessage httpResponseMessage1 = await httpClient.PostAsync(link, formUrlEncodedContent1);
					PlayVideo playVideo = this;
					string str4 = playVideo._str;
					playVideo._str = await httpResponseMessage1.Content.ReadAsStringAsync();
					playVideo = null;
				}
				else if (link.Contains("notfreeplus"))
				{
					string str5 = await (new WebClient()).DownloadStringTaskAsync(new Uri("https://api.vtvplus.vn/pro/index.php/api/v1/channel/stream/88/?ip_address=0&username=84972291119@vtvplus.vn&type_device=android&type_network=wifi&app_name=VTVPlus"));
					str = str5;
					ObjectPlus objectPlu = JsonConvert.DeserializeObject<ObjectPlus>(str);
					this._str = objectPlu.Data.get_Item(0).Url;
				}
				else if (!link.Contains("notfreemtv"))
				{
					this._str = link;
				}
				else
				{
					string[] strArray1 = link.Split(new char[] { '=' });
					Random random = new Random();
					Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
					str = await httpClient.GetStringAsync("http://api.mytvnet.vn//getToken");
					JObject jObjects = JObject.Parse(str);
					if (jObjects == null || jObjects["tid"] == null)
					{
						return;
					}
					else
					{
						long num = Convert.ToInt64((new Fc()).DeCodeTid(jObjects["tid"].ToString()));
						DateTime now = DateTime.get_Now();
						Fc fc = new Fc();
						dictionary2.Add("tid", fc.GetTid(num, now));
						Dictionary<string, string> dictionary3 = dictionary2;
						int num1 = random.Next(1, 10);
						dictionary3.Add("device_type", num1.ToString());
						Dictionary<string, string> dictionary4 = dictionary2;
						num1 = random.Next(1, 99);
						dictionary4.Add("app_v", num1.ToString());
						dictionary2.Add("lang", "vn");
						dictionary2.Add("channel_id", strArray1[1]);
						dictionary2.Add("mf_code", strArray1[2]);
						Dictionary<string, string> dictionary5 = dictionary2;
						num1 = random.Next(1, 10);
						dictionary5.Add("profile", num1.ToString());
						Dictionary<string, string> dictionary6 = dictionary2;
						num1 = random.Next(1, 100000);
						dictionary6.Add("member_id", num1.ToString());
						Dictionary<string, string> dictionary7 = dictionary2;
						num1 = random.Next(1, 1000000);
						dictionary7.Add("manufacturer_id", num1.ToString());
						Dictionary<string, string> dictionary8 = dictionary2;
						num1 = random.Next(0, 100000);
						dictionary8.Add("device_id", num1.ToString());
						FormUrlEncodedContent formUrlEncodedContent2 = new FormUrlEncodedContent(dictionary2);
						HttpResponseMessage httpResponseMessage2 = await httpClient.PostAsync("http://ott.mytvnet.vn/v5/channel/mobile/url", formUrlEncodedContent2);
						string str6 = await httpResponseMessage2.Content.ReadAsStringAsync();
						jObjects = JObject.Parse(str6);
						if (jObjects["result"].ToString() == "-1")
						{
							Dictionary<string, string> dictionary9 = new Dictionary<string, string>();
							dictionary9.Add("tid", fc.GetTid(num, now));
							num1 = random.Next(1, 10);
							dictionary9.Add("device_type", num1.ToString());
							num1 = random.Next(1, 99);
							dictionary9.Add("app_v", num1.ToString());
							dictionary9.Add("lang", "vn");
							dictionary9.Add("channel_id", strArray1[1]);
							dictionary9.Add("mf_code", strArray1[2]);
							num1 = random.Next(1, 10);
							dictionary9.Add("profile", num1.ToString());
							num1 = random.Next(1, 100000);
							dictionary9.Add("member_id", num1.ToString());
							num1 = random.Next(1, 1000000);
							dictionary9.Add("manufacturer_id", num1.ToString());
							num1 = random.Next(1, 100000);
							dictionary9.Add("device_id", num1.ToString());
							dictionary2 = dictionary9;
							formUrlEncodedContent2 = new FormUrlEncodedContent(dictionary2);
							HttpResponseMessage httpResponseMessage3 = await httpClient.PostAsync("http://api.mytvnet.vn/v5/channel/mobile/url", formUrlEncodedContent2);
							string str7 = await httpResponseMessage3.Content.ReadAsStringAsync();
							jObjects = JObject.Parse(str7);
							this._str = jObjects["data"]["url"].ToString();
						}
						else
						{
							this._str = jObjects["data"]["url"].ToString();
						}
						strArray1 = null;
						random = null;
						dictionary2 = null;
						fc = null;
					}
				}
				this.Player.Source = new Uri(this._str.Trim(), 0);
				this.Player.Play();
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				EasyTracker.GetTracker().SendException(string.Concat(this._str, " :", exception.get_Message()), false);
				this.Player.Source = new Uri(this._str, 0);
				this.Player.Play();
				this.Load.set_Visibility(1);
			}
			this.Load.set_Visibility(1);
		}

		private void GridMenu_OnTap(object sender, GestureEventArgs e)
		{
			this._timeShowMenu = 0;
		}

		private void ImgClose_OnTap(object sender, GestureEventArgs e)
		{
			if (this.GridLichChieu.get_Visibility() == null)
			{
				this.GridLichChieu.set_Visibility(1);
				return;
			}
			this.GridLichChieu.set_Visibility(0);
		}

		private void ImgCloseQc_OnTap(object sender, GestureEventArgs e)
		{
			this.GridQc.set_Visibility(1);
		}

		private void ImgFav_OnTap(object sender, GestureEventArgs e)
		{
			this._clickfav = true;
			this._addFav.ChangeChecked(PlayVideo.ChanelPlayer);
			this.LoadImageFav();
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Application.LoadComponent(this, new Uri("/tiviViet;component/PlayVideo.xaml", 2));
			this.LayoutRoot = (Grid)base.FindName("LayoutRoot");
			this.Player = (MediaPlayer)base.FindName("Player");
			this.GridMenu = (Grid)base.FindName("GridMenu");
			this.TxtChannelName = (TextBlock)base.FindName("TxtChannelName");
			this.GridServer = (StackPanel)base.FindName("GridServer");
			this.BtnServer1 = (Button)base.FindName("BtnServer1");
			this.BtnServer2 = (Button)base.FindName("BtnServer2");
			this.BtnServer3 = (Button)base.FindName("BtnServer3");
			this.BtnServer4 = (Button)base.FindName("BtnServer4");
			this.GridMenuLich = (StackPanel)base.FindName("GridMenuLich");
			this.ImgFav = (Image)base.FindName("ImgFav");
			this.GridQc = (Grid)base.FindName("GridQc");
			this.ImgCloseQc = (Image)base.FindName("ImgCloseQc");
			this.Qc320X50 = (Grid)base.FindName("Qc320X50");
			this.Ads = (AdView)base.FindName("Ads");
			this.GridLichChieu = (Grid)base.FindName("GridLichChieu");
			this.ListBoxLichChieu = (ListBox)base.FindName("ListBoxLichChieu");
			this.ImgClose = (Image)base.FindName("ImgClose");
			this.Load = (ProgressBar)base.FindName("Load");
		}

		private void LoadImageFav()
		{
			for (int i = 0; i < App.ListFavorite.get_Count(); i++)
			{
				if (App.ListFavorite.get_Item(i).ChanelName.Equals(PlayVideo.ChanelPlayer.ChanelName))
				{
					if (this._clickfav)
					{
						ShowToast.InitializeBasicToast(string.Concat("Bạn vừa thêm kênh : (", PlayVideo.ChanelPlayer.ChanelName, ") vào mục yêu thích"));
					}
					this.ImgFav.set_Source(new BitmapImage(new Uri("icon/fav.png", 0)));
					return;
				}
				if (this._clickfav)
				{
					ShowToast.InitializeBasicToast(string.Concat("Xóa kênh : (", PlayVideo.ChanelPlayer.ChanelName, ") khỏi mục kênh yêu thích"));
				}
				this.ImgFav.set_Source(new BitmapImage(new Uri("icon/unfav.png", 0)));
			}
		}

		private async void LoadLichChieu()
		{
			PlayVideo.<LoadLichChieu>d__18 variable = new PlayVideo.<LoadLichChieu>d__18();
			variable.<>4__this = this;
			variable.<>t__builder = AsyncVoidMethodBuilder.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<PlayVideo.<LoadLichChieu>d__18>(ref variable);
		}

		private void LoadQc()
		{
			this.GridQc.set_Visibility(0);
			Banner320x50 banner320x50 = new Banner320x50();
			((FrameworkElement)banner320x50).set_VerticalAlignment(0);
			((FrameworkElement)banner320x50).set_HorizontalAlignment(1);
			Banner320x50 banner320x501 = banner320x50;
			this.Qc320X50.get_Children().Add(banner320x501);
			AdManager adManager = new AdManager();
			adManager.LoadFailed320x50 += new LoadFailed320x50EventHandle(this.ad_LoadFailed320x50);
			AdManager.setWidgetCode("3ce029a8655b326148224d73b7d7a68b");
			adManager.OnNavigationTo(this, null, null, banner320x501);
			this.ImgCloseQc.set_Visibility(0);
		}

		protected override void OnBackKeyPress(CancelEventArgs e)
		{
			this.Player.Stop();
			this.Player.Source = null;
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			this.Player.Stop();
			this._sttPlay = true;
			this.Player.Source = null;
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			if (PlayVideo.ChanelPlayer.link4 != "")
			{
				this.BtnServer1.set_Visibility(0);
				this.BtnServer2.set_Visibility(0);
				this.BtnServer3.set_Visibility(0);
				this.BtnServer4.set_Visibility(0);
			}
			else if (PlayVideo.ChanelPlayer.Link3 != "")
			{
				this.BtnServer1.set_Visibility(0);
				this.BtnServer2.set_Visibility(0);
				this.BtnServer3.set_Visibility(0);
			}
			else if (PlayVideo.ChanelPlayer.Link2 != "")
			{
				this.BtnServer1.set_Visibility(0);
				this.BtnServer2.set_Visibility(0);
			}
			if (!this._sttPlay)
			{
				this.GetDirectLink(PlayVideo.ChanelPlayer.link);
				this.ChangeColorButton(1);
			}
			else
			{
				this.Player.Source = new Uri(this._str, 0);
				this.Player.Play();
			}
			if (PlayVideo.ChanelPlayer.typeChannel == "TrucTiep")
			{
				this.GridMenuLich.set_Visibility(1);
			}
		}

		private void player_CurrentStateChanged(object sender, RoutedEventArgs e)
		{
		}

		private void player_MediaOpened(object sender, RoutedEventArgs e)
		{
			this.GridMenu.set_Visibility(1);
		}

		private void Player_OnMediaFailed(object sender, ExceptionRoutedEventArgs e)
		{
			if (PlayVideo.ChanelPlayer.Link2 == "")
			{
				MessageBox.Show("Có lỗi xảy ra! Xin vui lòng thử lại sau hoặc gửi phản hồi cho chúng tôi");
				return;
			}
			this._countLoadChanel = this._countLoadChanel + 1;
			if (this._countLoadChanel == 1)
			{
				this.GetDirectLink(PlayVideo.ChanelPlayer.Link2);
				this.ChangeColorButton(2);
				return;
			}
			if (this._countLoadChanel == 2)
			{
				this.GetDirectLink(PlayVideo.ChanelPlayer.Link3);
				this.ChangeColorButton(3);
				return;
			}
			if (this._countLoadChanel != 3)
			{
				MessageBox.Show("Có lỗi xảy ra! Xin vui lòng thử lại sau hoặc gửi phản hồi cho chúng tôi");
				return;
			}
			this.GetDirectLink(PlayVideo.ChanelPlayer.Link4);
			this.ChangeColorButton(4);
		}

		private void Player_OnTap(object sender, GestureEventArgs e)
		{
			if (this.GridMenu.get_Visibility() == null)
			{
				this.GridMenu.set_Visibility(1);
				return;
			}
			this.GridMenu.set_Visibility(0);
		}

		private void Storyboard1_OnCompleted(object sender, EventArgs e)
		{
		}

		private void UIElement_OnTap(object sender, GestureEventArgs e)
		{
			if (this.GridLichChieu.get_Visibility() == null)
			{
				this.GridLichChieu.set_Visibility(1);
				return;
			}
			this.GridLichChieu.set_Visibility(0);
		}
	}
}