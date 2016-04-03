using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using VietTV.Controls;

namespace VietTV.Common
{
    public class PopupManager
    {
        //private static PopupManager _instance;

        //public static PopupManager Instance
        //{
        //    get 
        //    {
        //        if (_instance == null)
        //        {
        //            _instance = new PopupManager();
        //        }
        //        return _instance; 
        //    }
        //}
        public static Popup popup = new Popup();
        public event EventHandler ClosePopupFromPage;
        public static bool isopen = false;
        public static void Show(UserControl control)
        {
            //if (popup.IsOpen)
            //    popup.IsOpen = false;
            popup = new Popup();
            popup.Child = control;
            popup.IsOpen = true;
            isopen = true;
        }
        public static void ShowAlert(string content)
        {
            var control = new AlertCustomize(content);
            var popup = new Popup();
            popup.Child = control;
            popup.VerticalOffset = ((App.RootFrame.ActualHeight - control.Height) / 2) + 270;
            popup.HorizontalOffset = (App.RootFrame.ActualWidth - control.Width) / 2;
            popup.IsOpen = true;
            isopen = true;
        }
        //public static void ShowAboutInfo(string type,string duration)
        //{
        //    if (popup.IsOpen)
        //        popup.IsOpen = false;
        //    AboutMediaPlaying control = new AboutMediaPlaying(type,duration);
        //    popup = new Popup();
        //    popup.Child = control;
        //    popup.VerticalOffset = 160;// ((App.RootFrame.ActualWidth - control.Width)/2)-480;
        //    popup.HorizontalOffset = -160;//((App.RootFrame.ActualHeight - control.Height) / 2)-800;
        //    popup.IsOpen = true;
        //    isopen = true;
        //}
        public static void Close()
        {
            if (popup.IsOpen)
                popup.IsOpen = false;
            isopen = false;
        }
    }
}
