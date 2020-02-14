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
            Children = new LinkedList<ReadOnlyTree<TKey, TValue>>();
        }

        public TKey Key { get; private set; }

        public TValue Item { get; private set; }

        private LinkedList<ReadOnlyTree<TKey, TValue>> Children { get; }

        private ReadOnlyTree<TKey, TValue> Parent { get; set; }

        public static ReadOnlyTree<TKey, TValue> BuildOne(IEnumerable<TValue> source, Func<TValue, TKey> keySelector, Func<TValue, TKey> parentKeySelector)
        {
            return BuildMany(source, keySelector, parentKeySelector).FirstOrDefault();
        }

        public static IEnumerable<ReadOnlyTree<TKey, TValue>> BuildMany(IEnumerable<TValue> source, Func<TValue, TKey> keySelector, Func<TValue, TKey> parentKeySelector)
        {
            var itemCache = source.DistinctBy(item => keySelector(item)).ToDictionary(keySelector, x => new ReadOnlyTree<TKey, TValue> { Key = keySelector(x), Item = x });
            var treeItems = itemCache.Values;
            foreach (var item in treeItems)
            {
                var parentKey = parentKeySelector(item.Item);
                if (parentKey != null && itemCache.TryGetValue(parentKey, out var parent))
                {
                    item.Parent = parent;
                    parent.Children.AddLast(item);
                }
            }
            return treeItems.Where(x => x.Parent == null);
        }

        public IEnumerable<TValue> Traverse()
        {
            return Children.SelectMany(n => n.Traverse()).Prepend(Item);
        }
    }
}
