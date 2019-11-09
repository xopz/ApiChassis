namespace ApiChassi.WebApi.Utils.Extensions
{
    using System;
    using System.Collections;

    /// <summary>
    /// 
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="func"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static IEnumerable Select<TResult>(this IEnumerable source, Func<object, TResult> func)
        {
            foreach (var item in source)
            {
                yield return func(item);
            }
        }
    }
}
