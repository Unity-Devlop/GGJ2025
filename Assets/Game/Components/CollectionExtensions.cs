using System.Collections.Generic;
using System.Linq;

namespace UnityToolkit
{

    public static class CollectionExtensions
    {
        public static T RandomTake<T>(this ICollection<T> collection)
        {
            if (collection.Count == 0) return default;
            return collection.ElementAt(UnityEngine.Random.Range(0, collection.Count - 1));
        }

        public static T RandomTake<T>(this IReadOnlyList<T> list)
        {
            if (list.Count == 0) return default;
            return list[UnityEngine.Random.Range(0, list.Count - 1)];
        }

        public static T RandomTake<T>(this List<T> list)
        {
            if (list.Count == 0) return default;
            return list[UnityEngine.Random.Range(0, list.Count - 1)];
        }
    }
}