using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;
using VietTV.Common;
using VietTV.Model;
using VietTV.Services;

namespace VietTV.ViewModel
{
    public class MenuMainVM : ViewModelBase
    {
        private RepositoryServices repositoryServices;
        private NavigationHelper navigationHelper;

        private bool _isloading = true;
        public bool isLoading
        {
            get
            {
                return _isloading;
            }
            set
            {
                if (this._isloading != value)
                {
                    this._isloading = value;
                    this.RaisePropertyChanged("isLoading");
                }
            }
        }

        private GetListData _propData = new GetListData();
        public GetListData propData
        {
            get { return _propData; }
            set
            {
                if (_propData != value)
                {
                    _propData = value;
                    _chanelsByGroup = _propData.chanelsCollection[2].chanels;

                    var item = new GetListChanels();
                    item.groupId = groupChanelId;
                    item.groupName = "Kênh yêu thích";
                    item.chanels = this._propData.chanelsCollection.Last().chanels;
                    if (this._propData.chanelsCollection.First().groupId!=item.groupId)
                    this._propData.chanelsCollection.Insert(0, item);
                    RaisePropertyChanged("propData");
                }
            }
        }
        private GetListChanels _groupchanelitem=new GetListChanels();
        public GetListChanels groupChanelItem
        {
            get
            {
                return _groupchanelitem;
            }
            set
            {
                if (this._groupchanelitem != value)
                {
                    this._groupchanelitem = value;
                    this.RaisePropertyChanged("groupChanelItem");
                }
            }
        }

        public string groupChanelId = "channelFav";
        private ObservableCollection<Chanel> _chanelsByGroup;
        public ObservableCollection<Chanel> chanelsByGroup
        {
            get
            {
                return _chanelsByGroup;
            }
            set
            {
                if (this._chanelsByGroup != value)
                {
                    this._chanelsByGroup = value;
                    
                    //var chanel = new Chanel();
                    //chanel.chanelId = "#123";
                    //chanel.chanelName = "Thêm kênh yêu thích";
                    //chanel.icon = "/Assets/Images/addFavChanel.png";
                    //if (this.chanelsByGroup.Last().chanelId!=chanel.chanelId)
                    //{
                    //    this.chanelsByGroup.Add(chanel);
                    //}

                    this.RaisePropertyChanged("chanelsByGroup");
                }
            }
        }
        public RelayCommand<object> getDataFromServiceCommand { get; set; }
        public MenuMainVM(NavigationHelper _navigation)
        {
            repositoryServices=new RepositoryServices();
            navigationHelper = _navigation;
            getDataFromServiceCommand = new RelayCommand<object>(getDataFromService);
        }

        private async void getDataFromService(object obj)
        {
            isLoading = true;
            try
            {
                propData = await repositoryServices.GetDataChanelsTask();

            }
            catch (Exception ex)
            {
                propData = repositoryServices.GetResulTask();
            }
            isLoading = false;
            if (propData!=null)
            {
                chanelsByGroup = propData.chanelsCollection[0].chanels;
            }
        }
    }
}
