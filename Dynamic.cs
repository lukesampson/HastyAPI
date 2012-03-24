using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HastyAPI {
	public static class Dynamic {
		public static List<dynamic> AsList(object obj) {
			var list = obj as List<dynamic>;
			if(list != null) return list;

			return new List<dynamic>(new dynamic[] { obj });
		}
	}
}
