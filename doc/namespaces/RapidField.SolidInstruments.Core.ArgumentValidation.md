---
uid: RapidField.SolidInstruments.Core.ArgumentValidation
summary: *content
---

<!--
Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
-->

Defines a fluent pattern for evaluating argument validity.

<br />

![Core label](../images/Label.Core.300w.png)
- - -

### Installation

This library is available via [NuGet](https://docs.microsoft.com/en-us/nuget/quickstart/install-and-use-a-package-in-visual-studio). Use one of the commands below to download and install the library and all of its dependencies.

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

#### Fluent argument validation

[ValidationTarget](https://www.solidinstruments.com/api/RapidField.SolidInstruments.Core.ArgumentValidation.ValidationTarget-1.html) and [ValidationResult](https://www.solidinstruments.com/api/RapidField.SolidInstruments.Core.ArgumentValidation.ValidationResult-1.html) facilitate fluent argument validation syntax with a wide variety of out-of-the-box type support.

###### C#

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

<br />