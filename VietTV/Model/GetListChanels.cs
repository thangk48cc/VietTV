﻿using System;
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
        private string _groupid;
        public string groupId
        {
            get
            {
                return _groupid;
            }
            set
            {
                if (this._groupid != value)
                {
                    this._groupid = value;
                    this.RaisePropertyChanged("groupId");
                }
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

        Random rand = new Random();
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
                    _numchannel = this._chanels.Count;
                    this.RaisePropertyChanged("chanels");
                }
            }
        }

        private int _numchannel;
        public int numChannel
        {
            get
            {
                return _numchannel;
            }
            set
            {
                if (this._numchannel != value)
                {
                    this._numchannel = value;
                    this.RaisePropertyChanged("numChannel");
                }
            }
        }

    }

    //====================================================================

}
