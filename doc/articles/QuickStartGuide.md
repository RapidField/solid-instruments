<!--
Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
-->

# Quick Start Guide

Getting started with Solid Instruments is easy. This guide will help you incorporate the libraries and begin using them right away.

### About

Solid Instruments is a collection of [.NET](https://dotnet.microsoft.com/learn/dotnet/what-is-dotnet) libraries that expose APIs which make it easier to develop stable, secure, high-performance software. Solid Instruments isn't a framework. Think of it as a really big tool kit.

Solid Instruments is highly modular. Most of its component APIs are designed to be used both with and without other Solid Instruments APIs. The primitive features of the [Core](../api/RapidField.SolidInstruments.Core.html) library are heavily used by and, therefore, integrated with other Solid Instruments libraries. Broadly scoped features, on the other hand, are presented and served a la carte.

### Get Started

Solid Instruments is available via [NuGet](https://docs.microsoft.com/en-us/nuget/quickstart/install-and-use-a-package-in-visual-studio). Choose the library below that you want to import, then execute one of the associated commands using the [.NET CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/?tabs=netcore2x) or the [NuGet Package Manager console](https://docs.microsoft.com/en-us/nuget/tools/package-manager-console) to download and install it.

<br />

![Collections label](../images/Label.Collections.300w.png)
- - -

#### [RapidField.SolidInstruments.Collections](../api/RapidField.SolidInstruments.Collections.html)

<section>
Exposes useful collection types and features.
</section>

<br />

###### .NET CLI

```shell
dotnet add package RapidField.SolidInstruments.Collections
```

###### NuGet Package Manager

```shell
Install-Package RapidField.SolidInstruments.Collections
```

<br />

![Command label](../images/Label.Command.300w.png)
- - -

#### [RapidField.SolidInstruments.Command](../api/RapidField.SolidInstruments.Command.html)

<section>
Exposes implementations of the command and mediator patterns.
</section>

<br />

###### .NET CLI

```shell
dotnet add package RapidField.SolidInstruments.Command
```

###### NuGet Package Manager

```shell
Install-Package RapidField.SolidInstruments.Command
```

<br />

![Core label](../images/Label.Core.300w.png)
- - -

#### [RapidField.SolidInstruments.Core](../api/RapidField.SolidInstruments.Core.html)

<section>
Exposes foundational, general-purpose features.
</section>

<br />

###### .NET CLI

```shell
dotnet add package RapidField.SolidInstruments.Core
```

###### NuGet Package Manager

```shell
Install-Package RapidField.SolidInstruments.Core
```

<br />

![Cryptography label](../images/Label.Cryptography.300w.png)
- - -

#### [RapidField.SolidInstruments.Cryptography](../api/RapidField.SolidInstruments.Cryptography.html)

<section>
Exposes advanced security mechanics.
</section>

<br />

###### .NET CLI

```shell
dotnet add package RapidField.SolidInstruments.Cryptography
```

###### NuGet Package Manager

```shell
Install-Package RapidField.SolidInstruments.Cryptography
```

<br />

![Data Access label](../images/Label.DataAccess.300w.png)
- - -

#### [RapidField.SolidInstruments.DataAccess.EntityFramework](../api/RapidField.SolidInstruments.DataAccess.EntityFramework.html)

<section>
Provides Entity Framework implementations of the Solid Instruments data access abstractions.
</section>

<br />

###### .NET CLI

```shell
dotnet add package RapidField.SolidInstruments.DataAccess.EntityFramework
```

###### NuGet Package Manager

```shell
Install-Package RapidField.SolidInstruments.DataAccess.EntityFramework
```

<br />

![Event Authoring label](../images/Label.EventAuthoring.300w.png)
- - -

#### [RapidField.SolidInstruments.EventAuthoring](../api/RapidField.SolidInstruments.EventAuthoring.html)

<section>
Defines a simple pattern for producing and representing reportable application events.
</section>

<br />

###### .NET CLI

```shell
dotnet add package RapidField.SolidInstruments.EventAuthoring
```

###### NuGet Package Manager

```shell
Install-Package RapidField.SolidInstruments.EventAuthoring
```

<br />

![Inversion of Control label](../images/Label.InversionOfControl.300w.png)
- - -

#### [RapidField.SolidInstruments.InversionOfControl.Autofac](../api/RapidField.SolidInstruments.InversionOfControl.Autofac.html)

<section>
Exposes the Autofac implementation of Solid Instrument's inversion of control abstraction.
</section>

<br />

###### .NET CLI

```shell
dotnet add package RapidField.SolidInstruments.InversionOfControl.Autofac
```

###### NuGet Package Manager

```shell
Install-Package RapidField.SolidInstruments.InversionOfControl.Autofac
```

<br />

#### [RapidField.SolidInstruments.InversionOfControl.DotNetNative](../api/RapidField.SolidInstruments.InversionOfControl.DotNetNative.html)

<section>
Exposes the native .NET implementation of Solid Instrument's inversion of control abstraction.
</section>

<br />

###### .NET CLI

```shell
dotnet add package RapidField.SolidInstruments.InversionOfControl.DotNetNative
```

###### NuGet Package Manager

```shell
Install-Package RapidField.SolidInstruments.InversionOfControl.DotNetNative
```

<br />

![Mathematics label](../images/Label.Mathematics.300w.png)
- - -

#### [RapidField.SolidInstruments.Mathematics](../api/RapidField.SolidInstruments.Mathematics.html)

<section>
Provides a variety of intuitive mathematics APIs.
</section>

<br />

###### .NET CLI

```shell
dotnet add package RapidField.SolidInstruments.Mathematics
```

###### NuGet Package Manager

```shell
Install-Package RapidField.SolidInstruments.Mathematics
```

<br />

![Messaging label](../images/Label.Messaging.300w.png)
- - -

#### [RapidField.SolidInstruments.Messaging.AzureServiceBus](../api/RapidField.SolidInstruments.Messaging.AzureServiceBus.html)

<section>
Provides Azure Service Bus implementations of the Solid Instruments messaging abstractions.
</section>

<br />

###### .NET CLI

```shell
dotnet add package RapidField.SolidInstruments.Messaging.AzureServiceBus
```

###### NuGet Package Manager

```shell
Install-Package RapidField.SolidInstruments.Messaging.AzureServiceBus
```

<br />

#### [RapidField.SolidInstruments.Messaging.InMemory](../api/RapidField.SolidInstruments.Messaging.InMemory.html)

<section>
Provides in-memory implementations of the Solid Instruments messaging abstractions.
</section>

<br />

###### .NET CLI

```shell
dotnet add package RapidField.SolidInstruments.Messaging.InMemory
```

###### NuGet Package Manager

```shell
Install-Package RapidField.SolidInstruments.Messaging.InMemory
```

<br />

#### [RapidField.SolidInstruments.Messaging.RabbitMq](../api/RapidField.SolidInstruments.Messaging.RabbitMq.html)

<section>
Provides RabbitMQ implementations of the Solid Instruments messaging abstractions.
</section>

<br />

###### .NET CLI

```shell
dotnet add package RapidField.SolidInstruments.Messaging.RabbitMq
```

###### NuGet Package Manager

```shell
Install-Package RapidField.SolidInstruments.Messaging.RabbitMq
```

<br />

![Object Composition label](../images/Label.ObjectComposition.300w.png)
- - -

#### [RapidField.SolidInstruments.ObjectComposition](../api/RapidField.SolidInstruments.ObjectComposition.html)

<section>
Exposes simple tools for designing extensible and configurable object factories.
</section>

<br />

###### .NET CLI

```shell
dotnet add package RapidField.SolidInstruments.ObjectComposition
```

###### NuGet Package Manager

```shell
Install-Package RapidField.SolidInstruments.ObjectComposition
```

<br />

![Serialization label](../images/Label.Serialization.300w.png)
- - -

#### [RapidField.SolidInstruments.Serialization](../api/RapidField.SolidInstruments.Serialization.html)

<section>
Defines a reusable standard for custom type serializers.
</section>

<br />

###### .NET CLI

```shell
dotnet add package RapidField.SolidInstruments.Serialization
```

###### NuGet Package Manager

```shell
Install-Package RapidField.SolidInstruments.Serialization
```

<br />

![Service label](../images/Label.Service.300w.png)
- - -

#### [RapidField.SolidInstruments.Service](../api/RapidField.SolidInstruments.Service.html)

<section>
Exposes types that simplify service design.
</section>

<br />

###### .NET CLI

```shell
dotnet add package RapidField.SolidInstruments.Service
```

###### NuGet Package Manager

```shell
Install-Package RapidField.SolidInstruments.Service
```

<br />

![SignalProcessing label](../images/Label.SignalProcessing.300w.png)
- - -

#### [RapidField.SolidInstruments.SignalProcessing](../api/RapidField.SolidInstruments.SignalProcessing.html)

<section>
Facilitates digital signal processing.
</section>

<br />

###### .NET CLI

```shell
dotnet add package RapidField.SolidInstruments.SignalProcessing
```

###### NuGet Package Manager

```shell
Install-Package RapidField.SolidInstruments.SignalProcessing
```

<br />

![TextEncoding label](../images/Label.TextEncoding.300w.png)
- - -

#### [RapidField.SolidInstruments.TextEncoding](../api/RapidField.SolidInstruments.TextEncoding.html)

<section>
Offers text encoding types that are missing from the .NET Framework.
</section>

<br />

###### .NET CLI

```shell
dotnet add package RapidField.SolidInstruments.TextEncoding
```

###### NuGet Package Manager

```shell
Install-Package RapidField.SolidInstruments.TextEncoding
```