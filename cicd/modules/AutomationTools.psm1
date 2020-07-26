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

# Chocolatey package names
$ChoclateyPackageNameForCodecov = "codecov";
$ChoclateyPackageNameForDocFx = "docfx";
$ChoclateyPackageNameForDotNetCoreSdk = "dotnetcore-sdk";
$ChoclateyPackageNameForHub = "hub";
$ChoclateyPackageNameForLeanify = "leanify";
$ChoclateyPackageNameForNodeJs = "nodejs";
$ChoclateyPackageNameForOpenCover = "opencover.portable";
$ChoclateyPackageNameForOpenSsl = "openssl.light";
$ChoclateyPackageNameForPsake = "psake";
$ChoclateyPackageNameForRabbitMq = "rabbitmq";

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

# Installation/uninstallation suppression flags
$SuppressChocolatey = $false;
$SuppressCodecov = $false;
$SuppressDocFx = $false;
$SuppressDotNetCoreSdk = $false;
$SuppressHtmlMinifier = $false;
$SuppressHub = $true;
$SuppressLeanify = $true;
$SuppressNodeJs = $false;
$SuppressNuGet = $false;
$SuppressOpenCover = $false;
$SuppressOpenSsl = $true;
$SuppressPackageManagers = $false;
$SuppressPoshGit = $true;
$SuppressPowershellYaml = $false;
$SuppressPsake = $false;
$SuppressRabbitMq = $true;

# Modules
Import-Module $FilePathForCoreModule -Force;

<#
.Synopsis
Returns a boolean value indicating whether or not Chocolatey is installed in the current environment.
#>
Function GetChocolateyInstallationStatus
{
    Return (Get-Command $CommandNameForChocolatey -ErrorAction SilentlyContinue);
}

