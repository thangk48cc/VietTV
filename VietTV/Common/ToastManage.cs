using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietTV.Common
{
    public class ToastManage
    {
        public static void Show(string content)
        {
            PopupManager.ShowAlert(content);
        }
        public static void ShowTest(string content)
        {
            PopupManager.ShowAlert(content);
        }
    }
}
