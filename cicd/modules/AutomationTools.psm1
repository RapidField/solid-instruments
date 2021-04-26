# =================================================================================================================================
# Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
# =================================================================================================================================

<#
.Synopsis
This module defines CI/CD automation tooling for the project.
#>

# Directory names
$DirectoryNameForCicd = "cicd";
$DirectoryNameForCicdModules = "modules";
$DirectoryNameForCicdScripts = "scripts";
$DirectoryNameForCicdTools = "tools";

# File names
$FileNameForChocoExe = "choco.exe";
$FileNameForCoreModule = "Core.psm1";
$FileNameForNugetExe = "nuget.exe";

# Directory paths
$DirectoryPathForProjectRoot = (Get-Item $PSScriptRoot).Parent.Parent.FullName;
$DirectoryPathForCicd = Join-Path -Path "$DirectoryPathForProjectRoot" -ChildPath "$DirectoryNameForCicd";
$DirectoryPathForCicdModules = Join-Path -Path "$DirectoryPathForCicd" -ChildPath "$DirectoryNameForCicdModules";
$DirectoryPathForCicdScripts = Join-Path -Path "$DirectoryPathForCicd" -ChildPath "$DirectoryNameForCicdScripts";
$DirectoryPathForCicdTools = Join-Path -Path "$DirectoryPathForCicd" -ChildPath "$DirectoryNameForCicdTools";

# File paths
$FilePathForCoreModule = Join-Path -Path "$DirectoryPathForCicdModules" -ChildPath "$FileNameForCoreModule";
$FilePathForNuGetExe = Join-Path -Path "$DirectoryPathForCicdTools" -ChildPath "$FileNameForNugetExe";

# Install script URIs
$InstallScriptUriForChocolatey = "https://chocolatey.org/install.ps1";
$InstallScriptUriForNuGet = "https://dist.nuget.org/win-x86-commandline/v5.1.0/$FileNameForNugetExe";

# Environment targets
$EnvironemtnTargetForMachine = "Machine";
$EnvironmentTargetForProcess = "Process";
$EnvironmentTargetForUser = "User";

# Environment variables
$EnvironmentVariableNameForPath = "Path";

# Product names
$ProductNameForChocolatey = "Chocolatey";
$ProductNameForCodecov = "Codecov";
$ProductNameForDocFx = "DocFX";
$ProductNameForDotNet5Sdk = ".NET 5 SDK";
$ProductNameForDotNetCoreSdk = ".NET Core SDK";
$ProductNameForHtmlMinifier = "HTMLMinifier";
$ProductNameForHub = "hub";
$ProductNameForLeanify = "Leanify";
$ProductNameForNodeJs = "Node.js";
$ProductNameForNSwagStudio = "NSwagStudio";
$ProductNameForNuGet = "NuGet";
$ProductNameForOpenCover = "OpenCover";
$ProductNameForOpenSsl = "OpenSSL";
$ProductNameForPoshGit = "posh-git";
$ProductNameForPowershellYaml = "powershell-yaml";
$ProductNameForPsake = "psake";
$ProductNameForRabbitMq = "RabbitMQ";

# Product group names
$ProductGroupNameForAllAutomationTools = "all automation tools";
$ProductGroupNameForPackageManagers = "package managers";

# Chocolatey package names
$ChocolateyPackageNameForCodecov = "codecov";
$ChocolateyPackageNameForDocFx = "docfx";
$ChocolateyPackageNameForDotNet5Sdk = "dotnet-5.0-sdk";
$ChocolateyPackageNameForDotNetCoreSdk = "dotnetcore-sdk";
$ChocolateyPackageNameForHub = "hub";
$ChocolateyPackageNameForLeanify = "leanify";
$ChocolateyPackageNameForNodeJs = "nodejs";
$ChocolateyPackageNameForNSwagStudio = "nswagstudio";
$ChocolateyPackageNameForOpenCover = "opencover.portable";
$ChocolateyPackageNameForOpenSsl = "openssl.light";
$ChocolateyPackageNameForPsake = "psake";
$ChocolateyPackageNameForRabbitMq = "rabbitmq";

# NPM package names
$NpmPackageNameForHtmlMinifier = "html-minifier";

# Powershell package names
$PowershellModuleNameForPoshGit = "posh-git";
$PowershellModuleNameForPowershellYaml = "powershell-yaml";

# Command names
$CommandNameForChocolatey = "choco";
$CommandNameForCodecov = "codecov";
$CommandNameForHtmlMinifier = "html-minifier";
$CommandNameForHub = "hub";
$CommandNameForNpm = "npm";
$CommandNameForNuGet = "nuget";
$CommandNameForOpenCover = "opencover.console.exe";
$CommandNameForOpenSsl = "openssl";

# Sub-command names
$SubCommandNameForChocolateyInstall = "install";
$SubCommandNameForChocolateyUninstall = "uninstall";
$SubCommandNameForNpmInstall = "install";
$SubCommandNameForNpmUninstall = "uninstall";

# Command arguments
$CommandArgumentForChocolateyAcceptLicense = "--accept-license";
$CommandArgumentForChocolateyConfirm = "--confirm";
$CommandArgumentForChocolateyLimitOutput = "--limit-output";
$CommandArgumentForChocolateyNoProgress = "--no-progress";
$CommandArgumentForChocolateySkipAutouninstaller = "--skip-autouninstaller";
$CommandArgumentForChocolateySkipScripts = "--skip-scripts";
$CommandArgumentForNpmGlobal = "-g";
$CommandArgumentForNpmLogLevelError = "--loglevel error";

