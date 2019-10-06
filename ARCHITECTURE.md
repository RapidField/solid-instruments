<!--
Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
-->

[![Solid Instruments](SolidInstruments.Logo.Color.Transparent.500w.png)](README.md)
- - -

# System Architecture Guide

The **Solid Instruments** product deliverables consist of a collection of [**.NET Standard**](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) libraries which are distributed as [**NuGet**](https://docs.microsoft.com/en-us/nuget/what-is-nuget) packages via the [**NuGet Gallery**](https://www.nuget.org/packages?q=title%3ARapidField.SolidInstruments).

## Dependencies

The maintainers of **Solid Instruments** make every effort to minimize the inclusion of third-party dependencies. Several of the constituent libraries expose implementations of first-party abstractions for competing third-party product libraries. Those first-party abstractions are listed below with their accompanying implementations.

- [**RapidField.SolidInstruments.DataAccess**](src/RapidField.SolidInstruments.DataAccess/README.md)
  - [**RapidField.SolidInstruments.DataAccess.EntityFramework**](src/RapidField.SolidInstruments.DataAccess.EntityFramework/README.md)
- [**RapidField.SolidInstruments.InversionOfControl**](src/RapidField.SolidInstruments.InversionOfControl/README.md)
  - [**RapidField.SolidInstruments.InversionOfControl.Autofac**](src/RapidField.SolidInstruments.InversionOfControl.Autofac/README.md)
  - [**RapidField.SolidInstruments.InversionOfControl.DotNetNative**](src/RapidField.SolidInstruments.InversionOfControl.DotNetNative/README.md)
- [**RapidField.SolidInstruments.Messaging**](src/RapidField.SolidInstruments.Messaging/README.md)
  - [**RapidField.SolidInstruments.Messaging.AzureServiceBus**](src/RapidField.SolidInstruments.Messaging.AzureServiceBus/README.md)
  - [**RapidField.SolidInstruments.Messaging.RabbitMq**](src/RapidField.SolidInstruments.Messaging.RabbitMq/README.md)

To review the constituent library dependencies, navigate to any library from [this page](https://www.nuget.org/packages?q=title%3ARapidField.SolidInstruments) and expand the "Dependencies" section.

## Contributing

Ready to contribute? Before beginning work please read the [**Instructions for Contributing**](CONTRIBUTING.md).

<br />

- - -

<br />

[![RapidField](RapidField.Logo.Color.Black.Transparent.200w.png)](https://www.rapidfield.com)

###### Copyright (c) RapidField LLC. All rights reserved. "RapidField" and "Solid Instruments" are trademarks of RapidField LLC.