using System;
using System.Collections.Generic;
using System.Linq;
using EncoreTickets.SDK.Utilities.BaseTypesExtensions;

namespace EncoreTickets.SDK.Utilities.DataStructures.Tree
{
    public class ReadOnlyTree<TKey, TValue> : ITree<TKey, TValue>
    {
        private ReadOnlyTree()
        {
            Children = new List<ReadOnlyTree<TKey, TValue>>();
        }

        public TKey Key { get; private set; }

        public TValue Item { get; private set; }

        private List<ReadOnlyTree<TKey, TValue>> Children { get; }

        private ReadOnlyTree<TKey, TValue> Parent { get; set; }

        public static ReadOnlyTree<TKey, TValue> BuildOne(IEnumerable<TValue> source, Func<TValue, TKey> keySelector, Func<TValue, TKey> parentKeySelector)
        {
            return BuildMany(source, keySelector, parentKeySelector).FirstOrDefault();
        }

        public static IEnumerable<ReadOnlyTree<TKey, TValue>> BuildMany(IEnumerable<TValue> source, Func<TValue, TKey> keySelector, Func<TValue, TKey> parentKeySelector)
        {
            var itemCache = source.DistinctBy(item => keySelector(item)).ToDictionary(keySelector, x => new ReadOnlyTree<TKey, TValue> { Key = keySelector(x), Item = x });
            foreach (var item in itemCache.Values)
            {
                var parentKey = parentKeySelector(item.Item);
                if (parentKey != null && itemCache.TryGetValue(parentKey, out var parent))
                {
                    item.Parent = parent;
                    parent.Children.Add(item);
                }
            }
            return itemCache.Values.Where(x => x.Parent == null);
        }

        public IEnumerable<TValue> Traverse()
        {
            var traversedChildren = Children.SelectMany(n => n.Traverse());
            return EnumerableExtension.Prepend(traversedChildren, Item);
        }
    }
}
