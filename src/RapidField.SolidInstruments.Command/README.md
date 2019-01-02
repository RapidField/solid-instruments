<!--
Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
-->

![Solid Instruments logo](../../SolidInstruments.Logo.Color.Transparent.500w.png)
- - -

![Command label](Label.Command.300w.png)

The *RapidField.SolidInstruments.Command* library exposes implementations of the command and mediator patterns. Refer to [the project root documentation](../../README.md) for more information.

- [Command pattern abstraction](#command-pattern-abstraction)
- [Mediator pattern implementation](#mediator-pattern-implementation)

- - -

### Command pattern abstractions

[Command](Command.cs) and [CommandHandler](CommandHandler.cs) fulfill the command pattern.

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
<br/>

### Mediator pattern implementation

[CommandMediator](CommandMediator.cs) serves as a dependency resolver and processing intermediary for commands.

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
<br/>

- - -
<br />

![RapidField logo](../../RapidField.Logo.Color.Black.Transparent.200w.png)
<br /><br />
Copyright (c) RapidField LLC. All rights reserved. "RapidField" and "Solid Instruments" are trademarks of RapidField LLC.