# Installation/uninstallation suppression flags
$SuppressChocolatey = $false;
$SuppressCodecov = $false;
$SuppressDocFx = $false;
$SuppressDotNet5Sdk = $false;
$SuppressDotNetCoreSdk = $false;
$SuppressHtmlMinifier = $false;
$SuppressHub = $true;
$SuppressLeanify = $true;
$SuppressNodeJs = $false;
$SuppressNSwagStudio = $false;
$SuppressNuGet = $false;
$SuppressOpenCover = $false;
$SuppressOpenSsl = $true;
$SuppressPackageManagers = $false;
$SuppressPoshGit = $true;
$SuppressPowershellYaml = $false;
$SuppressPsake = $false;
$SuppressRabbitMq = $true;

# Modules
Import-Module "$FilePathForCoreModule" -Force;

<#
.Synopsis
Returns a boolean value indicating whether or not Chocolatey is installed in the current environment.
#>
Function GetChocolateyInstallationStatus
{
    Return (Get-Command "$CommandNameForChocolatey" -ErrorAction SilentlyContinue);
}

<#
.Synopsis
Returns a boolean value indicating whether or not the specified Chocolatey package is installed in the current environment.
#>
Function GetChocolateyPackageInstallationStatus
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $PackageName
    )

    Return (GetChocolateyInstallationStatus) -and (choco list --id-only --local-only | Where-Object { $_ -eq "$PackageName" });
}

<#
.Synopsis
Returns a boolean value indicating whether or not Codecov is installed in the current environment.
#>
Function GetCodecovInstallationStatus
{
    Return GetChocolateyPackageInstallationStatus -PackageName "$ChocolateyPackageNameForCodecov";
}

<#
.Synopsis
Returns a boolean value indicating whether or not DocFX is installed in the current environment.
#>
Function GetDocFxInstallationStatus
{
    Return GetChocolateyPackageInstallationStatus -PackageName "$ChocolateyPackageNameForDocFx";
}

<#
.Synopsis
Returns a boolean value indicating whether or not the .NET 5 SDK is installed in the current environment.
#>
Function GetDotNet5SdkInstallationStatus
{
    Return GetChocolateyPackageInstallationStatus -PackageName "$ChocolateyPackageNameForDotNet5Sdk";
}

<#
.Synopsis
Returns a boolean value indicating whether or not the .NET Core SDK is installed in the current environment.
#>
Function GetDotNetCoreSdkInstallationStatus
{
    Return GetChocolateyPackageInstallationStatus -PackageName "$ChocolateyPackageNameForDotNetCoreSdk";
}

<#
.Synopsis
Returns a boolean value indicating whether or not HTMLMinifier is installed in the current environment.
#>
Function GetHtmlMinifierInstallationStatus
{
    Return (Get-Command "$CommandNameForHtmlMinifier" -ErrorAction SilentlyContinue);
}

<#
.Synopsis
Returns a boolean value indicating whether or not hub is installed in the current environment.
#>
Function GetHubInstallationStatus
{
    Return GetChocolateyPackageInstallationStatus -PackageName "$ChocolateyPackageNameForHub";
}

<#
.Synopsis
Returns a boolean value indicating whether or not Leanify is installed in the current environment.
#>
Function GetLeanifyInstallationStatus
{
    Return GetChocolateyPackageInstallationStatus -PackageName "$ChocolateyPackageNameForLeanify";
}

<#
.Synopsis
Returns a boolean value indicating whether or not Node.js is installed in the current environment.
#>
Function GetNodeJsInstallationStatus
{
    Return GetChocolateyPackageInstallationStatus -PackageName "$ChocolateyPackageNameForNodeJs";
}

<#
.Synopsis
Returns a boolean value indicating whether or not NSwagStudio is installed in the current environment.
#>
Function GetNSwagStudioInstallationStatus
{
    Return GetChocolateyPackageInstallationStatus -PackageName "$ChocolateyPackageNameForNSwagStudio";
}

<#
.Synopsis
Returns a boolean value indicating whether or not NuGet is installed in the current environment.
#>
Function GetNuGetInstallationStatus
{
    Return (Test-Path -Path "$FilePathForNuGetExe");
}

<#
.Synopsis
Returns a boolean value indicating whether or not OpenCover is installed in the current environment.
#>
Function GetOpenCoverInstallationStatus
{
    Return GetChocolateyPackageInstallationStatus -PackageName "$ChocolateyPackageNameForOpenCover";
}

<#
.Synopsis
Returns a boolean value indicating whether or not OpenSSL is installed in the current environment.
#>
Function GetOpenSslInstallationStatus
{
    Return GetChocolateyPackageInstallationStatus -PackageName "$ChocolateyPackageNameForOpenSsl";
}

<#
.Synopsis
Returns a boolean value indicating whether or not posh-git is installed in the current environment.
#>
Function GetPoshGitInstallationStatus
{
    Return GetPowerShellModuleInstallationStatus -ModuleName "$PowershellModuleNameForPoshGit";
}

<#
.Synopsis
Returns a boolean value indicating whether or not the specified PowerShell module is installed in the current environment.
#>
Function GetPowerShellModuleInstallationStatus
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $ModuleName
    )

    Return Get-Module -ListAvailable -Name "$ModuleName";
}

<#
.Synopsis
Returns a boolean value indicating whether or not powershell-yaml is installed in the current environment.
#>
Function GetPowershellYamlInstallationStatus
{
    Return GetPowerShellModuleInstallationStatus -ModuleName "$PowershellModuleNameForPowershellYaml";
}

<#
.Synopsis
Returns a boolean value indicating whether or not psake is installed in the current environment.
#>
Function GetPsakeInstallationStatus
{
    Return GetChocolateyPackageInstallationStatus -PackageName "$ChocolateyPackageNameForPsake";
}

