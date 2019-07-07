<!--
Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
-->

![Solid Instruments logo](../../SolidInstruments.Logo.Color.Transparent.500w.png)
- - -

![Cryptography label](Label.Cryptography.300w.png)

The *RapidField.SolidInstruments.Cryptography* library exposes advanced security mechanics. Refer to [the project root documentation](../../README.md) for more information.

- [Bounded random value generation](#bounded-random-value-generation)
- [In-memory security](#in-memory-security)

- - -

### Bounded random value generation

The cryptography library exposes numerous random value generation extension methods.

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
<br/>

### In-memory security

[SecureBuffer](SecureBuffer.cs) represents a fixed-length bit field that is pinned in memory and encrypted at rest.

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
<br/>

- - -
<br />

![RapidField logo](../../RapidField.Logo.Color.Black.Transparent.200w.png)
<br /><br />
Copyright (c) RapidField LLC. All rights reserved. "RapidField" and "Solid Instruments" are trademarks of RapidField LLC.