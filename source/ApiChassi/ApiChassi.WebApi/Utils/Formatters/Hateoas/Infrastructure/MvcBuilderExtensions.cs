using System;
using System.Buffers;
using AspNetCore.Hateoas.Formatters;
using AspNetCore.Hateoas.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class MvcBuilderExtensions
	{
		public static IMvcBuilder AddHateoas(this IMvcBuilder builder, Action<HateoasOptions> options = null)
		{
			if (options != null) builder.Services.Configure(options);

			builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
			builder.Services.TryAdd(ServiceDescriptor
				.Singleton(serviceProvider => new JsonHateoasOutputFormatter(
					serviceProvider.GetRequiredService<IOptions<MvcJsonOptions>>().Value.SerializerSettings,
					serviceProvider.GetRequiredService<ArrayPool<char>>()))
			);

			builder.Services.TryAddEnumerable(ServiceDescriptor
				.Transient<IConfigureOptions<MvcOptions>, JsonHateoasMvcOptionsSetup>());
			return builder;
		}
	}
}