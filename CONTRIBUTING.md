<!--
Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
-->

[![Solid Instruments logo](SolidInstruments.Logo.Color.Transparent.500w.png)](README.md)
- - -

# Instructions for Contributing

**Solid Instruments** is [MIT-licensed](https://en.wikipedia.org/wiki/MIT_License). Your contributions must be your own work. Review the [license terms](LICENSE.txt) and the [**Code of Conduct**](CODE_OF_CONDUCT.md) before contributing.

For questions, contact [solidinstruments@rapidfield.com](mailto:solidinstruments@rapidfield.com).

## License

By contributing to **Solid Instruments** you assert and agree that:

1. Your contribution is your own original work.
2. You have the right to assign the copyright for the work (it is not owned by your employer, or you have been granted copyright assignment rights in writing).
3. Your contribution is licensed under [the terms](LICENSE.txt)  applied to the **Solid Instruments** project.

## Repository contents

Contents of the repository are organized as follows.

* [`/cicd`](/cicd) houses source objects which define the behavior of the CI/CD pipeline
* [`/doc`](/doc) contains content and configuration files that serve as the source for the [project website](https://www.solidinstruments.com).
* [`/example`](/example) contains sample projects that utilize the product libraries.
* [`/src`](/src) houses the source for the product libraries.
* [`/test`](/test) houses the test projects for the product libraries.

## Tooling

**Solid Instruments** is developed using [**Visual Studio**](https://visualstudio.microsoft.com/downloads) with the following extensions installed.

- [**CodeMaid**](http://www.codemaid.net) is used to maintain standard document formatting throughout the project.
- [**Visual Studio Spell Checker**](https://ewsoftware.github.io/VSSpellChecker) is used to uncover spelling errors.

CI/CD tooling is managed and defined by [`cicd/modules/AutomationTools.psm1`](cicd/modules/AutomationTools.psm1). The build environment setup process (initiated by [`cicd/scripts/ResetEnvironment.ps1`](cicd/scripts/ResetEnvironment.ps1)) installs the following tools.

### Command-line tools

* [**The .NET SDK**](https://docs.microsoft.com/en-us/dotnet/core/sdk) is the project's foundational build and test instrument.
* [**codecov.exe**](https://github.com/codecov/codecov-exe) publishes test coverage reports.
* [**DocFx**](https://dotnet.github.io/docfx/tutorial/docfx_getting_started.html) produces the [project website](https://www.solidinstruments.com).
* [**HTMLMinifier**](https://www.npmjs.com/package/html-minifier) minifies the project website.
* [**OpenCover**](https://github.com/OpenCover/opencover) executes the project tests and produces test coverage reports.
* [**powershell-yaml**](https://github.com/cloudbase/powershell-yaml) extracts configuration information during the build process.
* [**psake**](https://github.com/psake/psake) organizes and groups CI/CD operations (see [`psakefile.ps1`](psakefile.ps1)).

### Package managers

* [**Chocolatey**](https://chocolatey.org/about)
* [**npm**](https://docs.npmjs.com/about-npm)
* [**NuGet**](https://www.nuget.org)

## Design conventions

New contributors should familiarize themselves with the design conventions by reviewing existing source. When in doubt, ask for advice from experienced contributors or contact [solidinstruments@rapidfield.com](mailto:solidinstruments@rapidfield.com).

## Styling

[`.editorconfig`](.editorconfig) and [`CodeMaid.config`](CodeMaid.config), in combination, define the styling guidelines. When in doubt, look to examples within the source for styling guidance, or contact [solidinstruments@rapidfield.com](mailto:solidinstruments@rapidfield.com) with questions.

## Release versioning

Release versioning is controlled via [`appveyor.yml`](appveyor.yml). Please do not submit pull requests that modify the build version. The maintainers manage release versioning.

## Get started

Ready to start contributing? You know what to do.

```shell
git clone https://github.com/RapidField/solid-instruments.git
```

<br />

- - -

<br />

[![RapidField logo](RapidField.Logo.Color.Black.Transparent.200w.png)](https://www.rapidfield.com)

###### Copyright (c) RapidField LLC. All rights reserved. "RapidField" and "Solid Instruments" are trademarks of RapidField LLC.