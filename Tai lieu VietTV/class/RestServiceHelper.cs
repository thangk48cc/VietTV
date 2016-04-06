using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using tiviViet;

namespace tiviViet.ViewModels
{
	internal class RestServiceHelper
	{
		public RestServiceHelper()
		{
		}

		public async Task<string> GetDataTranfer(string requestUrl)
		{
			string stringAsync;
			try
			{
				if (App.SessionCookieContainer == null)
				{
					App.SessionCookieContainer = new CookieContainer();
				}
				HttpClientHandler httpClientHandler = new HttpClientHandler()
				{
					CookieContainer = App.SessionCookieContainer
				};
				HttpClient httpClient = new HttpClient(httpClientHandler);
				httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.71 Safari/537.36");
				stringAsync = await httpClient.GetStringAsync(requestUrl);
			}
			catch
			{
				stringAsync = null;
			}
			return stringAsync;
		}
	}
}