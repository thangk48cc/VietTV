using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
            loadPageHTML();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += timer_Tick;
            timer.Start();

            timerAutoOut.Interval = TimeSpan.FromSeconds(10);
            timerAutoOut.Tick += timerAutoOut_Tick;
            timerAutoOut.Start();
        }

        void MainPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            TiviMediaElement.Stretch = Stretch.Fill;
            if (e.Orientation == PageOrientation.LandscapeLeft || e.Orientation == PageOrientation.LandscapeRight)
            {

                //TiviMediaElement.Height = 480;//  Double.NaN;
                //TiviMediaElement.Width = Double.NaN;
                panelPlayTivi.Height = 480;
                panelPlayTivi.Margin = new Thickness(0, 0, 0, 0);
                TiviMediaElement.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                TiviMediaElement.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

                SystemTray.IsVisible = false;

            }
            else
            {
                panelPlayTivi.Height = 270;
                panelPlayTivi.Margin = new Thickness(0,50,0,0);
            }
        }
        async void loadPageHTML()
        {
            string link = "http://htvonline.com.vn/xem-phim/phim-mat-na-thien-than-Tap-1-hd-3536313623373634316E61.html";

            //HttpClient client = new HttpClient();
            //var html = await client.GetStringAsync(link);

            //var doc = new HtmlAgilityPack.HtmlDocument();
            //doc.LoadHtml(html);

            //var root = doc.DocumentNode;
            //var commonPosts = root.Descendants().Where(n => n.GetAttributeValue("id", "").Equals("play_video"));
            //var inputs = from input in doc.DocumentNode.Descendants("div")
            //             where (input.Attributes["id"] != null && input.Attributes["id"].Value == "play_video")
            //             select input;
            //foreach (var input in inputs)
            //{
            //    linkVideo = input.Attributes["data-source"].Value;
            //    //MessageBox.Show(input.Attributes["data-source"].Value);
            //    // John
            //}
            //var inputs1 = from input in doc.DocumentNode.Descendants("ul")
            //              where (input.Attributes["class"] != null && input.Attributes["class"].Value == "bxslider list_schedule-ul")
            //             select input;
            //HtmlAgilityPack.HtmlDocument innerDoc = new HtmlAgilityPack.HtmlDocument();
            //innerDoc.LoadHtml(link.InnerHtml);

            //// Select what I need
            //MessageBox.Show(innerDoc.DocumentNode.SelectSingleNode("//span[@class=\"pp-place-title\"]").InnerText);
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

        private void BtnItemGroupChanel_OnClick(object sender, RoutedEventArgs e)
        {
            var item = (GetListChanels)(sender as Button).DataContext;
            var vm = DataContext as MenuMainVM;
            vm.groupChanelItem = item;
            vm.chanelsByGroup = item.chanels;
            MenuSetting();
            NavigationService.GoBack();
        }
        private void BtnZoomPlayer_OnClick(object sender, RoutedEventArgs e)
        {
            panelPlayTivi.Height = Application.Current.Host.Content.ActualHeight;
            panelPlayTivi.Margin = new Thickness(0, 0, 0, 0);

            TiviMediaElement.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            TiviMediaElement.Stretch= Stretch.Uniform;
            TiviMediaElement.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

            SystemTray.IsVisible = false;

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
                 _streamLink ="http://live.kenhitv.vn:1935/liveweb/itv_web_500k.stream/playlist.m3u8";//"http://vp.xemtvhd.com/chn/vtc1/v.m3u8";
                //_streamLink = NavigationContext.QueryString["linkVideo"];
                InitMediaPlayer();
            }
            else
            {
                _streamLink = "http://live.kenhitv.vn:1935/liveweb/itv_web_500k.stream/playlist.m3u8";
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
                _mediaStreamFascade.Source = new Uri(_streamLink);
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
            if (item != null)
            {
                item.isLiked = !item.isLiked;
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
    }
}