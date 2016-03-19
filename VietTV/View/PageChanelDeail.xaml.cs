using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    public partial class PageChanelDeail : PhoneApplicationPage
    {
        public PageChanelDeail()
        {
            InitializeComponent();
            stbCloseMenu.Completed += stbCloseMenu_Completed;
            stbOpenMenu.Completed += stbOpenMenu_Completed;
            loadPageHTML();
        }

        async void loadPageHTML()
        {
            string link = "http://htvonline.com.vn/xem-phim/phim-mat-na-thien-than-Tap-1-hd-3536313623373634316E61.html";

            HttpClient client = new HttpClient();
            var html = await client.GetStringAsync(link);

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            var root = doc.DocumentNode;
            var commonPosts = root.Descendants().Where(n => n.GetAttributeValue("id", "").Equals("play_video"));
            var inputs = from input in doc.DocumentNode.Descendants("div")
                         where (input.Attributes["id"] != null && input.Attributes["id"].Value == "play_video")
                         select input;
            foreach (var input in inputs)
            {
                linkVideo = input.Attributes["data-source"].Value;
                //MessageBox.Show(input.Attributes["data-source"].Value);
                // John
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
            base.OnNavigatedTo(e);
        }

        private void BtnZoomPlayer_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/PlayerPage.xaml?linkVideo="+linkVideo, UriKind.RelativeOrAbsolute));
        }
    }
}