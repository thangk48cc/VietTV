using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietTV.Model
{
    public class GetListNotFreePlus : INotifyPropertyChanged
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

        private bool _status;
        public bool status
        {
            get
            {
                return _status;
            }
            set
            {
                if (this._status != value)
                {
                    this._status = value;
                    this.RaisePropertyChanged("status");
                }
            }
        }

        private ObservableCollection<NotFreePlus> _data;
        public ObservableCollection<NotFreePlus> data
        {
            get
            {
                return _data;
            }
            set
            {
                if (this._data != value)
                {
                    this._data = value;
                    this.RaisePropertyChanged("data");
                }
            }
        }

        private int _is_login;
        public int is_login
        {
            get
            {
                return _is_login;
            }
            set
            {
                if (this._is_login != value)
                {
                    this._is_login = value;
                    this.RaisePropertyChanged("is_login");
                }
            }
        }

    }

    //====================================================================

}
