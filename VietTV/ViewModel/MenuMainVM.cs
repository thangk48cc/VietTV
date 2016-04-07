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
                    _chanelsByGroup = this._propData.chanelsCollection.Last().chanels;
                    _groupchanels = CodePublic.groupChanels = value.chanelsCollection;

                    var item = new GetListChanels();
                    item.groupId = groupChanelId;
                    item.groupName = "Kênh yêu thích";
                    item.chanels = this._propData.chanelsCollection.Last().chanels;
                    if (this._propData.chanelsCollection.First().groupId!=item.groupId)
                    this._propData.chanelsCollection.Insert(0, item);
                    groupChanelItem = item;

                    RaisePropertyChanged("propData");
                }
            }
        }
        private ObservableCollection<GetListChanels> _groupchanels = new ObservableCollection<GetListChanels>();
        public ObservableCollection<GetListChanels> groupChanels
        {
            get
            {
                return _groupchanels;
            }
            set
            {
                if (this._groupchanels != value)
                {
                    this._groupchanels = value;
                    this.RaisePropertyChanged("groupChanels");
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
                    //if (this.chanelsByGroup.Last().chanelId != CodePublic.chanelIdAdd)
                    //{
                    //    propData.chanelsCollection[0].numChannel = chanelsByGroup.Count - 1;
                    //}
                    
                    this.RaisePropertyChanged("chanelsByGroup");
                }
            }
        }

        private ObservableCollection<KeyedList<string, Chanel>> _listshowing;
        public ObservableCollection<KeyedList<string, Chanel>> listShowing
        {
            get
            {
                return _listshowing;
            }
            set
            {
                if (this._listshowing != value)
                {
                    this._listshowing = value;

                    this.RaisePropertyChanged("listShowing");
                }
            }
        }

        private Chanel _chaneldetail;
        public Chanel chanelDetail
        {
            get
            {
                return _chaneldetail;
            }
            set
            {
                if (this._chaneldetail != value)
                {
                    this._chaneldetail = value;
                    this.RaisePropertyChanged("chanelDetail");
                }
            }
        }

        public RelayCommand<object> getDataFromServiceCommand { get; set; }
        public MenuMainVM(NavigationHelper _navigation)
        {
            repositoryServices=new RepositoryServices();
            navigationHelper = _navigation;
            getDataFromServiceCommand = new RelayCommand<object>(getDataFromService);
            getDataFromServiceCommand.Execute(null);
        }
        public Chanel chanelFav = new Chanel();
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
                    propData.chanelsCollection.First().chanels.Remove(propData.chanelsCollection.First().chanels.Last());
                groupChanels = propData.chanelsCollection;
                var item1 = propData.chanelsCollection.First();
                groupChanelItem = item1;
                
                var chanel1 = new Chanel();
                chanel1.chanelId = "#123";
                chanel1.chanelName = "Thêm kênh yêu thích";
                chanel1.icon = "/Assets/Images/addFavChanel.png";
                chanelFav = chanel1;
                chanelsByGroup = CodePublic.ReadDataFromIsolatedStorage();
                if (chanelsByGroup == null)
                    propData.chanelsCollection[0].numChannel = 0;
                else
                    propData.chanelsCollection[0].numChannel = chanelsByGroup.Count-1;
                if (chanelsByGroup==null)
                {
                     chanelsByGroup=new ObservableCollection<Chanel>();
                     chanelsByGroup.Add(chanel1);
                }
                else
                {
                        for (int i = 0; i < chanelsByGroup.Count; i++)
                        {
                            if (chanelsByGroup[i].chanelId == chanelFav.chanelId)
                            {
                                chanelsByGroup.Remove(chanelsByGroup[i]);
                            }
                        }
                        chanelsByGroup.Add(chanelFav);
                }
                int indexId = 0;
                foreach (var item in propData.chanelsCollection)
                {
                    indexId++;
                    var query1 = item.chanels.Select(x =>
                    {
                        x.groupName = item.groupName;
                        x.groupId = indexId;
                        if (!x.icon.Contains(".jpg")) x.icon = x.icon + ".jpg";
                        return x;
                    });
                    var lstChanel = new ObservableCollection<Chanel>(query1);
                    foreach (var chanel in lstChanel)
                    {
                        propData.chanelsCollectionInOne.Add(chanel);
                    }
                }
                var lst = propData.chanelsCollectionInOne;
                listShowing = CodePublic.getListToBiding(lst);
            }
        }
    }
}
