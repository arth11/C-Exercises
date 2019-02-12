using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GoogleAnalitics
{
  public  class GoogleAnalyticsApi
    {

        
    static void Main(string[] args)
        {
            GoogleAnaliticsItem item = new GoogleAnaliticsItem()
            {
                AdSenseId_a = "1",
                ItemCode_ic = "5430001111"
            };

            Track(item);
        }

        private static void Track(GoogleAnaliticsItem item)
        {
            if (string.IsNullOrEmpty(item.AdSenseId_a)) item.AdSenseId_a = "empty"; 
            if (string.IsNullOrEmpty(item.ItemCode_ic)) item.ItemCode_ic = "empty";

            var request = (HttpWebRequest)WebRequest.Create("https://www.google-analytics.com/collect");
            request.Method = "POST";

            // the request body we want to send
            var postData = new Dictionary<string, string>
                           {
                               { "v", "1" },
                               { "tid", "UA-7225725-62" }, //QA
                               { "cid", "555" },
                               { "t", item.AdSenseId_a },
                               { "ec", item.ItemCode_ic }

                           };
            //if (!string.IsNullOrEmpty(label))
            //{
            //    postData.Add("el", label);
            //}
            //if (value.HasValue)
            //{
            //    postData.Add("ev", value.ToString());
            //}

            var postDataString = postData
                .Aggregate("", (data, next) => string.Format("{0}&{1}={2}", data, next.Key,
                                                             next.Value))
                .TrimEnd('&');

            //  set the Content - Length header to the correct value
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataString);

            // write the request body to the request
            using (var writer = new StreamWriter(request.GetRequestStream()))
            {

                writer.Write(postDataString);
            }

            try
            {
                var webResponse = (HttpWebResponse)request.GetResponse();
                if (webResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Google Analytics tracking did not return OK 200");
                    //new Exception((int)webResponse.StatusCode,
                    //                      "Google Analytics tracking did not return OK 200");
                }
            }
            catch (Exception ex)
            {

                // do what you like here, we log to Elmah
                // ElmahLog.LogError(ex, "Google Analytics tracking failed");
            }
        }

        //public static void TrackEvent(GoogleAnaliticsItem analiticsItem)
        //{
        //    Track(HitType.@event, category, action, label, value);
        //}

        //public static void TrackPageview(string category, string action, string label, int? value = null)
        //{
        //    Track(HitType.@pageview, category, action, label, value);
        //}

     

       
    }
}
