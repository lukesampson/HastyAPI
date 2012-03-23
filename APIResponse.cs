using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HastyAPI {
	public class APIResponse {
		public int StatusCode { get; set; }
		public string Text { get; set; }

		public APIResponse EnsureStatus(int status) {
			if(this.StatusCode != status) {
				throw new Exception("Status code was " + StatusCode + ", expected " + status + "." + (Text != null ? " Server responded:\n" + Text : null));
			}
			return this;
		}
	}
}
