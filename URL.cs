using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace HastyAPI {
	/// <summary>
	/// A more practical version of Uri.
	/// </summary>
	[TypeConverter(typeof(URLConverter))]
	public class URL {
		string _value;

		public URL(string value) {
			_value = value;
		}

		// convert from URL to string
		public static implicit operator string(URL value) {
			return value._value;
		}

		// convert from string to URL
		public static implicit operator URL(String value) {
			if(!Uri.IsWellFormedUriString(value, UriKind.Absolute)) {
				throw new Exception("That doesn't look like a URL");
			}
			return new URL(value);
		}

		public static string operator +(URL basePath, string relPath) {
			Uri combined;
			if(Uri.TryCreate(new Uri(basePath._value), relPath, out combined)) return new URL(combined.ToString());
			throw new ArgumentException("Couldn't combine base path \"" + basePath + "\" with relative path \"" + relPath + "\"");
		}

		public class URLConverter : TypeConverter {
			public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value) {
				if(value is string) return new URL((string)value);

				return base.ConvertFrom(context, culture, value);
			}
		}
	}
}
