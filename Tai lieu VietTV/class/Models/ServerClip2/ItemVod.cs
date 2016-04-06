using System;
using System.Runtime.CompilerServices;

namespace tiviViet.Models.ServerClip2
{
	internal class ItemVod
	{
		public string Description
		{
			get;
			set;
		}

		public bool Lock
		{
			get;
			set;
		}

		public int Status
		{
			get;
			set;
		}

		public VodItem Vod
		{
			get;
			set;
		}

		public ItemVod()
		{
		}
	}
}