using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Xunit;
using HastyAPI;
using System.Collections;

namespace Tests {
	public class DynaJSONFacts {

		[Fact]
		public void Parse_Returns_Not_Null() {
			var json = DynaJSON.Parse("{ test: 1 }");
			Assert.NotNull(json);
		}

		[Fact]
		public void Simple_Number_Correct() {
			var json = DynaJSON.Parse("{ test: 1 }");

			Assert.Equal(1, json.test);
		}

		[Fact]
		public void Simple_Multiple_Values_Correct() {
			var json = Shared.GetJSON("simple");

			Assert.Equal("one", json.simple);
			Assert.Equal(42, json.number);
		}

		[Fact]
		public void Simple_List_Correct() {
			var json = Shared.GetJSON("simple_list");

			Assert.IsAssignableFrom<IList>(json.list);

			Assert.Equal(2, json.list.Count);
		}

		[Fact]
		public void Simple_Nested_Correct() {
			var json = Shared.GetJSON("simple_nested");

			Assert.Equal("one", json.@base.example);
			Assert.True(json.@base.sub.this_one_nested);
		}

		[Fact]
		public void Hypens_In_Names_Converted_To_Underscores() {
			var json = Shared.GetJSON("with_hyphens");

			Assert.Equal("1", json.message_count);
		}
	}
}