<#
.Synopsis
Returns a boolean value indicating whether or not Codecov is installed in the current environment.
#>
Function GetCodecovInstallationStatus
{
    Return (GetChocolateyInstallationStatus) -and (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForCodecov") });
}

<#
.Synopsis
Returns a boolean value indicating whether or not DocFX is installed in the current environment.
#>
Function GetDocFxInstallationStatus
{
    Return (GetChocolateyInstallationStatus) -and (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForDocFx") });
}

<#
.Synopsis
Returns a boolean value indicating whether or not the .NET Core SDK is installed in the current environment.
#>
Function GetDotNetCoreSdkInstallationStatus
{
    Return (GetChocolateyInstallationStatus) -and (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForDotNetCoreSdk") });
}

<#
.Synopsis
Returns a boolean value indicating whether or not HTMLMinifier is installed in the current environment.
#>
Function GetHtmlMinifierInstallationStatus
{
    Return (Get-Command $CommandNameForHtmlMinifier -ErrorAction SilentlyContinue);
}

<#
.Synopsis
Returns a boolean value indicating whether or not hub is installed in the current environment.
#>
Function GetHubInstallationStatus
{
    Return (GetChocolateyInstallationStatus) -and (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForHub") });
}

<#
.Synopsis
Returns a boolean value indicating whether or not Leanify is installed in the current environment.
#>
Function GetLeanifyInstallationStatus
{
    Return (GetChocolateyInstallationStatus) -and (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForLeanify") });
}

<#
.Synopsis
Returns a boolean value indicating whether or not Node.js is installed in the current environment.
#>
Function GetNodeJsInstallationStatus
{
    Return (GetChocolateyInstallationStatus) -and (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForNodeJs") });
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
    Return (GetChocolateyInstallationStatus) -and (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForOpenCover") });
}

<#
.Synopsis
Returns a boolean value indicating whether or not OpenSSL is installed in the current environment.
#>
Function GetOpenSslInstallationStatus
{
    Return (GetChocolateyInstallationStatus) -and (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForOpenSsl") });
}

<#
.Synopsis
Returns a boolean value indicating whether or not posh-git is installed in the current environment.
#>
Function GetPoshGitInstallationStatus
{
    Return Get-Module -ListAvailable -Name "$PowershellModuleNameForPoshGit";
}

<#
.Synopsis
Returns a boolean value indicating whether or not powershell-yaml is installed in the current environment.
#>
Function GetPowershellYamlInstallationStatus
{
    Return Get-Module -ListAvailable -Name "$PowershellModuleNameForPowershellYaml";
}

<#
.Synopsis
Returns a boolean value indicating whether or not psake is installed in the current environment.
#>
Function GetPsakeInstallationStatus
{
    Return (GetChocolateyInstallationStatus) -and (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForPsake") });
}

<#
.Synopsis
Returns a boolean value indicating whether or not RabbitMQ is installed in the current environment.
#>
Function GetRabbitMqInstallationStatus
{
    Return (GetChocolateyInstallationStatus) -and (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForRabbitMq") });
}

<#
.Synopsis
Installs all of the available automation tools in the current environment.
#>
Function InstallAllAutomationTools
{
    ComposeStart "Installing all automation tools.";
    InstallPackageManagers;
    InstallCodecov;
    InstallDocFx;
    InstallDotNetCoreSdk;
    InstallHtmlMinifier;
    InstallHub;
    InstallLeanify;
    InstallOpenCover;
    InstallOpenSsl;
    InstallPoshGit;
    InstallPowershellYaml;
    InstallPsake;
    InstallRabbitMq;
    ComposeFinish "Finished installing all automation tools.";
}

<#
.Synopsis
Installs Chocolatey in the current environment.
#>
Function InstallChocolatey
{
    If ($SuppressChoclatey -eq $true)
    {
        ComposeNormal "Suppressing installation of Chocolatey.";
        Return;
    }
    ElseIf (GetChocolateyInstallationStatus)
    {
        ComposeNormal "Chocolatey is already installed.";
        Return;
    }

    ComposeStart "Installing Chocolatey.";
    Stop-Process -Name "$FileNameForChocoExe" -Force -ErrorAction Ignore;
    Set-ExecutionPolicy Bypass -Scope Process -Force;
    iex ((New-Object System.Net.WebClient).DownloadString($InstallScriptUriForChocolatey));
    choco feature enable -n allowGlobalConfirmation;
    ComposeFinish "Finished installing Chocolatey.";
}

<#
.Synopsis
Installs Codecov in the current environment.
#>
Function InstallCodecov
{
    If ($SuppressCodecov -eq $true)
    {
        ComposeNormal "Suppressing installation of Codecov.";
        Return;
    }
    ElseIf (GetCodecovInstallationStatus)
    {
        ComposeNormal "Codecov is already installed.";
        Return;
    }

    ComposeStart "Installing Codecov.";
    UseChocolateyToInstall -PackageName "$ChoclateyPackageNameForCodecov";
    MakeCommandPathAvailableAll -Command $CommandNameForCodecov;
    ComposeFinish "Finished installing Codecov.";
}

<#
.Synopsis
Installs DocFX in the current environment.
#>
Function InstallDocFx
{
    If ($SuppressDocFx -eq $true)
    {
        ComposeNormal "Suppressing installation of DocFX.";
        Return;
    }
    ElseIf (GetDocFxInstallationStatus)
    {
        ComposeNormal "DocFX is already installed.";
        Return;
    }

    ComposeStart "Installing DocFX.";
    UseChocolateyToInstall -PackageName "$ChoclateyPackageNameForDocFx";
    ComposeFinish "Finished installing DocFX.";
}

<#
.Synopsis
Installs the .NET Core SDK in the current environment.
#>
Function InstallDotNetCoreSdk
{
    If ($SuppressDotNetCoreSdk -eq $true)
    {
        ComposeNormal "Suppressing installation of the .NET Core SDK.";
        Return;
    }
    ElseIf (GetDotNetCoreSdkInstallationStatus)
    {
        ComposeNormal "The .NET Core SDK is already installed.";
        Return;
    }

    ComposeStart "Installing the .NET Core SDK.";
    UseChocolateyToInstall -PackageName "$ChoclateyPackageNameForDotNetCoreSdk";
    ComposeFinish "Finished installing the .NET Core SDK.";
}

<#
.Synopsis
Installs HTMLMinifier in the current environment.
#>
Function InstallHtmlMinifier
{
    If ($SuppressHtmlMinifier -eq $true)
    {
        ComposeNormal "Suppressing installation of HTMLMinifier.";
        Return;
    }
    ElseIf (GetHtmlMinifierInstallationStatus)
    {
        ComposeNormal "HTMLMinifier is already installed.";
        Return;
    }

    ComposeStart "Installing HTMLMinifier.";
    UseNpmToInstall -PackageName "$NpmPackageNameForHtmlMinifier";
    MakeCommandPathAvailableAll -Command $CommandNameForHtmlMinifier;
    ComposeFinish "Finished installing HTMLMinifier.";
}

<#
.Synopsis
Installs hub in the current environment.
#>
Function InstallHub
{
    If ($SuppressHub -eq $true)
    {
        ComposeNormal "Suppressing installation of hub.";
        Return;
    }
    ElseIf (GetHubInstallationStatus)
    {
        ComposeNormal "hub is already installed.";
        Return;
    }

    ComposeStart "Installing hub.";
    UseChocolateyToInstall -PackageName "$ChoclateyPackageNameForHub";
    MakeCommandPathAvailableAll -Command $CommandNameForHub;
    ComposeFinish "Finished installing hub.";
}

<#
.Synopsis
Installs Leanify in the current environment.
#>
Function InstallLeanify
{
    If ($SuppressLeanify -eq $true)
    {
        ComposeNormal "Suppressing installation of Leanify.";
        Return;
    }
    ElseIf (GetLeanifyInstallationStatus)
    {
        ComposeNormal "Leanify is already installed.";
        Return;
    }

    ComposeStart "Installing Leanify.";
    UseChocolateyToInstall -PackageName "$ChoclateyPackageNameForLeanify";
    ComposeFinish "Finished installing Leanify.";
}

<#
.Synopsis
Installs Node.js in the current environment.
#>
Function InstallNodeJs
{
    If ($SuppressNodeJs -eq $true)
    {
        ComposeNormal "Suppressing installation of Node.js.";
        Return;
    }
    ElseIf (GetNodeJsInstallationStatus)
    {
        ComposeNormal "Node.js is already installed.";
        Return;
    }

    ComposeStart "Installing Node.js.";
    UseChocolateyToInstall -PackageName "$ChoclateyPackageNameForNodeJs";
    MakeCommandPathAvailableAll -Command $CommandNameForNpm;
    ComposeFinish "Finished installing Node.js.";
}

<#
.Synopsis
Installs NuGet in the current environment.
#>
Function InstallNuGet
{
    If ($SuppressNuGet -eq $true)
    {
        ComposeNormal "Suppressing installation of NuGet.";
        Return;
    }
    ElseIf (GetNuGetInstallationStatus)
    {
        ComposeNormal "NuGet is already installed.";
        Return;
    }

    If (-not (Test-Path "$DirectoryPathForCicdTools"))
    {
        New-Item -ItemType Directory -Path "$DirectoryPathForCicdTools" -Force | Out-Null;
    }

    ComposeStart "Installing NuGet.";
    Invoke-WebRequest -Uri $InstallScriptUriForNuGet -OutFile "$FilePathForNuGetExe";
    ComposeFinish "Finished installing NuGet.";
}

<#
.Synopsis
Installs OpenCover in the current environment.
#>
Function InstallOpenCover
{
    If ($SuppressOpenCover -eq $true)
    {
        ComposeNormal "Suppressing installation of OpenCover.";
        Return;
    }
    ElseIf (GetOpenCoverInstallationStatus)
    {
        ComposeNormal "OpenCover is already installed.";
        Return;
    }

    ComposeStart "Installing OpenCover.";
    UseChocolateyToInstall -PackageName "$ChoclateyPackageNameForOpenCover";
    MakeCommandPathAvailableAll -Command $CommandNameForOpenCover;
    ComposeFinish "Finished installing OpenCover.";
}

<#
.Synopsis
Installs OpenSSL in the current environment.
#>
Function InstallOpenSsl
{
    If ($SuppressOpenSsl -eq $true)
    {
        ComposeNormal "Suppressing installation of OpenSSL.";
        Return;
    }
    ElseIf (GetOpenSslInstallationStatus)
    {
        ComposeNormal "OpenSSL is already installed.";
        Return;
    }

    ComposeStart "Installing OpenSSL.";
    UseChocolateyToInstall -PackageName "$ChoclateyPackageNameForOpenSsl";
    MakeCommandPathAvailableAll -Command $CommandNameForOpenSsl;
    ComposeFinish "Finished installing OpenSSL.";
}

<#
.Synopsis
Installs all package managers in the current environment.
#>
Function InstallPackageManagers
{
    If ($SuppressPackageManagers -eq $true)
    {
        ComposeNormal "Suppressing installation of package managers.";
        Return;
    }

    ComposeStart "Installing package managers.";
    InstallChocolatey;
    InstallNodeJs;
    InstallNuGet;
    ComposeFinish "Finished installing package managers.";
}

<#
.Synopsis
Installs posh-git in the current environment.
#>
Function InstallPoshGit
{
    If ($SuppressPoshGit -eq $true)
    {
        ComposeNormal "Suppressing installation of posh-git.";
        Return;
    }
    ElseIf (GetPoshGitInstallationStatus)
    {
        ComposeNormal "posh-git is already installed.";
        Return;
    }

    ComposeStart "Installing posh-git.";
    UsePowerShellGalleryToInstall -ModuleName "$PowershellModuleNameForPoshGit";
    ComposeFinish "Finished installing posh-git.";
}

<#
.Synopsis
Installs powershell-yaml in the current environment.
#>
Function InstallPowershellYaml
{
    If ($SuppressPowershellYaml -eq $true)
    {
        ComposeNormal "Suppressing installation of powershell-yaml.";
        Return;
    }
    ElseIf (GetPowershellYamlInstallationStatus)
    {
        ComposeNormal "powershell-yaml is already installed.";
        Return;
    }

    ComposeStart "Installing powershell-yaml.";
    UsePowerShellGalleryToInstall -ModuleName "$PowershellModuleNameForPowershellYaml";
    ComposeFinish "Finished installing powershell-yaml.";
}

<#
.Synopsis
Installs psake in the current environment.
#>
Function InstallPsake
{
    If ($SuppressPsake -eq $true)
    {
        ComposeNormal "Suppressing installation of psake.";
        Return;
    }
    ElseIf (GetPsakeInstallationStatus)
    {
        ComposeNormal "psake is already installed.";
        Return;
    }

    ComposeStart "Installing psake.";
    UseChocolateyToInstall -PackageName "$ChoclateyPackageNameForPsake";
    ComposeFinish "Finished installing psake.";
}

<#
.Synopsis
Installs RabbitMQ in the current environment.
#>
Function InstallRabbitMq
{
    If ($SuppressRabbitMq -eq $true)
    {
        ComposeNormal "Suppressing installation of RabbitMQ.";
        Return;
    }
    ElseIf (GetRabbitMqInstallationStatus)
    {
        ComposeNormal "RabbitMQ is already installed.";
        Return;
    }

    ComposeStart "Installing RabbitMQ.";
    UseChocolateyToInstall -PackageName "$ChoclateyPackageNameForRabbitMq";
    ComposeFinish "Finished installing RabbitMQ.";
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
        $PathVariable = [System.Environment]::GetEnvironmentVariable("Path", $EnvironmentTarget);

        If ($PathVariable -like "*$CommandDirectoryPath*")
        {
            ComposeVerbose "Command path already available for $EnvironmentTarget target: $CommandDirectoryPath";
            Return;
        }

        $PathVariable = $PathVariable + ";$CommandDirectoryPath";
        [System.Environment]::SetEnvironmentVariable("Path", "$PathVariable", $EnvironmentTarget);
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

    MakeCommandPathAvailableMachine -Command $Command;
    MakeCommandPathAvailableProcess -Command $Command;
    MakeCommandPathAvailableUser -Command $Command;
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

    MakeCommandPathAvailable -Command $Command -EnvironmentTarget "Machine";
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

    MakeCommandPathAvailable -Command $Command -EnvironmentTarget "Process";
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

    MakeCommandPathAvailable -Command $Command -EnvironmentTarget "User";
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
    ComposeStart "Restoring all automation tools.";
    InstallPackageManagers;
    UninstallAllAutomationTools;
    InstallAllAutomationTools;
    ComposeFinish "Finished restoring all automation tools.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs Codecov in the current environment.
#>
Function RestoreCodecov
{
    ComposeStart "Restoring Codecov.";
    UninstallCodecov;
    InstallCodecov;
    ComposeFinish "Finished restoring Codecov.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs DocFX in the current environment.
#>
Function RestoreDocFx
{
    ComposeStart "Restoring DocFX.";
    UninstallDocFx;
    InstallDocFx;
    ComposeFinish "Finished restoring DocFX.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs the .NET Core SDK in the current environment.
#>
Function RestoreDotNetCoreSdk
{
    ComposeStart "Restoring the .NET Core SDK.";
    UninstallDotNetCoreSdk;
    InstallDotNetCoreSdk;
    ComposeFinish "Finished restoring the .NET Core SDK.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs HTMLMinifier in the current environment.
#>
Function RestoreHtmlMinifier
{
    ComposeStart "Restoring HTMLMinifier.";
    UninstallHtmlMinifier;
    InstallHtmlMinifier;
    ComposeFinish "Finished restoring HTMLMinifier.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs hub in the current environment.
#>
Function RestoreHtmlMinifier
{
    ComposeStart "Restoring hub.";
    UninstallHub;
    InstallHub;
    ComposeFinish "Finished restoring hub.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs Leanify in the current environment.
#>
Function RestoreLeanify
{
    ComposeStart "Restoring Leanify.";
    UninstallLeanify;
    InstallLeanify;
    ComposeFinish "Finished restoring Leanify.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs Node.js in the current environment.
#>
Function RestoreNodeJs
{
    ComposeStart "Restoring Node.js.";
    UninstallNodeJs;
    InstallNodeJs;
    ComposeFinish "Finished restoring Node.js.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs NuGet in the current environment.
#>
Function RestoreNuGet
{
    ComposeStart "Restoring NuGet.";
    UninstallNuGet;
    InstallNuGet;
    ComposeFinish "Finished restoring NuGet.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs OpenCover in the current environment.
#>
Function RestoreOpenCover
{
    ComposeStart "Restoring OpenCover.";
    UninstallOpenCover;
    InstallOpenCover;
    ComposeFinish "Finished restoring OpenCover.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs OpenSSL in the current environment.
#>
Function RestoreOpenSsl
{
    ComposeStart "Restoring OpenSSL.";
    UninstallOpenSsl;
    InstallOpenSsl;
    ComposeFinish "Finished restoring OpenSSL.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs posh-git in the current environment.
#>
Function RestorePoshGit
{
    ComposeStart "Restoring posh-git.";
    UninstallPoshGit;
    InstallPoshGit;
    ComposeFinish "Finished restoring posh-git.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs powershell-yaml in the current environment.
#>
Function RestorePowershellYaml
{
    ComposeStart "Restoring powershell-yaml.";
    UninstallPowershellYaml;
    InstallPowershellYaml;
    ComposeFinish "Finished restoring powershell-yaml.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs psake in the current environment.
#>
Function RestorePsake
{
    ComposeStart "Restoring psake.";
    UninstallPsake;
    InstallPsake;
    ComposeFinish "Finished restoring psake.";
}

<#
.Synopsis
Uninstalls, if necessary, and installs RabbitMQ in the current environment.
#>
Function RestoreRabbitMq
{
    ComposeStart "Restoring RabbitMQ.";
    UninstallRabbitMq;
    InstallRabbitMq;
    ComposeFinish "Finished restoring RabbitMQ.";
}

<#
.Synopsis
Uninstalls all available automation tools in the current environment.
#>
Function UninstallAllAutomationTools
{
    ComposeStart "Uninstalling all automation tools.";
    UninstallCodecov;
    UninstallDocFx;
    UninstallDotNetCoreSdk;
    UninstallHtmlMinifier;
    UninstallHub;
    UninstallLeanify;
    UninstallOpenCover;
    UninstallOpenSsl;
    UninstallPoshGit;
    UninstallPowershellYaml;
    UninstallPsake;
    UninstallRabbitMq;
    ComposeFinish "Finished uninstalling all automation tools.";
}

<#
.Synopsis
Uninstalls Codecov in the current environment.
#>
Function UninstallCodecov
{
    If ($SuppressCodecov -eq $true)
    {
        ComposeNormal "Suppressing uninstallation of Codecov.";
        Return;
    }
    ElseIf (GetCodecovInstallationStatus)
    {
        ComposeStart "Uninstalling Codecov.";
        UseChocolateyToUninstall -PackageName "$ChoclateyPackageNameForCodecov";
        ComposeFinish "Finished uninstalling Codecov.";
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
        ComposeNormal "Suppressing uninstallation of DocFX.";
        Return;
    }
    ElseIf (GetDocFxInstallationStatus)
    {
        ComposeStart "Uninstalling DocFX.";
        UseChocolateyToUninstall -PackageName "$ChoclateyPackageNameForDocFx";
        ComposeFinish "Finished uninstalling DocFX.";
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
        ComposeNormal "Suppressing uninstallation of the .NET Core SDK.";
        Return;
    }
    ElseIf (GetDotNetCoreSdkInstallationStatus)
    {
        ComposeStart "Uninstalling the .NET Core SDK.";
        UseChocolateyToUninstall -PackageName "$ChoclateyPackageNameForDotNetCoreSdk";
        ComposeFinish "Finished uninstalling the .NET Core SDK.";
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
        ComposeNormal "Suppressing uninstallation of HTMLMinifier.";
        Return;
    }
    ElseIf (GetHtmlMinifierInstallationStatus)
    {
        ComposeStart "Uninstalling HTMLMinifier.";
        UseNpmToUninstall -PackageName "$NpmPackageNameForHtmlMinifier";
        ComposeFinish "Finished uninstalling HTMLMinifier.";
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
        ComposeNormal "Suppressing uninstallation of hub.";
        Return;
    }
    ElseIf (GetLeanifyInstallationStatus)
    {
        ComposeStart "Uninstalling hub.";
        UseChocolateyToUninstall -PackageName "$ChoclateyPackageNameForHub";
        ComposeFinish "Finished uninstalling hub.";
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
        ComposeNormal "Suppressing uninstallation of Leanify.";
        Return;
    }
    ElseIf (GetLeanifyInstallationStatus)
    {
        ComposeStart "Uninstalling Leanify.";
        UseChocolateyToUninstall -PackageName "$ChoclateyPackageNameForLeanify";
        ComposeFinish "Finished uninstalling Leanify.";
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
        ComposeNormal "Suppressing uninstallation of Node.js.";
        Return;
    }
    ElseIf (GetNodeJsInstallationStatus)
    {
        ComposeStart "Uninstalling Node.js.";
        UseChocolateyToUninstall -PackageName "$ChoclateyPackageNameForNodeJs";
        ComposeFinish "Finished uninstalling Node.js.";
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
        ComposeNormal "Suppressing uninstallation of NuGet.";
        Return;
    }
    ElseIf (GetNuGetInstallationStatus)
    {
        ComposeStart "Uninstalling NuGet.";
        Remove-Item -Path "$FilePathForNuGetExe" -Confirm:$false -Force;
        ComposeFinish "Finished uninstalling NuGet.";
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
        ComposeNormal "Suppressing uninstallation of OpenCover.";
        Return;
    }
    ElseIf (GetOpenCoverInstallationStatus)
    {
        ComposeStart "Uninstalling OpenCover.";
        UseChocolateyToUninstall -PackageName "$ChoclateyPackageNameForOpenCover";
        ComposeFinish "Finished uninstalling OpenCover.";
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
        ComposeNormal "Suppressing uninstallation of OpenSSL.";
        Return;
    }
    ElseIf (GetOpenSslInstallationStatus)
    {
        ComposeStart "Uninstalling OpenSSL.";
        UseChocolateyToUninstall -PackageName "$ChoclateyPackageNameForOpenSsl";
        ComposeFinish "Finished uninstalling OpenSSL.";
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
        ComposeNormal "Suppressing uninstallation of posh-git.";
        Return;
    }
    ElseIf (GetPoshGitInstallationStatus)
    {
        ComposeStart "Uninstalling posh-git.";
        UsePowerShellGalleryToUninstall -ModuleName "$PowershellModuleNameForPoshGit";
        ComposeFinish "Finished uninstalling posh-git.";
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
        ComposeNormal "Suppressing uninstallation of powershell-yaml.";
        Return;
    }
    ElseIf (GetPowershellYamlInstallationStatus)
    {
        ComposeStart "Uninstalling powershell-yaml.";
        UsePowerShellGalleryToUninstall -ModuleName "$PowershellModuleNameForPowershellYaml";
        ComposeFinish "Finished uninstalling powershell-yaml.";
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
        ComposeNormal "Suppressing uninstallation of psake.";
        Return;
    }
    ElseIf (GetPsakeInstallationStatus)
    {
        ComposeStart "Uninstalling psake.";
        UseChocolateyToUninstall -PackageName "$ChoclateyPackageNameForPsake";
        ComposeFinish "Finished uninstalling psake.";
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
        ComposeNormal "Suppressing uninstallation of RabbitMQ.";
        Return;
    }
    ElseIf (GetRabbitMqInstallationStatus)
    {
        ComposeStart "Uninstalling RabbitMQ.";
        UseChocolateyToUninstall -PackageName "$ChoclateyPackageNameForRabbitMq";
        ComposeFinish "Finished uninstalling RabbitMQ.";
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

    ExecuteProcess -Path "$CommandNameForChocolatey" -Arguments "install $PackageName -y --accept-license --confirm --limit-output --no-progress";
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

    ExecuteProcess -Path "$CommandNameForChocolatey" -Arguments "uninstall $PackageName -y --confirm --limit-output";
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

    ExecuteProcess -Path "$CommandNameForNpm" -Arguments "install $PackageName -g --loglevel error";
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

    ExecuteProcess -Path "$CommandNameForNpm" -Arguments "uninstall $PackageName -g --loglevel error";
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

    Uninstall-Module -Confirm:$false -ErrorAction Stop -Force -Name "$ModuleName";
}