<#
.Synopsis
Returns a boolean value indicating whether or not RabbitMQ is installed in the current environment.
#>
Function GetRabbitMqInstallationStatus
{
    Return GetChocolateyPackageInstallationStatus -PackageName "$ChocolateyPackageNameForRabbitMq";
}

<#
.Synopsis
Installs all of the available automation tools in the current environment.
#>
Function InstallAllAutomationTools
{
    ComposeStart "Installing $ProductGroupNameForAllAutomationTools.";
    InstallPackageManagers;
    InstallCodecov;
    InstallDocFx;
    InstallDotNet5Sdk;
    InstallDotNetCoreSdk;
    InstallHtmlMinifier;
    InstallHub;
    InstallLeanify;
    InstallNSwagStudio;
    InstallOpenCover;
    InstallOpenSsl;
    InstallPoshGit;
    InstallPowershellYaml;
    InstallPsake;
    InstallRabbitMq;
    ComposeFinish "Finished installing $ProductGroupNameForAllAutomationTools.";
}

<#
.Synopsis
Installs Chocolatey in the current environment.
#>
Function InstallChocolatey
{
    If ($SuppressChocolatey -eq $true)
    {
        ComposeNormal "Suppressing installation of $ProductNameForChocolatey.";
        Return;
    }
    ElseIf (GetChocolateyInstallationStatus)
    {
        ComposeNormal "$ProductNameForChocolatey is already installed.";
        Return;
    }

    ComposeStart "Installing $ProductNameForChocolatey.";
    Stop-Process -Name "$FileNameForChocoExe" -Force -ErrorAction Ignore;
    Set-ExecutionPolicy Bypass -Scope Process -Force;
    iex ((New-Object System.Net.WebClient).DownloadString($InstallScriptUriForChocolatey));
    choco feature enable -n allowGlobalConfirmation;
    ComposeFinish "Finished installing $ProductNameForChocolatey.";
}

<#
.Synopsis
Installs Codecov in the current environment.
#>
Function InstallCodecov
{
    If ($SuppressCodecov -eq $true)
    {
        ComposeNormal "Suppressing installation of $PackageProductNameForCodecov.";
        Return;
    }
    ElseIf (GetCodecovInstallationStatus)
    {
        ComposeNormal "$ProductNameForCodecov is already installed.";
        Return;
    }

    ComposeStart "Installing $ProductNameForCodecov.";
    UseChocolateyToInstall -PackageName "$ChocolateyPackageNameForCodecov";
    MakeCommandPathAvailableAll -Command "$CommandNameForCodecov";
    ComposeFinish "Finished installing $ProductNameForCodecov.";
}

<#
.Synopsis
Installs DocFX in the current environment.
#>
Function InstallDocFx
{
    If ($SuppressDocFx -eq $true)
    {
        ComposeNormal "Suppressing installation of $ProductNameForDocFx.";
        Return;
    }
    ElseIf (GetDocFxInstallationStatus)
    {
        ComposeNormal "$ProductNameForDocFx is already installed.";
        Return;
    }

    ComposeStart "Installing $ProductNameForDocFx.";
    UseChocolateyToInstall -PackageName "$ChocolateyPackageNameForDocFx";
    ComposeFinish "Finished installing $ProductNameForDocFx.";
}

<#
.Synopsis
Installs the .NET 5 SDK in the current environment.
#>
Function InstallDotNet5Sdk
{
    If ($SuppressDotNet5Sdk -eq $true)
    {
        ComposeNormal "Suppressing installation of $ProductNameForDotNet5Sdk.";
        Return;
    }
    ElseIf (GetDotNet5SdkInstallationStatus)
    {
        ComposeNormal "$ProductNameForDotNet5Sdk is already installed.";
        Return;
    }

    ComposeStart "Installing $ProductNameForDotNet5Sdk.";
    UseChocolateyToInstall -PackageName "$ChocolateyPackageNameForDotNet5Sdk";
    ComposeFinish "Finished installing $ProductNameForDotNet5Sdk.";
}

<#
.Synopsis
Installs the .NET Core SDK in the current environment.
#>
Function InstallDotNetCoreSdk
{
    If ($SuppressDotNetCoreSdk -eq $true)
    {
        ComposeNormal "Suppressing installation of $ProductNameForDotNetCoreSdk.";
        Return;
    }
    ElseIf (GetDotNetCoreSdkInstallationStatus)
    {
        ComposeNormal "$ProductNameForDotNetCoreSdk is already installed.";
        Return;
    }

    ComposeStart "Installing $ProductNameForDotNetCoreSdk.";
    UseChocolateyToInstall -PackageName "$ChocolateyPackageNameForDotNetCoreSdk";
    ComposeFinish "Finished installing $ProductNameForDotNetCoreSdk.";
}

<#
.Synopsis
Installs HTMLMinifier in the current environment.
#>
Function InstallHtmlMinifier
{
    If ($SuppressHtmlMinifier -eq $true)
    {
        ComposeNormal "Suppressing installation of $ProductNameForHtmlMinifier.";
        Return;
    }
    ElseIf (GetHtmlMinifierInstallationStatus)
    {
        ComposeNormal "$ProductNameForHtmlMinifier is already installed.";
        Return;
    }

    ComposeStart "Installing $ProductNameForHtmlMinifier.";
    UseNpmToInstall -PackageName "$NpmPackageNameForHtmlMinifier";
    MakeCommandPathAvailableAll -Command "$CommandNameForHtmlMinifier";
    ComposeFinish "Finished installing $ProductNameForHtmlMinifier.";
}

