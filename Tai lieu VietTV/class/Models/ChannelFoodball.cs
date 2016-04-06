using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace tiviViet.Models
{
	public class ChannelFoodball
	{
		public List<DetailsChannel> Channels
		{
			get;
			set;
		}

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

		public ChannelFoodball()
		{
		}
	}
}