using System;
using System.Runtime.CompilerServices;

namespace tiviViet.Models.BongDa101
{
	public class ChannelTructiep
	{
		public string desc
		{
			get;
			set;
		}

		public string img1
		{
			get;
			set;
		}

		public string img2
		{
			get;
			set;
		}

		public string IsLive
		{
			get
			{
				if (string.IsNullOrEmpty(this.stream))
				{
					return "";
				}
				return "Đang trực tiếp";
			}
		}

		public string name1
		{
			get;
			set;
		}

		public string name2
		{
			get;
			set;
		}

		public string NameMath
		{
			get
			{
				return string.Concat(this.name1, " - ", this.name2);
			}
		}

		public string stream
		{
			get;
			set;
		}

		public string time
		{
			get;
			set;
		}

		public ChannelTructiep()
		{
		}
	}
}