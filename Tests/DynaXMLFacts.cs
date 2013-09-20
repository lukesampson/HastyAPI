using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using HastyAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests {
    [TestClass]
	public class DynaXMLFacts {

		public static XDocument GetXML(string name) {
			var path = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\Data\" + name + ".xml");
			return XDocument.Load(path);
		}

		[TestMethod]
		public void Can_Get_Root() {
			var o = GetXML("client_get_response").ToDynamic();
			Assert.IsNotNull(o.response);
		}

        [TestMethod]
		public void Can_Get_One_Child() {
			var o = GetXML("client_get_response").ToDynamic();
			Assert.IsNotNull(o.response.client);
		}

        [TestMethod]
		public void Can_Get_Content_Value() {
			var o = GetXML("client_get_response").ToDynamic();
			Assert.AreEqual("12", o.response.client.client_id);
		}

        [TestMethod]
		public void Can_Get_Attribute_Value_As_Well_As_Content_Value() {
			var o = GetXML("client_get_response").ToDynamic();

            Assert.AreEqual("AUD", o.response.client.credit.currency);
            Assert.AreEqual("true", o.response.client.credit.deprecated);
            Assert.AreEqual("0", o.response.client.credit.text);
		}

        [TestMethod]
		public void Simple_Element_Lists_Work() {
			var o = GetXML("dummy_lists").ToDynamic();

            Assert.AreEqual(2, o.lists.simple.item.Count);
			Assert.IsNull(o.lists.simple.text);
            Assert.AreEqual("one", o.lists.simple.item[0]);
            Assert.AreEqual("two", o.lists.simple.item[1]);
		}

        [TestMethod]
		public void Complex_Element_Lists_Work() {
			var o = GetXML("dummy_lists").ToDynamic();

            Assert.AreEqual(3, o.lists.complex.item.Count);
            Assert.AreEqual("one", o.lists.complex.item[0].test);
            Assert.AreEqual("complex", o.lists.complex.item[1].type);
            Assert.AreEqual("very_complex", o.lists.complex.item[2].type);
		}

        [TestMethod]
		public void Can_Call_As_List_On_List() {
			var o = GetXML("dummy_lists").ToDynamic();

			var list = Dynamic.AsList(o.lists.simple.item);
            Assert.AreEqual(2, list.Count);
		}

        [TestMethod]
		public void Can_Call_As_List_On_A_Non_List() {
			var o = GetXML("dummy_lists").ToDynamic();

			var list = Dynamic.AsList(o.lists.simple);
            Assert.AreEqual(1, list.Count);
		}

		public class Serialization {

			public void Test_Dummy() {
				var o = GetXML("dummy_lists").ToDynamic();
				Console.WriteLine(o.ToString());
			}

			public void Test_Real() {
				var o = GetXML("client_get_response").ToDynamic();

				Console.WriteLine(o.response.client.credit.ToString());
			}
		}
	}
}
