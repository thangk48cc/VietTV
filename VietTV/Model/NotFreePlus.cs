using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietTV.Model
{
    public class NotFreePlus : INotifyPropertyChanged
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

        private string _rank_id;
        public string rank_id
        {
            get
            {
                return _rank_id;
            }
            set
            {
                if (this._rank_id != value)
                {
                    this._rank_id = value;
                    this.RaisePropertyChanged("rank_id");
                }
            }
        }

        private string _type;
        public string type
        {
            get
            {
                return _type;
            }
            set
            {
                if (this._type != value)
                {
                    this._type = value;
                    this.RaisePropertyChanged("type");
                }
            }
        }

        private string _url;
        public string url
        {
            get
            {
                return _url;
            }
            set
            {
                if (this._url != value)
                {
                    this._url = value;
                    this.RaisePropertyChanged("url");
                }
            }
        }

    }

    //====================================================================

}
