using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using HastyAPI;

namespace Tests {
	public class FriendlyDynamicFacts {

		[Fact]
		public void UndefinedPropertyReturnsNull() {
			dynamic obj = new FriendlyDynamic();

			obj.one = "One";

			Assert.Equal("One", obj.one);
			Assert.Null(obj.two);
		}

		public class Serialization {


			[Fact]
			public void Simple_ToString_Matches_JSON() {
				var json = Shared.GetJSON("simple");
				var text = Shared.GetText("simple.js");

				Assert.Equal(text, json.ToString());
			}

			[Fact]
			public void List_Of_Primitives_Matches_JSON() {
				var json = Shared.GetJSON("list_of_primitives");
				var text = Shared.GetText("list_of_primitives.js");

				Assert.Equal(text, json.ToString());
			}

			[Fact]
			public void Simple_List_Matches_JSON() {
				var json = Shared.GetJSON("simple_list");
				var text = Shared.GetText("simple_list.js");

				Assert.Equal(text, json.ToString());
			}

			[Fact]
			public void Simple_Nested_Matches_JSON() {
				var json = Shared.GetJSON("simple_nested");
				var text = Shared.GetText("simple_nested.js");

				Assert.Equal(text, json.ToString());
			}
		}
	}
}
