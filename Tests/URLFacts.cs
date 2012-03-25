using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HastyAPI;
using Xunit;

namespace Tests {
	public class URLFacts {

		[Fact]
		public void Add_Operator_Works_On_Non_Root_Base_Paths() {
			URL basePath = "https://www.test.com/subdir/";
			Assert.Equal("https://www.test.com/subdir/rel", basePath + "/rel");
		}
	}
}
