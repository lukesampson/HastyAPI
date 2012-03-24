HastyAPI
========

HastyAPI makes it easy to write code to interact with web APIs.
			
It'll save you writing lots of tedious boilerplate code to make HTTP web requests, and process XML and JSON. By providing high-level, dynamic functions, it lets you concentrate on the web API itself.

Examples
--------

__Getting a URL__

    var text = new APIRequest("http://www.google.com").Get().Text;

__Posting data to a URL__

    var result = new APIRequest("https://www.googleapis.com/urlshortener/v1/url")
        .WithData(@"{ ""longUrl"":""http://www.google.com/"" }", "application/json")
        .Post()
        .Text;

or, you could use the `WithJSON` shortcut to do the same thing:

    var result = new APIRequest("https://www.googleapis.com/urlshortener/v1/url")
        .WithJSON(new { longUrl = "http://www.google.com/" })
        .Post()
        .Text;

value of `result`:

    {
      "kind": "urlshortener#url",
	  "id": "http://goo.gl/fbsS",
	  "longUrl": "http://www.google.com/"
	}

__Using JSON data returned from the API___

    var clicks = new APIRequest("https://www.googleapis.com/urlshortener/v1/url")
        .WithForm(new { shortUrl = "http://goo.gl/fbsS", projection = "FULL" })
	    .Get()
	    .AsJSON().analytics.allTime.shortUrlClicks; // <-- dynamic!

    // note: WithForm automatically issues querystring variables for GET requests
	// you could also do:
	//   new APIRequest("https://www.googleapis.com/urlshortener/v1/url?shortUrl=http://goo.gl.fbsS&projection=FULL")