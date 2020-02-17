using System.Collections.Generic;

namespace EncoreTickets.SDK.Utilities.DataStructures.Tree
{
    /// <summary>
    /// An interface for a tree data structure.
    /// </summary>
    /// <typeparam name="TKey">The type of a tree node key.</typeparam>
    /// <typeparam name="TValue">The type of a tree node.</typeparam>
    public interface ITree<out TKey, out TValue>
    {
        /// <summary>
        /// The key that defines the parent-child relationship in a tree.
        /// </summary>
        TKey Key { get; }

        /// <summary>
        /// The value of a tree node.
        /// </summary>
        TValue Item { get; }

        /// <summary>
        /// Traverses the tree and creates a collection of all nodes in the tree.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TValue> Traverse();
    }
}
