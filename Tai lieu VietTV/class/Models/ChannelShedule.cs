using System;
using System.Runtime.CompilerServices;

namespace tiviViet.Models
{
	public class ChannelShedule
	{
		public DetailsChannel Channel
		{
			get;
			set;
		}

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

		public ChannelShedule()
		{
		}
	}
}