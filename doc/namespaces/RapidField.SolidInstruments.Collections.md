---
uid: RapidField.SolidInstruments.Collections
summary: *content
---

<!--
Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
-->

Exposes useful collection types and features.

<br />

![Collections label](../images/Label.Collections.300w.png)
- - -

### Installation

This library is available via [**NuGet**](https://docs.microsoft.com/en-us/nuget/quickstart/install-and-use-a-package-in-visual-studio). Use one of the commands below to download and install the library and all of its dependencies.

###### .NET CLI

```shell
dotnet add package RapidField.SolidInstruments.Collections
```

###### NuGet Package Manager

```shell
Install-Package RapidField.SolidInstruments.Collections
```

### Usage

The examples below are provided to help you get started using the features of this library.

#### Circular buffers

[CircularBuffer](https://www.solidinstruments.com/api/RapidField.SolidInstruments.Collections.CircularBuffer-1.html) represents a thread-safe, contiguous, generic collection of elements.

###### C#

```csharp
var buffer = new CircularBuffer<int>(4);
Console.WriteLine(buffer.Length); // 0

// Write once, read once
buffer.Write(0);
Console.WriteLine(buffer.Read()) // 0
Console.WriteLine(buffer.Length); // 0

// Write once, read once
buffer.Write(1);
Console.WriteLine(buffer.Read()) // 1
Console.WriteLine(buffer.Length); // 0

// Write four times, read three times
buffer.Write(0);
buffer.Write(1);
buffer.Write(2);
buffer.Write(3);
Console.WriteLine(buffer.Length); // 4
Console.WriteLine(buffer.Read()) // 0
Console.WriteLine(buffer.Read()) // 1
Console.WriteLine(buffer.Read()) // 2
Console.WriteLine(buffer.Length); // 1
```

<br />

#### Infinite sequences

[InfiniteSequence](https://www.solidinstruments.com/api/RapidField.SolidInstruments.Collections.InfiniteSequence-1.html) represents a thread-safe, infinite sequence of calculated values.

###### C#

```csharp
public class BasicArithmeticSequence : InfiniteSequence<int>
{
    public BasicArithmeticSequence(int seedTerm)
        : base(seedTerm)
    {
    }

    public BasicArithmeticSequence(int[] seedTerms)
        : base(seedTerms)
    {
    }

    protected override int CalculateNext(IEnumerable<int> calculatedTerms)
    {
        // Defines a basic arithmetic sequence in which each term is
        // equal to the previous term plus one.
        return (calculatedTerms.Last() + 1);
    }
}

public class ExampleClass
{
    public void ExampleMethod()
    {
        var seedTerms = new int[] { 3, 4, 5 };
        var sequence = new BasicArithmeticSequence(seedTerms);
        var nextThree = sequence.CalculateNext(3); // { 6, 7, 8 }
        var middleTwo = sequence.ToArray(2, 2); // { 5, 6 }
        var termFifty = sequence[49]; // 52
    }
}
```

<br />

#### Pinned memory

[PinnedMemory](https://www.solidinstruments.com/api/RapidField.SolidInstruments.Collections.PinnedMemory.html) represents a fixed-length bit field that is pinned in memory.

###### C#

```csharp
var field = new byte[8];
var overwriteWithZerosOnDispose = true;

using (var pinnedMemory = new PinnedMemory(field, overwriteWithZerosOnDispose))
{
    // The field is now pinned in memory and will not be paged to disk.
    pinnedMemory[3] = 0xf6;
    pinnedMemory[5] = 0x4a;
}

// Optionally, zeros are written over the field upon disposal to
// protect it in memory prior to garbage collection.
var positionThree = field[3]; // 0x00
var positionFive = field[5]; // 0x00
```

<br />

#### Tree composition and traversal

[TreeNode](https://www.solidinstruments.com/api/RapidField.SolidInstruments.Collections.TreeNode-1.html) represents a node in a tree structure.

###### C#

```csharp
public class IntegerNode : TreeNode<int, IntegerNode>
{
    public IntegerNode()
        : base()
    {
    }

    public IntegerNode(int value)
        : base(value)
    {
    }

    public IntegerNode(int value, IEnumerable<IntegerNode> children)
        : base(value, children)
    {
    }
}

public class ExampleClass
{
    public void ExampleMethod()
    {
        var leafNodeOne = new IntegerNode(1);
        var leafNodeTwo = new IntegerNode(2);
        var leafNodeThree = new IntegerNode(3);
        var leafNodeFour = new IntegerNode(4);
        var leafNodeFive = new IntegerNode(5);

        var targetNode = new IntegerNode(9, new IntegerNode[]
        {
            leafNodeOne,
            new IntegerNode(12, new IntegerNode[]
            {
                leafNodeFive
            })
        });

        var rootNode = new IntegerNode(6, new IntegerNode[]
        {
            new IntegerNode(7, new IntegerNode[]
            {
                // This node gets destroyed.
                targetNode
            }),
            new IntegerNode(8, new IntegerNode[]
            {
                new IntegerNode(10, new IntegerNode[]
                {
                    leafNodeTwo
                }),
                new IntegerNode(11, new IntegerNode[]
                {
                    leafNodeThree,
                    leafNodeFour
                })
            })
        });

        targetNode.Destroy();

        Console.WriteLine(targetNode.Value); // 0
        Console.WriteLine(targetNode.Parent); // null
        Console.WriteLine(targetNode.Children.Count()); // 0
        Console.WriteLine(rootNode.Children.First().IsLeaf); // true
    }
}
```

<br />

### Namespaces

#### [RapidField.SolidInstruments.Collections.Extensions](https://www.solidinstruments.com/api/RapidField.SolidInstruments.Collections.Extensions.html)

<section>
Exposes collection type extensions.
</section>