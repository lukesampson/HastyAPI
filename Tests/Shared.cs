using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using HastyAPI;

namespace Tests {
	public class Shared {
		public static string GetText(string filename) {
			var path = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\Data\" + filename);
			return File.ReadAllText(path);
		}

		public static dynamic GetJSON(string name) {
			return DynaJSON.Parse(GetText(name + ".js"));
		}

		
	}
}
