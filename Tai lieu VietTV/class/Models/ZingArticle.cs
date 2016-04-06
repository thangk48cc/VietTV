using System;
using System.Runtime.CompilerServices;

namespace tiviViet.Models
{
	public class ZingArticle
	{
		public string ArticleId
		{
			get;
			set;
		}

		public string Cover
		{
			get;
			set;
		}

		public string DatetimeConvert
		{
			get
			{
				DateTime dateTime = Convert.ToDateTime(this.Time);
				return string.Format("{0} ngày {1}", dateTime.ToString("HH:mm"), dateTime.ToString("dd/MM/yyyy"));
			}
		}

		public string Link
		{
			get;
			set;
		}

		public string Sign
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

		public string Type
		{
			get;
			set;
		}

		public ZingArticle()
		{
		}
	}
}