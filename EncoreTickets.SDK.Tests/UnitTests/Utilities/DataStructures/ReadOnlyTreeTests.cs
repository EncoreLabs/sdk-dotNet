using System.Collections.Generic;
using System.Linq;
using EncoreTickets.SDK.Tests.Helpers;
using EncoreTickets.SDK.Utilities.DataStructures.Tree;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities.DataStructures
{
    internal class ReadOnlyTreeTests
    {
        private class TreeItem
        {
            public int Id { get; }

            public int? ParentId { get; }

            public TreeItem(int id, int? parentId)
            {
                Id = id;
                ParentId = parentId;
            }
        }

        [Test]
        public void BuildMany_Successful()
        {
            var testData = CreateTestData();

            var result = ReadOnlyTree<int?, TreeItem>.BuildAllTrees(testData.SetupList, item => item.Id, item => item.ParentId).ToList();

            Assert.AreEqual(testData.ExpectedParents.Count, result.Count);
            for (int i = 0; i < result.Count; i++)
            {
                AssertExtension.AreObjectsValuesEqual(testData.ExpectedParents[i], result[i].Item);
            }
        }

        [Test]
        public void BuildMany_WithoutNullParentKeys_Successful()
        {
            var item1 = new TreeItem(1, -1);
            var item2 = new TreeItem(2, 1);

            var result = ReadOnlyTree<int, TreeItem>.BuildAllTrees(new[] { item1, item2 }, item => item.Id, item => item.ParentId.Value).Single();

            AssertExtension.AreObjectsValuesEqual(item1, result.Item);
        }

        [Test]
        public void BuildMany_WithCycles_RemovesCyclesFromResult()
        {
            var item1 = new TreeItem(1, 2);
            var item2 = new TreeItem(2, 1);
            var item3 = new TreeItem(3, null);

            var result = ReadOnlyTree<int?, TreeItem>.BuildAllTrees(new[] { item1, item2, item3 }, item => item.Id, item => item.ParentId)
                .SelectMany(t => t.Traverse())
                .ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(item3, result[0]);
        }

        [Test]
        public void BuildMany_WithDuplicateKeys_IgnoresDuplicates()
        {
            var item1 = new TreeItem(1, -1);
            var item2 = new TreeItem(2, 1);
            var item3 = new TreeItem(2, -1);

            var result = ReadOnlyTree<int?, TreeItem>
                .BuildAllTrees(new[] { item1, item2, item3 }, item => item.Id, item => item.ParentId)
                .ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(item1, result[0].Item);
        }

        [Test]
        public void Traverse_Successful()
        {
            var testData = CreateTestData();
            var trees = ReadOnlyTree<int?, TreeItem>.BuildAllTrees(testData.SetupList, item => item.Id, item => item.ParentId).ToList();

            var result = trees.SelectMany(tree => tree.Traverse()).ToList();

            Assert.AreEqual(testData.ExpectedTraversalList.Count, result.Count);
            for (int i = 0; i < result.Count; i++)
            {
                AssertExtension.AreObjectsValuesEqual(testData.ExpectedTraversalList[i], result[i]);
            }
        }

        private (List<TreeItem> SetupList, List<TreeItem> ExpectedTraversalList, List<TreeItem> ExpectedParents) CreateTestData()
        {
            var tree1Parent = new TreeItem(1, null);
            var tree2Parent = new TreeItem(2, null);
            var tree1Level1Item1 = new TreeItem(3, 1);
            var tree1Level1Item2 = new TreeItem(4, 1);
            var tree1Level1Item3 = new TreeItem(5, 1);
            var tree2Level1Item1 = new TreeItem(6, 2);
            var tree2Level1Item2 = new TreeItem(7, 2);
            var tree1Level2Item1 = new TreeItem(8, 4);
            var setupList = new List<TreeItem>
            {
                tree1Parent, tree2Parent, tree1Level1Item1, tree1Level1Item2, tree1Level1Item3, tree2Level1Item1, tree2Level1Item2, tree1Level2Item1,
            };
            var expectedTraversalList = new List<TreeItem>
            {
                tree1Parent,
                tree1Level1Item1,
                tree1Level1Item2,
                tree1Level2Item1,
                tree1Level1Item3,
                tree2Parent,
                tree2Level1Item1,
                tree2Level1Item2,
            };
            var expectedParents = new List<TreeItem>
            {
                tree1Parent, tree2Parent,
            };
            return (setupList, expectedTraversalList, expectedParents);
        }
    }
}
