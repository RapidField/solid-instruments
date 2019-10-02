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
$SuppressDotNetCoreSdk = $true;
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

# Modules
Import-Module $FilePathForCoreModule -Force;

Function GetChocolateyInstallationStatus
{
    Return (Get-Command $CommandNameForChocolatey -ErrorAction SilentlyContinue);
}

Function GetCodecovInstallationStatus
{
    Return (GetChocolateyInstallationStatus) -and (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForCodecov") });
}

Function GetDocFxInstallationStatus
{
    Return (GetChocolateyInstallationStatus) -and (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForDocFx") });
}

Function GetDotNetCoreSdkInstallationStatus
{
    Return (GetChocolateyInstallationStatus) -and (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForDotNetCoreSdk") });
}

Function GetHtmlMinifierInstallationStatus
{
    Return (Get-Command $CommandNameForHtmlMinifier -ErrorAction SilentlyContinue);
}

Function GetHubInstallationStatus
{
    Return (GetChocolateyInstallationStatus) -and (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForHub") });
}

Function GetLeanifyInstallationStatus
{
    Return (GetChocolateyInstallationStatus) -and (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForLeanify") });
}

Function GetNodeJsInstallationStatus
{
    Return (GetChocolateyInstallationStatus) -and (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForNodeJs") });
}

Function GetNuGetInstallationStatus
{
    Return (Test-Path -Path "$FilePathForNuGetExe");
}

Function GetOpenCoverInstallationStatus
{
    Return (GetChocolateyInstallationStatus) -and (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForOpenCover") });
}

Function GetOpenSslInstallationStatus
{
    Return (GetChocolateyInstallationStatus) -and (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForOpenSsl") });
}

Function GetPoshGitInstallationStatus
{
    Return Get-Module -ListAvailable -Name "$PowershellModuleNameForPoshGit";
}

Function GetPowershellYamlInstallationStatus
{
    Return Get-Module -ListAvailable -Name "$PowershellModuleNameForPowershellYaml";
}

Function GetPsakeInstallationStatus
{
    Return (GetChocolateyInstallationStatus) -and (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForPsake") });
}

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
    ComposeFinish "Finished installing all automation tools.";
}

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
    ComposeFinish "Finished installing Chocolatey.";
}

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

Function MakeCommandPathAvailableMachine
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $Command
    )

    MakeCommandPathAvailable -Command $Command -EnvironmentTarget "Machine";
}

Function MakeCommandPathAvailableProcess
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $Command
    )

    MakeCommandPathAvailable -Command $Command -EnvironmentTarget "Process";
}

Function MakeCommandPathAvailableUser
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $Command
    )

    MakeCommandPathAvailable -Command $Command -EnvironmentTarget "User";
}

Function RefreshSession
{
    Import-Module "$env:ChocolateyInstall\helpers\chocolateyInstaller.psm1" -Force;
    Update-SessionEnvironment;
}

Function RestoreAllAutomationTools
{
    ComposeStart "Restoring all automation tools.";
    InstallPackageManagers;
    UninstallAllAutomationTools;
    InstallAllAutomationTools;
    ComposeFinish "Finished restoring all automation tools.";
}

Function RestoreCodecov
{
    ComposeStart "Restoring Codecov.";
    UninstallCodecov;
    InstallCodecov;
    ComposeFinish "Finished restoring Codecov.";
}

Function RestoreDocFx
{
    ComposeStart "Restoring DocFX.";
    UninstallDocFx;
    InstallDocFx;
    ComposeFinish "Finished restoring DocFX.";
}

Function RestoreDotNetCoreSdk
{
    ComposeStart "Restoring the .NET Core SDK.";
    UninstallDotNetCoreSdk;
    InstallDotNetCoreSdk;
    ComposeFinish "Finished restoring the .NET Core SDK.";
}

Function RestoreHtmlMinifier
{
    ComposeStart "Restoring HTMLMinifier.";
    UninstallHtmlMinifier;
    InstallHtmlMinifier;
    ComposeFinish "Finished restoring HTMLMinifier.";
}

Function RestoreHtmlMinifier
{
    ComposeStart "Restoring hub.";
    UninstallHub;
    InstallHub;
    ComposeFinish "Finished restoring hub.";
}

Function RestoreLeanify
{
    ComposeStart "Restoring Leanify.";
    UninstallLeanify;
    InstallLeanify;
    ComposeFinish "Finished restoring Leanify.";
}

Function RestoreNodeJs
{
    ComposeStart "Restoring Node.js.";
    UninstallNodeJs;
    InstallNodeJs;
    ComposeFinish "Finished restoring Node.js.";
}

Function RestoreNuGet
{
    ComposeStart "Restoring NuGet.";
    UninstallNuGet;
    InstallNuGet;
    ComposeFinish "Finished restoring NuGet.";
}

Function RestoreOpenCover
{
    ComposeStart "Restoring OpenCover.";
    UninstallOpenCover;
    InstallOpenCover;
    ComposeFinish "Finished restoring OpenCover.";
}

Function RestoreOpenSsl
{
    ComposeStart "Restoring OpenSSL.";
    UninstallOpenSsl;
    InstallOpenSsl;
    ComposeFinish "Finished restoring OpenSSL.";
}

Function RestorePoshGit
{
    ComposeStart "Restoring posh-git.";
    UninstallPoshGit;
    InstallPoshGit;
    ComposeFinish "Finished restoring posh-git.";
}

Function RestorePowershellYaml
{
    ComposeStart "Restoring powershell-yaml.";
    UninstallPowershellYaml;
    InstallPowershellYaml;
    ComposeFinish "Finished restoring powershell-yaml.";
}

Function RestorePsake
{
    ComposeStart "Restoring psake.";
    UninstallPsake;
    InstallPsake;
    ComposeFinish "Finished restoring psake.";
}

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
    ComposeFinish "Finished uninstalling all automation tools.";
}

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

Function UseChocolateyToInstall
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $PackageName
    )

    ExecuteProcess -Path "$CommandNameForChocolatey" -Arguments "install $PackageName -y --accept-license --confirm --limit-output --no-progress";
}

Function UseChocolateyToUninstall
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $PackageName
    )

    ExecuteProcess -Path "$CommandNameForChocolatey" -Arguments "uninstall $PackageName -y --confirm --limit-output";
}

Function UseNpmToInstall
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $PackageName
    )

    ExecuteProcess -Path "$CommandNameForNpm" -Arguments "install $PackageName -g --loglevel error";
}

Function UseNpmToUninstall
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $PackageName
    )

    ExecuteProcess -Path "$CommandNameForNpm" -Arguments "uninstall $PackageName -g --loglevel error";
}

Function UsePowerShellGalleryToInstall
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $ModuleName
    )

    Install-Module -Confirm:$false -ErrorAction Stop -Force -Name "$ModuleName";
}

Function UsePowerShellGalleryToUninstall
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $ModuleName
    )

    Uninstall-Module -Confirm:$false -ErrorAction Stop -Force -Name "$ModuleName";
}