using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace YaleAlarm
{
    public class YaleAlarm
    {

        private static readonly Lazy<YaleAlarm> lazy = new Lazy<YaleAlarm>(() => new YaleAlarm());
        public static YaleAlarm Instance { get => lazy.Value; }

        public static string YALE_STATE_ARM_FULL = "arm";
        public const string YALE_STATE_ARM_PARTIAL = "home";
        public const string YALE_STATE_DISARM = "disarm";


        const string _HOST = "https://mob.yalehomesystem.co.uk/yapi";
        const string _ENDPOINT_TOKEN = "/o/token/";
        const string _ENDPOINT_SERVICES = "/services/";
        const string _ENDPOINT_GET_MODE = "/api/panel/mode/";
        const string _ENDPOINT_SET_MODE = "/api/panel/mode/";
        const string _ENDPOINT_DEVICES_STATUS = "/api/panel/device_status/";

        const string _YALE_AUTH_TOKEN = "VnVWWDZYVjlXSUNzVHJhcUVpdVNCUHBwZ3ZPakxUeXNsRU1LUHBjdTpkd3RPbE15WEtENUJ5ZW1GWHV0am55eGhrc0U3V0ZFY2p0dFcyOXRaSWNuWHlSWHFsWVBEZ1BSZE1xczF4R3VwVTlxa1o4UE5ubGlQanY5Z2hBZFFtMHpsM0h4V3dlS0ZBcGZzakpMcW1GMm1HR1lXRlpad01MRkw3MGR0bmNndQ==";
        WebClient client;
        ResonseModel resonseModel;

        public YaleAlarm()
        {


        }

        public bool GetAuthenticated()
        {
            var url = _HOST + _ENDPOINT_TOKEN;
            var pModel = new PostModel
            {
                grant_type = "password",
                username = "charles.jardine@nathan-software.com",
                password = "Revelati0n4040"
            };

            var reqparm = new System.Collections.Specialized.NameValueCollection();
            reqparm.Add("grant_type", pModel.grant_type);
            reqparm.Add("username", pModel.username);
            reqparm.Add("password", pModel.password);

            client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + _YALE_AUTH_TOKEN;
            byte[] responsebytes = client.UploadValues(url, "POST", reqparm);
            string json = Encoding.UTF8.GetString(responsebytes);
            resonseModel = JsonConvert.DeserializeObject<ResonseModel>(json);

            if (resonseModel.access_token == null)
            {
                Console.WriteLine("Failed to authenticate with Yale Smart Alarm.Error:");
                return false;
            }
            else
            {
                return true;
            }
        }

        public string AlarmStatus()
        {
            var url = _HOST + _ENDPOINT_GET_MODE;
            client.Headers[HttpRequestHeader.Authorization] = "Bearer " + resonseModel.access_token;

            Uri Url = new Uri(url);
            var json = client.DownloadString(Url);
            var drModel  = JsonConvert.DeserializeObject<DataRoot>(json);
            if (drModel != null)
                return drModel.data[0].mode;
            else
                return string.Empty;
        }

        public bool ArmDevice(string mode)
        {
            var url = _HOST + _ENDPOINT_SET_MODE;

            var reqparm = new System.Collections.Specialized.NameValueCollection();
            reqparm.Add("area", "1");
            reqparm.Add("mode", mode);
           
            client.Headers[HttpRequestHeader.Authorization] = "Bearer " + resonseModel.access_token;
            byte[] responsebytes = client.UploadValues(url, "POST", reqparm);
            string json = Encoding.UTF8.GetString(responsebytes);

            return true;
        }

      
        public class PostModel
        {
            public string grant_type { get; set; }
            public string username { get; set; }
            public string password { get; set; }
        }

        public class ResonseModel
        {
            public int expires_in { get; set; }
            public string token_type { get; set; }
            public string access_token { get; set; }
            public string scope { get; set; }
            public string refresh_token { get; set; }
        }

        public class Datum
        {
            public string area { get; set; }
            public string mode { get; set; }
        }

        public class DataRoot
        {
            public bool result { get; set; }
            public string code { get; set; }
            public string message { get; set; }
            public string token { get; set; }
            public List<Datum> data { get; set; }
            public string time { get; set; }
        }

    }
}