<#
.Synopsis
Installs hub in the current environment.
#>
Function InstallHub
{
    If ($SuppressHub -eq $true)
    {
        ComposeNormal "Suppressing installation of $ProductNameForHub.";
        Return;
    }
    ElseIf (GetHubInstallationStatus)
    {
        ComposeNormal "$ProductNameForHub is already installed.";
        Return;
    }

    ComposeStart "Installing $ProductNameForHub.";
    UseChocolateyToInstall -PackageName "$ChocolateyPackageNameForHub";
    MakeCommandPathAvailableAll -Command "$CommandNameForHub";
    ComposeFinish "Finished installing $ProductNameForHub.";
}

<#
.Synopsis
Installs Leanify in the current environment.
#>
Function InstallLeanify
{
    If ($SuppressLeanify -eq $true)
    {
        ComposeNormal "Suppressing installation of $ProductNameForLeanify.";
        Return;
    }
    ElseIf (GetLeanifyInstallationStatus)
    {
        ComposeNormal "$ProductNameForLeanify is already installed.";
        Return;
    }

    ComposeStart "Installing $ProductNameForLeanify.";
    UseChocolateyToInstall -PackageName "$ChocolateyPackageNameForLeanify";
    ComposeFinish "Finished installing $ProductNameForLeanify.";
}

<#
.Synopsis
Installs Node.js in the current environment.
#>
Function InstallNodeJs
{
    If ($SuppressNodeJs -eq $true)
    {
        ComposeNormal "Suppressing installation of $ProductNameForNodeJs.";
        Return;
    }
    ElseIf (GetNodeJsInstallationStatus)
    {
        ComposeNormal "$ProductNameForNodeJs is already installed.";
        Return;
    }

    ComposeStart "Installing $ProductNameForNodeJs.";
    UseChocolateyToInstall -PackageName "$ChocolateyPackageNameForNodeJs";
    MakeCommandPathAvailableAll -Command "$CommandNameForNpm";
    ComposeFinish "Finished installing $ProductNameForNodeJs.";
}

<#
.Synopsis
Installs NSwagStudio in the current environment.
#>
Function InstallNSwagStudio
{
    If ($SuppressNSwagStudio -eq $true)
    {
        ComposeNormal "Suppressing installation of $ProductNameForNSwagStudio.";
        Return;
    }
    ElseIf (GetNSwagStudioInstallationStatus)
    {
        ComposeNormal "$ProductNameForNSwagStudio is already installed.";
        Return;
    }

    ComposeStart "Installing $ProductNameForNSwagStudio.";
    UseChocolateyToInstall -PackageName "$ChocolateyPackageNameForNSwagStudio";
    ComposeFinish "Finished installing $ProductNameForNSwagStudio.";
}

<#
.Synopsis
Installs NuGet in the current environment.
#>
Function InstallNuGet
{
    If ($SuppressNuGet -eq $true)
    {
        ComposeNormal "Suppressing installation of $ProductNameForNuGet.";
        Return;
    }
    ElseIf (GetNuGetInstallationStatus)
    {
        ComposeNormal "$ProductNameForNuGet is already installed.";
        Return;
    }

    If (-not (Test-Path "$DirectoryPathForCicdTools"))
    {
        New-Item -ItemType Directory -Path "$DirectoryPathForCicdTools" -Force | Out-Null;
    }

    ComposeStart "Installing $ProductNameForNuGet.";
    Invoke-WebRequest -Uri $InstallScriptUriForNuGet -OutFile "$FilePathForNuGetExe";
    ComposeFinish "Finished installing $ProductNameForNuGet.";
}

<#
.Synopsis
Installs OpenCover in the current environment.
#>
Function InstallOpenCover
{
    If ($SuppressOpenCover -eq $true)
    {
        ComposeNormal "Suppressing installation of $ProductNameForOpenCover.";
        Return;
    }
    ElseIf (GetOpenCoverInstallationStatus)
    {
        ComposeNormal "$ProductNameForOpenCover is already installed.";
        Return;
    }

    ComposeStart "Installing $ProductNameForOpenCover.";
    UseChocolateyToInstall -PackageName "$ChocolateyPackageNameForOpenCover";
    MakeCommandPathAvailableAll -Command "$CommandNameForOpenCover";
    ComposeFinish "Finished installing $ProductNameForOpenCover.";
}

<#
.Synopsis
Installs OpenSSL in the current environment.
#>
Function InstallOpenSsl
{
    If ($SuppressOpenSsl -eq $true)
    {
        ComposeNormal "Suppressing installation of $ProductNameForOpenSsl.";
        Return;
    }
    ElseIf (GetOpenSslInstallationStatus)
    {
        ComposeNormal "$ProductNameForOpenSsl is already installed.";
        Return;
    }

    ComposeStart "Installing $ProductNameForOpenSsl.";
    UseChocolateyToInstall -PackageName "$ChocolateyPackageNameForOpenSsl";
    MakeCommandPathAvailableAll -Command "$CommandNameForOpenSsl";
    ComposeFinish "Finished installing $ProductNameForOpenSsl.";
}

<#
.Synopsis
Installs all package managers in the current environment.
#>
Function InstallPackageManagers
{
    If ($SuppressPackageManagers -eq $true)
    {
        ComposeNormal "Suppressing installation of $ProductGroupNameForPackageManagers.";
        Return;
    }

    ComposeStart "Installing $ProductGroupNameForPackageManagers.";
    InstallChocolatey;
    InstallNodeJs;
    InstallNuGet;
    ComposeFinish "Finished installing $ProductGroupNameForPackageManagers.";
}

