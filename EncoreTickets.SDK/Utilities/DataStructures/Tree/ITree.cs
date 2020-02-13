using System.Collections.Generic;

namespace EncoreTickets.SDK.Utilities.DataStructures.Tree
{
    public interface ITree<out TKey, out TValue>
    {
        TKey Key { get; }

        TValue Item { get; }

        IEnumerable<TValue> Traverse();
    }
}
