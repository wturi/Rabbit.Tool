using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Wechat
{
   public  class WechatQR
    {
       /// <summary>
       /// 获取二维码
       /// </summary>
       /// <param name="appid"></param>
       /// <param name="appsecret"></param>
       /// <param name="scene_str"></param>
       /// <returns></returns>
        public static string GetQrcode(string appid, string appsecret, string scene_str)
        {
            string QrcodeUrl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";//WxQrcodeAPI接口
            string AccessToken = Token.GetToken();
            QrcodeUrl = string.Format(QrcodeUrl, AccessToken);
            string PostJson = "{\"action_name\": \"QR_LIMIT_STR_SCENE\", \"action_info\": {\"scene\": {\"scene_str\": \"" + scene_str + "\"}}}";
            string ReText = GetResponse(PostJson, QrcodeUrl);//此处省略了 WebRequestPostOrGet即为WebHttpRequest发送Post请求
            var resXml = JsonConvert.DeserializeXNode(ReText, "res");
            var node = resXml.Element("res").Element("ticket");
            if (node != null)
            {
                return "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + node.Value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 发送POST包，获得回复。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string GetResponse(string data, string url)
        {
            HttpWebRequest myHttpWebRequest = null;
            string strReturnCode;
            myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            myHttpWebRequest.ProtocolVersion = HttpVersion.Version10;

            myHttpWebRequest.Method = "POST";
            myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
            var bs = Encoding.UTF8.GetBytes(data);

            myHttpWebRequest.ContentLength = bs.Length;

            using (var reqStream = myHttpWebRequest.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }


            using (var myWebResponse = myHttpWebRequest.GetResponse())
            {
                var readStream = new StreamReader(myWebResponse.GetResponseStream(), encoding: Encoding.UTF8);
                strReturnCode = readStream.ReadToEnd();
            }

            return strReturnCode;
        }
    }
}
