using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HastyAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests {
    [TestClass]
	public class URLFacts {

		[TestMethod]
		public void Add_Operator_Works_On_Non_Root_Base_Paths() {
			URL basePath = "https://www.test.com/subdir/";
			Assert.AreEqual("https://www.test.com/subdir/rel", basePath + "/rel");
		}
	}
}
