using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using VietTV.Common;
using VietTV.Model;
using VietTV.ViewModel;

namespace VietTV.View
{
    public partial class PageAddFavorite : PhoneApplicationPage
    {
        public PageAddFavorite()
        {
            InitializeComponent();
            stbCloseMenu.Completed += stbCloseMenu_Completed;
            stbOpenMenu.Completed += stbOpenMenu_Completed;
            lstChanelsLiked = CodePublic.ReadDataFromIsolatedStorage();
            if (lstChanelsLiked == null)
            {
                return;
            }
            var vm = DataContext as MenuMainVM;
            if (vm!=null)
            {
                
                foreach (var chanelLiked in lstChanelsLiked)
                {
                    foreach (var itemChanel in vm.propData.chanelsCollection)
                    {
                        if (chanelLiked.groupName!=null && chanelLiked.groupName.Equals(itemChanel.groupName))
                        {
                            foreach (var chanel in itemChanel.chanels)
                            {
                                if (chanelLiked.chanelId.Equals(chanel.chanelId))
                                {
                                    chanel.isLiked = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            searchChanel(txtSearch.Text);
            base.OnNavigatedTo(e);
        }

        ObservableCollection<Chanel> lstChanelsLiked=new ObservableCollection<Chanel>(); 
        void stbOpenMenu_Completed(object sender, EventArgs e)
        {
            isOpen = true;
        }

        private bool isOpen = false;
        void stbCloseMenu_Completed(object sender, EventArgs e)
        {
            isOpen = false;
        }

        void MenuSetting()
        {
            if (isOpen)
            {
                stbCloseMenu.Begin();
                btnMenuMain.Background = new SolidColorBrush(Color.FromArgb(255, 0, 154, 216));
                grdChePanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                stbOpenMenu.Begin();
                btnMenuMain.Background = new SolidColorBrush(Color.FromArgb(255, 79, 184, 229));
                grdChePanel.Visibility = Visibility.Visible;
            }
        }
        private void BtnMenuMain_OnClick(object sender, RoutedEventArgs e)
        {
            MenuSetting();
        }

        private void BtnItemGroupChanel_OnClick(object sender, RoutedEventArgs e)
        {
            var item = (GetListChanels)(sender as Button).DataContext;
            var vm = DataContext as MenuMainVM;
            vm.groupChanelItem = item;
            if (item.groupId == CodePublic.groupChanelId)
            {
                vm.chanelsByGroup = CodePublic.ReadDataFromIsolatedStorage();
                //while (vm.chanelsByGroup.Contains(vm.chanelFav))
                //{
                //    vm.chanelsByGroup.Remove(vm.chanelFav);
                //}
                //vm.chanelsByGroup.Add(vm.chanelFav);
                if (vm.chanelsByGroup != null)
                {
                    for (int i = 0; i < vm.chanelsByGroup.Count; i++)
                    {
                        if (vm.chanelsByGroup[i].chanelId == vm.chanelFav.chanelId)
                        {
                            vm.chanelsByGroup.Remove(vm.chanelsByGroup[i]);
                        }
                    }
                    vm.chanelsByGroup.Add(vm.chanelFav);
                }
                else
                {
                    vm.chanelsByGroup=new ObservableCollection<Chanel>();
                    vm.chanelsByGroup.Add(vm.chanelFav);
                }
            }
            else
                vm.chanelsByGroup = item.chanels;
            
            MenuSetting();
            NavigationService.GoBack();
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (isOpen)
            {
                MenuSetting();
                e.Cancel = true;
                return;
            }
            var vm = DataContext as MenuMainVM;
            vm.chanelsByGroup = CodePublic.ReadDataFromIsolatedStorage();
            //while (vm.chanelsByGroup.Contains(vm.chanelFav))
            //{
            //    vm.chanelsByGroup.Remove(vm.chanelFav);
            //}
            for (int i = 0; i < vm.chanelsByGroup.Count; i++)
            {
                if (vm.chanelsByGroup[i].chanelId == vm.chanelFav.chanelId)
                {
                    vm.chanelsByGroup.Remove(vm.chanelsByGroup[i]);
                }
            }
            vm.chanelsByGroup.Add(vm.chanelFav);
            base.OnBackKeyPress(e);
        }

        private void BtnChanelItem_OnClick(object sender, RoutedEventArgs e)
        {
            var item = (Chanel) (sender as Button).DataContext;
            if (item!=null)
            {
                if (lstChanelsLiked==null)
                {
                    lstChanelsLiked = new ObservableCollection<Chanel>();
                }
                item.isLiked = !item.isLiked;
                if (item.isLiked)
                {
                    lstChanelsLiked.Add(item);
                    CodePublic.SaveDataToIsolatedStorage(lstChanelsLiked);
                }
                else
                {
                    foreach (var chanel in lstChanelsLiked)
                    {
                        if (chanel.chanelId==item.chanelId)
                        {
                            lstChanelsLiked.Remove(chanel);
                            CodePublic.SaveDataToIsolatedStorage(lstChanelsLiked);
                            break;
                        }
                    }
                }
                 var vm = DataContext as MenuMainVM;
                if(vm!=null) vm.propData.chanelsCollection[0].numChannel = lstChanelsLiked.Count - 1;
                //var lst = CodePublic.ReadDataFromIsolatedStorage();
            }

            //var chanel = CodePublic.ReadDataFromIsolatedStorage();
           // MessageBox.Show(chanel[0].chanelName);
        }

        private void TxtSearch_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            searchChanel(txtSearch.Text);
        }

        private void searchChanel(string text)
        {
            var vm = DataContext as MenuMainVM;
            var query = from mContact in vm.propData.chanelsCollectionInOne
                        where
                           mContact.chanelName.ToLower().Contains(text.ToLower())
                        select mContact;
            var listQuery = new ObservableCollection<Chanel>(query);
            vm.listShowing = CodePublic.getListToBiding(listQuery);
        }
        private void TxtSearch_OnActionIconTapped(object sender, EventArgs e)
        {
            txtSearch.Text = "";
        }

        private void GrdChePanel_OnTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MenuSetting();
        }

        private void BtnBroadcastSchedule_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(
                        new Uri(
                            "/View/PageBroadcastSchedule.xaml",
                            UriKind.RelativeOrAbsolute));
            MenuSetting();
        }
    }
}