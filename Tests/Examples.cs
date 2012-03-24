using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HastyAPI;

namespace Tests {
	public class Examples {

		public void Get_A_URL() {
			var text = new APIRequest("https://www.googleapis.com/urlshortener/v1/url?shortUrl=http://goo.gl/fbsS").Get().Text;
			Console.WriteLine(text);
		}

		public void Post_Data() {
			var result = new APIRequest("https://www.googleapis.com/urlshortener/v1/url")
				.WithData(@"{ ""longUrl"":""http://www.google.com/"" }", "application/json")
				.Post()
				.Text;

			Console.WriteLine(result);
		}

		public void Post_JSON_Data() {
			var result = new APIRequest("https://www.googleapis.com/urlshortener/v1/url")
				.WithJSON(new { longUrl = "http://www.google.com/" })
				.Post()
				.Text;

			Console.WriteLine(result);
		}

		public void Working_With_JSON_Data() {
			var clicks = new APIRequest("https://www.googleapis.com/urlshortener/v1/url")
				.WithForm(new {	shortUrl = "http://goo.gl/fbsS", projection = "FULL" })
				.Get()
				.AsJSON().analytics.allTime.shortUrlClicks; // <-- dynamic!

			// note: WithFormData data are automatically issued as querystring variables for GET requests
			// You could also do:
			//   new APIRequest("https://www.googleapis.com/urlshortener/v1/url?shortUrl=http://goo.gl.fbsS&projection=FULL")

			Console.WriteLine(clicks);
		}
	}
}
