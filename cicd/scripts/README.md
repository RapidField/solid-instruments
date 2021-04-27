<!--
Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
-->

[![Solid Instruments](../../SolidInstruments.Logo.Color.Transparent.500w.png)](../../README.md)
- - -

# /cicd/scripts

This document describes the purpose of the [:file_folder:`/cicd/scripts`]() directory.

## Purpose

This path contains scripts that serve as entry points for pipeline invocation.

- [:page_facing_up:`EncryptCodeSigningCertificate.ps1`](EncryptCodeSigningCertificate.ps1) scripts the encryption of the **Solid Instruments** code signing certificate.
- [:page_facing_up:`ExecuteCicdBuild.ps1`](ExecuteCicdBuild.ps1) scripts the standard CI/CD build process.
- [:page_facing_up:`ExecuteCicdDeployment.ps1`](ExecuteCicdDeployment.ps1) scripts the deployment process for successful CI/CD master builds.
- [:page_facing_up:`GenerateTestCertificates.ps1`](GenerateTestCertificates.ps1) scripts generation of self-signed X509 certificates that are used for testing.
- [:page_facing_up:`ResetEnvironment.ps1`](ResetEnvironment.ps1) scripts the build environment setup/reset process.

## License

[![License](https://img.shields.io/github/license/rapidfield/solid-instruments?style=flat&color=lightseagreen&label=license&logo=open-access&logoColor=lightgrey)](https://github.com/RapidField/solid-instruments/blob/master/LICENSE.txt)

**Solid Instruments** is [MIT-licensed](https://en.wikipedia.org/wiki/MIT_License). Review the [**License Terms**](../../LICENSE.txt) for more information.

<br />

- - -

<br />

[![RapidField](../../RapidField.Logo.Color.Black.Transparent.200w.png)](https://www.rapidfield.com)

###### Copyright (c) RapidField LLC. All rights reserved. "RapidField" and "Solid Instruments" are trademarks of RapidField LLC.