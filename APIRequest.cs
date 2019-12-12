using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace HastyAPI {
    public class APIRequest {
        private string _url;
        private object _headers;
		private string _agent;
        private string _data;
        private NetworkCredential _credentials;
        private Encoding _encoding = Encoding.UTF8;
        private string _contentType;
		private CookieCollection _cookies;
		private bool _autoRedirect = true;

        public APIRequest(string url) {
            _url = url;
        }

        public APIRequest WithHeaders(object headers) {
            _headers = headers;
            return this;
        }

        public APIRequest WithForm(object vars) {
            string data = "";
            var dic = vars.AsDictionary();
            foreach(var pair in dic) {
                if(data.Length > 0) data += "&";
                var value = pair.Value;
                if(value == null) value = "";
                data += HttpUtility.UrlEncode(pair.Key) + "=" + HttpUtility.UrlEncode(value);
            }
            _data = data;
            _contentType = "application/x-www-form-urlencoded";
            return this;
        }

        public APIRequest WithJSON(object json) {
            _data = new JavaScriptSerializer().Serialize(json);
            _contentType = "application/json";
            return this;
        }

        public APIRequest WithData(string data, string contentType = null) {
            _data = data;
            _contentType = contentType;
            return this;
        }

        public APIRequest WithBasicCredentials(string username, string password) {
            _credentials = new NetworkCredential(username, password);
            return this;
        }

        public APIRequest WithEncoding(Encoding encoding) {
            _encoding = encoding;
            return this;
        }

		public APIRequest WithUserAgent(string agent) {
			_agent = agent;
			return this;
		}

		public APIRequest WithCookies(CookieCollection cookies) {
			if(_cookies == null) {
				_cookies = cookies;
			} else {
				_cookies.Add(cookies);
			}
			return this;
		}

		public APIRequest NoAutoRedirect() {
			_autoRedirect = false;
			return this;
		}

        public APIResponse Post() {
            return Send("POST");
        }

        public APIResponse Get() {
            return Send("GET");
        }

        public APIResponse Put() {
            return Send("PUT");
        }

        public APIResponse Send(string method) {
			HttpWebRequest req = null;

			if(_data != null) {
				if(method.Equals("GET", StringComparison.OrdinalIgnoreCase)) {
					req = (HttpWebRequest)WebRequest.Create(_url + "?" + _data);

					SetCommon(req, method);
					// note: don't send content type for get

				} else {
					req = (HttpWebRequest)WebRequest.Create(_url);
					SetCommon(req, method);

					req.ContentType = _contentType;

					var dataBytes = _encoding.GetBytes(_data);
					req.ContentLength = dataBytes.Length;

					var reqStream = req.GetRequestStream();
					reqStream.Write(dataBytes, 0, dataBytes.Length);
					reqStream.Close();
				}
			} else {
				req = (HttpWebRequest)WebRequest.Create(_url);
				if(method.Equals("POST", StringComparison.OrdinalIgnoreCase)) {
					req.ContentLength = 0;
				}
				SetCommon(req, method);
			}

			HttpWebResponse response;
			try {
				response = (HttpWebResponse)req.GetResponse();
			} catch(WebException e) {
				if(e.Status == WebExceptionStatus.ConnectFailure) {
					throw new Exception("Couldn't connect to " + req.RequestUri.GetLeftPart(UriPartial.Authority), e);
                } else if(e.Status == WebExceptionStatus.TrustFailure) {
                    throw new Exception("Bad SSL certificate for " + req.RequestUri.GetLeftPart(UriPartial.Authority), e);
                }
				return e.Response.ToAPIResponse();
			}

			return response.ToAPIResponse();
		}

		void SetCommon(HttpWebRequest req, string method) {
			req.WithCredentials(_credentials)
				.WithHeaders(_headers).Method = method;

			req.AllowAutoRedirect = _autoRedirect;

			req.CookieContainer = new CookieContainer();
			if(_cookies != null) {
				req.CookieContainer.Add(_cookies);
			}

			if(_agent != null) {
				req.WithUserAgent(_agent);
			}
		}

        static HashSet<string> forceAcceptHosts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// Always accept SSL certificates for the specified host name.
        /// </summary>
        /// <example>
        /// if(HttpContext.Current.IsDebuggingEnabled) APIRequest.ForceAcceptCertificate("localhost");
        /// </example>
        /// <param name="host"></param>
        public static void ForceAcceptCertificate(string host) {
            lock(forceAcceptHosts) {
                if(!forceAcceptHosts.Contains(host)) forceAcceptHosts.Add(host);
            }
            ServicePointManager.ServerCertificateValidationCallback = AcceptForcedCertificateCallback;
        }

        private static bool AcceptForcedCertificateCallback(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors errors) {
            var req = sender as HttpWebRequest;
            if(req == null) return errors == SslPolicyErrors.None;

            lock(forceAcceptHosts) {
                return forceAcceptHosts.Contains(req.RequestUri.Host);
            }
        }
    }
}
