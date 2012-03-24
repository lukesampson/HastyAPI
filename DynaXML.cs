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
			var result = new ExpandoObject() as IDictionary<string, object>;

			var root = xml.Root;
			AddElement(root, result);

			Func<string> tostring = () => DynamicToString(result);
			((dynamic)result).ToString = tostring;

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
				var obj = new ExpandoObject() as IDictionary<string, object>;

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

				Func<string> tostring = () => DynamicToString(obj);
				((dynamic)obj).ToString = tostring;

			} else { // simple value
				if(addToList) {
					list.Add(el.Value);
				} else {
					parent[name] = el.Value;
				}
			}
			
		}

		#region ToString
		private static string Indent(int nestlevel) {
			return new string(' ', 4 * nestlevel);;
		}

		private static string DynamicToString(IDictionary<string, object> obj, int nestlevel = 0) {
			var indent = Indent(nestlevel);

			var str = "";
			var i = 0;
			foreach(var pair in obj) {
				// ignore functions
				if(typeof(MulticastDelegate).IsAssignableFrom(pair.Value.GetType())) continue;

				if(i > 0) str += ",\r\n";

				str += indent + pair.Key + " = ";
				var list = pair.Value as IList<dynamic>;
				var dic = pair.Value as IDictionary<string, object>;

				if(list != null) {
					str += "[";
					for(var j = 0; j < list.Count; j++) {
						if(j > 0) str += ",";
						str += "\r\n" + Indent(nestlevel + 1);
						var subdic = list[j] as IDictionary<string, object>;
						if(subdic != null) {
							str += "{\r\n" + DynamicToString(subdic, nestlevel + 2) + "\r\n" + Indent(nestlevel + 1) + "}";
						} else {
							str += "\"" + list[j] + "\"";
						}
					}
					str += "\r\n" + indent + "]";
				} else if(dic != null) {
					str += "{\r\n" + DynamicToString(dic, nestlevel + 1) + "\r\n" + indent + "}";
				} else {
					str += "\"" + pair.Value.ToString() + "\"";
				}
				i++;
			}

			return str;
		}
		#endregion
	}

}

namespace System.Xml.Linq {

	public static class DynaXMLExtensions {
		public static dynamic ToDynamic(this XDocument doc) {
			return HastyAPI.DynaXML.Parse(doc);
		}
	}
}
