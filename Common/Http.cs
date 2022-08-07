using System;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;


namespace Common {

    public class EHttp : Exception { public EHttp(string message):base(message){} }
    public class EHttp404Error : EHttp { public EHttp404Error(string message):base(message){} }
    public static class Http {


        /**
        @param string url
        @return string which need to be deserialized with JsonConvert.Deserialize<T>
        */
        public static string get(string url) {
            string jsonStr = "";
            try {
                HttpWebRequest request = WebRequest.CreateHttp(url);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    jsonStr = reader.ReadToEnd();
                }
            } catch (WebException e) {
                if (e.Status == WebExceptionStatus.ProtocolError &&
                    e.Response != null)
                {
                    var resp = (HttpWebResponse) e.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new EHttp404Error( e.Message );
                    }
                }
                if( e.Response != null ) {
                    string errorData = new StreamReader(e.Response.GetResponseStream())
                            .ReadToEnd();
                    throw new EHttp( "Http.post(): url:" + url+ " errorData:" + errorData );
                }
                throw new EHttp( e.Message );
            }
            return jsonStr;
        }

        public static string post(string url, string json) {
            string jsonStr = "";
            try {
                WebRequest request = WebRequest.Create(url);
                request.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(json);
                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    jsonStr = reader.ReadToEnd();
                }
                response.Close();
            } catch (WebException e) {
                string errorData = new StreamReader(e.Response.GetResponseStream())
                          .ReadToEnd();
                throw new EHttp( "Http.post(): url:" + url+ " json: "+ json+" errorData:" + errorData );
            } catch (Exception e) {
                throw new EHttp( "Http.post(): url:" + url+ " json: "+ json+" error:" + JsonConvert.SerializeObject(e) );
            }
            return jsonStr;
        }

        public static string put(string url, string json) {
            string jsonStr = "";
            try {
                WebRequest request = WebRequest.Create(url);
                request.Method = "PUT";
                byte[] byteArray = Encoding.UTF8.GetBytes(json);
                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    jsonStr = reader.ReadToEnd();
                }
                response.Close();
            } catch (Exception e) {
                throw new EHttp( "Http.put(): url:" + url+ " json: "+ json+" error:" + e.Message );
            }
            return jsonStr;
        }

        public static string delete(string url) {
            string jsonStr = "";
            try {
                WebRequest request = WebRequest.Create(url);
                request.Method = "DELETE";
                WebResponse response = request.GetResponse();
                using (Stream dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    jsonStr = reader.ReadToEnd();
                }
                response.Close();
            } catch (Exception e) {
                throw new EHttp( "Http.deete(): url:" + url+ " error:" + e.Message );
            }
            return jsonStr;
        }
    }
}