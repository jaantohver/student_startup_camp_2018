using System;
using System.Text;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using static System.Diagnostics.Debug;

namespace ParkingApp
{
    public static class Networking
    {
        public static string UrlBase = "0.0.0.0";

        public static async Task<KeyValuePair<bool, string>> UploadImage (byte[] bytes, CancellationToken cancelToken = default (CancellationToken))
        {
            string url = UrlBase + "uploadImage";

            ByteArrayContent body = new ByteArrayContent (bytes);

            string error = "Unknown error occurred";

            using (HttpClient client = new HttpClient ()) {
                client.Timeout = TimeSpan.FromSeconds (30);

                try {
                    using (HttpResponseMessage response = await client.PostAsync (url, body, cancelToken))
                    using (HttpContent content = response.Content) {
                        string result = await content.ReadAsStringAsync ();

                        WriteLine (result);

                        JObject json = JObject.Parse (result);

                        if (json["status"]?.Value<string> () != null && json["status"].Value<string> () == "OK") {
                            return new KeyValuePair<bool, string> (true, json["imageId"].Value<string> ());
                        } else {
                            error = json["status"].Value<string> ();
                        }
                    }
                } catch (TaskCanceledException) {
                    //Timeout
                    error = "Timeout";
                } catch (HttpRequestException) {
                    //No Network
                    error = "No Internet Connection";
                } catch (JsonReaderException) {
                    //Response isn't in json format
                    error = "Error parsing json";
                }
            }

            return new KeyValuePair<bool, string> (false, error);
        }

        static async Task<string> Post (string endpoint, JObject json)
        {
            string url = UrlBase + endpoint;

            StringContent body = new StringContent (json.ToString (), Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient ()) {
                client.Timeout = TimeSpan.FromSeconds (30);

                string error = string.Empty;

                try {
                    using (HttpResponseMessage response = await client.PostAsync (url, body))
                    using (HttpContent content = response.Content) {
                        string result = await content.ReadAsStringAsync ();

                        return result;
                    }

                } catch (TaskCanceledException) {
                    //Timeout
                    error = "timeout";
                } catch (HttpRequestException) {
                    //No Network
                    error = "no network";
                }

                return "{\"" + "status" + "\":\"" + error + "\"}";
            }
        }
    }
}