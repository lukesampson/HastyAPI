using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace HastyAPI {
	public class DynaJSON {
		public void Parse(string text) {
			var js = new JavaScriptSerializer().DeserializeObject(text);
		}
	}
}
