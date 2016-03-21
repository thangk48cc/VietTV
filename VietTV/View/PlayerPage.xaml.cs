using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using SM.Media;
using SM.Media.Utility;
using SM.Media.Web;
using VietTV.Common;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace VietTV.View
{
    public partial class PlayerPage : PhoneApplicationPage
    {
        public PlayerPage()
        {
            InitializeComponent();
            this.DataContext = (App.Current as App).chanelDetail;
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += timer_Tick;
            timer.Start();

            timerAutoOut.Interval = TimeSpan.FromSeconds(10);
            timerAutoOut.Tick += timerAutoOut_Tick;
            timerAutoOut.Start();
        }
       
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
                if (NavigationService.CanGoBack)
                NavigationService.GoBack();
            }
            else
            {
                timerAutoOut.Stop();
                timerAutoOut = null;
            }
        }
        DispatcherTimer timerAutoOut=new DispatcherTimer();
       
            static readonly IApplicationInformation ApplicationInformation = ApplicationInformationFactory.Default;
            IMediaElementManager _mediaElementManager;
            private DispatcherTimer _positionTimer;
            private IMediaStreamFascade _mediaStreamFascade;
            //private TimeSpan _previousPosition;
            private IHttpClients _httpClients;
            private string _streamLink = string.Empty;


            protected override void OnNavigatedTo(NavigationEventArgs e)
            {
                //string type = NavigationContext.QueryString["type"];
                //this.type = type;
                //var vm = DataContext as ViewVideoPlayerVM;
                //vm.GetInformationCommand.Execute(type);
                //if (show == false)
                //    ShowOrCloseControl();
                //base.OnNavigatedTo(e);
                if (e.NavigationMode == NavigationMode.New)
                {
                    // "http://www.nasa.gov/multimedia/nasatv/NTV-Public-IPS.m3u8"
                    // "http://devimages.apple.com/iphone/samples/bipbop/bipbopall.m3u8"
                    //"https://devimages.apple.com.edgekey.net/streaming/examples/bipbop_16x9/bipbop_16x9_variant.m3u8"
                    //"http://www.nasa.gov/multimedia/nasatv/NTV-Public-IPS.m3u8"
                    //"http://10.61.9.73:555/_ogKk0kbOxI69y7Z9TX9S9g5wlX+Lu2d+A+4Wglf0K-vNSIv2cebviZSKazEOnEzCGYcZAmnOU92iQhRwguJGB1cyvFmyQnLuB6iY-QlSGgC=_.m3u8"
                    // "http://10.61.9.73:555/_z1+6IvcmvV29y7Z9TX9S9dgpXlLfBjLr7oc22en6pm4NSIv2cebvi+tySDzTusrSGYcZAmnOU90j6Ld6lRf4S3+k5sPkHpda_.m3u8"
                    // "http://manifest.googlevideo.com/api/manifest/hls_variant/ip/220.231.122.23/key/yt5/itag/0/playlist_type/DVR/hfr/1/sver/3/gcr/vn/fexp/3300110%2C3300110%2C3300132%2C3300132%2C3300137%2C3300137%2C3300161%2C3300161%2C3310366%2C3310366%2C3310700%2C3310700%2C3312118%2C3312118%2C3312478%2C3312478%2C900244%2C907259%2C930666%2C932404%2C941004%2C943909%2C945247%2C947209%2C947215%2C948124%2C952302%2C952605%2C952901%2C953912%2C957103%2C957105%2C957201/signature/A0CFD8540DAA4902C7ED7738F434EE19D9831024.70336EDF0A1BE62F13A8F9033F937CDCC63134E8/sparams/gcr%2Chfr%2Cid%2Cip%2Cipbits%2Citag%2Cplaylist_type%2Cpmbypass%2Csource%2Cexpire/ipbits/0/id/39ya9uHPAeA.1/upn/UD8wzrE77U4/expire/1415708857/pmbypass/yes/source/yt_live_broadcast/keepalive/yes/file/index.m3u8"
                    //"http://manifest.googlevideo.com/api/manifest/hls_variant/playlist_type/DVR/id/L03m-JgHU_U.1/fexp/3300110%2C3300110%2C3300132%2C3300132%2C3300137%2C3300137%2C3300161%2C3300161%2C3310366%2C3310366%2C3310700%2C3310700%2C3312118%2C3312118%2C3312478%2C3312478%2C900244%2C907259%2C930666%2C932404%2C941004%2C943909%2C945247%2C947209%2C947215%2C948124%2C952302%2C952605%2C952901%2C953912%2C957103%2C957105%2C957201/pmbypass/yes/sver/3/keepalive/yes/expire/1415709194/ip/220.231.122.23/signature/5DC955066AF1B9B59E6B59D3FE97A83FEDE41BD7.9567F10FCB776211CA61AC498C014E31CCE7E1D5/sparams/gcr%2Chfr%2Cid%2Cip%2Cipbits%2Citag%2Cplaylist_type%2Cpmbypass%2Csource%2Cexpire/source/yt_live_broadcast/key/yt5/itag/0/gcr/vn/upn/oWbY4DvoH1I/ipbits/0/hfr/1/file/index.m3u8"
                    // "http://manifest.googlevideo.com/api/manifest/hls_variant/source/yt_live_broadcast/keepalive/yes/sver/3/ipbits/0/hfr/1/playlist_type/DVR/signature/CF85A78F590AF54F07DEDFBB6720F127327836F3.63EC4F952C83C87E2D71695CAAE8C4165F8F30C3/pmbypass/yes/ip/220.231.122.23/gcr/vn/itag/0/upn/iEXP8nx6cts/expire/1415710373/sparams/gcr%2Chfr%2Cid%2Cip%2Cipbits%2Citag%2Cplaylist_type%2Cpmbypass%2Csource%2Cexpire/id/L03m-JgHU_U.1/fexp/3300110%2C3300110%2C3300132%2C3300132%2C3300137%2C3300137%2C3300161%2C3300161%2C3310366%2C3310366%2C3310700%2C3310700%2C3312118%2C3312118%2C3312478%2C3312478%2C900244%2C907259%2C930666%2C932404%2C941004%2C943909%2C945247%2C947209%2C947215%2C948124%2C952302%2C952605%2C952901%2C953912%2C957103%2C957105%2C957201/key/yt5/file/index.m3u8"

                    //_streamLink = "http://www.nasa.gov/multimedia/nasatv/NTV-Public-IPS.m3u8";
                    //if (type == "PROGRAM")
                    //    _streamLink = PublicCode.tvShow.linkTVShow; //"http://htvlive.1c656bad.cdnviet.com/19fc94584c8362a0f39a0b08efeb8f0e1458424112/htv7.720p.stream/playlist.m3u8";
                    //if (type == "TVSHOW")
                    //    _streamLink = PublicCode.tvShow.linkTVShow;
                    //if (type == "LIVENOW")
                    //    _streamLink = PublicCode.tvShow.linkTVShow;
                    //if (type == "CHANNEL")
                    //    _streamLink = PublicCode.channel.linkChannel;
                    //_streamLink ="http://live.kenhitv.vn:1935/liveweb/itv_web_500k.stream/playlist.m3u8";//"http://vp.xemtvhd.com/chn/vtc1/v.m3u8";
                    _streamLink = NavigationContext.QueryString["linkVideo"];
                    InitMediaPlayer();
                }
                else
                {
                    InitMediaPlayer();
                }
            }
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
                MessageBox.Show("Có lỗi xảy ra trong quá trình tải video, vui lòng thử lại sau!\nLink: "+_streamLink); 
                //if (PopupManager.popup.IsOpen)
                //{
                //    PopupManager.popup.IsOpen = false;
                //}
                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();
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
                if(VolumeSlider==null) return;
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
            if(!NavigationService.CanGoBack)
            base.OnBackKeyPress(e);
        }

        private void BtnShare_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ShareLinkTask share = new ShareLinkTask();
                if (type == "PROGRAM" || type == "LIVENOW"||type=="TVSHOW")
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
    }
    
}