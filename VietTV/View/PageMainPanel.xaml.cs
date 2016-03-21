using System;
using System.Collections.Generic;
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

        private void stbOpenMenu_Completed(object sender, EventArgs e)
        {
            isOpen = true;
        }

        private bool isOpen = false;

        private void stbCloseMenu_Completed(object sender, EventArgs e)
        {
            isOpen = false;
        }

        private void MenuSetting()
        {
            if (isOpen)
            {
                stbCloseMenu.Begin();
                btnMenuMain.Background = new SolidColorBrush(Color.FromArgb(255, 0, 154, 216));
            }
            else
            {
                stbOpenMenu.Begin();
                btnMenuMain.Background = new SolidColorBrush(Color.FromArgb(255, 79, 184, 229));
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
            if (item.groupId == vm.groupChanelId)
            {
                var chanel = new Chanel();
                chanel.chanelId = "#123";
                chanel.chanelName = "Thêm kênh yêu thích";
                chanel.icon = "/Assets/Images/addFavChanel.png";
                if (vm.chanelsByGroup.Last().chanelId != chanel.chanelId)
                {
                    vm.chanelsByGroup.Add(chanel);
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

        
    }
}