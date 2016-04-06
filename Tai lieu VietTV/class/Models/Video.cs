using System;
using System.Runtime.CompilerServices;

namespace tiviViet.Models
{
	public class Video
	{
		public string Date
		{
			get;
			set;
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

		public string UrlImage
		{
			get;
			set;
		}

		public Video(string url, string urlImage, string title, string date)
		{
			this.Url = url;
			this.UrlImage = urlImage;
			this.Title = title;
			this.Date = date;
		}

		public Video(string url, string image, string title)
		{
			this.Title = title;
			this.Url = url;
			this.UrlImage = image;
		}
	}
}