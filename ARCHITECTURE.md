<!--
Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
-->

[![Solid Instruments](SolidInstruments.Logo.Color.Transparent.500w.png)](README.md)
- - -

# Architectural Guide

The **Solid Instruments** product deliverables consist of a collection of [**.NET Standard**](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) libraries which are distributed as [**NuGet**](https://docs.microsoft.com/en-us/nuget/what-is-nuget) packages via the [**NuGet Gallery**](https://www.nuget.org/packages?q=title%3ARapidField.SolidInstruments).

## Principles

Our team is committed to seeking, adopting and maintaining best practices. Here are some philosophies that guide our decision making at a high level.

### Reliability over flexibility

In most cases, the team should...

:thumbsup: **favor a design that**

- advertises its modes of failure
- fails as advertised
- cultivates trust among users

:thumbsdown: **over a design that**

- corrects anticipated user mistakes
- tolerates external faults
- casts doubt upon expected behavior

### Consistency over novelty

In most cases, the team should...

:thumbsup: **favor a design that**

- uses perennial technologies and/or patterns
- establishes or adheres to meaningful conventions
- sets forth clear, helpful, repeatable examples for others

:thumbsdown: **over a design that**

- introduces new technologies and/or patterns
- departs from established, meaningful conventions
- confuses others

### Clarity over brevity

In most cases, the team should...

:thumbsup: **favor a design that**

- employs unambiguous terminology and naming conventions
- is thoroughly documented
- can be readily understood and used by others

:thumbsdown: **over a design that**

- was developed quickly
- saves visual space on screen
- can only be understood and used after careful research

## Dependencies

The maintainers of **Solid Instruments** make every effort to minimize the inclusion of third-party dependencies. Several of the constituent libraries expose implementations of first-party abstractions for competing third-party product libraries. Those first-party abstractions are listed below with their accompanying implementations.

- [`RapidField.SolidInstruments.DataAccess`](src/RapidField.SolidInstruments.DataAccess/README.md)
  - [`RapidField.SolidInstruments.DataAccess.EntityFramework`](src/RapidField.SolidInstruments.DataAccess.EntityFramework/README.md)
- [`RapidField.SolidInstruments.InversionOfControl`](src/RapidField.SolidInstruments.InversionOfControl/README.md)
  - [`RapidField.SolidInstruments.InversionOfControl.Autofac`](src/RapidField.SolidInstruments.InversionOfControl.Autofac/README.md)
  - [`RapidField.SolidInstruments.InversionOfControl.DotNetNative`](src/RapidField.SolidInstruments.InversionOfControl.DotNetNative/README.md)
- [`RapidField.SolidInstruments.Messaging`](src/RapidField.SolidInstruments.Messaging/README.md)
  - [`RapidField.SolidInstruments.Messaging.AzureServiceBus`](src/RapidField.SolidInstruments.Messaging.AzureServiceBus/README.md)
  - [`RapidField.SolidInstruments.Messaging.RabbitMq`](src/RapidField.SolidInstruments.Messaging.RabbitMq/README.md)

To review the constituent library dependencies, navigate to any library from [this page](https://www.nuget.org/packages?q=title%3ARapidField.SolidInstruments) and expand the "Dependencies" section.

## More information

If you haven't already, please review the [**Instructions for Contributing**](CONTRIBUTING.md), where you will find guidance on other topics such as support and revision control. For a thorough treatment on coding standards review the [**Development Guidelines**](GUIDELINES.md). For questions, contact [solidinstruments@rapidfield.com](mailto:solidinstruments@rapidfield.com).

<br />

- - -

<br />

[![RapidField](RapidField.Logo.Color.Black.Transparent.200w.png)](https://www.rapidfield.com)

###### Copyright (c) RapidField LLC. All rights reserved. "RapidField" and "Solid Instruments" are trademarks of RapidField LLC.