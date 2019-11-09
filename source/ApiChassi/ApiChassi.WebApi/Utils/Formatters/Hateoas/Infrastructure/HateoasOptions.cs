using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;

namespace AspNetCore.Hateoas.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public class HateoasOptions
    {
        private readonly List<ILinksRequirement> _links;

        /// <summary>
        /// 
        /// </summary>
        public HateoasOptions()
        {
            _links = new List<ILinksRequirement>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<ILinksRequirement> Requirements => _links.AsReadOnly();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routeName"></param>
        /// <param name="values"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public HateoasOptions AddLink<T>(string routeName, Func<T, object> values = null)
            where T : class
        {
            return Add(new ResourceLink<T>(routeName, WrapRouteValuesSelector(values)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routeName"></param>
        /// <param name="Func{T, bool}"></param>
        /// <param name="predicate"></param>
        /// <param name="Func{T, object}>"></param>
        /// <param name="routeValuesSelector"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public HateoasOptions AddLinkWhen<T>(string routeName,
            Func<T, bool> predicate,
            Func<T, object> routeValuesSelector = null)
            where T : class
        {
            return Add(new ResourceLink<T>(
                routeName,
                WrapRouteValuesSelector(routeValuesSelector),
                predicate
            ));
        }

        private HateoasOptions Add(ILinksRequirement req)
        {
            _links.Add(req);

            return this;
        }

        private Func<T, RouteValueDictionary> WrapRouteValuesSelector<T>(Func<T, object> routeValuesSelector)
        {
            if (routeValuesSelector == null) return r => new RouteValueDictionary();
            return r => new RouteValueDictionary(routeValuesSelector(r));
        }
    }
}