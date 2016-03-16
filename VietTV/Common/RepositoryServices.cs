using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VietTV.Model;

namespace VietTV.Common
{
    public class RepositoryServices
    {
        public string link = "http://localhost/EnglishApp/channels";

        public async Task<GetListData> GetDataChanelsTask()
        {
            var http = new HttpClient();
            string data = await http.GetStringAsync(link);
            if (data!=null)
            {
                data = "{ chanelsCollection: " + data + "}";
            }
            return JsonConvert.DeserializeObject<GetListData>(data);
        }
    }
}
