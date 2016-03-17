using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietTV.Model
{
    public class Chanel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private string _chanelid;
        public string chanelId
        {
            get
            {
                return _chanelid;
            }
            set
            {
                if (this._chanelid != value)
                {
                    this._chanelid = value;
                    this.RaisePropertyChanged("chanelId");
                }
            }
        }
        private string _chanelname;
        public string chanelName
        {
            get
            {
                return _chanelname;
            }
            set
            {
                if (this._chanelname != value)
                {
                    this._chanelname = value;
                    this.RaisePropertyChanged("chanelName");
                }
            }
        }

        private string _icon;
        public string icon
        {
            get
            {
                return _icon;
            }
            set
            {
                if (this._icon != value)
                {
                    this._icon = value;
                    if (chanelId == null) chanelId = "333";
                    if (!chanelId.Equals("#123"))
                        this.icon = "/Assets/Images/todayTV.png";
                    else
                        this.icon = "/Assets/Images/addFavChanel.png";
                    this.RaisePropertyChanged("icon");
                }
            }
        }

        private string _typechannel;
        public string typeChannel
        {
            get
            {
                return _typechannel;
            }
            set
            {
                if (this._typechannel != value)
                {
                    this._typechannel = value;
                    this.RaisePropertyChanged("typeChannel");
                }
            }
        }

        private string _link4;
        public string link4
        {
            get
            {
                return _link4;
            }
            set
            {
                if (this._link4 != value)
                {
                    this._link4 = value;
                    this.RaisePropertyChanged("link4");
                }
            }
        }

        private string _link3;
        public string link3
        {
            get
            {
                return _link3;
            }
            set
            {
                if (this._link3 != value)
                {
                    this._link3 = value;
                    this.RaisePropertyChanged("link3");
                }
            }
        }

        private string _link2;
        public string link2
        {
            get
            {
                return _link2;
            }
            set
            {
                if (this._link2 != value)
                {
                    this._link2 = value;
                    this.RaisePropertyChanged("link2");
                }
            }
        }

        

        private string _link;
        public string link
        {
            get
            {
                return _link;
            }
            set
            {
                if (this._link != value)
                {
                    this._link = value;
                    this.RaisePropertyChanged("link");
                }
            }
        }

    }

    //====================================================================

}
