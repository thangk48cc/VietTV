using System;
using System.Runtime.CompilerServices;

namespace tiviViet.Models.Bongda3
{
	public class Focus
	{
		public string Id
		{
			get;
			set;
		}

		public string Image
		{
			get;
			set;
		}

		public string IsLive
		{
			get
			{
				if (string.IsNullOrEmpty(this.Url))
				{
					return "";
				}
				return "Đang trực tiếp";
			}
		}

		public string Name
		{
			get;
			set;
		}

		public DateTime Time
		{
			get;
			set;
		}

		public string TimeDate
		{
			get
			{
				return this.Time.ToString("dd-MM-yyyy");
			}
		}

		public string TimeTime
		{
			get
			{
				return this.Time.ToString("HH:mm");
			}
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

		public Focus()
		{
		}
	}
}