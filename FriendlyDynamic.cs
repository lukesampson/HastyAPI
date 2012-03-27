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
