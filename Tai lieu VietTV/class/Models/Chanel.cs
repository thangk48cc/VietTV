using System;
using tiviViet.ViewModels;

namespace tiviViet.Models
{
	public class Chanel : ViewModelBase
	{
		public string chanelName;

		public string icon;

		public string typeChannel;

		public string link;

		public bool fvStatus;

		public string iconcheck;

		public string chanelId;

		public string link2;

		public string link3;

		public string link4;

		public string ChanelId
		{
			get
			{
				return this.chanelId;
			}
			set
			{
				if (this.chanelId != value)
				{
					this.chanelId = value;
					base.OnPropertyChanged("chanelId");
				}
			}
		}

		public string ChanelName
		{
			get
			{
				return this.chanelName;
			}
			set
			{
				if (this.chanelName != value)
				{
					this.chanelName = value;
					base.OnPropertyChanged("chanelName");
				}
			}
		}

		public bool FvStatus
		{
			get
			{
				return this.fvStatus;
			}
			set
			{
				if (this.fvStatus != value)
				{
					this.fvStatus = value;
					base.OnPropertyChanged("FvStatus");
				}
			}
		}

		public string Icon
		{
			get
			{
				return this.icon;
			}
			set
			{
				if (this.icon != value)
				{
					this.icon = value;
					base.OnPropertyChanged("Icon");
				}
			}
		}

		public string Iconcheck
		{
			get
			{
				return this.iconcheck;
			}
			set
			{
				if (this.iconcheck != value)
				{
					this.iconcheck = value;
					base.OnPropertyChanged("Iconcheck");
				}
			}
		}

		public string Link
		{
			get
			{
				return this.link;
			}
			set
			{
				if (this.link != value)
				{
					this.link = value;
					base.OnPropertyChanged("Link");
				}
			}
		}

		public string Link2
		{
			get
			{
				return this.link2;
			}
			set
			{
				if (this.link2 != value)
				{
					this.link2 = value;
					base.OnPropertyChanged("Link2");
				}
			}
		}

		public string Link3
		{
			get
			{
				return this.link3;
			}
			set
			{
				if (this.link3 != value)
				{
					this.link3 = value;
					base.OnPropertyChanged("Link3");
				}
			}
		}

		public string Link4
		{
			get
			{
				return this.link4;
			}
			set
			{
				if (this.link4 != value)
				{
					this.link4 = value;
					base.OnPropertyChanged("Link4");
				}
			}
		}

		public string TypeChannel
		{
			get
			{
				return this.typeChannel;
			}
			set
			{
				if (this.typeChannel != value)
				{
					this.typeChannel = value;
					base.OnPropertyChanged("TypeChannel");
				}
			}
		}

		public Chanel()
		{
		}
	}
}