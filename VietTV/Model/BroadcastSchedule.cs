using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietTV.Model
{
    public class BroadcastSchedule : INotifyPropertyChanged
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
        private string _time;
        public string time
        {
            get
            {
                return _time;
            }
            set
            {
                if (this._time != value)
                {
                    this._time = value;
                    this.RaisePropertyChanged("time");
                }
            }
        }
        private string _title;
        public string title
        {
            get
            {
                return _title;
            }
            set
            {
                if (this._title != value)
                {
                    this._title = value;
                    this.RaisePropertyChanged("title");
                }
            }
        }

    }
}
