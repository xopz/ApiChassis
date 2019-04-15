using System;
using System.Buffers;
using AspNetCore.Hateoas.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace Microsoft.Extensions.DependencyInjection
{
	internal class JsonHateoasMvcOptionsSetup : IConfigureOptions<MvcOptions>
	{
		private readonly ArrayPool<char> _charPool;
		private readonly JsonSerializerSettings _jsonSerializerSettings;

		public JsonHateoasMvcOptionsSetup(IOptions<MvcJsonOptions> jsonOptions, ArrayPool<char> charPool)
		{
			if (jsonOptions == null)
				throw new ArgumentNullException(nameof(jsonOptions));

			_jsonSerializerSettings = jsonOptions.Value.SerializerSettings;
			_charPool = charPool ?? throw new ArgumentNullException(nameof(charPool));
		}

		public void Configure(MvcOptions options)
		{
			options.OutputFormatters.Add(new JsonHateoasOutputFormatter(_jsonSerializerSettings, _charPool));
			options.FormatterMappings.SetMediaTypeMappingForFormat(
				"json+hateoas",
				MediaTypeHeaderValue.Parse((StringSegment) JsonHateoasOutputFormatter.ApplicationJsonHateoas)
			);
		}
	}
}