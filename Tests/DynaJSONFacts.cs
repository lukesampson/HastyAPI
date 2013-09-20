using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using HastyAPI;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests {
    [TestClass]
	public class DynaJSONFacts {

		[TestMethod]
		public void Parse_Returns_Not_Null() {
			var json = DynaJSON.Parse("{ test: 1 }");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(json);
		}

        [TestMethod]
		public void Simple_Number_Correct() {
			var json = DynaJSON.Parse("{ test: 1 }");

			Assert.AreEqual(1, json.test);
		}

        [TestMethod]
		public void Simple_Multiple_Values_Correct() {
			var json = Shared.GetJSON("simple");

            Assert.AreEqual("one", json.simple);
            Assert.AreEqual(42, json.number);
		}

        [TestMethod]
		public void Simple_List_Correct() {
			var json = Shared.GetJSON("simple_list");

            Assert.IsInstanceOfType(json.list, typeof(IList));
			//Assert.IsAssignableFrom<IList>(json.list);

            Assert.AreEqual(2, json.list.Count);
		}

        [TestMethod]
		public void Simple_Nested_Correct() {
			var json = Shared.GetJSON("simple_nested");

            Assert.AreEqual("one", json.@base.example);
			Assert.IsTrue(json.@base.sub.this_one_nested);
		}

        [TestMethod]
		public void Hypens_In_Names_Converted_To_Underscores() {
			var json = Shared.GetJSON("with_hyphens");

            Assert.AreEqual("1", json.message_count);
		}

        [TestMethod]
		public void Array_Of_Objects() {
			var json = DynaJSON.Parse("[ { item: 1 }, { item: 2 } ]");

            Assert.AreEqual(2, json.Count);
            Assert.AreEqual(1, json[0].item);
            Assert.AreEqual(2, json[1].item);
		}

        [TestMethod]
		public void Empty_String() {
			var json = DynaJSON.Parse("");

            Assert.AreEqual("", json);
		}
	}
}
