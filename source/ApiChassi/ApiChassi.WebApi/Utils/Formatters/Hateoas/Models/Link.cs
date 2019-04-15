using Newtonsoft.Json;

namespace AspNetCore.Hateoas.Models
{
	public class Link
	{
		public Link(string rel, string href, string method)
		{
			Rel = rel;
			Href = href;
			Method = method;
		}

		[JsonProperty(Order = 1)]
		public string Href { get; private set; }

		[JsonProperty(Order = 0)]
		public string Rel { get; private set; }

		[JsonProperty(Order = 2)]
		public string Method { get; private set; }
	}
}