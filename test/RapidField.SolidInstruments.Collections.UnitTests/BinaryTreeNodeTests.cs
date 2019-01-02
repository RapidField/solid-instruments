// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace RapidField.SolidInstruments.Collections.UnitTests
{
    [TestClass]
    public class BinaryTreeNodeTests
    {
        [TestMethod]
        public void AddChild_ShouldProduceDesiredResults_ForTargetContainingChildNodeArgument()
        {
            // Arrange.
            var childNode = new BinaryTreeNode<Int32>();
            var target = new BinaryTreeNode<Int32>(0);
            target.AddChild(childNode);

            // Act.
            var result = target.AddChild(childNode);

            // Assert.
            result.Should().BeFalse();
        }

        [TestMethod]
        public void AddChild_ShouldProduceDesiredResults_ForTargetNotContainingChildNodeArgument()
        {
            // Arrange.
            var childNode = new BinaryTreeNode<Int32>();
            var target = new BinaryTreeNode<Int32>();

            // Act.
            var result = target.AddChild(childNode);

            // Assert.
            result.Should().BeTrue();
            target.Children.Should().Contain(childNode);
            childNode.Parent.Should().BeSameAs(target);
        }

        [TestMethod]
        public void AddChild_ShouldRaiseArgumentNullException_ForNullChildNodeArgument()
        {
            // Arrange.
            var childNode = (BinaryTreeNode<Int32>)null;
            var target = new BinaryTreeNode<Int32>();

            // Act.
            var action = new Action(() =>
            {
                target.AddChild(childNode);
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>($"because {nameof(childNode)} is null");
        }

        [TestMethod]
        public void Constructor_ShouldProduceDesiredResults_ForValidArguments()
        {
            // Arrange.
            var target = (BinaryTreeNode<Int32>)null;
            var value = 1;
            var childOne = new BinaryTreeNode<Int32>(2);
            var childTwo = new BinaryTreeNode<Int32>(3);

            // Act.
            var action = new Action(() =>
            {
                target = new BinaryTreeNode<Int32>(value, childOne, childTwo);
            });

            // Assert.
            action.Should().NotThrow();
            target.Children.Count().Should().Be(2);
            target.IsLeaf.Should().BeFalse();
            target.IsRoot.Should().BeTrue();
            target.Parent.Should().BeNull();
            target.Value.Should().Be(value);
            childOne.IsLeaf.Should().BeTrue();
            childOne.IsRoot.Should().BeFalse();
            childOne.Parent.Should().BeSameAs(target);
            childTwo.IsLeaf.Should().BeTrue();
            childTwo.IsRoot.Should().BeFalse();
            childTwo.Parent.Should().BeSameAs(target);
        }

        [TestMethod]
        public void Constructor_ShouldRaiseArgumentException_ForChildCollectionWithNullNode()
        {
            // Arrange.
            var target = (BinaryTreeNode<Int32>)null;
            var value = 1;
            var childOne = new BinaryTreeNode<Int32>(2);
            var childTwo = (BinaryTreeNode<Int32>)null;

            // Act.
            var action = new Action(() =>
            {
                target = new BinaryTreeNode<Int32>(value, childOne, childTwo);
            });

            // Assert.
            action.Should().Throw<ArgumentException>($"because {nameof(childTwo)} is null");
        }

        [TestMethod]
        public void Depth_ShouldReturnValidResult()
        {
            // Arrange.
            var leafNodeOne = new BinaryTreeNode<Int32>(1);
            var leafNodeTwo = new BinaryTreeNode<Int32>(2);
            var leafNodeThree = new BinaryTreeNode<Int32>(3);
            var leafNodeFour = new BinaryTreeNode<Int32>(4);
            var leafNodeFive = new BinaryTreeNode<Int32>(5);
            var rootNode = new BinaryTreeNode<Int32>(6,
                new BinaryTreeNode<Int32>(7,
                    new BinaryTreeNode<Int32>(9,
                        leafNodeOne,
                        new BinaryTreeNode<Int32>(12,
                            leafNodeFive)),
                    new BinaryTreeNode<Int32>(8,
                        new BinaryTreeNode<Int32>(10,
                            leafNodeTwo),
                        new BinaryTreeNode<Int32>(11,
                            leafNodeThree,
                            leafNodeFour))));

            // Assert.
            leafNodeOne.Depth.Should().Be(3);
            leafNodeOne.Parent.Depth.Should().Be(2);
            leafNodeOne.Parent.Parent.Depth.Should().Be(1);
            leafNodeTwo.Depth.Should().Be(4);
            leafNodeThree.Depth.Should().Be(4);
            leafNodeFour.Depth.Should().Be(4);
            leafNodeFive.Depth.Should().Be(4);
            leafNodeFive.Parent.Parent.Depth.Should().Be(2);
            rootNode.Depth.Should().Be(0);
        }

        [TestMethod]
        public void Height_ShouldReturnValidResult()
        {
            // Arrange.
            var leafNodeOne = new BinaryTreeNode<Int32>(1);
            var leafNodeTwo = new BinaryTreeNode<Int32>(2);
            var leafNodeThree = new BinaryTreeNode<Int32>(3);
            var leafNodeFour = new BinaryTreeNode<Int32>(4);
            var leafNodeFive = new BinaryTreeNode<Int32>(5);
            var rootNode = new BinaryTreeNode<Int32>(6,
                new BinaryTreeNode<Int32>(7,
                    new BinaryTreeNode<Int32>(9,
                        leafNodeOne,
                        new BinaryTreeNode<Int32>(12,
                            leafNodeFive)),
                    new BinaryTreeNode<Int32>(8,
                        new BinaryTreeNode<Int32>(10,
                            leafNodeTwo),
                        new BinaryTreeNode<Int32>(11,
                            leafNodeThree,
                            leafNodeFour))));

            // Assert.
            leafNodeOne.Height.Should().Be(0);
            leafNodeTwo.Height.Should().Be(0);
            leafNodeThree.Height.Should().Be(0);
            leafNodeFour.Height.Should().Be(0);
            leafNodeFive.Height.Should().Be(0);
            leafNodeFive.Parent.Parent.Height.Should().Be(2);
            rootNode.Height.Should().Be(4);
        }

        [TestMethod]
        public void RemoveChild_ShouldProduceDesiredResults_ForTargetContainingChildNodeArgument()
        {
            // Arrange.
            var childNode = new BinaryTreeNode<Int32>();
            var target = new BinaryTreeNode<Int32>(0, childNode);

            // Act.
            var result = target.RemoveChild(childNode);

            // Assert.
            result.Should().BeTrue();
            target.Children.Should().NotContain(childNode);
            childNode.Parent.Should().BeNull();
        }

        [TestMethod]
        public void RemoveChild_ShouldProduceDesiredResults_ForTargetNotContainingChildNodeArgument()
        {
            // Arrange.
            var childNode = new BinaryTreeNode<Int32>();
            var target = new BinaryTreeNode<Int32>();

            // Act.
            var result = target.RemoveChild(childNode);

            // Assert.
            result.Should().BeFalse();
        }

        [TestMethod]
        public void RemoveChild_ShouldRaiseArgumentNullException_ForNullChildNodeArgument()
        {
            // Arrange.
            var target = new BinaryTreeNode<Int32>();
            var childNode = (BinaryTreeNode<Int32>)null;

            // Act.
            var action = new Action(() =>
            {
                target.RemoveChild(childNode);
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>($"because {nameof(childNode)} is null");
        }
    }
}