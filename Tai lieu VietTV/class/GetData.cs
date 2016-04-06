using HtmlAgilityPack;
using Microsoft.Phone.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using tiviViet.Models;
using tiviViet.Models.BongDa101;
using tiviViet.Models.Bongda3;
using tiviViet.Models.ServerBongda2;
using tiviViet.Models.ServerClip2;

namespace tiviViet.ViewModels
{
	internal class GetData
	{
		private static string _linkgetM3U8;

		private CancellationTokenSource _cts;

		static GetData()
		{
			GetData._linkgetM3U8 = "";
		}

		public GetData()
		{
		}

		public static async void CheckAppVersion()
		{
			GetData.<CheckAppVersion>d__22 variable = new GetData.<CheckAppVersion>d__22();
			variable.<>t__builder = AsyncVoidMethodBuilder.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<GetData.<CheckAppVersion>d__22>(ref variable);
		}

		public static bool CheckQc()
		{
			if (!IsolatedStorageSettings.get_ApplicationSettings().Contains("Quangcao"))
			{
				return false;
			}
			return IsolatedStorageSettings.get_ApplicationSettings().get_Item("Quangcao").ToString().Equals("lucvao");
		}

		public static ObservableCollection<Ads> GetAdsOffline(string filename)
		{
			ObservableCollection<Ads> observableCollection = new ObservableCollection<Ads>();
			IsolatedStorageFileStream isolatedStorageFileStream = new IsolatedStorageFileStream(filename, 3, IsolatedStorageFile.GetUserStoreForApplication());
			try
			{
				StreamReader streamReader = new StreamReader(isolatedStorageFileStream);
				try
				{
					JToken first = JArray.Parse(streamReader.ReadToEnd()).First;
					while (first != null)
					{
						Ads ad = new Ads()
						{
							Title = first.Value<string>("title"),
							Image = first.Value<string>("image"),
							Id = first.Value<string>("id"),
							Description = first.Value<string>("description")
						};
						first = first.Next;
						observableCollection.Add(ad);
					}
				}
				finally
				{
					if (streamReader != null)
					{
						streamReader.Dispose();
					}
				}
			}
			finally
			{
				if (isolatedStorageFileStream != null)
				{
					isolatedStorageFileStream.Dispose();
				}
			}
			return observableCollection;
		}

		public static async Task<ObservableCollection<Ads>> GetAdsOnline()
		{
			GetData.<GetAdsOnline>d__27 variable = new GetData.<GetAdsOnline>d__27();
			variable.<>t__builder = AsyncTaskMethodBuilder<ObservableCollection<Ads>>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<GetData.<GetAdsOnline>d__27>(ref variable);
			return variable.<>t__builder.get_Task();
		}

		public static async Task<ObservableCollection<ChannelTructiep>> GetBongDa101Vn()
		{
			GetData.<GetBongDa101Vn>d__19 variable = new GetData.<GetBongDa101Vn>d__19();
			variable.<>t__builder = AsyncTaskMethodBuilder<ObservableCollection<ChannelTructiep>>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<GetData.<GetBongDa101Vn>d__19>(ref variable);
			return variable.<>t__builder.get_Task();
		}

		public static async Task<string> Getbongda3M3U8(string channelId)
		{
			GetData.<Getbongda3M3U8>d__7 variable = new GetData.<Getbongda3M3U8>d__7();
			variable.channelId = channelId;
			variable.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<GetData.<Getbongda3M3U8>d__7>(ref variable);
			return variable.<>t__builder.get_Task();
		}

		public static async Task<ObservableCollection<Vod>> GetClipBongDa2()
		{
			GetData.<GetClipBongDa2>d__17 variable = new GetData.<GetClipBongDa2>d__17();
			variable.<>t__builder = AsyncTaskMethodBuilder<ObservableCollection<Vod>>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<GetData.<GetClipBongDa2>d__17>(ref variable);
			return variable.<>t__builder.get_Task();
		}

