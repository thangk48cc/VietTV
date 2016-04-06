using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace tiviViet.Models.ServerClip2
{
	internal class ClipVod
	{
		public string Description
		{
			get;
			set;
		}

		public int Status
		{
			get;
			set;
		}

		public List<Vod> Vods
		{
			get;
			set;
		}

		public ClipVod()
		{
		}
	}
}