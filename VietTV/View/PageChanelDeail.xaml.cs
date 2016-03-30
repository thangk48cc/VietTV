using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using HtmlAgilityPack;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sen.HTMLParser;
using SM.Media;
using SM.Media.Utility;
using SM.Media.Web;
using VietTV.Common;
using VietTV.Model;
using VietTV.ViewModel;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace VietTV.View
{
    public partial class PageChanelDeail : PhoneApplicationPage
    {
        public PageChanelDeail()
        {
            InitializeComponent();
            //this.DataContext = (App.Current as App).chanelDetail;
            stbCloseMenu.Completed += stbCloseMenu_Completed;
            stbOpenMenu.Completed += stbOpenMenu_Completed;
            //loadPageHTML();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += timer_Tick;
            timer.Start();

            timerAutoOut.Interval = TimeSpan.FromSeconds(10);
            timerAutoOut.Tick += timerAutoOut_Tick;
            timerAutoOut.Start();
            grdBackgroundPlayer.Visibility = Visibility.Collapsed;
            btnZoomPlayer.Visibility = Visibility.Collapsed;

           // this.Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            string link = "http://htvonline.com.vn/livetv/htv7-34336E61.html";
            WebClient codeSampleReq = new WebClient();
            codeSampleReq.DownloadStringCompleted += codeSampleReq_DownloadStringCompleted;
            codeSampleReq.DownloadStringAsync(new Uri(link));
            //CodeSamples.ItemsSource = codes;
        }

        private void codeSampleReq_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string link = "http://code.msdn.microsoft.com/";
            try
            {
                HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.OptionFixNestedTags = true;
                htmlDoc.LoadHtml(e.Result);
                HtmlNode divContainer = htmlDoc.GetElementbyId("div_schedule");
                if (divContainer != null)
                {
                    HtmlNodeCollection nodes = divContainer.SelectNodes("//div/div/ul/li");
                    var data =
                        htmlDoc.DocumentNode.SelectSingleNode("//script[contains(text(), 'Blablabla')]").InnerHtml;
                    foreach (HtmlNode trNode in nodes)
                    {
                        BroadcastSchedule newSchedule = new BroadcastSchedule();
                        HtmlNode time = trNode.SelectSingleNode("p/b");
                        Console.WriteLine(time);
                        CodeSample newSample = new CodeSample();
                        HtmlNode titleNode = trNode.SelectSingleNode("p/span");
                        if (titleNode != null)
                        {
                            newSample.Title = titleNode.InnerHtml.Trim();
                        }

                        HtmlNode summaryNode = trNode.SelectSingleNode("td[@class='itemBody']/div/div[@class='customcontribution']/a");
                        if (summaryNode != null)
                        {
                            newSample.Summary = summaryNode.InnerHtml.Trim();
                        }
                        else
                        {
                            summaryNode = trNode.SelectSingleNode("td[@class='itemBody']/div/a[@class='profile-usercard-hover']");

                        }
                        if (summaryNode != null)
                        {
                            newSample.Summary = summaryNode.InnerHtml.Trim();
                        }
                        HtmlNode descNode = trNode.SelectSingleNode("td[@class='itemBody']/div[@class='summaryBox']");
                        if (descNode != null)
                        {
                            newSample.Description = descNode.InnerHtml.Trim();
                        }

                        HtmlNode divTech = trNode.SelectSingleNode("td[@class='itemBody']/div/div/div[@id='Technologies']");
                        if (divTech != null)
                        {
                            StringBuilder techNames = new StringBuilder();
                            foreach (HtmlNode techAnchor in divTech.ChildNodes)
                            {
                                if (techAnchor.Name.StartsWith("a"))
                                {
                                    techNames.Append(techAnchor.InnerHtml.Trim() + " ; ");
                                }
                            }
                            newSample.Technologies = techNames.ToString();
                        }
                        if (newSample.Technologies != null)
                        {
                            if (newSample.Technologies.ToString().ToLower().Contains("windows phone"))
                            {
                                newSample.ImageUrl = "/images/wp.jpg";
                            }
                            else if (newSample.Technologies.ToString().ToLower().Contains("windows store"))
                            {
                                newSample.ImageUrl = "/images/w8.png";
                            }
                            else if (newSample.Technologies.ToString().ToLower().Contains("visual studio"))
                            {
                                newSample.ImageUrl = "/images/vs.png";
                            }
                            else if (newSample.Technologies.ToString().ToLower().Contains("asp.net"))
                            {
                                newSample.ImageUrl = "/images/aspnet.png";
                            }
                            else
                            {
                                newSample.ImageUrl = "/images/net.jpg";
                            }
                        }
                        codes.Add(newSample);
                    }
                }
                var list = codes;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to download" + ex.Message);
            }
        }

        private bool isLandscape = false;
        void MainPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            TiviMediaElement.Stretch = Stretch.Fill;
            if (e.Orientation == PageOrientation.LandscapeLeft || e.Orientation == PageOrientation.LandscapeRight)
            {
                grdMenuLeft.Visibility =  Visibility.Collapsed;
                tbNameChanel.Visibility = Visibility.Visible;
                isLandscape = true;
                btnUnZoomPlayer.Visibility = Visibility.Collapsed;
                    panelPlayTivi.Height = Application.Current.Host.Content.ActualWidth;
                    panelPlayTivi.Width = Application.Current.Host.Content.ActualHeight;
                    panelPlayTivi.Margin = new Thickness(0, 0, 0, 0);
                    TiviMediaElement.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                    TiviMediaElement.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                    SystemTray.IsVisible = false;  
            }
            else
            {
                tbNameChanel.Visibility = Visibility.Collapsed;
                btnUnZoomPlayer.Visibility = Visibility.Visible;
                isLandscape = false;
                if (isZoom)
                {
                    grdMenuLeft.Visibility = Visibility.Collapsed;
                    panelPlayTivi.Height = 270;
                    panelPlayTivi.Width = Application.Current.Host.Content.ActualWidth;
                    panelPlayTivi.Margin = new Thickness(0, 0, 0, 0);
                    panelPlayTivi.VerticalAlignment = VerticalAlignment.Center;
                    TiviMediaElement.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                    TiviMediaElement.Stretch = Stretch.Fill;
                    TiviMediaElement.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

                    SystemTray.IsVisible = false;
                    grdBackgroundPlayer.Visibility = Visibility.Visible;
                }
                else
                {
                    grdMenuLeft.Visibility = Visibility.Visible;
                    panelPlayTivi.VerticalAlignment = VerticalAlignment.Top;
                    panelPlayTivi.Height = 270;
                    panelPlayTivi.Margin = new Thickness(0, 50, 0, 0);
                    //TiviMediaElement.Width = Double.NaN;
                    panelPlayTivi.Width = Application.Current.Host.Content.ActualWidth;
                    TiviMediaElement.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                    TiviMediaElement.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                    SystemTray.IsVisible = true;
                }
                
            }
        }
        ObservableCollection<CodeSample> codes = new ObservableCollection<CodeSample>();
        async void loadPageHTML()
        {
            string link = "http://code.msdn.microsoft.com/";
            try
            {
                HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.OptionFixNestedTags = true;
                htmlDoc.LoadHtml(link);
                HtmlNode divContainer = htmlDoc.GetElementbyId("directoryItems");
                if (divContainer != null)
                {
                    HtmlNodeCollection nodes = divContainer.SelectNodes("//table/tr");
                    foreach (HtmlNode trNode in nodes)
                    {
                        CodeSample newSample = new CodeSample();
                        HtmlNode titleNode = trNode.SelectSingleNode("td[@class='itemBody']/div[@class='itemTitle']/a");
                        if (titleNode != null)
                        {
                            newSample.Title = titleNode.InnerHtml.Trim();
                        }

                        HtmlNode summaryNode = trNode.SelectSingleNode("td[@class='itemBody']/div/div[@class='customcontribution']/a");
                        if (summaryNode != null)
                        {
                            newSample.Summary = summaryNode.InnerHtml.Trim();
                        }
                        else
                        {
                            summaryNode = trNode.SelectSingleNode("td[@class='itemBody']/div/a[@class='profile-usercard-hover']");

                        }
                        if (summaryNode != null)
                        {
                            newSample.Summary = summaryNode.InnerHtml.Trim();
                        }
                        HtmlNode descNode = trNode.SelectSingleNode("td[@class='itemBody']/div[@class='summaryBox']");
                        if (descNode != null)
                        {
                            newSample.Description = descNode.InnerHtml.Trim();
                        }

                        HtmlNode divTech = trNode.SelectSingleNode("td[@class='itemBody']/div/div/div[@id='Technologies']");
                        if (divTech != null)
                        {
                            StringBuilder techNames = new StringBuilder();
                            foreach (HtmlNode techAnchor in divTech.ChildNodes)
                            {
                                if (techAnchor.Name.StartsWith("a"))
                                {
                                    techNames.Append(techAnchor.InnerHtml.Trim() + " ; ");
                                }
                            }
                            newSample.Technologies = techNames.ToString();
                        }
                        if (newSample.Technologies != null)
                        {
                            if (newSample.Technologies.ToString().ToLower().Contains("windows phone"))
                            {
                                newSample.ImageUrl = "/images/wp.jpg";
                            }
                            else if (newSample.Technologies.ToString().ToLower().Contains("windows store"))
                            {
                                newSample.ImageUrl = "/images/w8.png";
                            }
                            else if (newSample.Technologies.ToString().ToLower().Contains("visual studio"))
                            {
                                newSample.ImageUrl = "/images/vs.png";
                            }
                            else if (newSample.Technologies.ToString().ToLower().Contains("asp.net"))
                            {
                                newSample.ImageUrl = "/images/aspnet.png";
                            }
                            else
                            {
                                newSample.ImageUrl = "/images/net.jpg";
                            }
                        }
                        codes.Add(newSample);
                    }
                }
                var list = codes;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to download" + ex.Message);
            }
        }

        private String linkVideo = "";
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
        private void GrdChePanel_OnTap(object sender, GestureEventArgs e)
        {
            MenuSetting();
        }
        private void BtnMenuMain_OnClick(object sender, RoutedEventArgs e)
        {
            MenuSetting();
        }

        private void BtnItemGroupChanel_OnClick(object sender, RoutedEventArgs e)
        {
            //var item = (GetListChanels)(sender as Button).DataContext;
            //var vm = DataContext as MenuMainVM;
            //vm.groupChanelItem = item;
            //vm.chanelsByGroup = item.chanels;
            //MenuSetting();
            //NavigationService.GoBack();

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
                    vm.chanelsByGroup = new ObservableCollection<Chanel>();
                    vm.chanelsByGroup.Add(vm.chanelFav);
                }
            }
            else
                vm.chanelsByGroup = item.chanels;

            MenuSetting();
            NavigationService.GoBack();
        }

        private bool isZoom = false;
        private void BtnZoomPlayer_OnClick(object sender, RoutedEventArgs e)
        {
            if (isZoom)
            {
                grdMenuLeft.Visibility = Visibility.Visible;
                tbChanel.Visibility = Visibility.Collapsed;
                if (isLandscape)
                {
                    return;
                }
                panelPlayTivi.VerticalAlignment = VerticalAlignment.Top;
                TiviMediaElement.Stretch = Stretch.Fill;
                panelPlayTivi.Height = 270;
                panelPlayTivi.Width = Application.Current.Host.Content.ActualWidth;
                panelPlayTivi.Margin = new Thickness(0, 50, 0, 0);
                TiviMediaElement.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                TiviMediaElement.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                grdBackgroundPlayer.Visibility = Visibility.Collapsed;
                isZoom = false;
                SystemTray.IsVisible = true;
            }
            else
            {
                tbChanel.Visibility = Visibility.Visible;
                grdMenuLeft.Visibility = Visibility.Collapsed;
                if (isLandscape)
                {
                    return;
                }
                panelPlayTivi.Height = 270;
                panelPlayTivi.Margin = new Thickness(0, 0, 0, 0);
                panelPlayTivi.VerticalAlignment = VerticalAlignment.Center;
                TiviMediaElement.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                TiviMediaElement.Stretch = Stretch.Fill;
                TiviMediaElement.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

                SystemTray.IsVisible = false;
                grdBackgroundPlayer.Visibility = Visibility.Visible;
                isZoom = true;
            }
            

            //stbFullScreen.Begin();
           // NavigationService.Navigate(new Uri("/View/PlayerPage.xaml?linkVideo="+linkVideo, UriKind.RelativeOrAbsolute));
        }

        private string chanelId = "";
        private string chanelName = "";
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("chanelId"))
            {
                chanelId = NavigationContext.QueryString["chanelId"];
                tbTitle.Tag = chanelId;
            }
            chanelName = (App.Current as App).chanelDetail.chanelName.Trim();
            tbTitle.Text = chanelName.ToUpper();
            
            if (e.NavigationMode == NavigationMode.New)
            {
                GetDirectLink((App.Current as App).chanelDetail.link);
                //_streamLink = (App.Current as App).chanelDetail.link;  //"http://live.kenhitv.vn:1935/liveweb/itv_web_500k.stream/playlist.m3u8";//"http://vp.xemtvhd.com/chn/vtc1/v.m3u8";
                //_streamLink = NavigationContext.QueryString["linkVideo"];
                InitMediaPlayer();
            }
            else
            {
                //_streamLink = "http://live.kenhitv.vn:1935/liveweb/itv_web_500k.stream/playlist.m3u8";
                InitMediaPlayer();
            }
            base.OnNavigatedTo(e);
        }
        
        #region============player=================
        void timerAutoOut_Tick(object sender, EventArgs e)
        {
            if (isOpenedMedia == false)
            {
                //if (PopupManager.popup.IsOpen)
                //{
                //    PopupManager.popup.IsOpen = false;
                //    //return;
                //}
                //ToastManage.Show("Có lỗi xảy ra trong quá trình xử lý, vui lòng thử lại sau!");
                timerAutoOut.Stop();
                timerAutoOut = null;
                //if (NavigationService.CanGoBack)
                //    NavigationService.GoBack();
            }
            else
            {
                timerAutoOut.Stop();
                timerAutoOut = null;
            }
        }
        DispatcherTimer timerAutoOut = new DispatcherTimer();

        static readonly IApplicationInformation ApplicationInformation = ApplicationInformationFactory.Default;
        IMediaElementManager _mediaElementManager;
        private DispatcherTimer _positionTimer;
        private IMediaStreamFascade _mediaStreamFascade;
        //private TimeSpan _previousPosition;
        private IHttpClients _httpClients;
        private string _streamLink = string.Empty;
        #region===========TAP - show or hide==============
        private bool show = false;
        private void ShowOrCloseControl()
        {
            if (show)
            {
                stbCloseControls.Begin();
                show = false;
            }
            else
            {
                stbOpenControls.Begin();
                show = true;
            }
        }
        DispatcherTimer timer = new DispatcherTimer();
        private void MdaMain_OnTap(object sender, GestureEventArgs e)
        {
            timer.Start();
            ShowOrCloseControl();
        }
        void timer_Tick(object sender, EventArgs e)
        {
            if (show)
                ShowOrCloseControl();
            timer.Stop();
        }
        private void grid1_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            show = true;
            timer.Stop();
        }

        private void grid1_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            show = true;
            timer.Start();
        }
        #endregion========================================
        private string type = "TVSHOW";
        private void BtnInfo_OnClick(object sender, RoutedEventArgs e)
        {
            // AboutMediaPlaying ab=new AboutMediaPlaying(type1);
            // PopupManager.ShowAboutInfo(type,"0");
        }
        private double volum = 0;
        private void BtnMute_OnClick(object sender, RoutedEventArgs e)
        {
            string ul = "/Assets/Icon/video_volum.png";
            if (TiviMediaElement.IsMuted)
            {
                ul = "/Assets/Icon/video_volum.png";
                VolumeSlider.Value = volum;
                TiviMediaElement.IsMuted = false;
            }

            else
            {
                ul = "/Assets/Icon/video_unvolum.png";
                volum = VolumeSlider.Value;
                TiviMediaElement.IsMuted = true;
                VolumeSlider.Value = 0;
            }
            if (VolumeSlider == null) return;
            if (VolumeSlider.Value == 0)
            {
                ul = "/Assets/Icon/video_unvolum.png";
            }
            else
            {
                ul = "/Assets/Icon/video_volum.png";
            }
            BitmapImage bmp = new BitmapImage(new Uri(ul, UriKind.Relative));
            imgMute.Source = null;
            imgMute.Source = bmp;
        }
        private void InitMediaPlayer()
        {
            _mediaElementManager = new MediaElementManager(Dispatcher, () =>
            {
                UpdateState(MediaElementState.Opening);
                return TiviMediaElement;
            },
                me => UpdateState(MediaElementState.Closed));

            _httpClients = new HttpClients(userAgent: ApplicationInformation.CreateUserAgent());

            _positionTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _positionTimer.Tick += OnPositionSamplerOnTick;

            if (null == TiviMediaElement)
            {
                Debug.WriteLine("MainPage Play no media element");
                return;
            }

            InitializeMediaStream();
            if (!string.IsNullOrEmpty(_streamLink))
            {
                try
                {
                    _mediaStreamFascade.Source = new Uri(_streamLink,0);
                }
                catch (Exception)
                {
                    MessageBox.Show("Sai định dạng link tivi!");
                }
            }
        }

        private void OnPositionSamplerOnTick(object sender, EventArgs e)
        {
            if (null == TiviMediaElement || (MediaElementState.Playing != TiviMediaElement.CurrentState && MediaElementState.Paused != TiviMediaElement.CurrentState))
            {
                TimeElapsedTextBlock.Text = "--:--:--";

                return;
            }

            var position = TiviMediaElement.Position;

            //if (positionSample == _previousPosition)
            //    return;

            //_previousPosition = positionSample;

            TimeElapsedTextBlock.Text = FormatTimeSpan(position);
            //if (position.TotalSeconds < 600)
            //{
            TimelineSlider.Maximum = position.TotalSeconds;
            TimelineSlider.Value = position.TotalSeconds;
            //}

            if (_mediaStreamFascade.SeekTarget != null)
                Debug.WriteLine("OnPositionSamplerOnTick: " + _mediaStreamFascade.SeekTarget.Value.TotalSeconds);
            Debug.WriteLine("OnPositionSamplerOnTick: " + TimeElapsedTextBlock.Text);
        }

        private string FormatTimeSpan(TimeSpan timeSpan)
        {
            var sb = new StringBuilder();

            if (timeSpan < TimeSpan.Zero)
            {
                sb.Append('-');

                timeSpan = -timeSpan;
            }

            if (timeSpan.Days > 1)
                sb.AppendFormat(timeSpan.ToString(@"%d\."));

            sb.Append(timeSpan.ToString(@"hh\:mm\:ss"));

            return sb.ToString();
        }


        private void UpdateState(MediaElementState state)
        {
            Debug.WriteLine("MediaElement State: " + state);

            //if (MediaElementState.Buffering == state && null != TiviMediaElement)
            //    //MediaStateBox.Text = string.Format("Buffering {0:F2}%", mediaElement1.BufferingProgress * 100);
            //else
            //    //MediaStateBox.Text = state.ToString();

            string str = "/Assets/Icon/transport.pause.png";

            switch (state)
            {
                case MediaElementState.Paused:
                    str = "/Assets/Icon/transport.play.png";
                    //errorBox.Visibility = Visibility.Collapsed;
                    break;
                case MediaElementState.Playing:
                    str = "/Assets/Icon/transport.pause.png";
                    //errorBox.Visibility = Visibility.Collapsed;
                    break;
                default:
                    str = "/Assets/Icon/transport.pause.png";
                    //errorBox.Visibility = Visibility.Collapsed;
                    break;
            }
            var bmp = new BitmapImage(new Uri(str, UriKind.Relative));
            imgPause.Source = bmp;

            OnPositionSamplerOnTick(null, null);
        }

        private void InitializeMediaStream()
        {
            if (null != _mediaStreamFascade)
                return;

            _mediaStreamFascade = MediaStreamFascadeSettings.Parameters.Create(_httpClients, _mediaElementManager.SetSourceAsync);

            _mediaStreamFascade.SetParameter(_mediaElementManager);

            _mediaStreamFascade.StateChange += TsMediaManagerOnStateChange;
        }

        private void TsMediaManagerOnStateChange(object sender, TsMediaManagerStateEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                var message = e.Message;
                Debug.WriteLine("TsMediaManagerOnStateChange : " + e.Message);

                if (!string.IsNullOrWhiteSpace(message))
                {
                    //errorBox.Text = message;
                    //errorBox.Visibility = Visibility.Visible;
                }

                TiviMediaElement_CurrentStateChanged(null, null);
            });
        }

        void TiviMediaElement_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            var state = null == TiviMediaElement ? MediaElementState.Closed : TiviMediaElement.CurrentState;
            if (state == MediaElementState.Paused)
            {
                //if (CheckConnectNetwork.checkNetworkConnection() == false)
                //{
                //    MessageBox.Show("Lỗi kết nối mạng! Vui lòng kiểm tra lại dữ liệu kết nối.");
                //}
            }
            if (null != _mediaStreamFascade)
            {
                var managerState = _mediaStreamFascade.State;

                if (MediaElementState.Closed == state)
                {
                    if (TsMediaManager.MediaState.OpenMedia == managerState || TsMediaManager.MediaState.Opening == managerState || TsMediaManager.MediaState.Playing == managerState)
                        state = MediaElementState.Opening;
                }
            }

            UpdateState(state);
        }

        private bool isOpenedMedia = false;
        private void TiviMediaElement_OnMediaOpened(object sender, RoutedEventArgs e)
        {
            //
            if (!TiviMediaElement.NaturalDuration.TimeSpan.Equals(TimeSpan.Zero))
            {
                TimelineSlider.Maximum = TiviMediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                TotalDurationTextBlock.Text = FormatTimeSpan(TiviMediaElement.NaturalDuration.TimeSpan);
            }
            else
            {
                //TimelineSlider.IsEnabled = false;
                //TotalDurationTextBlock.Text = string.Empty;
                TimelineSlider.Maximum = 1; //10 phút
            }
            isOpenedMedia = true;
            TiviMediaElement.Play();
            TiviMediaElement.Volume = CalcVolume(VolumeSlider.Value);

            _positionTimer.Start();
            ProsesProgressBar.Visibility = Visibility.Collapsed;
        }

        private void TiviMediaElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            //if (CheckConnectNetwork.checkNetworkConnection() == false)
            //{
            //    MessageBox.Show("Lỗi kết nối mạng! Vui lòng kiểm tra lại dữ liệu kết nối.");
            //    return;

            //}
            MessageBox.Show("Có lỗi xảy ra trong quá trình tải video, vui lòng thử lại sau!\nLink: " + _streamLink);
            //if (PopupManager.popup.IsOpen)
            //{
            //    PopupManager.popup.IsOpen = false;
            //}
            ////if (NavigationService.CanGoBack)
            ////    NavigationService.GoBack();
            Debug.WriteLine("TiviMediaElement_MediaFailed: ");
        }

        private void TiviMediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("TiviMediaElement_MediaEnded: ");
        }


        private void TiviMediaElement_BufferingProgressChanged(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("TiviMediaElement_BufferingProgressChanged: ");
        }

        private void PlayPauseButton_OnClick(object sender, RoutedEventArgs e)
        {
            PlayAgainButton.Visibility = Visibility.Collapsed;
            string str = "/Assets/Icon/transport.pause.png";
            imgPause.Source = null;
            if (TiviMediaElement.CurrentState == MediaElementState.Playing)
            {
                _positionTimer.Stop();
                TiviMediaElement.Pause();
            }
            else if (TiviMediaElement.CurrentState == MediaElementState.Paused)
            {
                InitMediaPlayer();
            }
            else
            {
                TiviMediaElement.Play();
                _positionTimer.Start();
            }
            UpdateState(TiviMediaElement.CurrentState);
        }

        private void VolumeSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (TiviMediaElement != null) TiviMediaElement.Volume = CalcVolume(VolumeSlider.Value);
            string ul = "/Assets/Icon/video_volum.png";
            if (VolumeSlider == null) return;
            if (VolumeSlider.Value == 0)
            {
                ul = "/Assets/Icon/video_unvolum.png";
            }
            else
            {
                ul = "/Assets/Icon/video_volum.png";
            }
            BitmapImage bmp = new BitmapImage(new Uri(ul, UriKind.Relative));
            imgMute.Source = null;
            imgMute.Source = bmp;

        }

        private double CalcVolume(double value)
        {
            var volumeValue = 0.0;
            if (value >= 0 && value <= 0.1)
            {
                //Gia tri volume tu 0 - 80
                //volume = (value/0.1)* 0.8
                volumeValue = (value / 0.1) * 0.8;
            }
            else if (value > 0.1 && value <= 1)
            {
                //Gia tri volume tu 0.8 - 1
                //
                volumeValue = ((value - 0.1) / 0.9) * 0.2 + 0.8;
            }

            return volumeValue;
        }

        private void TimelineSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (null == TiviMediaElement || TiviMediaElement.CurrentState != MediaElementState.Playing)
                return;
            if (TiviMediaElement.CanSeek)
            {
                TiviMediaElement.Position = TimeSpan.FromSeconds(TimelineSlider.Value);
            }
        }

        private bool isLike = true;
        private void BtnLike_OnClick(object sender, RoutedEventArgs e)
        {
            var item = (App.Current as App).chanelDetail;
            var lstChanelsLiked = CodePublic.ReadDataFromIsolatedStorage();
            if (item != null)
            {
                if (lstChanelsLiked == null)
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
                        if (chanel.chanelId == item.chanelId)
                        {
                            lstChanelsLiked.Remove(chanel);
                            CodePublic.SaveDataToIsolatedStorage(lstChanelsLiked);
                            break;
                        }
                    }
                }
                var vm = DataContext as MenuMainVM;
                if (vm != null) vm.propData.chanelsCollection[0].numChannel = lstChanelsLiked.Count - 1;
            }
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            //if (PopupManager.popup.IsOpen)
            //{
            //    PopupManager.popup.IsOpen = false;
            //    e.Cancel = true;
            //    return;
            //}
            if (!NavigationService.CanGoBack)
                base.OnBackKeyPress(e);
        }

        private void BtnShare_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ShareLinkTask share = new ShareLinkTask();
                if (type == "PROGRAM" || type == "LIVENOW" || type == "TVSHOW")
                {
                    MessageBox.Show("Chương trình này không cho phép chia sẻ!"); return;
                }
                if (type == "CHANNEL")
                {
                    //share.LinkUri = new Uri(HttpHelper.domainShare+""+PublicCode.channel.link, UriKind.RelativeOrAbsolute);
                    ////  share.Message = PublicCode.filmsPart.numberOfViews;
                    //share.Title = PublicCode.channel.name;
                    //share.Message = PublicCode.channel.description;
                    //share.Show();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Link chia sẻ của kênh không đúng định dạng!");
            }
        }
        #endregion

        private void BtnServer_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            SelectedServerLiveTv(button.Tag.ToString(),(App.Current as App).chanelDetail);
            var tbSelected = button.GetFirstLogicalChildByType<TextBlock>(true);
            buttonServerSelected(tbSelected);
        }

        void buttonServerSelected(TextBlock tbSelected)
        {
            tbServer1.Foreground =
                tbServer2.Foreground =
                    tbServer3.Foreground = tbServer4.Foreground = new SolidColorBrush(Colors.DarkGray);
            tbSelected.Foreground = new SolidColorBrush(Colors.White);
        }
        private void SelectedServerLiveTv(string nServer, Chanel chanel)
        {
            switch (nServer)
            {
                case "1":
                    _streamLink = chanel.link;
                    //TiviMediaElement.Source = new Uri(chanel.link,UriKind.RelativeOrAbsolute);
                    //TiviMediaElement.Play();
                    break;
                case "2":
                    _streamLink = chanel.link2;
                    //TiviMediaElement.Source = new Uri(chanel.link2, UriKind.RelativeOrAbsolute);
                    //TiviMediaElement.Play();
                    break;
                case "3":
                    _streamLink = chanel.link3;
                    //TiviMediaElement.Source = new Uri(chanel.link3, UriKind.RelativeOrAbsolute);
                    //TiviMediaElement.Play();
                    break;
                case "4":
                    _streamLink = chanel.link4;
                    //TiviMediaElement.Source = new Uri(chanel.link4, UriKind.RelativeOrAbsolute);
                    //TiviMediaElement.Play();
                    break;
                default:
                    _streamLink = chanel.link;
                    //TiviMediaElement.Source = new Uri(chanel.link, UriKind.RelativeOrAbsolute);
                    //TiviMediaElement.Play();
                    break;

            }
            GetDirectLink(_streamLink);
            InitMediaPlayer();
        }

        private void GrdBackgroundPlayer_OnTap(object sender, GestureEventArgs e)
        {
            ShowOrCloseControl();
        }

        private void BtnCalendar_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(
                        new Uri(
                            "/View/PageBroadcastSchedule.xaml",
                            UriKind.RelativeOrAbsolute));
            MenuSetting();
        }

        private void PanelPlayTivi_OnDoubleTap(object sender, GestureEventArgs e)
        {
            if(!isLandscape)
            BtnZoomPlayer_OnClick(null,null);
        }

        #region==========get link==============
        private int _countLink = 0;
        private async void GetDirectLink(string link)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.IfModifiedSince = new DateTimeOffset?(DateTimeOffset.Now);
            if ((App.Current as App).chanelDetail.typeChannel.EndsWith("TrucTiep"))
            {
                if (this._countLink > 0 && !link.Contains("m3u8"))
                {
                    string stringAsync = await httpClient.GetStringAsync(string.Concat("http://winphonevn.com/server/getlinkbongda.php?url=", link));
                    link = stringAsync;
                }
                this._countLink = this._countLink + 1;
            }
           // this.Player.Source = null;
            string[] strArray = null;
            string str = "";
           // this.Load.set_Visibility(0);
            if (link.Contains("!"))
            {
                strArray = link.Split(new char[] { '!' });
            }
            try
            {
                if (strArray != null)
                {
                    str = await httpClient.GetStringAsync(strArray[0]);
                }
                if (link.Contains("fptlive"))
                {
                    string str1 = link;
                    char[] chrArray = new char[] { '/' };
                    string str2 = str1.Split(chrArray)[1];
                    httpClient = new HttpClient();
                    Dictionary<string, string> dictionary = new Dictionary<string, string>();
                    dictionary.Add("id", str2);
                    dictionary.Add("quality", "3");
                    dictionary.Add("mobile", "web");
                    dictionary.Add("type", "newchannel");
                    FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(dictionary);
                    HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri("http://fptplay.net/show/getlinklivetv", UriKind.RelativeOrAbsolute),
                        Content = formUrlEncodedContent
                    };
                    HttpRequestMessage uri = httpRequestMessage;
                    uri.Headers.Referrer = new Uri("http://fptplay.net/");
                    uri.Headers.Add("X-Requested-With", "XMLHttpRequest");
                    HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(uri, HttpCompletionOption.ResponseContentRead);
                    string str3 = await httpResponseMessage.Content.ReadAsStringAsync();
                    if (str3 != null && str3.Trim() != "")
                    {
                        string item = (string)JObject.Parse(str3)["stream"];
                        if (item.Trim() != "")
                        {
                            this._str = item;
                        }
                    }
                }
                else if (strArray != null)
                {
                    Match match = (new Regex(strArray[1], RegexOptions.Compiled)).Match(str);  
                    if (match.Success && match.Groups[1].Value.StartsWith("http"))
                    {
                        this._str = HttpUtility.UrlDecode(match.Groups[1].Value);
                    }
                }
                else if (link.Contains("app.101vn"))
                {
                    Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
                    dictionary1.Add("talachua", "101vn");
                    dictionary1.Add("package", "duahau3");
                    dictionary1.Add("version", "10");
                    FormUrlEncodedContent formUrlEncodedContent1 = new FormUrlEncodedContent(dictionary1);
                    HttpResponseMessage httpResponseMessage1 = await httpClient.PostAsync(link, formUrlEncodedContent1);
                    PageChanelDeail playVideo = this;
                    string str4 = playVideo._str;
                    playVideo._str = await httpResponseMessage1.Content.ReadAsStringAsync();
                    playVideo = null;
                }
                else if (link.Contains("notfreeplus"))
                {
                    string str5 = await (new WebClient()).DownloadStringTaskAsync(new Uri("https://api.vtvplus.vn/pro/index.php/api/v1/channel/stream/88/?ip_address=0&username=84972291119@vtvplus.vn&type_device=android&type_network=wifi&app_name=VTVPlus"));
                    str = str5;
                    GetListNotFreePlus objectPlu = JsonConvert.DeserializeObject<GetListNotFreePlus>(str);
                    this._str = objectPlu.data[0].url;
                    //string[] arrStrings = _str.Split('?');
                    //this._str = arrStrings[0];
                }
                else if (!link.Contains("notfreemtv"))
                {
                    this._str = link;
                }
                else
                {
                    string[] strArray1 = link.Split(new char[] { '=' });
                    Random random = new Random();
                    Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
                    str = await httpClient.GetStringAsync("http://api.mytvnet.vn//getToken");
                    JObject jObjects = JObject.Parse(str);
                    if (jObjects == null || jObjects["tid"] == null)
                    {
                        return;
                    }
                    else
                    {
                        //long num = Convert.ToInt64((new Fc()).DeCodeTid(jObjects["tid"].ToString()));
                        //DateTime now = DateTime.Now;
                        //Fc fc = new Fc();
                        //dictionary2.Add("tid", fc.GetTid(num, now));
                        dictionary2.Add("tid", jObjects["tid"].ToString());
                        Dictionary<string, string> dictionary3 = dictionary2;
                        int num1 = random.Next(1, 10);
                        dictionary3.Add("device_type", num1.ToString());
                        Dictionary<string, string> dictionary4 = dictionary2;
                        num1 = random.Next(1, 99);
                        dictionary4.Add("app_v", num1.ToString());
                        dictionary2.Add("lang", "vn");
                        dictionary2.Add("channel_id", strArray1[1]);
                        dictionary2.Add("mf_code", strArray1[2]);
                        Dictionary<string, string> dictionary5 = dictionary2;
                        num1 = random.Next(1, 10);
                        dictionary5.Add("profile", num1.ToString());
                        Dictionary<string, string> dictionary6 = dictionary2;
                        num1 = random.Next(1, 100000);
                        dictionary6.Add("member_id", num1.ToString());
                        Dictionary<string, string> dictionary7 = dictionary2;
                        num1 = random.Next(1, 1000000);
                        dictionary7.Add("manufacturer_id", num1.ToString());
                        Dictionary<string, string> dictionary8 = dictionary2;
                        num1 = random.Next(0, 100000);
                        dictionary8.Add("device_id", num1.ToString());
                        FormUrlEncodedContent formUrlEncodedContent2 = new FormUrlEncodedContent(dictionary2);
                        HttpResponseMessage httpResponseMessage2 = await httpClient.PostAsync("http://ott.mytvnet.vn/v5/channel/mobile/url", formUrlEncodedContent2);
                        string str6 = await httpResponseMessage2.Content.ReadAsStringAsync();
                        jObjects = JObject.Parse(str6);
                        if (jObjects["result"].ToString() == "-1")
                        {
                            Dictionary<string, string> dictionary9 = new Dictionary<string, string>();
                            //dictionary9.Add("tid", fc.GetTid(num, now));
                            num1 = random.Next(1, 10);
                            dictionary9.Add("device_type", num1.ToString());
                            num1 = random.Next(1, 99);
                            dictionary9.Add("app_v", num1.ToString());
                            dictionary9.Add("lang", "vn");
                            dictionary9.Add("channel_id", strArray1[1]);
                            dictionary9.Add("mf_code", strArray1[2]);
                            num1 = random.Next(1, 10);
                            dictionary9.Add("profile", num1.ToString());
                            num1 = random.Next(1, 100000);
                            dictionary9.Add("member_id", num1.ToString());
                            num1 = random.Next(1, 1000000);
                            dictionary9.Add("manufacturer_id", num1.ToString());
                            num1 = random.Next(1, 100000);
                            dictionary9.Add("device_id", num1.ToString());
                            dictionary2 = dictionary9;
                            formUrlEncodedContent2 = new FormUrlEncodedContent(dictionary2);
                            HttpResponseMessage httpResponseMessage3 = await httpClient.PostAsync("http://api.mytvnet.vn/v5/channel/mobile/url", formUrlEncodedContent2);
                            string str7 = await httpResponseMessage3.Content.ReadAsStringAsync();
                            jObjects = JObject.Parse(str7);
                            this._str = jObjects["data"]["url"].ToString();
                        }
                        else
                        {
                            this._str = jObjects["data"]["url"].ToString();
                        }
                        strArray1 = null;
                        random = null;
                        dictionary2 = null;
                       // fc = null;
                    }
                }

                _streamLink = this._str;
                //  return _str;
                //this.Player.Source = new Uri(this._str.Trim(), 0);
                //this.Player.Play();
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                //EasyTracker.GetTracker().SendException(string.Concat(this._str, " :", exception.get_Message()), false);
                //this.Player.Source = new Uri(this._str, 0);
                //this.Player.Play();
                //this.Load.set_Visibility(1);
                _streamLink = (App.Current as App).chanelDetail.link;
            }
            //this.Load.set_Visibility(1);
        }
        private string _str = "";
        #endregion
    }
}