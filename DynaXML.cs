using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Dynamic;

namespace HastyAPI {
	public class DynaXML {

		public static dynamic Parse(string text) {
			return Parse(XDocument.Parse(text));
		}

		public static dynamic Parse(XDocument xml) {
			var result = new FriendlyDynamic() as IDictionary<string, object>;

			var root = xml.Root;
			AddElement(root, result);

			return result;
		}

		private static void AddElement(XElement el, IDictionary<string, object> parent) {
			var name = el.Name.LocalName;

			// check for multiple elements with the same name and convert to list
			var addToList = parent.ContainsKey(name);
			IList<dynamic> list = null;
			if(addToList) {
				var container = parent[name];
				list = container as IList<dynamic>;
				if(list == null) {
					// convert to list
					list = new List<dynamic>(new dynamic[] { container });
					parent[name] = list;
				}
			}

			if(el.HasElements || el.HasAttributes) { // complex object
				var obj = new FriendlyDynamic() as IDictionary<string, object>;

				if(addToList) {
					list.Add(obj);
				} else {
					parent[name] = obj;
				}

				foreach(var attr in el.Attributes()) {
					obj[attr.Name.LocalName] = attr.Value;
				}

				foreach(var child in el.Elements()) {
					AddElement(child, obj);
				}

				if(!el.HasElements && !el.IsEmpty) {
					var valName = Char.IsUpper(name[0]) ? "Text" : "text"; // mimic case
					obj[valName] = el.Value;
				}
			} else { // simple value
				if(addToList) {
					list.Add(el.Value);
				} else {
					parent[name] = el.Value;
				}
			}
			
		}
	}

}

namespace System.Xml.Linq {

	public static class DynaXMLExtensions {
		public static dynamic ToDynamic(this XDocument doc) {
			return HastyAPI.DynaXML.Parse(doc);
		}
	}
}