<#
.Synopsis
Installs posh-git in the current environment.
#>
Function InstallPoshGit
{
    If ($SuppressPoshGit -eq $true)
    {
        ComposeNormal "Suppressing installation of $ProductNameForPoshGit.";
        Return;
    }
    ElseIf (GetPoshGitInstallationStatus)
    {
        ComposeNormal "$ProductNameForPoshGit is already installed.";
        Return;
    }

    ComposeStart "Installing $ProductNameForPoshGit.";
    UsePowerShellGalleryToInstall -ModuleName "$PowershellModuleNameForPoshGit";
    ComposeFinish "Finished installing $ProductNameForPoshGit.";
}

<#
.Synopsis
Installs powershell-yaml in the current environment.
#>
Function InstallPowershellYaml
{
    If ($SuppressPowershellYaml -eq $true)
    {
        ComposeNormal "Suppressing installation of $ProductNameForPowershellYaml.";
        Return;
    }
    ElseIf (GetPowershellYamlInstallationStatus)
    {
        ComposeNormal "$ProductNameForPowershellYaml is already installed.";
        Return;
    }

    ComposeStart "Installing $ProductNameForPowershellYaml.";
    UsePowerShellGalleryToInstall -ModuleName "$PowershellModuleNameForPowershellYaml";
    ComposeFinish "Finished installing $ProductNameForPowershellYaml.";
}

<#
.Synopsis
Installs psake in the current environment.
#>
Function InstallPsake
{
    If ($SuppressPsake -eq $true)
    {
        ComposeNormal "Suppressing installation of $ProductNameForPsake.";
        Return;
    }
    ElseIf (GetPsakeInstallationStatus)
    {
        ComposeNormal "$ProductNameForPsake is already installed.";
        Return;
    }

    ComposeStart "Installing $ProductNameForPsake.";
    UseChocolateyToInstall -PackageName "$ChocolateyPackageNameForPsake";
    ComposeFinish "Finished installing $ProductNameForPsake.";
}

<#
.Synopsis
Installs RabbitMQ in the current environment.
#>
Function InstallRabbitMq
{
    If ($SuppressRabbitMq -eq $true)
    {
        ComposeNormal "Suppressing installation of $ProductNameForRabbitMq.";
        Return;
    }
    ElseIf (GetRabbitMqInstallationStatus)
    {
        ComposeNormal "$ProductNameForRabbitMq is already installed.";
        Return;
    }

    ComposeStart "Installing $ProductNameForRabbitMq.";
    UseChocolateyToInstall -PackageName "$ChocolateyPackageNameForRabbitMq";
    ComposeFinish "Finished installing $ProductNameForRabbitMq.";
}

