using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HastyAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests {
    [TestClass]
	public class FriendlyDynamicFacts {

		[TestMethod]
		public void UndefinedPropertyReturnsNull() {
			dynamic obj = new FriendlyDynamic();

			obj.one = "One";

			Assert.AreEqual("One", obj.one);
			Assert.IsNull(obj.two);
		}

        [TestClass]
		public class Serialization {


            [TestMethod]
			public void Simple_ToString_Matches_JSON() {
				var json = Shared.GetJSON("simple");
				var text = Shared.GetText("simple.js");

				Assert.AreEqual(text, json.ToString());
			}

            [TestMethod]
			public void List_Of_Primitives_Matches_JSON() {
				var json = Shared.GetJSON("list_of_primitives");
				var text = Shared.GetText("list_of_primitives.js");

                Assert.AreEqual(text, json.ToString());
			}

            [TestMethod]
			public void Simple_List_Matches_JSON() {
				var json = Shared.GetJSON("simple_list");
				var text = Shared.GetText("simple_list.js");

                Assert.AreEqual(text, json.ToString());
			}

            [TestMethod]
			public void Simple_Nested_Matches_JSON() {
				var json = Shared.GetJSON("simple_nested");
				var text = Shared.GetText("simple_nested.js");

                Assert.AreEqual(text, json.ToString());
			}
		}
	}
}
