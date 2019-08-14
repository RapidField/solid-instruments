---
uid: RapidField.SolidInstruments.Command
summary: *content
---

<!--
Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
-->

Exposes implementations of the command and mediator patterns.

<br />

![Command label](../images/Label.Command.300w.png)
- - -

### Installation

This library is available via [**NuGet**](https://docs.microsoft.com/en-us/nuget/quickstart/install-and-use-a-package-in-visual-studio). Use one of the commands below to download and install the library and all of its dependencies.

###### .NET CLI

```shell
dotnet add package RapidField.SolidInstruments.Command
```

###### NuGet Package Manager

```shell
Install-Package RapidField.SolidInstruments.Command
```

### Usage

The examples below are provided to help you get started using the features of this library.

#### Command pattern abstractions

[Command](https://www.solidinstruments.com/api/RapidField.SolidInstruments.Command.Command.html) and [CommandHandler](https://www.solidinstruments.com/api/RapidField.SolidInstruments.Command.CommandHandler-1.html) fulfill the command pattern.

###### C#

```csharp
public class SubtractionCommand : Command<int>
{
    public SubtractionCommand(int minuend, int subtrahend)
        : base()
    {
        Minuend = minuend;
        Subtrahend = subtrahend;
    }

    public int Minuend { get; set; }
    public int Subtrahend { get; set; }
}

public class SubtractionCommandHandler : CommandHandler<SubtractionCommand, int>
{
    public SubtractionCommandHandler()
        : base()
    {
    }

    protected override int Process(SubtractionCommand command, ConcurrencyControlToken controlToken)
    {
        return (command.Minuend - command.Subtrahend);
    }
}
```

<br />

#### Mediator pattern implementation

[CommandMediator](https://www.solidinstruments.com/api/RapidField.SolidInstruments.Command.CommandMediator.html) serves as a dependency resolver and processing intermediary for commands.

###### C#

```csharp
public class Subtractor
{
    private readonly ICommandMediator Mediator;

    public Subtractor(ICommandMediator mediator)
    {
        Mediator = mediator;
    }

    public int Subtract(int minuend, int subtrahend)
    {
        var command = new SubtractionCommand(minuend, subtrahend);
        return Mediator.Process(command);
    }
}
```

<br />