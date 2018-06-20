using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace HastyAPI {
	public static class Extensions {
		public static APIResponse ToAPIResponse(this WebResponse wr) {
			var hwr = wr as HttpWebResponse;
			if(hwr == null) throw new Exception("Not an HttpWebResponse");

			var sr = new StreamReader(hwr.GetResponseStream());
			var responseText = sr.ReadToEnd();
			sr.Close();

			return new APIResponse(hwr.Headers, (int)hwr.StatusCode,
				hwr.ContentType, responseText, hwr.Cookies);
		}

		public static HttpWebRequest WithCredentials(this HttpWebRequest request, NetworkCredential credentials) {
			if(credentials != null) {
				request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(credentials.UserName + ":" + credentials.Password));
			}
			return request;
		}

		public static HttpWebRequest WithUserAgent(this HttpWebRequest request, string agent) {
			request.UserAgent = agent;
			return request;
		}

		public static HttpWebRequest WithHeaders(this HttpWebRequest request, object headers) {
			if(headers != null) {
				foreach(var pair in headers.AsDictionary()) {
					request.Headers.Add(pair.Key, pair.Value);
				}
			}
			return request;
		}

		public static IDictionary<string, string> AsDictionary(this object obj) {
			IDictionary<string, string> vardic;
			if(obj is IDictionary<string, string>) {
				vardic = (IDictionary<string, string>)obj;
			} else {
				vardic = new Dictionary<string, string>();
				foreach(var prop in obj.GetType().GetProperties()) {
					var value = prop.GetValue(obj, null);
					vardic.Add(prop.Name, value.ToString());
				}
			}
			return vardic;
		}
	}
}
