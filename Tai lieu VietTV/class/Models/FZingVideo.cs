using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;

namespace tiviViet.Models
{
	public class FZingVideo
	{
		public FZingVideo()
		{
		}

		public class ZingArticleDetailByVideo
		{
			[JsonProperty("response")]
			public FZingVideo.ZingVideo Video
			{
				get;
				set;
			}

			public ZingArticleDetailByVideo()
			{
			}
		}

		public class ZingVideo
		{
			[JsonProperty("other_url")]
			public FZingVideo.ZingVideoDetail Detail
			{
				get;
				set;
			}

			public ZingVideo()
			{
			}
		}

		public class ZingVideoDetail
		{
			public string Video3Gp
			{
				get;
				set;
			}

			public string Video480
			{
				get;
				set;
			}

			public string Video720
			{
				get;
				set;
			}

			public ZingVideoDetail()
			{
			}
		}
	}
}