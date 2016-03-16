using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Phone.Controls;

namespace VietTV.Services
{
    /// <summary>
    /// Navigation helper, to manage navigation from anywhere
    /// </summary>
    public class NavigationHelper
    {
        private PhoneApplicationFrame _frame;

        public NavigationHelper(PhoneApplicationFrame frame)
        {
            _frame = frame;
        }

        /// <summary>
        /// Initialize the navigation helper with a navigation frame
        /// </summary>
        /// <param name="frame"></param>
        public void Initialize(PhoneApplicationFrame frame)
        {
            _frame = frame;
        }

        /// <summary>
        /// Go back in navigation history
        /// </summary>
        public void GoBack()
        {
            if (_frame.CanGoBack)
                _frame.GoBack();
        }
        public delegate void EventHandler(object sender);
        public static event EventHandler Succes;
        public void GoBack(string a)
        {
            GoBackSuccess();
            if (_frame.CanGoBack)
                _frame.GoBack();
        }

        public static void GoBackSuccess()
        {
            if (Succes != null)
            {
                Succes(null);
            }
        }
        /// <summary>
        /// Navigate to an url
        /// </summary>
        /// <param name="url"></param>
        public void Navigate<T>()
        {
            var type = typeof(T);
            //_frame=new PhoneApplicationFrame();
            var testvalue = (type.FullName.Substring(type.FullName.IndexOf('.')).Replace('.', '/')) + ".xaml";

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                _frame.Navigate(new Uri(testvalue, UriKind.Relative));
            });
        }
        public void Navigate<T>(string valueSend)
        {
            var type = typeof(T);
            //_frame=new PhoneApplicationFrame();

            var testvalue = (type.FullName.Substring(type.FullName.IndexOf('.')).Replace('.', '/')) + ".xaml?" + valueSend;

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                _frame.Navigate(new Uri(testvalue, UriKind.Relative));
            });
        }

        /// <summary>
        /// Removes an entry from the navigation back stack
        /// </summary>
        public void RemoveBackEntry()
        {
            _frame.RemoveBackEntry();
        }

        /// <summary>
        /// Exits the application 
        /// </summary>
        public void Exit()
        {

        }
    }
}
