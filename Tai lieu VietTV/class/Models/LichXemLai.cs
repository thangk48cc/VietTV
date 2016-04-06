using System;
using System.Runtime.CompilerServices;

namespace tiviViet.Models
{
	public class LichXemLai
	{
		public string Desc
		{
			get;
			set;
		}

		public string IconTv
		{
			get;
			set;
		}

		public string IconVideo
		{
			get;
			set;
		}

		public string Time
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public string Url
		{
			get;
			set;
		}

		public LichXemLai(string time, string title, string desc, string url, string icontv, string iconvideo)
		{
			this.Time = time;
			this.Title = title;
			this.Desc = desc;
			this.Url = url;
			this.IconTv = icontv;
			this.IconVideo = iconvideo;
		}
	}
}