// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Collections.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RapidField.SolidInstruments.Collections.UnitTests.Extensions
{
    [TestClass]
    public class ITreeNodeExtensionsTests
    {
        [TestMethod]
        public void TraverseDown_ShouldProduceDesiredResults_ForActionsAgainstInternalNode_WithoutPredicate()
        {
            // Arrange.
            var leafNodeOne = new TreeNode<Int32>(1);
            var leafNodeTwo = new TreeNode<Int32>(2);
            var leafNodeThree = new TreeNode<Int32>(3);
            var leafNodeFour = new TreeNode<Int32>(4);
            var leafNodeFive = new TreeNode<Int32>(5);
            var rootNode = new TreeNode<Int32>(6, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(7, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(9, new TreeNode<Int32>[]
                    {
                        leafNodeOne,
                        new TreeNode<Int32>(12, new TreeNode<Int32>[]
                        {
                            leafNodeFive
                        })
                    })
                }),
                new TreeNode<Int32>(8, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(10, new TreeNode<Int32>[]
                    {
                        leafNodeTwo
                    }),
                    new TreeNode<Int32>(11, new TreeNode<Int32>[]
                    {
                        leafNodeThree,
                        leafNodeFour
                    })
                })
            });

            // Act.
            leafNodeFive.Parent.Parent.TraverseDown(SetValueToZeroAction);
            var result = rootNode.TraverseDown(SumDownFunction);

            // Assert.
            result.Should().Be(51);
        }

        [TestMethod]
        public void TraverseDown_ShouldProduceDesiredResults_ForActionsAgainstInternalNode_WithPredicate()
        {
            // Arrange.
            var leafNodeOne = new TreeNode<Int32>(1);
            var leafNodeTwo = new TreeNode<Int32>(2);
            var leafNodeThree = new TreeNode<Int32>(3);
            var leafNodeFour = new TreeNode<Int32>(4);
            var leafNodeFive = new TreeNode<Int32>(5);
            var rootNode = new TreeNode<Int32>(6, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(7, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(9, new TreeNode<Int32>[]
                    {
                        leafNodeOne,
                        new TreeNode<Int32>(12, new TreeNode<Int32>[]
                        {
                            leafNodeFive
                        })
                    })
                }),
                new TreeNode<Int32>(8, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(10, new TreeNode<Int32>[]
                    {
                        leafNodeTwo
                    }),
                    new TreeNode<Int32>(11, new TreeNode<Int32>[]
                    {
                        leafNodeThree,
                        leafNodeFour
                    })
                })
            });

            // Act.
            leafNodeFive.Parent.Parent.TraverseDown(SetValueToZeroAction, node => node.Value != 12);
            var result = rootNode.TraverseDown(SumDownFunction);

            // Assert.
            result.Should().Be(68);
        }

        [TestMethod]
        public void TraverseDown_ShouldProduceDesiredResults_ForActionsAgainstLeafNode_WithoutPredicate()
        {
            // Arrange.
            var leafNodeOne = new TreeNode<Int32>(1);
            var leafNodeTwo = new TreeNode<Int32>(2);
            var leafNodeThree = new TreeNode<Int32>(3);
            var leafNodeFour = new TreeNode<Int32>(4);
            var leafNodeFive = new TreeNode<Int32>(5);
            var rootNode = new TreeNode<Int32>(6, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(7, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(9, new TreeNode<Int32>[]
                    {
                        leafNodeOne,
                        new TreeNode<Int32>(12, new TreeNode<Int32>[]
                        {
                            leafNodeFive
                        })
                    })
                }),
                new TreeNode<Int32>(8, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(10, new TreeNode<Int32>[]
                    {
                        leafNodeTwo
                    }),
                    new TreeNode<Int32>(11, new TreeNode<Int32>[]
                    {
                        leafNodeThree,
                        leafNodeFour
                    })
                })
            });

            // Act.
            leafNodeFive.TraverseDown(SetValueToZeroAction);
            var result = rootNode.TraverseDown(SumDownFunction);

            // Assert.
            result.Should().Be(73);
        }

        [TestMethod]
        public void TraverseDown_ShouldProduceDesiredResults_ForActionsAgainstLeafNode_WithPredicate()
        {
            // Arrange.
            var leafNodeOne = new TreeNode<Int32>(1);
            var leafNodeTwo = new TreeNode<Int32>(2);
            var leafNodeThree = new TreeNode<Int32>(3);
            var leafNodeFour = new TreeNode<Int32>(4);
            var leafNodeFive = new TreeNode<Int32>(5);
            var rootNode = new TreeNode<Int32>(6, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(7, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(9, new TreeNode<Int32>[]
                    {
                        leafNodeOne,
                        new TreeNode<Int32>(12, new TreeNode<Int32>[]
                        {
                            leafNodeFive
                        })
                    })
                }),
                new TreeNode<Int32>(8, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(10, new TreeNode<Int32>[]
                    {
                        leafNodeTwo
                    }),
                    new TreeNode<Int32>(11, new TreeNode<Int32>[]
                    {
                        leafNodeThree,
                        leafNodeFour
                    })
                })
            });

            // Act.
            leafNodeFive.TraverseDown(SetValueToZeroAction, node => node.Value != 5);
            var result = rootNode.TraverseDown(SumDownFunction);

            // Assert.
            result.Should().Be(78);
        }

        [TestMethod]
        public void TraverseDown_ShouldProduceDesiredResults_ForActionsAgainstRootNode_WithoutPredicate()
        {
            // Arrange.
            var leafNodeOne = new TreeNode<Int32>(1);
            var leafNodeTwo = new TreeNode<Int32>(2);
            var leafNodeThree = new TreeNode<Int32>(3);
            var leafNodeFour = new TreeNode<Int32>(4);
            var leafNodeFive = new TreeNode<Int32>(5);
            var rootNode = new TreeNode<Int32>(6, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(7, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(9, new TreeNode<Int32>[]
                    {
                        leafNodeOne,
                        new TreeNode<Int32>(12, new TreeNode<Int32>[]
                        {
                            leafNodeFive
                        })
                    })
                }),
                new TreeNode<Int32>(8, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(10, new TreeNode<Int32>[]
                    {
                        leafNodeTwo
                    }),
                    new TreeNode<Int32>(11, new TreeNode<Int32>[]
                    {
                        leafNodeThree,
                        leafNodeFour
                    })
                })
            });

            // Act.
            rootNode.TraverseDown(SetValueToZeroAction);
            var result = rootNode.TraverseDown(SumDownFunction);

            // Assert.
            result.Should().Be(0);
        }

        [TestMethod]
        public void TraverseDown_ShouldProduceDesiredResults_ForActionsAgainstRootNode_WithPredicate()
        {
            // Arrange.
            var leafNodeOne = new TreeNode<Int32>(1);
            var leafNodeTwo = new TreeNode<Int32>(2);
            var leafNodeThree = new TreeNode<Int32>(3);
            var leafNodeFour = new TreeNode<Int32>(4);
            var leafNodeFive = new TreeNode<Int32>(5);
            var rootNode = new TreeNode<Int32>(6, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(7, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(9, new TreeNode<Int32>[]
                    {
                        leafNodeOne,
                        new TreeNode<Int32>(12, new TreeNode<Int32>[]
                        {
                            leafNodeFive
                        })
                    })
                }),
                new TreeNode<Int32>(8, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(10, new TreeNode<Int32>[]
                    {
                        leafNodeTwo
                    }),
                    new TreeNode<Int32>(11, new TreeNode<Int32>[]
                    {
                        leafNodeThree,
                        leafNodeFour
                    })
                })
            });

            // Act.
            rootNode.TraverseDown(SetValueToZeroAction, node => node.Value != 9);
            var result = rootNode.TraverseDown(SumDownFunction);

            // Assert.
            result.Should().Be(27);
        }

        [TestMethod]
        public void TraverseDown_ShouldProduceDesiredResults_ForFunctionsAgainstInternalNode_WithoutPredicate()
        {
            // Arrange.
            var leafNodeOne = new TreeNode<Int32>(1);
            var leafNodeTwo = new TreeNode<Int32>(2);
            var leafNodeThree = new TreeNode<Int32>(3);
            var leafNodeFour = new TreeNode<Int32>(4);
            var leafNodeFive = new TreeNode<Int32>(5);
            var rootNode = new TreeNode<Int32>(6, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(7, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(9, new TreeNode<Int32>[]
                    {
                        leafNodeOne,
                        new TreeNode<Int32>(12, new TreeNode<Int32>[]
                        {
                            leafNodeFive
                        })
                    })
                }),
                new TreeNode<Int32>(8, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(10, new TreeNode<Int32>[]
                    {
                        leafNodeTwo
                    }),
                    new TreeNode<Int32>(11, new TreeNode<Int32>[]
                    {
                        leafNodeThree,
                        leafNodeFour
                    })
                })
            });

            // Act.
            var result = leafNodeFive.Parent.Parent.TraverseDown(SumDownFunction);

            // Assert.
            result.Should().Be(27);
        }

        [TestMethod]
        public void TraverseDown_ShouldProduceDesiredResults_ForFunctionsAgainstInternalNode_WithPredicate()
        {
            // Arrange.
            var leafNodeOne = new TreeNode<Int32>(1);
            var leafNodeTwo = new TreeNode<Int32>(2);
            var leafNodeThree = new TreeNode<Int32>(3);
            var leafNodeFour = new TreeNode<Int32>(4);
            var leafNodeFive = new TreeNode<Int32>(5);
            var rootNode = new TreeNode<Int32>(6, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(7, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(9, new TreeNode<Int32>[]
                    {
                        leafNodeOne,
                        new TreeNode<Int32>(12, new TreeNode<Int32>[]
                        {
                            leafNodeFive
                        })
                    })
                }),
                new TreeNode<Int32>(8, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(10, new TreeNode<Int32>[]
                    {
                        leafNodeTwo
                    }),
                    new TreeNode<Int32>(11, new TreeNode<Int32>[]
                    {
                        leafNodeThree,
                        leafNodeFour
                    })
                })
            });

            // Act.
            var result = leafNodeFive.Parent.Parent.TraverseDown(SumDownFunction, node => node.Value != 12);

            // Assert.
            result.Should().Be(10);
        }

        [TestMethod]
        public void TraverseDown_ShouldProduceDesiredResults_ForFunctionsAgainstLeafNode_WithoutPredicate()
        {
            // Arrange.
            var leafNodeOne = new TreeNode<Int32>(1);
            var leafNodeTwo = new TreeNode<Int32>(2);
            var leafNodeThree = new TreeNode<Int32>(3);
            var leafNodeFour = new TreeNode<Int32>(4);
            var leafNodeFive = new TreeNode<Int32>(5);
            var rootNode = new TreeNode<Int32>(6, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(7, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(9, new TreeNode<Int32>[]
                    {
                        leafNodeOne,
                        new TreeNode<Int32>(12, new TreeNode<Int32>[]
                        {
                            leafNodeFive
                        })
                    })
                }),
                new TreeNode<Int32>(8, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(10, new TreeNode<Int32>[]
                    {
                        leafNodeTwo
                    }),
                    new TreeNode<Int32>(11, new TreeNode<Int32>[]
                    {
                        leafNodeThree,
                        leafNodeFour
                    })
                })
            });

            // Act.
            var result = leafNodeFive.TraverseDown(SumDownFunction);

            // Assert.
            result.Should().Be(5);
        }

        [TestMethod]
        public void TraverseDown_ShouldProduceDesiredResults_ForFunctionsAgainstLeafNode_WithPredicate()
        {
            // Arrange.
            var leafNodeOne = new TreeNode<Int32>(1);
            var leafNodeTwo = new TreeNode<Int32>(2);
            var leafNodeThree = new TreeNode<Int32>(3);
            var leafNodeFour = new TreeNode<Int32>(4);
            var leafNodeFive = new TreeNode<Int32>(5);
            var rootNode = new TreeNode<Int32>(6, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(7, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(9, new TreeNode<Int32>[]
                    {
                        leafNodeOne,
                        new TreeNode<Int32>(12, new TreeNode<Int32>[]
                        {
                            leafNodeFive
                        })
                    })
                }),
                new TreeNode<Int32>(8, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(10, new TreeNode<Int32>[]
                    {
                        leafNodeTwo
                    }),
                    new TreeNode<Int32>(11, new TreeNode<Int32>[]
                    {
                        leafNodeThree,
                        leafNodeFour
                    })
                })
            });

            // Act.
            var result = leafNodeFive.TraverseDown(SumDownFunction, node => node.Value != 5);

            // Assert.
            result.Should().Be(0);
        }

        [TestMethod]
        public void TraverseDown_ShouldProduceDesiredResults_ForFunctionsAgainstRootNode_WithoutPredicate()
        {
            // Arrange.
            var leafNodeOne = new TreeNode<Int32>(1);
            var leafNodeTwo = new TreeNode<Int32>(2);
            var leafNodeThree = new TreeNode<Int32>(3);
            var leafNodeFour = new TreeNode<Int32>(4);
            var leafNodeFive = new TreeNode<Int32>(5);
            var rootNode = new TreeNode<Int32>(6, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(7, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(9, new TreeNode<Int32>[]
                    {
                        leafNodeOne,
                        new TreeNode<Int32>(12, new TreeNode<Int32>[]
                        {
                            leafNodeFive
                        })
                    })
                }),
                new TreeNode<Int32>(8, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(10, new TreeNode<Int32>[]
                    {
                        leafNodeTwo
                    }),
                    new TreeNode<Int32>(11, new TreeNode<Int32>[]
                    {
                        leafNodeThree,
                        leafNodeFour
                    })
                })
            });

            // Act.
            var result = rootNode.TraverseDown(SumDownFunction);

            // Assert.
            result.Should().Be(78);
        }

        [TestMethod]
        public void TraverseDown_ShouldProduceDesiredResults_ForFunctionsAgainstRootNode_WithPredicate()
        {
            // Arrange.
            var leafNodeOne = new TreeNode<Int32>(1);
            var leafNodeTwo = new TreeNode<Int32>(2);
            var leafNodeThree = new TreeNode<Int32>(3);
            var leafNodeFour = new TreeNode<Int32>(4);
            var leafNodeFive = new TreeNode<Int32>(5);
            var rootNode = new TreeNode<Int32>(6, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(7, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(9, new TreeNode<Int32>[]
                    {
                        leafNodeOne,
                        new TreeNode<Int32>(12, new TreeNode<Int32>[]
                        {
                            leafNodeFive
                        })
                    })
                }),
                new TreeNode<Int32>(8, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(10, new TreeNode<Int32>[]
                    {
                        leafNodeTwo
                    }),
                    new TreeNode<Int32>(11, new TreeNode<Int32>[]
                    {
                        leafNodeThree,
                        leafNodeFour
                    })
                })
            });

            // Act.
            var result = rootNode.TraverseDown(SumDownFunction, node => node.Value != 9);

            // Assert.
            result.Should().Be(51);
        }

        [TestMethod]
        public void TraverseDown_ShouldRaiseArgumentNullException_ForActions_WithNullAction()
        {
            // Arrange.
            var target = new TreeNode<Int32>(1, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(2)
            });

            // Act.
            var action = new Action(() =>
            {
                target.TraverseDown(null);
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void TraverseDown_ShouldRaiseArgumentNullException_ForActions_WithNullPredicate()
        {
            // Arrange.
            var target = new TreeNode<Int32>(1, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(2)
            });

            // Act.
            var action = new Action(() =>
            {
                target.TraverseDown(SetValueToZeroAction, null);
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void TraverseDown_ShouldRaiseArgumentNullException_ForFunctions_WithNullFunction()
        {
            // Arrange.
            var target = new TreeNode<Int32>(1, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(2)
            });

            // Act.
            var action = new Action(() =>
            {
                var result = target.TraverseDown<Int32, Int32>(null);
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void TraverseDown_ShouldRaiseArgumentNullException_ForFunctions_WithNullPredicate()
        {
            // Arrange.
            var target = new TreeNode<Int32>(1, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(2)
            });

            // Act.
            var action = new Action(() =>
            {
                var result = target.TraverseDown(SumDownFunction, null);
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void TraverseUp_ShouldProduceDesiredResults_ForActionsAgainstInternalNode_WithoutPredicate()
        {
            // Arrange.
            var leafNodeOne = new TreeNode<Int32>(1);
            var leafNodeTwo = new TreeNode<Int32>(2);
            var leafNodeThree = new TreeNode<Int32>(3);
            var leafNodeFour = new TreeNode<Int32>(4);
            var leafNodeFive = new TreeNode<Int32>(5);
            var rootNode = new TreeNode<Int32>(6, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(7, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(9, new TreeNode<Int32>[]
                    {
                        leafNodeOne,
                        new TreeNode<Int32>(12, new TreeNode<Int32>[]
                        {
                            leafNodeFive
                        })
                    })
                }),
                new TreeNode<Int32>(8, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(10, new TreeNode<Int32>[]
                    {
                        leafNodeTwo
                    }),
                    new TreeNode<Int32>(11, new TreeNode<Int32>[]
                    {
                        leafNodeThree,
                        leafNodeFour
                    })
                })
            });

            // Act.
            leafNodeFive.Parent.Parent.TraverseUp(SetValueToZeroAction);
            var result = leafNodeFive.TraverseUp(SumUpFunction);

            // Assert.
            result.Should().Be(17);
        }

        [TestMethod]
        public void TraverseUp_ShouldProduceDesiredResults_ForActionsAgainstInternalNode_WithPredicate()
        {
            // Arrange.
            var leafNodeOne = new TreeNode<Int32>(1);
            var leafNodeTwo = new TreeNode<Int32>(2);
            var leafNodeThree = new TreeNode<Int32>(3);
            var leafNodeFour = new TreeNode<Int32>(4);
            var leafNodeFive = new TreeNode<Int32>(5);
            var rootNode = new TreeNode<Int32>(6, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(7, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(9, new TreeNode<Int32>[]
                    {
                        leafNodeOne,
                        new TreeNode<Int32>(12, new TreeNode<Int32>[]
                        {
                            leafNodeFive
                        })
                    })
                }),
                new TreeNode<Int32>(8, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(10, new TreeNode<Int32>[]
                    {
                        leafNodeTwo
                    }),
                    new TreeNode<Int32>(11, new TreeNode<Int32>[]
                    {
                        leafNodeThree,
                        leafNodeFour
                    })
                })
            });

            // Act.
            leafNodeFive.Parent.Parent.TraverseUp(SetValueToZeroAction, node => node.Value != 7);
            var result = leafNodeFive.TraverseUp(SumUpFunction);

            // Assert.
            result.Should().Be(30);
        }

        [TestMethod]
        public void TraverseUp_ShouldProduceDesiredResults_ForActionsAgainstLeafNode_WithoutPredicate()
        {
            // Arrange.
            var leafNodeOne = new TreeNode<Int32>(1);
            var leafNodeTwo = new TreeNode<Int32>(2);
            var leafNodeThree = new TreeNode<Int32>(3);
            var leafNodeFour = new TreeNode<Int32>(4);
            var leafNodeFive = new TreeNode<Int32>(5);
            var rootNode = new TreeNode<Int32>(6, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(7, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(9, new TreeNode<Int32>[]
                    {
                        leafNodeOne,
                        new TreeNode<Int32>(12, new TreeNode<Int32>[]
                        {
                            leafNodeFive
                        })
                    })
                }),
                new TreeNode<Int32>(8, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(10, new TreeNode<Int32>[]
                    {
                        leafNodeTwo
                    }),
                    new TreeNode<Int32>(11, new TreeNode<Int32>[]
                    {
                        leafNodeThree,
                        leafNodeFour
                    })
                })
            });

            // Act.
            leafNodeFive.TraverseUp(SetValueToZeroAction);
            var result = leafNodeFive.TraverseUp(SumUpFunction);

            // Assert.
            result.Should().Be(0);
        }

        [TestMethod]
        public void TraverseUp_ShouldProduceDesiredResults_ForActionsAgainstLeafNode_WithPredicate()
        {
            // Arrange.
            var leafNodeOne = new TreeNode<Int32>(1);
            var leafNodeTwo = new TreeNode<Int32>(2);
            var leafNodeThree = new TreeNode<Int32>(3);
            var leafNodeFour = new TreeNode<Int32>(4);
            var leafNodeFive = new TreeNode<Int32>(5);
            var rootNode = new TreeNode<Int32>(6, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(7, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(9, new TreeNode<Int32>[]
                    {
                        leafNodeOne,
                        new TreeNode<Int32>(12, new TreeNode<Int32>[]
                        {
                            leafNodeFive
                        })
                    })
                }),
                new TreeNode<Int32>(8, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(10, new TreeNode<Int32>[]
                    {
                        leafNodeTwo
                    }),
                    new TreeNode<Int32>(11, new TreeNode<Int32>[]
                    {
                        leafNodeThree,
                        leafNodeFour
                    })
                })
            });

            // Act.
            leafNodeFive.TraverseUp(SetValueToZeroAction, node => node.Value != 5);
            var result = leafNodeFive.TraverseUp(SumUpFunction);

            // Assert.
            result.Should().Be(39);
        }

        [TestMethod]
        public void TraverseUp_ShouldProduceDesiredResults_ForActionsAgainstRootNode_WithoutPredicate()
        {
            // Arrange.
            var leafNodeOne = new TreeNode<Int32>(1);
            var leafNodeTwo = new TreeNode<Int32>(2);
            var leafNodeThree = new TreeNode<Int32>(3);
            var leafNodeFour = new TreeNode<Int32>(4);
            var leafNodeFive = new TreeNode<Int32>(5);
            var rootNode = new TreeNode<Int32>(6, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(7, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(9, new TreeNode<Int32>[]
                    {
                        leafNodeOne,
                        new TreeNode<Int32>(12, new TreeNode<Int32>[]
                        {
                            leafNodeFive
                        })
                    })
                }),
                new TreeNode<Int32>(8, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(10, new TreeNode<Int32>[]
                    {
                        leafNodeTwo
                    }),
                    new TreeNode<Int32>(11, new TreeNode<Int32>[]
                    {
                        leafNodeThree,
                        leafNodeFour
                    })
                })
            });

            // Act.
            rootNode.TraverseUp(SetValueToZeroAction);
            var result = leafNodeFive.TraverseUp(SumUpFunction);

            // Assert.
            result.Should().Be(33);
        }

        [TestMethod]
        public void TraverseUp_ShouldProduceDesiredResults_ForActionsAgainstRootNode_WithPredicate()
        {
            // Arrange.
            var leafNodeOne = new TreeNode<Int32>(1);
            var leafNodeTwo = new TreeNode<Int32>(2);
            var leafNodeThree = new TreeNode<Int32>(3);
            var leafNodeFour = new TreeNode<Int32>(4);
            var leafNodeFive = new TreeNode<Int32>(5);
            var rootNode = new TreeNode<Int32>(6, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(7, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(9, new TreeNode<Int32>[]
                    {
                        leafNodeOne,
                        new TreeNode<Int32>(12, new TreeNode<Int32>[]
                        {
                            leafNodeFive
                        })
                    })
                }),
                new TreeNode<Int32>(8, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(10, new TreeNode<Int32>[]
                    {
                        leafNodeTwo
                    }),
                    new TreeNode<Int32>(11, new TreeNode<Int32>[]
                    {
                        leafNodeThree,
                        leafNodeFour
                    })
                })
            });

            // Act.
            rootNode.TraverseUp(SetValueToZeroAction, node => node.Value != 6);
            var result = leafNodeFive.TraverseUp(SumUpFunction);

            // Assert.
            result.Should().Be(39);
        }

        [TestMethod]
        public void TraverseUp_ShouldProduceDesiredResults_ForFunctionsAgainstInternalNode_WithoutPredicate()
        {
            // Arrange.
            var leafNodeOne = new TreeNode<Int32>(1);
            var leafNodeTwo = new TreeNode<Int32>(2);
            var leafNodeThree = new TreeNode<Int32>(3);
            var leafNodeFour = new TreeNode<Int32>(4);
            var leafNodeFive = new TreeNode<Int32>(5);
            var rootNode = new TreeNode<Int32>(6, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(7, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(9, new TreeNode<Int32>[]
                    {
                        leafNodeOne,
                        new TreeNode<Int32>(12, new TreeNode<Int32>[]
                        {
                            leafNodeFive
                        })
                    })
                }),
                new TreeNode<Int32>(8, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(10, new TreeNode<Int32>[]
                    {
                        leafNodeTwo
                    }),
                    new TreeNode<Int32>(11, new TreeNode<Int32>[]
                    {
                        leafNodeThree,
                        leafNodeFour
                    })
                })
            });

            // Act.
            var result = leafNodeFive.Parent.Parent.TraverseUp(SumUpFunction);

            // Assert.
            result.Should().Be(22);
        }

        [TestMethod]
        public void TraverseUp_ShouldProduceDesiredResults_ForFunctionsAgainstInternalNode_WithPredicate()
        {
            // Arrange.
            var leafNodeOne = new TreeNode<Int32>(1);
            var leafNodeTwo = new TreeNode<Int32>(2);
            var leafNodeThree = new TreeNode<Int32>(3);
            var leafNodeFour = new TreeNode<Int32>(4);
            var leafNodeFive = new TreeNode<Int32>(5);
            var rootNode = new TreeNode<Int32>(6, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(7, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(9, new TreeNode<Int32>[]
                    {
                        leafNodeOne,
                        new TreeNode<Int32>(12, new TreeNode<Int32>[]
                        {
                            leafNodeFive
                        })
                    })
                }),
                new TreeNode<Int32>(8, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(10, new TreeNode<Int32>[]
                    {
                        leafNodeTwo
                    }),
                    new TreeNode<Int32>(11, new TreeNode<Int32>[]
                    {
                        leafNodeThree,
                        leafNodeFour
                    })
                })
            });

            // Act.
            var result = leafNodeFive.Parent.Parent.TraverseUp(SumUpFunction, node => node.Value != 7);

            // Assert.
            result.Should().Be(9);
        }

        [TestMethod]
        public void TraverseUp_ShouldProduceDesiredResults_ForFunctionsAgainstLeafNode_WithoutPredicate()
        {
            // Arrange.
            var leafNodeOne = new TreeNode<Int32>(1);
            var leafNodeTwo = new TreeNode<Int32>(2);
            var leafNodeThree = new TreeNode<Int32>(3);
            var leafNodeFour = new TreeNode<Int32>(4);
            var leafNodeFive = new TreeNode<Int32>(5);
            var rootNode = new TreeNode<Int32>(6, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(7, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(9, new TreeNode<Int32>[]
                    {
                        leafNodeOne,
                        new TreeNode<Int32>(12, new TreeNode<Int32>[]
                        {
                            leafNodeFive
                        })
                    })
                }),
                new TreeNode<Int32>(8, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(10, new TreeNode<Int32>[]
                    {
                        leafNodeTwo
                    }),
                    new TreeNode<Int32>(11, new TreeNode<Int32>[]
                    {
                        leafNodeThree,
                        leafNodeFour
                    })
                })
            });

            // Act.
            var result = leafNodeFive.TraverseUp(SumUpFunction);

            // Assert.
            result.Should().Be(39);
        }

        [TestMethod]
        public void TraverseUp_ShouldProduceDesiredResults_ForFunctionsAgainstLeafNode_WithPredicate()
        {
            // Arrange.
            var leafNodeOne = new TreeNode<Int32>(1);
            var leafNodeTwo = new TreeNode<Int32>(2);
            var leafNodeThree = new TreeNode<Int32>(3);
            var leafNodeFour = new TreeNode<Int32>(4);
            var leafNodeFive = new TreeNode<Int32>(5);
            var rootNode = new TreeNode<Int32>(6, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(7, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(9, new TreeNode<Int32>[]
                    {
                        leafNodeOne,
                        new TreeNode<Int32>(12, new TreeNode<Int32>[]
                        {
                            leafNodeFive
                        })
                    })
                }),
                new TreeNode<Int32>(8, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(10, new TreeNode<Int32>[]
                    {
                        leafNodeTwo
                    }),
                    new TreeNode<Int32>(11, new TreeNode<Int32>[]
                    {
                        leafNodeThree,
                        leafNodeFour
                    })
                })
            });

            // Act.
            var result = leafNodeFive.TraverseUp(SumUpFunction, node => node.Value != 5);

            // Assert.
            result.Should().Be(0);
        }

        [TestMethod]
        public void TraverseUp_ShouldProduceDesiredResults_ForFunctionsAgainstRootNode_WithoutPredicate()
        {
            // Arrange.
            var leafNodeOne = new TreeNode<Int32>(1);
            var leafNodeTwo = new TreeNode<Int32>(2);
            var leafNodeThree = new TreeNode<Int32>(3);
            var leafNodeFour = new TreeNode<Int32>(4);
            var leafNodeFive = new TreeNode<Int32>(5);
            var rootNode = new TreeNode<Int32>(6, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(7, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(9, new TreeNode<Int32>[]
                    {
                        leafNodeOne,
                        new TreeNode<Int32>(12, new TreeNode<Int32>[]
                        {
                            leafNodeFive
                        })
                    })
                }),
                new TreeNode<Int32>(8, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(10, new TreeNode<Int32>[]
                    {
                        leafNodeTwo
                    }),
                    new TreeNode<Int32>(11, new TreeNode<Int32>[]
                    {
                        leafNodeThree,
                        leafNodeFour
                    })
                })
            });

            // Act.
            var result = rootNode.TraverseUp(SumUpFunction);

            // Assert.
            result.Should().Be(6);
        }

        [TestMethod]
        public void TraverseUp_ShouldProduceDesiredResults_ForFunctionsAgainstRootNode_WithPredicate()
        {
            // Arrange.
            var leafNodeOne = new TreeNode<Int32>(1);
            var leafNodeTwo = new TreeNode<Int32>(2);
            var leafNodeThree = new TreeNode<Int32>(3);
            var leafNodeFour = new TreeNode<Int32>(4);
            var leafNodeFive = new TreeNode<Int32>(5);
            var rootNode = new TreeNode<Int32>(6, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(7, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(9, new TreeNode<Int32>[]
                    {
                        leafNodeOne,
                        new TreeNode<Int32>(12, new TreeNode<Int32>[]
                        {
                            leafNodeFive
                        })
                    })
                }),
                new TreeNode<Int32>(8, new TreeNode<Int32>[]
                {
                    new TreeNode<Int32>(10, new TreeNode<Int32>[]
                    {
                        leafNodeTwo
                    }),
                    new TreeNode<Int32>(11, new TreeNode<Int32>[]
                    {
                        leafNodeThree,
                        leafNodeFour
                    })
                })
            });

            // Act.
            var result = rootNode.TraverseUp(SumUpFunction, node => node.Value != 6);

            // Assert.
            result.Should().Be(0);
        }

        [TestMethod]
        public void TraverseUp_ShouldRaiseArgumentNullException_ForActions_WithNullAction()
        {
            // Arrange.
            var target = new TreeNode<Int32>(1, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(2)
            });

            // Act.
            var action = new Action(() =>
            {
                target.TraverseUp(null);
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void TraverseUp_ShouldRaiseArgumentNullException_ForActions_WithNullPredicate()
        {
            // Arrange.
            var target = new TreeNode<Int32>(1, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(2)
            });

            // Act.
            var action = new Action(() =>
            {
                target.TraverseUp(SetValueToZeroAction, null);
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void TraverseUp_ShouldRaiseArgumentNullException_ForFunctions_WithNullFunction()
        {
            // Arrange.
            var target = new TreeNode<Int32>(1, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(2)
            });

            // Act.
            var action = new Action(() =>
            {
                var result = target.TraverseUp<Int32, Int32>(null);
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void TraverseUp_ShouldRaiseArgumentNullException_ForFunctions_WithNullPredicate()
        {
            // Arrange.
            var target = new TreeNode<Int32>(1, new TreeNode<Int32>[]
            {
                new TreeNode<Int32>(2)
            });

            // Act.
            var action = new Action(() =>
            {
                var result = target.TraverseUp(SumUpFunction, null);
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        private static readonly Action<ITreeNode<Int32>> SetValueToZeroAction = (node) =>
        {
            node.Value = 0;
        };

        private static readonly Func<ITreeNode<Int32>, IEnumerable<Int32>, Int32> SumDownFunction = (node, childResults) =>
        {
            return node.Value + childResults.Sum();
        };

        private static readonly Func<ITreeNode<Int32>, Int32, Int32> SumUpFunction = (node, parentResult) =>
        {
            return node.Value + parentResult;
        };
    }
}