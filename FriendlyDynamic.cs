using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace HastyAPI {

	/// <summary>
	/// A javascript-like dynamic object that returns null for undefined properties
	/// </summary>
	public class FriendlyDynamic : DynamicObject, IDictionary<string, object> {
		Dictionary<string, object> _dictionary = new Dictionary<string, object>();

		public override bool TrySetMember(SetMemberBinder binder, object value) {
			_dictionary[binder.Name] = value;
			return true;
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result) {
			if(!_dictionary.TryGetValue(binder.Name, out result))
				result = null;
			return true;
		}

		public override string ToString() {
			return DynamicToString(this, 0);
		}

		private static string DynamicToString(FriendlyDynamic dyn, int level) {
			var canCompact = level < 2 && CanCompact(dyn);

			var str = "{";
			str += canCompact ? " " : "\r\n";

			var i = 0;
			foreach(var pair in dyn._dictionary) {
				str += PairToString(pair, level + 1, canCompact);
				if(i < dyn._dictionary.Count - 1) str += "," + (canCompact ? " " : "");
				str += canCompact ? "" : "\r\n";
				i++;
			}

			str += (canCompact ? " " : Indent(level)) + "}";
			return str;
		}

		private static string PairToString(KeyValuePair<string, object> pair, int level, bool canCompact) {
			var str = canCompact ? "" : Indent(level);
			str += "\"" + pair.Key + "\": " + ObjectToString(pair.Value, level);
			return str;
		}

		private static string ObjectToString(object obj, int level) {
			if(obj is IList<object>) {
				return ListToString(obj as IList<object>, level);
			} else if(obj is FriendlyDynamic) {
				return DynamicToString(obj as FriendlyDynamic, level);
			} else if(obj is string) {
				return "\"" + obj + "\"";
			} else if(obj is bool) {
				return obj.ToString().ToLower();
			} else return obj.ToString();
		}

		private static string ListToString(IList<object> list, int level) {
			var canCompact = CanCompact(list);

			var str = "[";
			str += canCompact ? "" : "\r\n";

			var i = 0;
			foreach(var obj in list) {
				str += (canCompact ? (i == 0 ? "" : " ") : Indent(level + 1)) + ObjectToString(obj, level + 1);
				if(i < list.Count - 1) str += ",";
				str += canCompact ? "" : "\r\n";
				i++;
			}
			str += (canCompact ? "" : Indent(level)) + "]";
			return str;
		}

		private static bool CanCompact(FriendlyDynamic dyn) {
			if(dyn._dictionary.Count > 5) return false;
			foreach(var pair in dyn._dictionary) {
				if(!CanCompact(pair.Value)) return false;
			}
			return true;
		}

		private static bool CanCompact(object obj) {
			if(obj is List<object>) return false;
			if(obj is FriendlyDynamic) return false;
			return true;
		}

		private static bool CanCompact(IList<object> list) {
			foreach(var obj in list) {
				if(!CanCompact(obj)) return false;
			}
			return true;
		}

		private static string Indent(int level) {
			return new string(' ', level * 4);
		}

		#region IDictionary<TKey, TValue> implementation
		void IDictionary<string, object>.Add(string key, object value) {
			_dictionary.Add(key, value);
		}

		bool IDictionary<string, object>.ContainsKey(string key) {
			return _dictionary.ContainsKey(key);
		}

		ICollection<string> IDictionary<string, object>.Keys {
			get { return _dictionary.Keys; }
		}

		bool IDictionary<string, object>.Remove(string key) {
			return _dictionary.Remove(key);
		}

		bool IDictionary<string, object>.TryGetValue(string key, out object value) {
			return _dictionary.TryGetValue(key, out value);
		}

		ICollection<object> IDictionary<string, object>.Values {
			get { return _dictionary.Values; }
		}

		object IDictionary<string, object>.this[string key] {
			get {
				return _dictionary[key];
			}
			set {
				_dictionary[key] = value;
			}
		}

		void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item) {
			((ICollection<KeyValuePair<string, object>>)_dictionary).Add(item);
		}

		void ICollection<KeyValuePair<string, object>>.Clear() {
			((ICollection<KeyValuePair<string, object>>)_dictionary).Clear();
		}

		bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item) {
			return ((ICollection<KeyValuePair<string, object>>)_dictionary).Contains(item);
		}

		void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) {
			((ICollection<KeyValuePair<string, object>>)_dictionary).CopyTo(array, arrayIndex);
		}

		int ICollection<KeyValuePair<string, object>>.Count {
			get {
				return ((ICollection<KeyValuePair<string, object>>)_dictionary).Count;
			}
		}

		bool ICollection<KeyValuePair<string, object>>.IsReadOnly {
			get {
				return ((ICollection<KeyValuePair<string, object>>)_dictionary).IsReadOnly;
			}
		}

		bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item) {
			return ((ICollection<KeyValuePair<string, object>>)_dictionary).Remove(item);
		}

		IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator() {
			return ((ICollection<KeyValuePair<string, object>>)_dictionary).GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return ((System.Collections.IEnumerable)_dictionary).GetEnumerator();
		}
		#endregion
	}
}
