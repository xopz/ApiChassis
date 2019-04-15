using System;
using Microsoft.AspNetCore.Routing;

namespace AspNetCore.Hateoas.Infrastructure
{
	public interface ILinksRequirement
	{
		string Name { get; }
		Type ResourceType { get; }

		RouteValueDictionary GetRouteValues(object input);
		bool IsLinkAllowed(object input);
	}
}