using Microsoft.Phone.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace tiviViet.ViewModels
{
	public class SService
	{
		public static bool IsFirstInstallation;

		public SService()
		{
		}

		public static KeyValuePair<string, string> CreateParameter(string key, string value)
		{
			return new KeyValuePair<string, string>(key, value);
		}

		public static HttpMethod GetHttpMethod(SHttpMethod method)
		{
			switch (method)
			{
				case SHttpMethod.Get:
				{
					return HttpMethod.Get;
				}
				case SHttpMethod.Post:
				{
					return HttpMethod.Post;
				}
				case SHttpMethod.Put:
				{
					return HttpMethod.Put;
				}
			}
			return HttpMethod.Delete;
		}

		private static void GoToMarket(string appId)
		{
			MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask();
			marketplaceDetailTask.set_ContentIdentifier(appId);
			marketplaceDetailTask.set_ContentType(1);
			marketplaceDetailTask.Show();
		}

		public static bool IsNewUpdate(string currentVersion, string newVersion)
		{
			int num = int.Parse(string.Join("", currentVersion.Split(new char[] { '.' })));
			return int.Parse(string.Join("", newVersion.Split(new char[] { '.' }))) > num;
		}

		public static async Task<bool> IsRemoteFileExisted(string url)
		{
			SService.<IsRemoteFileExisted>d__3 variable = new SService.<IsRemoteFileExisted>d__3();
			variable.url = url;
			variable.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<SService.<IsRemoteFileExisted>d__3>(ref variable);
			return variable.<>t__builder.get_Task();
		}

		public static List<KeyValuePair<string, string>> Parameters(List<KeyValuePair<string, string>> parameters)
		{
			return parameters;
		}

		public static async Task<T> Request<T>(string url, SHttpMethod method, List<KeyValuePair<string, string>> parameters = null, List<KeyValuePair<string, string>> headers = null, bool isNotCache = false)
		{
			SService.<Request>d__6<T> variable = new SService.<Request>d__6<T>();
			variable.url = url;
			variable.method = method;
			variable.parameters = parameters;
			variable.headers = headers;
			variable.isNotCache = isNotCache;
			variable.<>t__builder = AsyncTaskMethodBuilder<T>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<SService.<Request>d__6<T>>(ref variable);
			return variable.<>t__builder.get_Task();
		}

		public static async Task<Stream> RequestReturnStream(string url, SHttpMethod method, double timeout = 20000, bool isNotCache = false)
		{
			SService.<RequestReturnStream>d__4 variable = new SService.<RequestReturnStream>d__4();
			variable.url = url;
			variable.method = method;
			variable.timeout = timeout;
			variable.isNotCache = isNotCache;
			variable.<>t__builder = AsyncTaskMethodBuilder<Stream>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<SService.<RequestReturnStream>d__4>(ref variable);
			return variable.<>t__builder.get_Task();
		}

		public static async Task<string> RequestReturnString(string url, SHttpMethod method, List<KeyValuePair<string, string>> parameters = null, bool isNotCache = false)
		{
			SService.<RequestReturnString>d__5 variable = new SService.<RequestReturnString>d__5();
			variable.url = url;
			variable.method = method;
			variable.parameters = parameters;
			variable.isNotCache = isNotCache;
			variable.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<SService.<RequestReturnString>d__5>(ref variable);
			return variable.<>t__builder.get_Task();
		}
	}
}