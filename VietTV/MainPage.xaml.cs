using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json.Linq;
using SettingsPageAnimation.Framework;
using VietTV.Model;
using VietTV.Resources;

namespace VietTV
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            _feContainer = this.Container as FrameworkElement;
            GetSuggestVideo();
        }

        #region======= set up menu ============

        private double widthMenu = 380;
        private double _dragDistanceToOpen = 75.0;

        private double _dragDistanceToClose = 305.0;

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

            grdHide.Visibility = Visibility.Collapsed;
            var trans = _feContainer.GetHorizontalOffset().Transform;

            trans.Animate(trans.X, 0, TranslateTransform.XProperty, 300, 0, new CubicEase

            {

                EasingMode = EasingMode.EaseOut

            });

            _isSettingsOpen = false;

        }

        private void OpenSettings()
        {
            grdHide.Visibility = Visibility.Visible;
            var trans = _feContainer.GetHorizontalOffset().Transform;

            trans.Animate(trans.X, widthMenu, TranslateTransform.XProperty, 300, 0, new CubicEase

            {

                EasingMode = EasingMode.EaseOut

            });

            _isSettingsOpen = true;

        }

        private bool canSlide = true;

        private void GestureListener_OnDragDelta(object sender, DragDeltaGestureEventArgs e)
        {

            if (canSlide == false) return;

            if (e.Direction == System.Windows.Controls.Orientation.Horizontal && e.HorizontalChange > 0 && !_isSettingsOpen)
            {

                double offset = _feContainer.GetHorizontalOffset().Value + e.HorizontalChange;

                if (offset > _dragDistanceToOpen)

                    this.OpenSettings();

                else

                    _feContainer.SetHorizontalOffset(offset);

            }

            if (e.Direction == System.Windows.Controls.Orientation.Horizontal && e.HorizontalChange < 0 && _isSettingsOpen)
            {

                double offsetContainer = _feContainer.GetHorizontalOffset().Value + e.HorizontalChange;

                if (offsetContainer < _dragDistanceToClose)

                    this.CloseSettings();

                else

                    _feContainer.SetHorizontalOffset(offsetContainer);

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

                    this.OpenSettings();

            }

            if (e.Direction == System.Windows.Controls.Orientation.Horizontal && e.HorizontalChange < 0 && _isSettingsOpen)
            {

                if (e.HorizontalChange > _dragDistanceNegative)

                    this.ResetLayoutRoot();

                else

                    this.CloseSettings();

            }

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


        private void AddPage(Object obj)
        {

            //PageAddContent.Children.Clear();


            if (obj is Page)
            {

                var a = obj as Page;

                PageAddContent.Children.Add(a);

            }

            else
            {

                if (obj is UserControl)
                {

                    var a = obj as UserControl;

                    PageAddContent.Children.Add(a);

                }

            }

        }
        private void AddNewPage(Object obj)
        {

            PageAddContent.Children.Clear();

            if (obj is Page)
            {

                var a = obj as Page;

                PageAddContent.Children.Add(a);

            }

            else
            {

                if (obj is UserControl)
                {

                    var a = obj as UserControl;

                    PageAddContent.Children.Add(a);

                }

            }

        }
        #endregion=============================


        private void GrdHide_OnTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            CloseSettings();
        }

        private async void GetSuggestVideo()
        {
            try
            {
                var res = new Common.RepositoryServices();
                GetListData dataChanels = await res.GetDataChanelsTask();
                MessageBox.Show(dataChanels.chanelsCollection.First().groupName);
            }
            catch (Exception exception)
            {
            }
        }

        private void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string t = e.Result.ToString();
                MessageBox.Show(t);
            }
            catch (Exception exception)
            {
            }
        }
        
    }
}