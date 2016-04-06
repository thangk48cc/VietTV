using System;
using System.Runtime.CompilerServices;

namespace tiviViet.Models
{
	internal class MenuVtv
	{
		public string ChannelName
		{
			get;
			set;
		}

		public string ImageChannel
		{
			get;
			set;
		}

		public string Url
		{
			get;
			set;
		}

		public MenuVtv(string channelname, string imagechannel, string url)
		{
			this.ChannelName = channelname;
			this.ImageChannel = imagechannel;
			this.Url = url;
		}
	}
}