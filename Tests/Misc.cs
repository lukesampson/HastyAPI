using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HastyAPI;

namespace Tests {

	public class Misc {

		public void Test_Headers() {
			new APIRequest("http://google.com").Get();
		}
	}
}