<#
.Synopsis
Exposes the path for the specified command using the specified environment target.
#>
Function MakeCommandPathAvailable
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $Command,
        [Parameter(Mandatory = $true, Position = 1)]
        [String] $EnvironmentTarget
    )

    Get-Command "$Command" | ForEach-Object `
    {
        $CommandDirectoryPath = Split-Path $_.Source;
        $PathVariable = [System.Environment]::GetEnvironmentVariable("$EnvironmentVariableNameForPath", "$EnvironmentTarget");

        If ($PathVariable -like "*$CommandDirectoryPath*")
        {
            ComposeVerbose "Command path already available for $EnvironmentTarget target: $CommandDirectoryPath";
            Return;
        }

        $PathVariable = "$PathVariable;$CommandDirectoryPath";
        [System.Environment]::SetEnvironmentVariable("$EnvironmentVariableNameForPath", "$PathVariable", "$EnvironmentTarget");
        RefreshSession;
        ComposeVerbose "Added command path for $EnvironmentTarget target: $CommandDirectoryPath";
        Return;
    }
}

<#
.Synopsis
Exposes the path for the specified command using all environment targets.
#>
Function MakeCommandPathAvailableAll
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $Command
    )

    MakeCommandPathAvailableMachine -Command "$Command";
    MakeCommandPathAvailableProcess -Command "$Command";
    MakeCommandPathAvailableUser -Command "$Command";
}

<#
.Synopsis
Exposes the path for the specified command using the Machine environment target.
#>
Function MakeCommandPathAvailableMachine
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $Command
    )

    MakeCommandPathAvailable -Command "$Command" -EnvironmentTarget "$EnvironemtnTargetForMachine";
}

<#
.Synopsis
Exposes the path for the specified command using the Process environment target.
#>
Function MakeCommandPathAvailableProcess
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $Command
    )

    MakeCommandPathAvailable -Command "$Command" -EnvironmentTarget "$EnvironmentTargetForProcess";
}

<#
.Synopsis
Exposes the path for the specified command using the User environment target.
#>
Function MakeCommandPathAvailableUser
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $Command
    )

    MakeCommandPathAvailable -Command "$Command" -EnvironmentTarget "$EnvironmentTargetForUser";
}

<#
.Synopsis
Updates the session environment and ensures that Chocolatey is available.
#>
Function RefreshSession
{
    Import-Module "$env:ChocolateyInstall\helpers\chocolateyInstaller.psm1" -Force;
    Update-SessionEnvironment;
}

<#
.Synopsis
Uninstalls, if necessary, and installs all available automation tools in the current environment.
#>
Function RestoreAllAutomationTools
{
    ComposeStart "Restoring $ProductGroupNameForAllAutomationTools.";
    InstallPackageManagers;
    UninstallAllAutomationTools;
    InstallAllAutomationTools;
    ComposeFinish "Finished restoring $ProductGroupNameForAllAutomationTools.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs Codecov in the current environment.
#>
Function RestoreCodecov
{
    ComposeStart "Restoring $ProductNameForCodecov.";
    UninstallCodecov;
    InstallCodecov;
    ComposeFinish "Finished restoring $ProductNameForCodecov.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs DocFX in the current environment.
#>
Function RestoreDocFx
{
    ComposeStart "Restoring $ProductNameForDocFx.";
    UninstallDocFx;
    InstallDocFx;
    ComposeFinish "Finished restoring $ProductNameForDocFx.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs the .NET 5 SDK in the current environment.
#>
Function RestoreDotNet5Sdk
{
    ComposeStart "Restoring $ProductNameForDotNet5Sdk.";
    UninstallDotNet5Sdk;
    InstallDotNet5Sdk;
    ComposeFinish "Finished restoring $ProductNameForDotNet5Sdk.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs the .NET Core SDK in the current environment.
#>
Function RestoreDotNetCoreSdk
{
    ComposeStart "Restoring $ProductNameForDotNetCoreSdk.";
    UninstallDotNetCoreSdk;
    InstallDotNetCoreSdk;
    ComposeFinish "Finished restoring $ProductNameForDotNetCoreSdk.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs HTMLMinifier in the current environment.
#>
Function RestoreHtmlMinifier
{
    ComposeStart "Restoring $ProductNameForHtmlMinifier.";
    UninstallHtmlMinifier;
    InstallHtmlMinifier;
    ComposeFinish "Finished restoring $ProductNameForHtmlMinifier.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs hub in the current environment.
#>
Function RestoreHtmlMinifier
{
    ComposeStart "Restoring $ProductNameForHub.";
    UninstallHub;
    InstallHub;
    ComposeFinish "Finished restoring $ProductNameForHub.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs Leanify in the current environment.
#>
Function RestoreLeanify
{
    ComposeStart "Restoring $ProductNameForLeanify.";
    UninstallLeanify;
    InstallLeanify;
    ComposeFinish "Finished restoring $ProductNameForLeanify.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs Node.js in the current environment.
#>
Function RestoreNodeJs
{
    ComposeStart "Restoring $ProductNameForNodeJs.";
    UninstallNodeJs;
    InstallNodeJs;
    ComposeFinish "Finished restoring $ProductNameForNodeJs.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs NSwagStudio in the current environment.
#>
Function RestoreNSwagStudio
{
    ComposeStart "Restoring $ProductNameForNSwagStudio.";
    UninstallNSwagStudio;
    InstallNSwagStudio;
    ComposeFinish "Finished restoring $ProductNameForNSwagStudio.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs NuGet in the current environment.
#>
Function RestoreNuGet
{
    ComposeStart "Restoring $ProductNameForNuGet.";
    UninstallNuGet;
    InstallNuGet;
    ComposeFinish "Finished restoring $ProductNameForNuGet.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs OpenCover in the current environment.
#>
Function RestoreOpenCover
{
    ComposeStart "Restoring $ProductNameForOpenCover.";
    UninstallOpenCover;
    InstallOpenCover;
    ComposeFinish "Finished restoring $ProductNameForOpenCover.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs OpenSSL in the current environment.
#>
Function RestoreOpenSsl
{
    ComposeStart "Restoring $ProductNameForOpenSsl.";
    UninstallOpenSsl;
    InstallOpenSsl;
    ComposeFinish "Finished restoring $ProductNameForOpenSsl.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs posh-git in the current environment.
#>
Function RestorePoshGit
{
    ComposeStart "Restoring $ProductNameForPoshGit.";
    UninstallPoshGit;
    InstallPoshGit;
    ComposeFinish "Finished restoring $ProductNameForPoshGit.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs powershell-yaml in the current environment.
#>
Function RestorePowershellYaml
{
    ComposeStart "Restoring $ProductNameForPowershellYaml.";
    UninstallPowershellYaml;
    InstallPowershellYaml;
    ComposeFinish "Finished restoring $ProductNameForPowershellYaml.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs psake in the current environment.
#>
Function RestorePsake
{
    ComposeStart "Restoring $ProductNameForPsake.";
    UninstallPsake;
    InstallPsake;
    ComposeFinish "Finished restoring $ProductNameForPsake.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs RabbitMQ in the current environment.
#>
Function RestoreRabbitMq
{
    ComposeStart "Restoring $ProductNameForRabbitMq.";
    UninstallRabbitMq;
    InstallRabbitMq;
    ComposeFinish "Finished restoring $ProductNameForRabbitMq.";
}

<#
.Synopsis
Uninstalls all available automation tools in the current environment.
#>
Function UninstallAllAutomationTools
{
    ComposeStart "Uninstalling $ProductGroupNameForAllAutomationTools.";
    UninstallCodecov;
    UninstallDocFx;
    UninstallDotNet5Sdk;
    UninstallDotNetCoreSdk;
    UninstallHtmlMinifier;
    UninstallHub;
    UninstallLeanify;
    UninstallNSwagStudio;
    UninstallOpenCover;
    UninstallOpenSsl;
    UninstallPoshGit;
    UninstallPowershellYaml;
    UninstallPsake;
    UninstallRabbitMq;
    ComposeFinish "Finished uninstalling $ProductGroupNameForAllAutomationTools.";
}

<#
.Synopsis
Uninstalls Codecov in the current environment.
#>
Function UninstallCodecov
{
    If ($SuppressCodecov -eq $true)
    {
        ComposeNormal "Suppressing uninstallation of $ProductNameForCodecov.";
        Return;
    }
    ElseIf (GetCodecovInstallationStatus)
    {
        ComposeStart "Uninstalling $ProductNameForCodecov.";
        UseChocolateyToUninstall -PackageName "$ChocolateyPackageNameForCodecov";
        ComposeFinish "Finished uninstalling $ProductNameForCodecov.";
    }
}

<#
.Synopsis
Uninstalls DocFX in the current environment.
#>
Function UninstallDocFx
{
    If ($SuppressDocFx -eq $true)
    {
        ComposeNormal "Suppressing uninstallation of $ProductNameForDocFx.";
        Return;
    }
    ElseIf (GetDocFxInstallationStatus)
    {
        ComposeStart "Uninstalling $ProductNameForDocFx.";
        UseChocolateyToUninstall -PackageName "$ChocolateyPackageNameForDocFx";
        ComposeFinish "Finished uninstalling $ProductNameForDocFx.";
    }
}

<#
.Synopsis
Uninstalls the .NET 5 SDK in the current environment.
#>
Function UninstallDotNet5Sdk
{
    If ($SuppressDotNet5Sdk -eq $true)
    {
        ComposeNormal "Suppressing uninstallation of $ProductNameForDotNet5Sdk.";
        Return;
    }
    ElseIf (GetDotNet5SdkInstallationStatus)
    {
        ComposeStart "Uninstalling $ProductNameForDotNet5Sdk.";
        UseChocolateyToUninstall -PackageName "$ChocolateyPackageNameForDotNet5Sdk";
        ComposeFinish "Finished uninstalling $ProductNameForDotNet5Sdk.";
    }
}

<#
.Synopsis
Uninstalls the .NET Core SDK in the current environment.
#>
Function UninstallDotNetCoreSdk
{
    If ($SuppressDotNetCoreSdk -eq $true)
    {
        ComposeNormal "Suppressing uninstallation of $ProductNameForDotNetCoreSdk.";
        Return;
    }
    ElseIf (GetDotNetCoreSdkInstallationStatus)
    {
        ComposeStart "Uninstalling $ProductNameForDotNetCoreSdk.";
        UseChocolateyToUninstall -PackageName "$ChocolateyPackageNameForDotNetCoreSdk";
        ComposeFinish "Finished uninstalling $ProductNameForDotNetCoreSdk.";
    }
}

<#
.Synopsis
Uninstalls HTMLMinifier in the current environment.
#>
Function UninstallHtmlMinifier
{
    If ($SuppressHtmlMinifier -eq $true)
    {
        ComposeNormal "Suppressing uninstallation of $ProductNameForHtmlMinifier.";
        Return;
    }
    ElseIf (GetHtmlMinifierInstallationStatus)
    {
        ComposeStart "Uninstalling $ProductNameForHtmlMinifier.";
        UseNpmToUninstall -PackageName "$NpmPackageNameForHtmlMinifier";
        ComposeFinish "Finished uninstalling $ProductNameForHtmlMinifier.";
    }
}

<#
.Synopsis
Uninstalls hub in the current environment.
#>
Function UninstallHub
{
    If ($SuppressHub -eq $true)
    {
        ComposeNormal "Suppressing uninstallation of $ProductNameForHub.";
        Return;
    }
    ElseIf (GetHubInstallationStatus)
    {
        ComposeStart "Uninstalling $ProductNameForHub.";
        UseChocolateyToUninstall -PackageName "$ChocolateyPackageNameForHub";
        ComposeFinish "Finished uninstalling $ProductNameForHub.";
    }
}

<#
.Synopsis
Uninstalls Leanify in the current environment.
#>
Function UninstallLeanify
{
    If ($SuppressLeanify -eq $true)
    {
        ComposeNormal "Suppressing uninstallation of $ProductNameForLeanify.";
        Return;
    }
    ElseIf (GetLeanifyInstallationStatus)
    {
        ComposeStart "Uninstalling $ProductNameForLeanify.";
        UseChocolateyToUninstall -PackageName "$ChocolateyPackageNameForLeanify";
        ComposeFinish "Finished uninstalling $ProductNameForLeanify.";
    }
}

<#
.Synopsis
Uninstalls Node.js in the current environment.
#>
Function UninstallNodeJs
{
    If ($SuppressNodeJs -eq $true)
    {
        ComposeNormal "Suppressing uninstallation of $ProductNameForNodeJs.";
        Return;
    }
    ElseIf (GetNodeJsInstallationStatus)
    {
        ComposeStart "Uninstalling $ProductNameForNodeJs.";
        UseChocolateyToUninstall -PackageName "$ChocolateyPackageNameForNodeJs";
        ComposeFinish "Finished uninstalling $ProductNameForNodeJs.";
    }
}

<#
.Synopsis
Uninstalls NSwagStudio in the current environment.
#>
Function UninstallNSwagStudio
{
    If ($SuppressNSwagStudio -eq $true)
    {
        ComposeNormal "Suppressing uninstallation of $ProductNameForNSwagStudio.";
        Return;
    }
    ElseIf (GetNSwagStudioInstallationStatus)
    {
        ComposeStart "Uninstalling $ProductNameForNSwagStudio.";
        UseChocolateyToUninstall -PackageName "$ChocolateyPackageNameForNSwagStudio";
        ComposeFinish "Finished uninstalling $ProductNameForNSwagStudio.";
    }
}

<#
.Synopsis
Uninstalls NuGet in the current environment.
#>
Function UninstallNuGet
{
    If ($SuppressNuGet -eq $true)
    {
        ComposeNormal "Suppressing uninstallation of $ProductNameForNuGet.";
        Return;
    }
    ElseIf (GetNuGetInstallationStatus)
    {
        ComposeStart "Uninstalling $ProductNameForNuGet.";
        Remove-Item -Path "$FilePathForNuGetExe" -Confirm:$false -Force;
        ComposeFinish "Finished uninstalling $ProductNameForNuGet.";
    }
}

<#
.Synopsis
Uninstalls OpenCover in the current environment.
#>
Function UninstallOpenCover
{
    If ($SuppressOpenCover -eq $true)
    {
        ComposeNormal "Suppressing uninstallation of $ProductNameForOpenCover.";
        Return;
    }
    ElseIf (GetOpenCoverInstallationStatus)
    {
        ComposeStart "Uninstalling $ProductNameForOpenCover.";
        UseChocolateyToUninstall -PackageName "$ChocolateyPackageNameForOpenCover";
        ComposeFinish "Finished uninstalling $ProductNameForOpenCover.";
    }
}

<#
.Synopsis
Uninstalls OpenSSL in the current environment.
#>
Function UninstallOpenSsl
{
    If ($SuppressOpenSsl -eq $true)
    {
        ComposeNormal "Suppressing uninstallation of $ProductNameForOpenSsl.";
        Return;
    }
    ElseIf (GetOpenSslInstallationStatus)
    {
        ComposeStart "Uninstalling $ProductNameForOpenSsl.";
        UseChocolateyToUninstall -PackageName "$ChocolateyPackageNameForOpenSsl";
        ComposeFinish "Finished uninstalling $ProductNameForOpenSsl.";
    }
}

<#
.Synopsis
Uninstalls posh-git in the current environment.
#>
Function UninstallPoshGit
{
    If ($SuppressPoshGit -eq $true)
    {
        ComposeNormal "Suppressing uninstallation of $ProductNameForPoshGit.";
        Return;
    }
    ElseIf (GetPoshGitInstallationStatus)
    {
        ComposeStart "Uninstalling $ProductNameForPoshGit.";
        UsePowerShellGalleryToUninstall -ModuleName "$PowershellModuleNameForPoshGit";
        ComposeFinish "Finished uninstalling $ProductNameForPoshGit.";
    }
}

<#
.Synopsis
Uninstalls powershell-yaml in the current environment.
#>
Function UninstallPowershellYaml
{
    If ($SuppressPowershellYaml -eq $true)
    {
        ComposeNormal "Suppressing uninstallation of $ProductNameForPowershellYaml.";
        Return;
    }
    ElseIf (GetPowershellYamlInstallationStatus)
    {
        ComposeStart "Uninstalling $ProductNameForPowershellYaml.";
        UsePowerShellGalleryToUninstall -ModuleName "$PowershellModuleNameForPowershellYaml";
        ComposeFinish "Finished uninstalling $ProductNameForPowershellYaml.";
    }
}

<#
.Synopsis
Uninstalls psake in the current environment.
#>
Function UninstallPsake
{
    If ($SuppressPsake -eq $true)
    {
        ComposeNormal "Suppressing uninstallation of $ProductNameForPsake.";
        Return;
    }
    ElseIf (GetPsakeInstallationStatus)
    {
        ComposeStart "Uninstalling $ProductNameForPsake.";
        UseChocolateyToUninstall -PackageName "$ChocolateyPackageNameForPsake";
        ComposeFinish "Finished uninstalling $ProductNameForPsake.";
    }
}

<#
.Synopsis
Uninstalls RabbitMQ in the current environment.
#>
Function UninstallRabbitMq
{
    If ($SuppressRabbitMq -eq $true)
    {
        ComposeNormal "Suppressing uninstallation of $ProductNameForRabbitMq.";
        Return;
    }
    ElseIf (GetRabbitMqInstallationStatus)
    {
        ComposeStart "Uninstalling $ProductNameForRabbitMq.";
        UseChocolateyToUninstall -PackageName "$ChocolateyPackageNameForRabbitMq";
        ComposeFinish "Finished uninstalling $ProductNameForRabbitMq.";
    }
}

<#
.Synopsis
Uses Chocolatey to install the specified package in the current environment.
#>
Function UseChocolateyToInstall
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $PackageName
    )

    ExecuteProcess -Path "$CommandNameForChocolatey" -Arguments "$SubCommandNameForChocolateyInstall $PackageName $CommandArgumentForChocolateyAcceptLicense $CommandArgumentForChocolateyConfirm $CommandArgumentForChocolateyLimitOutput $CommandArgumentForChocolateyNoProgress";
}

<#
.Synopsis
Uses Chocolatey to uninstall the specified package in the current environment.
#>
Function UseChocolateyToUninstall
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $PackageName
    )

    If ((GetChocolateyPackageInstallationStatus -PackageName "$PackageName") -eq $true)
    {
        ExecuteProcess -Path "$CommandNameForChocolatey" -Arguments "$SubCommandNameForChocolateyUninstall $PackageName $CommandArgumentForChocolateyLimitOutput $CommandArgumentForChocolateySkipAutouninstaller $CommandArgumentForChocolateySkipScripts";
    }
}

<#
.Synopsis
Uses npm to install the specified package in the current environment.
#>
Function UseNpmToInstall
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $PackageName
    )

    ExecuteProcess -Path "$CommandNameForNpm" -Arguments "$SubCommandNameForNpmInstall $PackageName $CommandArgumentForNpmGlobal $CommandArgumentForNpmLogLevelError";
}

<#
.Synopsis
Uses npm to uninstall the specified package in the current environment.
#>
Function UseNpmToUninstall
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $PackageName
    )

    ExecuteProcess -Path "$CommandNameForNpm" -Arguments "$SubCommandNameForNpmUninstall $PackageName $CommandArgumentForNpmGlobal $CommandArgumentForNpmLogLevelError";
}

<#
.Synopsis
Uses the PowerShell Gallery to install the specified package in the current environment.
#>
Function UsePowerShellGalleryToInstall
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $ModuleName
    )

    Install-Module -Confirm:$false -ErrorAction Stop -Force -Name "$ModuleName";
}

<#
.Synopsis
Uses the PowerShell Gallery to uninstall the specified package in the current environment.
#>
Function UsePowerShellGalleryToUninstall
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $ModuleName
    )

    If ((GetPowerShellModuleInstallationStatus -ModuleName "$ModuleName") -eq $true)
    {
        Uninstall-Module -Confirm:$false -ErrorAction Stop -Force -Name "$ModuleName";
    }
}