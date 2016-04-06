using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using tiviViet.Models;

namespace tiviViet.ViewModels
{
	public class ZingService
	{
		public static string Host;

		static ZingService()
		{
			ZingService.Host = "http://stat.sfone095.com";
		}

		public ZingService()
		{
		}

		public static async Task<ApiResult<ZingArticleDetailByLiveStream>> GetArticleDetailByArticle(string id)
		{
			ApiResult<ZingArticleDetailByLiveStream> apiResult = await SService.Request<ApiResult<ZingArticleDetailByLiveStream>>(string.Format("{0}/api/zing/article/{1}/{2}", ZingService.Host, "article", id), SHttpMethod.Get, null, null, false);
			return apiResult;
		}

		public static async Task<ApiResult<ObservableCollection<ZingArticleDetailByPicture>>> GetArticleDetailByPicture(string id)
		{
			ApiResult<ObservableCollection<ZingArticleDetailByPicture>> apiResult = await SService.Request<ApiResult<ObservableCollection<ZingArticleDetailByPicture>>>(string.Format("{0}/api/zing/article/{1}/{2}", ZingService.Host, "picture", id), SHttpMethod.Get, null, null, false);
			return apiResult;
		}

		public static async Task<ApiResult<string>> GetArticleDetailByVideo(string id)
		{
			ApiResult<string> apiResult = await SService.Request<ApiResult<string>>(string.Format("{0}/api/zing/article/{1}/{2}", ZingService.Host, "video", id), SHttpMethod.Get, null, null, false);
			return apiResult;
		}

		public static async Task<ApiResult<ObservableCollection<ZingArticle>>> GetArticles()
		{
			ApiResult<ObservableCollection<ZingArticle>> apiResult = await SService.Request<ApiResult<ObservableCollection<ZingArticle>>>(string.Concat(ZingService.Host, "/api/zing/articles"), SHttpMethod.Get, null, null, false);
			return apiResult;
		}

		public static async Task<string> GetLinkZingVideo(string id)
		{
			ApiResult<string> apiResult = await SService.Request<ApiResult<string>>(string.Format("http://stat.sfone095.com/api/zing/article/video/{0}", id), SHttpMethod.Get, null, null, false);
			return apiResult.Data;
		}

		public static async Task<string> GetLinkZingVideoDirect(string id)
		{
			string empty;
			string video720;
			FZingVideo.ZingArticleDetailByVideo zingArticleDetailByVideo = await SService.Request<FZingVideo.ZingArticleDetailByVideo>(string.Format("http://api.tv.zing.vn/2.0/media/info?api_key=74af1bc77ef7a6089e7022a1d1ed8841&media_id={0}", id), SHttpMethod.Get, null, null, false);
			FZingVideo.ZingArticleDetailByVideo zingArticleDetailByVideo1 = zingArticleDetailByVideo;
			if (zingArticleDetailByVideo1 == null || zingArticleDetailByVideo1.Video == null || zingArticleDetailByVideo1.Video.Detail == null)
			{
				empty = string.Empty;
			}
			else
			{
				string str = string.Empty;
				if (string.IsNullOrEmpty(zingArticleDetailByVideo1.Video.Detail.Video720))
				{
					video720 = (string.IsNullOrEmpty(zingArticleDetailByVideo1.Video.Detail.Video480) ? zingArticleDetailByVideo1.Video.Detail.Video3Gp : zingArticleDetailByVideo1.Video.Detail.Video480);
				}
				else
				{
					video720 = zingArticleDetailByVideo1.Video.Detail.Video720;
				}
				string str1 = video720;
				if (!str1.StartsWith("http://"))
				{
					str1 = string.Concat("http://", str1);
				}
				empty = str1;
			}
			return empty;
		}
	}
}