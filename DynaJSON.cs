using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Dynamic;
using Newtonsoft.Json.Linq;

namespace HastyAPI {
    public class DynaJSON {
        public static dynamic Parse(string text) {
            var js = Newtonsoft.Json.JsonConvert.DeserializeObject(text);
            return GetValue(js) ?? text;
        }

        private static dynamic GetObject(JObject jobj) {
            var obj = new FriendlyDynamic() as IDictionary<string, object>;
            foreach(var pair in jobj) {
                obj.Add(pair.Key.Replace('-', '_'), GetValue(pair.Value));
            }
            return obj;
        }

        private static object GetValue(object val) {
            if(val is JArray) {
                return GetList(val as JArray);
            } else if(val is JObject) {
                return GetObject(val as JObject);
            } else if(val is JValue) {
                return ((JValue)val).Value;
            }

            return val; // primitive
        }

        private static object GetToken(JToken token) {
            return token.GetType();
        }

        private static dynamic GetList(JArray ary) {
            var list = new List<object>(ary.Count);
            foreach(var e in ary) {
                list.Add(GetValue(e));
            }
            return list;
        }
    }
}