		public static ObservableCollection<ChannelTv> GetChannelOffline(string filename)
		{
			ObservableCollection<ChannelTv> observableCollection;
			IsolatedStorageFileStream isolatedStorageFileStream = new IsolatedStorageFileStream(filename, 3, IsolatedStorageFile.GetUserStoreForApplication());
			try
			{
				StreamReader streamReader = new StreamReader(isolatedStorageFileStream);
				try
				{
					observableCollection = JsonConvert.DeserializeObject<ObservableCollection<ChannelTv>>(streamReader.ReadToEnd());
				}
				finally
				{
					if (streamReader != null)
					{
						streamReader.Dispose();
					}
				}
			}
			finally
			{
				if (isolatedStorageFileStream != null)
				{
					isolatedStorageFileStream.Dispose();
				}
			}
			return observableCollection;
		}

		public static async Task<ObservableCollection<ChannelTv>> GetChannelOnline(string filename)
		{
			GetData.<GetChannelOnline>d__2 variable = new GetData.<GetChannelOnline>d__2();
			variable.<>t__builder = AsyncTaskMethodBuilder<ObservableCollection<ChannelTv>>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<GetData.<GetChannelOnline>d__2>(ref variable);
			return variable.<>t__builder.get_Task();
		}

		public async Task<string> getData(string path, bool bNoCatch, bool bRip)
		{
			GetData.<getData>d__13 variable = new GetData.<getData>d__13();
			variable.<>4__this = this;
			variable.path = path;
			variable.bNoCatch = bNoCatch;
			variable.bRip = bRip;
			variable.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<GetData.<getData>d__13>(ref variable);
			return variable.<>t__builder.get_Task();
		}

		public static async Task<ObservableCollection<Articlelist>> GetDataBao(string url)
		{
			GetData.<GetDataBao>d__4 variable = new GetData.<GetDataBao>d__4();
			variable.url = url;
			variable.<>t__builder = AsyncTaskMethodBuilder<ObservableCollection<Articlelist>>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<GetData.<GetDataBao>d__4>(ref variable);
			return variable.<>t__builder.get_Task();
		}

		public static async Task<string> GetDataDetailsBao(string url)
		{
			GetData.<GetDataDetailsBao>d__3 variable = new GetData.<GetDataDetailsBao>d__3();
			variable.url = url;
			variable.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<GetData.<GetDataDetailsBao>d__3>(ref variable);
			return variable.<>t__builder.get_Task();
		}

		public static async Task<ObservableCollection<LichXemLai>> GetLichXemLai(string vtv, string date)
		{
			GetData.<GetLichXemLai>d__8 variable = new GetData.<GetLichXemLai>d__8();
			variable.vtv = vtv;
			variable.date = date;
			variable.<>t__builder = AsyncTaskMethodBuilder<ObservableCollection<LichXemLai>>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<GetData.<GetLichXemLai>d__8>(ref variable);
			return variable.<>t__builder.get_Task();
		}

		public static async Task<string> GetLinkBongDa2(string channelId)
		{
			GetData.<GetLinkBongDa2>d__18 variable = new GetData.<GetLinkBongDa2>d__18();
			variable.channelId = channelId;
			variable.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<GetData.<GetLinkBongDa2>d__18>(ref variable);
			return variable.<>t__builder.get_Task();
		}

		public static async Task<Bongda3> GetLinkBongda3()
		{
			GetData.<GetLinkBongda3>d__6 variable = new GetData.<GetLinkBongda3>d__6();
			variable.<>t__builder = AsyncTaskMethodBuilder<Bongda3>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<GetData.<GetLinkBongda3>d__6>(ref variable);
			return variable.<>t__builder.get_Task();
		}

