using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SettingsPageAnimation.Framework;
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
            _feContainer = this.Container as FrameworkElement;
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
            if(vm.chanelsByGroup!=null)
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
                if(vm!=null) vm.propData.chanelsCollection[0].numChannel = lstChanelsLiked.Count==1? 1 : lstChanelsLiked.Count - 1;
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

        #region==========menu===============
        private bool canSlide = true;
        private void GestureListener_OnDragDelta(object sender, DragDeltaGestureEventArgs e)
        {

            if (canSlide == false) return;

            if (e.Direction == System.Windows.Controls.Orientation.Horizontal && e.HorizontalChange > 0 && !_isSettingsOpen)
            {

                double offset = _feContainer.GetHorizontalOffset().Value + e.HorizontalChange;

                if (offset > _dragDistanceToOpen)
                    MenuSetting();
                // this.OpenSettings();

                // else

                //  _feContainer.SetHorizontalOffset(offset);

            }

            if (e.Direction == System.Windows.Controls.Orientation.Horizontal && e.HorizontalChange < 0 && _isSettingsOpen)
            {

                double offsetContainer = _feContainer.GetHorizontalOffset().Value + e.HorizontalChange;

                if (offsetContainer < _dragDistanceToClose)

                    //this.CloseSettings();
                    MenuSetting();
                //  else

                // _feContainer.SetHorizontalOffset(offsetContainer);

            }

        }

        private void GestureListener_OnDragCompleted(object sender, DragCompletedGestureEventArgs e)
        {

            if (canSlide == false) return;

            if (e.Direction == System.Windows.Controls.Orientation.Horizontal && e.HorizontalChange > 0 && !_isSettingsOpen)
            {

                if (e.HorizontalChange < _dragDistanceToOpen)

                    this.ResetLayoutRoot();

                else
                    MenuSetting();
                // this.OpenSettings();

            }

            if (e.Direction == System.Windows.Controls.Orientation.Horizontal && e.HorizontalChange < 0 && _isSettingsOpen)
            {

                if (e.HorizontalChange > _dragDistanceNegative)

                    this.ResetLayoutRoot();

                else
                    MenuSetting();
                // this.CloseSettings();

            }

        }
        private double widthMenu = 480;
        private double _dragDistanceToOpen = 75.0;

        private double _dragDistanceToClose = 405.0;

        private double _dragDistanceNegative = -75.0;

        private FrameworkElement _feContainer;

        private bool _isSettingsOpen = false;

        private void TapOCMenu()
        {

            if (_isSettingsOpen)
            {

                canSlide = false;

                CloseSettings();

            }

            else
            {

                canSlide = true;

                OpenSettings();

            }

        }

        private void CloseSettings()
        {

            //grdHide.Visibility = Visibility.Collapsed;
            var trans = _feContainer.GetHorizontalOffset().Transform;

            trans.Animate(trans.X, 0, TranslateTransform.XProperty, 480, 0, new CubicEase

            {

                EasingMode = EasingMode.EaseOut

            });

            _isSettingsOpen = false;

        }

        private void OpenSettings()
        {
            //grdHide.Visibility = Visibility.Visible;
            var trans = _feContainer.GetHorizontalOffset().Transform;

            trans.Animate(trans.X, widthMenu, TranslateTransform.XProperty, 480, 0, new CubicEase

            {

                EasingMode = EasingMode.EaseOut

            });

            _isSettingsOpen = true;

        }
        private void SettingsStateGroup_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {

            ResetLayoutRoot();

        }

        private void ResetLayoutRoot()
        {

            if (!_isSettingsOpen)

                _feContainer.SetHorizontalOffset(0.0);

            else

                _feContainer.SetHorizontalOffset(widthMenu);

        }

        #endregion
    }
}