using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Model
{
    public class RemoteRepository
    {
        public IEnumerable<Entry> GetAllEntries()
        {
            var responseString = DownloadStringData(new CookieContainer(), "http://192.168.0.103:90/");
            var list = JsonConvert.DeserializeObject<List<Entry>>(responseString);
            return list;
        }

        public static String DownloadStringData(CookieContainer cookies, string siteURL)
        {
            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;

            httpRequest = (HttpWebRequest)WebRequest.Create(siteURL);

            httpRequest.CookieContainer = cookies;

            try
            {
                Task<HttpWebResponse> task = Task.Factory.FromAsync(
                    httpRequest.BeginGetResponse,
                    asyncResult => httpResponse = (HttpWebResponse)httpRequest.EndGetResponse(asyncResult),
                    (object)null);
                task.Wait();
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    var stream = httpResponse.GetResponseStream();
                    var sr = new StreamReader(stream);
                    var content = sr.ReadToEnd();
                    return content;
                }
                return null;
            }
            catch (WebException we)
            {
                return null;
            }
            finally
            {
                if (httpResponse != null)
                {
                    httpResponse.Dispose();
                }
            }
        }
    }
}
