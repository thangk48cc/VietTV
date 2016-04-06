using System;
using System.Runtime.CompilerServices;

namespace tiviViet.Models.ServerBongda2
{
	internal class YoSportMatch
	{
		private string _leagues;

		private string _thumb1;

		private string _thumb2;

		private string _thumb;

		public string Id
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

		public string League
		{
			get;
			set;
		}

		public string Leagues
		{
			get
			{
				if (!string.IsNullOrEmpty(this.League))
				{
					return this.League;
				}
				return this._leagues;
			}
			set
			{
				this._leagues = value;
			}
		}

		public string Name
		{
			get;
			set;
		}

		public DateTime StartTime
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

		public string Thumb1
		{
			get
			{
				return this._thumb1;
			}
			set
			{
				this._thumb1 = value.Replace("http://yosport.vnhttp://yosport.vn", "http://yosport.vn");
			}
		}

		public string Thumb2
		{
			get
			{
				return this._thumb2;
			}
			set
			{
				this._thumb2 = value.Replace("http://yosport.vnhttp://yosport.vn", "http://yosport.vn");
			}
		}

		public string Url
		{
			get;
			set;
		}

		public YoSportMatch()
		{
		}
	}
}