<!--
Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
-->

![Solid Instruments logo](../../SolidInstruments.Logo.Color.Transparent.500w.png)
- - -

![Core label](Label.Core.300w.png)

The *RapidField.SolidInstruments.Core* library exposes foundational, general-purpose features. Refer to [the project root documentation](../../README.md) for more information.

- [Concurrency control abstractions](#concurrency-control-abstractions)
- [Fluent argument validation](#fluent-argument-validation)

- - -

### Concurrency control abstractions

[ConcurrencyControl](./Concurrency/ConcurrencyControl.cs) represents a concurrency control mechanism.

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
<br/>

### Fluent argument validation

[ValidationTarget](ArgumentValidation/ValidationTarget.cs) and [ValidationResult](ArgumentValidation/ValidationResult.cs) facilitate fluent argument validation syntax with a wide variety of out-of-the-box type support.

```csharp
public class ExampleClass
{
    public readonly IEnumerable<object> Objects;

    public ExampleClass(IEnumerable<object> objects)
    {
        // Raises the appropriate exception types if:
        // - objects is null, or
        // - objects contains zero elements, or
        // - objects contains more than ten elements, or
        // - objects contains a null element
        Objects = objects.RejectIf()
            .IsNullOrEmpty()
            .OrIf(argument => argument.Count() > 10)
            .OrIf(argument => argument.Any(element => element is null));
    }
}
```
<br/>

- - -
<br />

![RapidField logo](../../RapidField.Logo.Color.Black.Transparent.200w.png)
<br /><br />
Copyright (c) RapidField LLC. All rights reserved. "RapidField" and "Solid Instruments" are trademarks of RapidField LLC.