using System;
using System.Runtime.CompilerServices;

namespace tiviViet.Models
{
	internal class SuggestVideo
	{
		public int ChannelId
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public int ZoneParentId
		{
			get;
			set;
		}

		public int ZoneVideoId
		{
			get;
			set;
		}

		public SuggestVideo()
		{
		}
	}
}