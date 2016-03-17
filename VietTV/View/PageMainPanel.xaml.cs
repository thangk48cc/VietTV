using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
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
            }
            else
            {
                stbOpenMenu.Begin();
                btnMenuMain.Background = new SolidColorBrush(Color.FromArgb(255,79,184,229));
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
            var item = (GetListChanels)(sender as Button).DataContext;
            var vm = DataContext as MenuMainVM;
            vm.chanelsByGroup = item.chanels;
            MenuSetting();
        }
    }
}