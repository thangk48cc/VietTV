using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        public  GetListData GetResulTask()
        {
            string data = ReadFile("Services/channels.txt");//
            // MessageBox.Show(data);
            if (data == null)
            {
                MessageBox.Show("Tải dữ liệu thất bại, vui lòng thử lại");
                return null;
            }
            else
            {
                data = "{ \"chanelsCollection\": " + data + "}";
            }
            return JsonConvert.DeserializeObject<GetListData>(data);
        }
        public string ReadFile(string filePath)
        {
            //this verse is loaded for the first time so fill it from the text file
            var ResrouceStream = Application.GetResourceStream(new Uri(filePath, UriKind.Relative));
            if (ResrouceStream != null)
            {
                Stream myFileStream = ResrouceStream.Stream;
                if (myFileStream.CanRead)
                {
                    StreamReader myStreamReader = new StreamReader(myFileStream);

                    //read the content here
                    return myStreamReader.ReadToEnd();
                }
            }
            return "NULL";
        }
    }
}
