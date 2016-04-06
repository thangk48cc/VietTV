using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using tiviViet.ViewModels;

namespace tiviViet.Models
{
	public class ChannelTv : ViewModelBase
	{
		public ObservableCollection<Chanel> chanels
		{
			get;
			set;
		}

		public ObservableCollection<Chanel> Chanels
		{
			get
			{
				return this.chanels;
			}
			set
			{
				if ((object)this.chanels != (object)value)
				{
					this.chanels = value;
					base.OnPropertyChanged("chanels");
				}
			}
		}

		public string groupName
		{
			get;
			set;
		}

		public string GroupName
		{
			get
			{
				return this.groupName;
			}
			set
			{
				if (this.groupName != value)
				{
					this.groupName = value;
					base.OnPropertyChanged("groupName");
				}
			}
		}

		public ChannelTv()
		{
		}
	}
}