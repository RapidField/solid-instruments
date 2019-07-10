---
uid: RapidField.SolidInstruments.Cryptography.Extensions
summary: *content
---

<!--
Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
-->

Exposes extensions that support advanced security mechanics.

<br />

![Cryptography label](../images/Label.Cryptography.300w.png)
- - -

### Installation

This library is available via [NuGet](https://docs.microsoft.com/en-us/nuget/quickstart/install-and-use-a-package-in-visual-studio). Use one of the commands below to download and install the library and all of its dependencies.

###### .NET CLI

```shell
dotnet add package RapidField.SolidInstruments.Cryptography
```

###### NuGet Package Manager

```shell
Install-Package RapidField.SolidInstruments.Cryptography
```

### Usage

The examples below are provided to help you get started using the features of this library.

#### Bounded random value generation

The cryptography extensions library exposes numerous random value generation extension methods.

###### C#

```csharp
var floor = new DateTime(1000, 1, 1, 1, 1, 1, DateTimeKind.Utc);
var ceiling = new DateTime(2000, 1, 1, 1, 1, 1, DateTimeKind.Utc);
var array = new DateTime[100];

using (var randomNumberGenerator = RandomNumberGenerator.Create())
{
    // Generates 100 random DateTime values between year 1000 and 2000.
    randomNumberGenerator.FillDateTimeArray(array, floor, ceiling);
}
```

<br />