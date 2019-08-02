---
uid: RapidField.SolidInstruments.Cryptography
summary: *content
---

<!--
Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
-->

Exposes advanced security mechanics.

<br />

![Cryptography label](../images/Label.Cryptography.300w.png)
- - -

### Installation

This library is available via [**NuGet**](https://docs.microsoft.com/en-us/nuget/quickstart/install-and-use-a-package-in-visual-studio). Use one of the commands below to download and install the library and all of its dependencies.

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

#### In-memory security

[SecureBuffer](https://www.solidinstruments.com/api/RapidField.SolidInstruments.Cryptography.SecureBuffer.html) represents a fixed-length bit field that is pinned in memory and encrypted at rest.

###### C#

```csharp
// Allocates 1,024 bytes that are encrypted and pinned in memory.
using (var secureBuffer = new SecureBuffer(1024))
{
    secureBuffer.Access(buffer =>
    {
        // In this context, buffer is an unencrypted bit field.
    });
}
```

<br />

### Namespaces

#### [RapidField.SolidInstruments.Cryptography.Extensions](https://www.solidinstruments.com/api/RapidField.SolidInstruments.Cryptography.Extensions.html)

<section>
Exposes extensions that support advanced security mechanics.
</section>

#### [RapidField.SolidInstruments.Cryptography.Hashing](https://www.solidinstruments.com/api/RapidField.SolidInstruments.Cryptography.Hashing.html)

<section>
Provides abstractions for common hashing functions.
</section>

#### [RapidField.SolidInstruments.Cryptography.Symmetric](https://www.solidinstruments.com/api/RapidField.SolidInstruments.Cryptography.Symmetric.html)

<section>
Provides abstractions for common symmetric-key encryption algorithms.
</section>