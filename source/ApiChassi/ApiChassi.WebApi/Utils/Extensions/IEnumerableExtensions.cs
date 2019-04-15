namespace ApiChassi.WebApi.Utils.Extensions
{
    using System;
    using System.Collections;

    public static class IEnumerableExtensions
    {
        public static IEnumerable Select<TResult>(this IEnumerable source, Func<object, TResult> func)
        {
            foreach (var item in source)
            {
                yield return func(item);
            }
        }
    }
}
