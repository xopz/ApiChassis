using System;
using Microsoft.AspNetCore.Routing;

namespace AspNetCore.Hateoas.Infrastructure
{
	public class ResourceLink<T> : ILinksRequirement
	{
		private readonly Func<T, bool> _isLinkAllowed;
		private readonly Func<T, RouteValueDictionary> _valuesSelector;

		public ResourceLink(string name, Func<T, RouteValueDictionary> valuesSelector, Func<T, bool> isLinkAllowed = null)
		{
			ResourceType = typeof(T);
			Name = name;
			_valuesSelector = valuesSelector;
			_isLinkAllowed = isLinkAllowed;
		}

		public string Name { get; }

		public Type ResourceType { get; }

		public RouteValueDictionary GetRouteValues(object input)
		{
			return _valuesSelector((T) input);
		}

		public bool IsLinkAllowed(object input)
		{
			return _isLinkAllowed == null || _isLinkAllowed((T) input);
		}
	}
}