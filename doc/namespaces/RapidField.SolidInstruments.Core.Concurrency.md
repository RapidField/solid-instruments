---
uid: RapidField.SolidInstruments.Core.Concurrency
summary: *content
---

<!--
Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
-->

Provides configurable abstractions for concurrency control primitives.

<br />

![Core label](../images/Label.Core.300w.png)
- - -

### Installation

This library is available via [**NuGet**](https://docs.microsoft.com/en-us/nuget/quickstart/install-and-use-a-package-in-visual-studio). Use one of the commands below to download and install the library and all of its dependencies.

###### .NET CLI

```shell
dotnet add package RapidField.SolidInstruments.Core
```

###### NuGet Package Manager

```shell
Install-Package RapidField.SolidInstruments.Core
```

### Usage

The examples below are provided to help you get started using the features of this library.

#### Concurrency control abstractions

[ConcurrencyControl](https://www.solidinstruments.com/api/RapidField.SolidInstruments.Core.Concurrency.ConcurrencyControl.html) represents a concurrency control mechanism.

###### C#

```csharp
public class ExampleClass : IDisposable
{
    private readonly IConcurrencyControl Control;

    public ExampleClass()
    {
        // Permits two threads to obtain control concurrently.
        var mode = ConcurrencyControlMode.DuplexSemaphore;
        Control = ConcurrencyControl.New(mode);
    }

    public void Dispose()
    {
        Control.Dispose();
    }

    public void ExecuteActions(Action[] actions)
    {
        // Two threads can enter concurrently because Control is a
        // duplex semaphore. In this example, ExecuteActions processes
        // two batches of actions at a time.
        using (var controlToken = Control.Enter())
        {
            Console.WriteLine("Processing a new batch.");

            foreach (var action in actions)
            {
                controlToken.AttachTask(Task.Run(action));
            }

            // Disposal of controlToken waits for all attached
            // tasks to complete before releasing control to other
            // waiting threads.
        }

        Console.WriteLine("Batch completed.")
    }
}
```

<br />