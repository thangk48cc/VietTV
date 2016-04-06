using System;
using System.Runtime.CompilerServices;

namespace tiviViet.Models
{
	public class ZingArticleDetailByPicture
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

		public string Picture
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

		public ZingArticleDetailByPicture()
		{
		}
	}
}