		public static async Task<string> GetLinkMp4Vtc(string link)
		{
			GetData.<GetLinkMp4Vtc>d__11 variable = new GetData.<GetLinkMp4Vtc>d__11();
			variable.link = link;
			variable.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<GetData.<GetLinkMp4Vtc>d__11>(ref variable);
			return variable.<>t__builder.get_Task();
		}

		public static async Task<string> GetLinkMp4Vtv(string link)
		{
			GetData.<GetLinkMp4Vtv>d__24 variable = new GetData.<GetLinkMp4Vtv>d__24();
			variable.link = link;
			variable.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<GetData.<GetLinkMp4Vtv>d__24>(ref variable);
			return variable.<>t__builder.get_Task();
		}

		public static async Task<string> GetLinkShedule(string channelId)
		{
			GetData.<GetLinkShedule>d__5 variable = new GetData.<GetLinkShedule>d__5();
			variable.channelId = channelId;
			variable.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<GetData.<GetLinkShedule>d__5>(ref variable);
			return variable.<>t__builder.get_Task();
		}

		public static async Task<List<Related>> GetLiveFocus(string accessToken)
		{
			GetData.<GetLiveFocus>d__14 variable = new GetData.<GetLiveFocus>d__14();
			variable.accessToken = accessToken;
			variable.<>t__builder = AsyncTaskMethodBuilder<List<Related>>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<GetData.<GetLiveFocus>d__14>(ref variable);
			return variable.<>t__builder.get_Task();
		}

		public static async Task<List<YoSportMatch>> GetLiveMatch(string accessToken)
		{
			GetData.<GetLiveMatch>d__15 variable = new GetData.<GetLiveMatch>d__15();
			variable.<>t__builder = AsyncTaskMethodBuilder<List<YoSportMatch>>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<GetData.<GetLiveMatch>d__15>(ref variable);
			return variable.<>t__builder.get_Task();
		}

		public static async Task<ObservableCollection<Video>> GetVideoVtc(string link, string type)
		{
			GetData.<GetVideoVtc>d__9 variable = new GetData.<GetVideoVtc>d__9();
			variable.link = link;
			variable.type = type;
			variable.<>t__builder = AsyncTaskMethodBuilder<ObservableCollection<Video>>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<GetData.<GetVideoVtc>d__9>(ref variable);
			return variable.<>t__builder.get_Task();
		}

		public static async Task<ObservableCollection<Video>> GetVideoXemLai(string link)
		{
			GetData.<GetVideoXemLai>d__10 variable = new GetData.<GetVideoXemLai>d__10();
			variable.link = link;
			variable.<>t__builder = AsyncTaskMethodBuilder<ObservableCollection<Video>>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<GetData.<GetVideoXemLai>d__10>(ref variable);
			return variable.<>t__builder.get_Task();
		}

		public static async Task<UserInfo> Init()
		{
			GetData.<Init>d__16 variable = new GetData.<Init>d__16();
			variable.<>t__builder = AsyncTaskMethodBuilder<UserInfo>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<GetData.<Init>d__16>(ref variable);
			return variable.<>t__builder.get_Task();
		}

		public static async void PostJsonHttpClient()
		{
			GetData.<PostJsonHttpClient>d__20 variable = new GetData.<PostJsonHttpClient>d__20();
			variable.<>t__builder = AsyncVoidMethodBuilder.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<GetData.<PostJsonHttpClient>d__20>(ref variable);
		}

		public static async Task<ObservableCollection<Video>> SearchVideo(string link)
		{
			GetData.<SearchVideo>d__25 variable = new GetData.<SearchVideo>d__25();
			variable.link = link;
			variable.<>t__builder = AsyncTaskMethodBuilder<ObservableCollection<Video>>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<GetData.<SearchVideo>d__25>(ref variable);
			return variable.<>t__builder.get_Task();
		}

		private static void ShowStore(string id)
		{
			MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask();
			marketplaceDetailTask.set_ContentIdentifier(id);
			marketplaceDetailTask.set_ContentType(1);
			marketplaceDetailTask.Show();
		}
	}
}