using System;
using Microsoft.AspNetCore.Routing;

namespace AspNetCore.Hateoas.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResourceLink<T> : ILinksRequirement
    {
        private readonly Func<T, bool> _isLinkAllowed;
        private readonly Func<T, RouteValueDictionary> _valuesSelector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="valuesSelector"></param>
        /// <param name="isLinkAllowed"></param>
        public ResourceLink(string name, Func<T, RouteValueDictionary> valuesSelector, Func<T, bool> isLinkAllowed = null)
        {
            ResourceType = typeof(T);
            Name = name;
            _valuesSelector = valuesSelector;
            _isLinkAllowed = isLinkAllowed;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        public Type ResourceType { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RouteValueDictionary GetRouteValues(object input)
        {
            return _valuesSelector((T)input);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IsLinkAllowed(object input)
        {
            return _isLinkAllowed == null || _isLinkAllowed((T)input);
        }
    }
}