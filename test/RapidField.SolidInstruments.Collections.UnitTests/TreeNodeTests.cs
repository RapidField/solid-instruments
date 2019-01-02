// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RapidField.SolidInstruments.Collections.UnitTests
{
    [TestClass]
    public class TreeNodeTests
    {
        [TestMethod]
        public void AddChild_ShouldProduceDesiredResults_ForTargetContainingChildNodeArgument()
        {
            // Arrange.
            var childNode = new TestTreeNode();
            var target = new TreeNode<Int32, TestTreeNode>(0);
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
            var childNode = new TestTreeNode();
            var target = new TreeNode<Int32, TestTreeNode>();

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
            var childNode = (TestTreeNode)null;
            var target = new TreeNode<Int32, TestTreeNode>();

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
            var target = (TestTreeNode)null;
            var value = 1;
            var childOne = new TestTreeNode(2);
            var childTwo = new TestTreeNode(3);
            var childThree = new TestTreeNode(5);
            var children = new TestTreeNode[]
            {
                childOne,
                childTwo,
                childThree
            };

            // Act.
            var action = new Action(() =>
            {
                target = new TestTreeNode(value, children);
            });

            // Assert.
            action.Should().NotThrow();
            target.Children.Count().Should().Be(3);
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
            childThree.IsLeaf.Should().BeTrue();
            childThree.IsRoot.Should().BeFalse();
            childThree.Parent.Should().BeSameAs(target);
        }

        [TestMethod]
        public void Constructor_ShouldRaiseArgumentException_ForChildCollectionWithNullNode()
        {
            // Arrange.
            var target = (TestTreeNode)null;
            var value = 1;
            var childOne = new TestTreeNode(2);
            var childTwo = (TestTreeNode)null;
            var childThree = new TestTreeNode(5);
            var children = new TestTreeNode[]
            {
                childOne,
                childTwo,
                childThree
            };

            // Act.
            var action = new Action(() =>
            {
                target = new TestTreeNode(value, children);
            });

            // Assert.
            action.Should().Throw<ArgumentException>($"because {nameof(children)} contains a null node");
        }

        [TestMethod]
        public void Depth_ShouldReturnValidResult()
        {
            // Arrange.
            var leafNodeOne = new TestTreeNode(1);
            var leafNodeTwo = new TestTreeNode(2);
            var leafNodeThree = new TestTreeNode(3);
            var leafNodeFour = new TestTreeNode(4);
            var leafNodeFive = new TestTreeNode(5);
            var rootNode = new TestTreeNode(6, new TestTreeNode[]
            {
                new TestTreeNode(7, new TestTreeNode[]
                {
                    new TestTreeNode(9, new TestTreeNode[]
                    {
                        leafNodeOne,
                        new TestTreeNode(12, new TestTreeNode[]
                        {
                            leafNodeFive
                        })
                    })
                }),
                new TestTreeNode(8, new TestTreeNode[]
                {
                    new TestTreeNode(10, new TestTreeNode[]
                    {
                        leafNodeTwo
                    }),
                    new TestTreeNode(11, new TestTreeNode[]
                    {
                        leafNodeThree,
                        leafNodeFour
                    })
                })
            });

            // Assert.
            leafNodeOne.Depth.Should().Be(3);
            leafNodeTwo.Depth.Should().Be(3);
            leafNodeThree.Depth.Should().Be(3);
            leafNodeFour.Depth.Should().Be(3);
            leafNodeFive.Depth.Should().Be(4);
            leafNodeFive.Parent.Parent.Depth.Should().Be(2);
            rootNode.Depth.Should().Be(0);
        }

        [TestMethod]
        public void Destroy_ShouldProduceDesiredResults()
        {
            // Arrange.
            var leafNodeOne = new TestTreeNode(1);
            var leafNodeTwo = new TestTreeNode(2);
            var leafNodeThree = new TestTreeNode(3);
            var leafNodeFour = new TestTreeNode(4);
            var leafNodeFive = new TestTreeNode(5);
            var targetNode = new TestTreeNode(9, new TestTreeNode[]
            {
                leafNodeOne,
                new TestTreeNode(12, new TestTreeNode[]
                {
                    leafNodeFive
                })
            });
            var rootNode = new TestTreeNode(6, new TestTreeNode[]
            {
                new TestTreeNode(7, new TestTreeNode[]
                {
                    targetNode
                }),
                new TestTreeNode(8, new TestTreeNode[]
                {
                    new TestTreeNode(10, new TestTreeNode[]
                    {
                        leafNodeTwo
                    }),
                    new TestTreeNode(11, new TestTreeNode[]
                    {
                        leafNodeThree,
                        leafNodeFour
                    })
                })
            });

            // Act.
            targetNode.Destroy();

            // Assert.
            targetNode.Value.Should().Be(0);
            targetNode.Parent.Should().BeNull();
            targetNode.Children.Count().Should().Be(0);
            rootNode.Children.First().IsLeaf.Should().BeTrue();
        }

        [TestMethod]
        public void Height_ShouldReturnValidResult()
        {
            // Arrange.
            var leafNodeOne = new TestTreeNode(1);
            var leafNodeTwo = new TestTreeNode(2);
            var leafNodeThree = new TestTreeNode(3);
            var leafNodeFour = new TestTreeNode(4);
            var leafNodeFive = new TestTreeNode(5);
            var rootNode = new TestTreeNode(6, new TestTreeNode[]
            {
                new TestTreeNode(7, new TestTreeNode[]
                {
                    new TestTreeNode(9, new TestTreeNode[]
                    {
                        leafNodeOne,
                        new TestTreeNode(12, new TestTreeNode[]
                        {
                            leafNodeFive
                        })
                    })
                }),
                new TestTreeNode(8, new TestTreeNode[]
                {
                    new TestTreeNode(10, new TestTreeNode[]
                    {
                        leafNodeTwo
                    }),
                    new TestTreeNode(11, new TestTreeNode[]
                    {
                        leafNodeThree,
                        leafNodeFour
                    })
                })
            });

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
            var childNode = new TestTreeNode();
            var target = new TestTreeNode(0, new TestTreeNode[]
            {
                childNode
            });

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
            var childNode = new TestTreeNode();
            var target = new TestTreeNode();

            // Act.
            var result = target.RemoveChild(childNode);

            // Assert.
            result.Should().BeFalse();
        }

        [TestMethod]
        public void RemoveChild_ShouldRaiseArgumentNullException_ForNullChildNodeArgument()
        {
            // Arrange.
            var target = new TestTreeNode();
            var childNode = (TestTreeNode)null;

            // Act.
            var action = new Action(() =>
            {
                target.RemoveChild(childNode);
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>($"because {nameof(childNode)} is null");
        }

        private class TestTreeNode : TreeNode<Int32, TestTreeNode>
        {
            public TestTreeNode()
                : base()
            {
                return;
            }

            public TestTreeNode(Int32 value)
                : base(value)
            {
                return;
            }

            public TestTreeNode(Int32 value, IEnumerable<TestTreeNode> children)
                : base(value, children)
            {
                return;
            }
        }
    }
}