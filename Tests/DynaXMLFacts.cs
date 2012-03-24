using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using HastyAPI;
using Xunit;

namespace Tests {
	public class DynaXMLFacts {

		public static XDocument GetXML(string name) {
			var path = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\Data\" + name + ".xml");
			return XDocument.Load(path);
		}

		[Fact]
		public void Can_Get_Root() {
			var o = GetXML("client_get_response").ToDynamic();
			Assert.NotNull(o.response);
		}

		[Fact]
		public void Can_Get_One_Child() {
			var o = GetXML("client_get_response").ToDynamic();
			Assert.NotNull(o.response.client);
		}

		[Fact]
		public void Can_Get_Content_Value() {
			var o = GetXML("client_get_response").ToDynamic();
			Assert.Equal("12", o.response.client.client_id);
		}

		[Fact]
		public void Can_Get_Attribute_Value_As_Well_As_Content_Value() {
			var o = GetXML("client_get_response").ToDynamic();

			Assert.Equal("AUD", o.response.client.credit.currency);
			Assert.Equal("true", o.response.client.credit.deprecated);
			Assert.Equal("0", o.response.client.credit.text);
		}

		[Fact]
		public void Simple_Element_Lists_Work() {
			var o = GetXML("dummy_lists").ToDynamic();

			Assert.Equal(2, o.lists.simple.item.Count);
			Assert.Throws<Microsoft.CSharp.RuntimeBinder.RuntimeBinderException>(() => o.lists.simple.text);
			Assert.Equal("one", o.lists.simple.item[0]);
			Assert.Equal("two", o.lists.simple.item[1]);
		}

		[Fact]
		public void Complex_Element_Lists_Work() {
			var o = GetXML("dummy_lists").ToDynamic();

			Assert.Equal(3, o.lists.complex.item.Count);
			Assert.Equal("one", o.lists.complex.item[0].test);
			Assert.Equal("complex", o.lists.complex.item[1].type);
			Assert.Equal("very_complex", o.lists.complex.item[2].type);
		}

		[Fact]
		public void Can_Call_As_List_On_List() {
			var o = GetXML("dummy_lists").ToDynamic();

			var list = Dynamic.AsList(o.lists.simple.item);
			Assert.Equal(2, list.Count);
		}

		[Fact]
		public void Can_Call_As_List_On_A_Non_List() {
			var o = GetXML("dummy_lists").ToDynamic();

			var list = Dynamic.AsList(o.lists.simple);
			Assert.Equal(1, list.Count);
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
