using System;
using System.Runtime.CompilerServices;

namespace tiviViet.Models
{
	internal class ClipFoodball
	{
		public string AwayImage
		{
			get;
			set;
		}

		public string AwayName
		{
			get;
			set;
		}

		public string FLeagueName
		{
			get;
			set;
		}

		public string HomeImage
		{
			get;
			set;
		}

		public string HomeName
		{
			get;
			set;
		}

		public string IsLive
		{
			get
			{
				if (string.IsNullOrEmpty(this.Link))
				{
					return "";
				}
				return "Đang trực tiếp";
			}
		}

		public string Link
		{
			get;
			set;
		}

		public string Name
		{
			get
			{
				return string.Format("{0} - {1}", this.HomeName, this.AwayName);
			}
		}

		public string Score
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

		public string Type
		{
			get;
			set;
		}

		public ClipFoodball()
		{
		}
	}
}