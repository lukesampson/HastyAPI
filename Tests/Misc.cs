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

        public void Test_Certificate() {
            APIRequest.ForceAcceptCertificate("control.windows");

            new APIRequest("https://control.windows:44301/")
                .Get();

            new APIRequest("https://localhost:44301/")
                .Get();
        }
	}
}
