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
using HtmlAgilityPack;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using VietTV.Common;
using VietTV.Model;
using VietTV.ViewModel;

namespace VietTV.View
{
    public partial class PageMainPanel : PhoneApplicationPage
    {
        public PageMainPanel()
        {
            InitializeComponent();
            var vm = DataContext as MenuMainVM;
            vm.getDataFromServiceCommand.Execute(null);
            stbCloseMenu.Completed += stbCloseMenu_Completed;
            stbOpenMenu.Completed += stbOpenMenu_Completed;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void stbOpenMenu_Completed(object sender, EventArgs e)
        {
            isOpen = true;
        }

        private bool isOpen = false;

        private void stbCloseMenu_Completed(object sender, EventArgs e)
        {
            isOpen = false;
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (isOpen)
            {
                MenuSetting();
                e.Cancel = true;
                return;
            }
            if (MessageBox.Show("Bạn chắc chắn muốn thoát khỏi ứng dụng?","Thoát",MessageBoxButton.OKCancel)==MessageBoxResult.Cancel)
            {
                e.Cancel = true;
                return;
            }
            base.OnBackKeyPress(e);
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

        private void lstChanelsMenu_SelectedChange(object sender, SelectionChangedEventArgs e)
        {
            var item = (GetListChanels) sender;
            var vm = DataContext as MenuMainVM;
            vm.chanelsByGroup = item.chanels;
        }

        private void BtnItemGroupChanel_OnClick(object sender, RoutedEventArgs e)
        {
            var item = (GetListChanels) (sender as Button).DataContext;
            var vm = DataContext as MenuMainVM;
            vm.groupChanelItem = item;
            vm.chanelsByGroup = item.chanels;
                if (vm!=null && vm.chanelsByGroup!=null )
                {
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
                            vm.chanelsByGroup = new ObservableCollection<Chanel>();
                            vm.chanelsByGroup.Add(vm.chanelFav);
                        }
                    }
                }
            MenuSetting();
        }
        
        private void BtnItemChanel_OnClick(object sender, RoutedEventArgs e)
        {
            var item = (Chanel) (sender as Button).DataContext;
            if (item != null)
            {
                if (item.chanelId == CodePublic.chanelIdAdd)
                {
                    NavigationService.Navigate(new Uri("/View/PageAddFavorite.xaml", UriKind.RelativeOrAbsolute));
                }
                else
                {
                    var vm = DataContext as MenuMainVM;
                    vm.chanelDetail = item;
                    (App.Current as App).chanelDetail = item;
                    NavigationService.Navigate(
                        new Uri(
                            "/View/PageChanelDeail.xaml?chanelId=" + item.chanelId + "&chanelName = " + item.chanelName,
                            UriKind.RelativeOrAbsolute));
                }
            }

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