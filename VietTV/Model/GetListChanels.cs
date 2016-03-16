using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietTV.Model
{
    public class GetListChanels : INotifyPropertyChanged
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

        private string _groupname;
        public string groupName
        {
            get
            {
                return _groupname;
            }
            set
            {
                if (this._groupname != value)
                {
                    this._groupname = value;
                    this.RaisePropertyChanged("groupName");
                }
            }
        }

        private ObservableCollection<Chanel> _chanels;
        public ObservableCollection<Chanel> chanels
        {
            get
            {
                return _chanels;
            }
            set
            {
                if (this._chanels != value)
                {
                    this._chanels = value;
                    this.RaisePropertyChanged("chanels");
                }
            }
        }

    }

    //====================================================================

}
