using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietTV.Model
{
    public class GetListData : INotifyPropertyChanged
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
        private ObservableCollection<GetListChanels> _chanelsCollection = new ObservableCollection<GetListChanels>();
        public ObservableCollection<GetListChanels> chanelsCollection
        {
            get
            {
                return _chanelsCollection;
            }
            set
            {
                if (this._chanelsCollection != value)
                {
                    this._chanelsCollection = value;
                    
                    this.RaisePropertyChanged("chanelsCollection");
                }
            }
        }

        private ObservableCollection<Chanel> _chanelsCollectionInOne = new ObservableCollection<Chanel>();
        public ObservableCollection<Chanel> chanelsCollectionInOne
        {
            get
            {
                return _chanelsCollectionInOne;
            }
            set
            {
                if (this._chanelsCollectionInOne != value)
                {
                    this._chanelsCollectionInOne = value;
                    this.RaisePropertyChanged("chanelsCollectionInOne");
                }
            }
        }
    }
}
