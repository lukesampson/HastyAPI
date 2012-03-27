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
	}
}
