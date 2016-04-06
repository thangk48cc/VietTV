using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace tiviViet.Models.ServerBongDa3
{
	public class JsonLiveData : INotifyPropertyChanged
	{
		private string _id;

		private string _lg;

		private string _lgName;

		private string _t1;

		private string _t2;

		private string _t1Name;

		private string _t2Name;

		private string _t1Img;

		private string _t2Img;

		private string _st;

		private string _stEn;

		private string _stId;

		private string _sc1;

		private string _sc2;

		private string _chn;

		private string _dateLive;

		private long _time;

		private int _reg;

		public string Chn
		{
			get
			{
				return this._chn;
			}
			set
			{
				if (value == this._chn)
				{
					return;
				}
				this._chn = value;
				this.NotifyPropertyChanged("chn");
			}
		}

		public string DateLive
		{
			get
			{
				return this._dateLive;
			}
			set
			{
				if (value == this._dateLive)
				{
					return;
				}
				this._dateLive = value;
				this.NotifyPropertyChanged("date_live");
			}
		}

		public string Id
		{
			get
			{
				return this._id;
			}
			set
			{
				if (value == this._id)
				{
					return;
				}
				this._id = value;
				this.NotifyPropertyChanged("id");
			}
		}

		public string Lg
		{
			get
			{
				return this._lg;
			}
			set
			{
				if (value == this._lg)
				{
					return;
				}
				this._lg = value;
				this.NotifyPropertyChanged("lg");
			}
		}

		public string LgName
		{
			get
			{
				return this._lgName;
			}
			set
			{
				if (value == this._lgName)
				{
					return;
				}
				this._lgName = value;
				this.NotifyPropertyChanged("lg_name");
			}
		}

		public int Reg
		{
			get
			{
				return this._reg;
			}
			set
			{
				if (value == this._reg)
				{
					return;
				}
				this._reg = value;
				this.NotifyPropertyChanged("reg");
			}
		}

		public string Sc1
		{
			get
			{
				return this._sc1;
			}
			set
			{
				if (value == this._sc1)
				{
					return;
				}
				this._sc1 = value;
				this.NotifyPropertyChanged("sc1");
			}
		}

		public string Sc2
		{
			get
			{
				return this._sc2;
			}
			set
			{
				if (value == this._sc2)
				{
					return;
				}
				this._sc2 = value;
				this.NotifyPropertyChanged("sc2");
			}
		}

		public string St
		{
			get
			{
				return this._st;
			}
			set
			{
				if (value == this._st)
				{
					return;
				}
				this._st = value;
				this.NotifyPropertyChanged("st");
			}
		}

		public string StEn
		{
			get
			{
				return this._stEn;
			}
			set
			{
				if (value == this._stEn)
				{
					return;
				}
				this._stEn = value;
				this.NotifyPropertyChanged("st_en");
			}
		}

		public string StId
		{
			get
			{
				return this._stId;
			}
			set
			{
				if (value == this._stId)
				{
					return;
				}
				this._stId = value;
				this.NotifyPropertyChanged("st_id");
			}
		}

		public string T1
		{
			get
			{
				return this._t1;
			}
			set
			{
				if (value == this._t1)
				{
					return;
				}
				this._t1 = value;
				this.NotifyPropertyChanged("t1");
			}
		}

		public string T1Img
		{
			get
			{
				return this._t1Img;
			}
			set
			{
				if (value == this._t1Img)
				{
					return;
				}
				this._t1Img = value;
				this.NotifyPropertyChanged("t2");
			}
		}

		public string T1Name
		{
			get
			{
				return this._t1Name;
			}
			set
			{
				if (value == this._t1Name)
				{
					return;
				}
				this._t1Name = value;
				this.NotifyPropertyChanged("t1_name");
			}
		}

		public string T2
		{
			get
			{
				return this._t2;
			}
			set
			{
				if (value == this._t2)
				{
					return;
				}
				this._t2 = value;
				this.NotifyPropertyChanged("t2");
			}
		}

		public string T2Img
		{
			get
			{
				return this._t2Img;
			}
			set
			{
				if (value == this._t2Img)
				{
					return;
				}
				this._t2Img = value;
				this.NotifyPropertyChanged("t2");
			}
		}

		public string T2Name
		{
			get
			{
				return this._t2Name;
			}
			set
			{
				if (value == this._t2Name)
				{
					return;
				}
				this._t2Name = value;
				this.NotifyPropertyChanged("t2_name");
			}
		}

		public long Time
		{
			get
			{
				return this._time;
			}
			set
			{
				if (value == this._time)
				{
					return;
				}
				this._time = value;
				this.NotifyPropertyChanged("time");
			}
		}

		public JsonLiveData()
		{
		}

		private void NotifyPropertyChanged(string info)
		{
			if (this.PropertyChanged == null)
			{
				return;
			}
			this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(info));
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}