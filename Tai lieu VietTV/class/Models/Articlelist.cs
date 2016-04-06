using System;
using System.Runtime.CompilerServices;
using tiviViet.ViewModels;

namespace tiviViet.Models
{
	public class Articlelist
	{
		public string BaomoiUrl
		{
			get;
			set;
		}

		public int Comments
		{
			get;
			set;
		}

		public int ContentId
		{
			get;
			set;
		}

		public string ContentUrl
		{
			get;
			set;
		}

		public string Converdate
		{
			get
			{
				return ConverTime.DateTimeToString(this.Date);
			}
		}

		public double Date
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public int HasImage
		{
			get;
			set;
		}

		public string Images
		{
			get;
			set;
		}

		public string LandscapeAvatar
		{
			get;
			set;
		}

		public int LandscapeAvatarHeight
		{
			get;
			set;
		}

		public int LandscapeAvatarWidth
		{
			get;
			set;
		}

		public int ListId
		{
			get;
			set;
		}

		public string ListName
		{
			get;
			set;
		}

		public string ListType
		{
			get;
			set;
		}

		public string Nguon
		{
			get
			{
				return string.Concat("(", this.SourceName, ")");
			}
		}

		public string PortraitAvatar
		{
			get;
			set;
		}

		public int PortraitAvatarHeight
		{
			get;
			set;
		}

		public int PortraitAvatarWidth
		{
			get;
			set;
		}

		public string ShortBody
		{
			get;
			set;
		}

		public int SourceId
		{
			get;
			set;
		}

		public string SourceName
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public int ZoneId
		{
			get;
			set;
		}

		public string ZoneName
		{
			get;
			set;
		}

		public Articlelist()
		{
		}
	}
}