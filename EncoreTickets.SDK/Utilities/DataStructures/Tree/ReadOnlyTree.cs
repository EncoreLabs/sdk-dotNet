using System;
using System.Collections.Generic;
using System.Linq;
using EncoreTickets.SDK.Utilities.BaseTypesExtensions;

namespace EncoreTickets.SDK.Utilities.DataStructures.Tree
{
    /// <summary>
    /// The implementation of the ITree&lt;TKey, TValue&gt; interface.
    /// It defines a tree that can be built based on the specified criteria and traversed afterwards.
    /// The tree nodes can have an arbitrary number of children.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class ReadOnlyTree<TKey, TValue> : ITree<TKey, TValue>
    {
        private ReadOnlyTree()
        {
            Children = new LinkedList<ReadOnlyTree<TKey, TValue>>();
        }

        /// <inheritdoc />
        public TKey Key { get; private set; }

        /// <inheritdoc />
        public TValue Item { get; private set; }

        private LinkedList<ReadOnlyTree<TKey, TValue>> Children { get; }

        private ReadOnlyTree<TKey, TValue> Parent { get; set; }

        /// <summary>
        /// Builds the collection of all trees that can be defined by the source collection and the key selectors.
        /// </summary>
        /// <param name="source">The source collection.</param>
        /// <param name="keySelector">The function to map an item to its key.</param>
        /// <param name="parentKeySelector">The function to map an item to the key of its parent.</param>
        /// <returns>The collection of the root nodes of the trees built based on the specified criteria.</returns>
        public static IEnumerable<ReadOnlyTree<TKey, TValue>> BuildAllTrees(IEnumerable<TValue> source, Func<TValue, TKey> keySelector, Func<TValue, TKey> parentKeySelector)
        {
            var treeItems = source.DistinctBy(item => keySelector(item)).Select(x => new ReadOnlyTree<TKey, TValue> { Key = keySelector(x), Item = x }).ToList();
            var itemCache = treeItems.ToDictionary(x => keySelector(x.Item), x => x);
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

        /// <inheritdoc />
        public IEnumerable<TValue> Traverse()
        {
            var traversedChildren = Children.SelectMany(n => n.Traverse());
            return EnumerableExtension.Prepend(traversedChildren, Item);
        }
    }
}
