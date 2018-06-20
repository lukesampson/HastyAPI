using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml.Linq;

namespace HastyAPI {
	public class APIResponse {
		public WebHeaderCollection Headers { get; private set; }
		public int StatusCode { get; private set; }
		public string ContentType { get; private set; }
		public string Text { get; private set; }
		public CookieCollection Cookies { get; private set; }

		public APIResponse(WebHeaderCollection headers, int statusCode, string contentType, string text, CookieCollection cookies) {
			Headers = headers;
			StatusCode = statusCode;
			Text = text;
			Cookies = cookies;
		}

		public APIResponse EnsureStatus(int status) {
			if(this.StatusCode != status) {
				throw new Exception("Status code was " + StatusCode + ", expected " + status + "." + (Text != null ? " Server responded:\n" + Text : null));
			}
			return this;
		}

		public dynamic AsJSON() {
			return DynaJSON.Parse(Text);
		}

		public dynamic AsXML() {
			return DynaXML.Parse(Text);
		}
	}
}
