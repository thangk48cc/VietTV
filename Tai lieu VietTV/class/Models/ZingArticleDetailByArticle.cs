using System;
using System.Runtime.CompilerServices;

namespace tiviViet.Models
{
	public class ZingArticleDetailByArticle
	{
		private string _caption;

		public string Caption
		{
			get
			{
				if (!string.IsNullOrEmpty(this._caption))
				{
					this._caption = this._caption.Trim();
				}
				return this._caption;
			}
			set
			{
				this._caption = value;
			}
		}

		public string Content
		{
			get;
			set;
		}

		public string Datetime
		{
			get;
			set;
		}

		public string DatetimeConvert
		{
			get
			{
				DateTime dateTime = Convert.ToDateTime(this.Datetime);
				return string.Format("{0} ngày {1}", dateTime.ToString("HH:mm"), dateTime.ToString("dd/MM/yyyy"));
			}
		}

		public string Image
		{
			get;
			set;
		}

		public string Type
		{
			get;
			set;
		}

		public ZingArticleDetailByArticle()
		{
		}
	}
}