# =================================================================================================================================
# Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
# =================================================================================================================================

# This file defines CI/CD automation tooling for the project.
# =================================================================================================================================

# Module configuration
# =================================================================================================================================

# Directory names
$DirectoryNameForCicd = "cicd";
$DirectoryNameForCicdModules = "modules";
$DirectoryNameForCicdScripts = "scripts";
$DirectoryNameForCicdTools = "tools";

# File names
$FileNameForNugetExe = "nuget.exe";

# Directory paths
$DirectoryPathForProjectRoot = (Get-Item $PSScriptRoot).Parent.Parent.FullName;
$DirectoryPathForCicd = Join-Path -Path "$DirectoryPathForProjectRoot" -ChildPath "$DirectoryNameForCicd";
$DirectoryPathForCicdModules = Join-Path -Path "$DirectoryPathForCicd" -ChildPath "$DirectoryNameForCicdModules";
$DirectoryPathForCicdScripts = Join-Path -Path "$DirectoryPathForCicd" -ChildPath "$DirectoryNameForCicdScripts";
$DirectoryPathForCicdTools = Join-Path -Path "$DirectoryPathForCicd" -ChildPath "$DirectoryNameForCicdTools";

# File paths
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

# Get
# =================================================================================================================================

function GetChocolateyInstallationStatus {
    return (Get-Command $CommandNameForChocolatey -ErrorAction SilentlyContinue);
}

function GetCodecovInstallationStatus {
    return (GetChocolateyInstallationStatus) -And (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForCodecov") });
}

function GetDocFxInstallationStatus {
    return (GetChocolateyInstallationStatus) -And (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForDocFx") });
}

function GetDotNetCoreSdkInstallationStatus {
    return (GetChocolateyInstallationStatus) -And (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForDotNetCoreSdk") });
}

function GetHtmlMinifierInstallationStatus {
    return (Get-Command $CommandNameForHtmlMinifier -ErrorAction SilentlyContinue);
}

function GetHubInstallationStatus {
    return (GetChocolateyInstallationStatus) -And (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForHub") });
}

function GetLeanifyInstallationStatus {
    return (GetChocolateyInstallationStatus) -And (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForLeanify") });
}

function GetNodeJsInstallationStatus {
    return (GetChocolateyInstallationStatus) -And (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForNodeJs") });
}

function GetNuGetInstallationStatus {
    return (Test-Path -Path "$FilePathForNuGetExe");
}

function GetOpenCoverInstallationStatus {
    return (GetChocolateyInstallationStatus) -And (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForOpenCover") });
}

function GetOpenSslInstallationStatus {
    return (GetChocolateyInstallationStatus) -And (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForOpenSsl") });
}

function GetPoshGitInstallationStatus {
    return Get-Module -ListAvailable -Name "$PowershellModuleNameForPoshGit";
}

function GetPowershellYamlInstallationStatus {
    return Get-Module -ListAvailable -Name "$PowershellModuleNameForPowershellYaml";
}

function GetPsakeInstallationStatus {
    return (GetChocolateyInstallationStatus) -And (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForPsake") });
}

# Install
# =================================================================================================================================

function InstallAllAutomationTools {
    Write-Host -ForegroundColor DarkCyan "Installing all automation tools.";
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
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing all automation tools. <<<`n";
}

function InstallChocolatey {
    If ($SuppressChoclatey -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing installation of Chocolatey.";
        return;
    }
    ElseIf (GetChocolateyInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Chocolatey is already installed.";
        return;
    }

    Write-Host -ForegroundColor DarkCyan "Installing Chocolatey.";
    Stop-Process -Name "choco.exe" -Force -ErrorAction Ignore;
    Set-ExecutionPolicy Bypass -Scope Process -Force;
    iex ((New-Object System.Net.WebClient).DownloadString($InstallScriptUriForChocolatey));
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing Chocolatey. <<<`n";
}

function InstallCodecov {
    If ($SuppressCodecov -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing installation of Codecov.";
        return;
    }
    ElseIf (GetCodecovInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Codecov is already installed.";
        return;
    }

    Write-Host -ForegroundColor DarkCyan "Installing Codecov.";
    choco install $ChoclateyPackageNameForCodecov -y --accept-license --confirm --limit-output --no-progress
    MakeCommandPathAvailableAll -Command $CommandNameForCodecov;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing Codecov. <<<`n";
}

function InstallDocFx {
    If ($SuppressDocFx -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing installation of DocFX.";
        return;
    }
    ElseIf (GetDocFxInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "DocFX is already installed.";
        return;
    }

    Write-Host -ForegroundColor DarkCyan "Installing DocFX.";
    choco install $ChoclateyPackageNameForDocFx -y --accept-license --confirm --limit-output --no-progress
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing DocFX. <<<`n";
}

function InstallDotNetCoreSdk {
    If ($SuppressDotNetCoreSdk -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing installation of the .NET Core SDK.";
        return;
    }
    ElseIf (GetDotNetCoreSdkInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "The .NET Core SDK is already installed.";
        return;
    }

    Write-Host -ForegroundColor DarkCyan "Installing the .NET Core SDK.";
    choco install $ChoclateyPackageNameForDotNetCoreSdk -y --accept-license --confirm --limit-output --no-progress
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing the .NET Core SDK. <<<`n";
}

function InstallHtmlMinifier {
    If ($SuppressHtmlMinifier -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing installation of HTMLMinifier.";
        return;
    }
    ElseIf (GetHtmlMinifierInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "HTMLMinifier is already installed.";
        return;
    }

    Write-Host -ForegroundColor DarkCyan "Installing HTMLMinifier.";
    npm install $NpmPackageNameForHtmlMinifier -g --loglevel error
    MakeCommandPathAvailableAll -Command $CommandNameForHtmlMinifier;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing HTMLMinifier. <<<`n";
}

function InstallHub {
    If ($SuppressHub -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing installation of hub.";
        return;
    }
    ElseIf (GetHubInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "hub is already installed.";
        return;
    }

    Write-Host -ForegroundColor DarkCyan "Installing hub.";
    choco install $ChoclateyPackageNameForHub -y --accept-license --confirm --limit-output --no-progress
    MakeCommandPathAvailableAll -Command $CommandNameForHub;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing hub. <<<`n";
}

function InstallLeanify {
    If ($SuppressLeanify -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing installation of Leanify.";
        return;
    }
    ElseIf (GetLeanifyInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Leanify is already installed.";
        return;
    }

    Write-Host -ForegroundColor DarkCyan "Installing Leanify.";
    choco install $ChoclateyPackageNameForLeanify -y --accept-license --confirm --limit-output --no-progress
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing Leanify. <<<`n";
}

function InstallNodeJs {
    If ($SuppressNodeJs -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing installation of Node.js.";
        return;
    }
    ElseIf (GetNodeJsInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Node.js is already installed.";
        return;
    }

    Write-Host -ForegroundColor DarkCyan "Installing Node.js.";
    choco install $ChoclateyPackageNameForNodeJs -y --accept-license --confirm --limit-output --no-progress
    MakeCommandPathAvailableAll -Command $CommandNameForNpm;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing Node.js. <<<`n";
}

function InstallNuGet {
    If ($SuppressNuGet -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing installation of NuGet.";
        return;
    }
    ElseIf (GetNuGetInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "NuGet is already installed.";
        return;
    }

    If (-not (Test-Path "$DirectoryPathForCicdTools")) {
        New-Item -ItemType Directory -Path "$DirectoryPathForCicdTools" -Force | Out-Null;
    }

    Write-Host -ForegroundColor DarkCyan "Installing NuGet.";
    Invoke-WebRequest -Uri $InstallScriptUriForNuGet -OutFile "$FilePathForNuGetExe"
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing NuGet. <<<`n";
}

function InstallOpenCover {
    If ($SuppressOpenCover -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing installation of OpenCover.";
        return;
    }
    ElseIf (GetOpenCoverInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "OpenCover is already installed.";
        return;
    }

    Write-Host -ForegroundColor DarkCyan "Installing OpenCover.";
    choco install $ChoclateyPackageNameForOpenCover -y --accept-license --confirm --limit-output --no-progress
    MakeCommandPathAvailableAll -Command $CommandNameForOpenCover;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing OpenCover. <<<`n";
}

function InstallOpenSsl {
    If ($SuppressOpenSsl -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing installation of OpenSSL.";
        return;
    }
    ElseIf (GetOpenSslInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "OpenSSL is already installed.";
        return;
    }

    Write-Host -ForegroundColor DarkCyan "Installing OpenSSL.";
    choco install $ChoclateyPackageNameForOpenSsl -y --accept-license --confirm --limit-output --no-progress
    MakeCommandPathAvailableAll -Command $CommandNameForOpenSsl;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing OpenSSL. <<<`n";
}

function InstallPackageManagers {
    If ($SuppressPackageManagers -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing installation of package managers.";
        return;
    }

    Write-Host -ForegroundColor DarkCyan "Installing package managers.";
    InstallChocolatey;
    InstallNodeJs;
    InstallNuGet;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing package managers. <<<`n";
}

function InstallPoshGit {
    If ($SuppressPoshGit -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing installation of posh-git.";
        return;
    }
    ElseIf (GetPoshGitInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "posh-git is already installed.";
        return;
    }

    Write-Host -ForegroundColor DarkCyan "Installing posh-git.";
    Install-Module -AcceptLicense -Confirm:$false -Force -Name "$PowershellModuleNameForPoshGit";
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing posh-git. <<<`n";
}

function InstallPowershellYaml {
    If ($SuppressPowershellYaml -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing installation of powershell-yaml.";
        return;
    }
    ElseIf (GetPowershellYamlInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "powershell-yaml is already installed.";
        return;
    }

    Write-Host -ForegroundColor DarkCyan "Installing powershell-yaml.";
    Install-Module -AcceptLicense -Confirm:$false -Force -Name "$PowershellModuleNameForPowershellYaml";
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing powershell-yaml. <<<`n";
}

function InstallPsake {
    If ($SuppressPsake -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing installation of psake.";
        return;
    }
    ElseIf (GetPsakeInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "psake is already installed.";
        return;
    }

    Write-Host -ForegroundColor DarkCyan "Installing psake.";
    choco install $ChoclateyPackageNameForPsake -y --accept-license --confirm --limit-output --no-progress
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing psake. <<<`n";
}

# Make
# =================================================================================================================================

function MakeCommandPathAvailable {
    Param (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $Command,
        [Parameter(Mandatory = $true, Position = 1)]
        [String] $EnvironmentTarget
    )

    Get-Command "$Command" | ForEach-Object {
        $CommandDirectoryPath = Split-Path $_.Source;
        $PathVariable = [System.Environment]::GetEnvironmentVariable("Path", $EnvironmentTarget);

        If ($PathVariable -like "*$CommandDirectoryPath*") {
            Write-Host -ForegroundColor DarkCyan "Command path already available for $EnvironmentTarget target: $CommandDirectoryPath";
            return;
        }

        $PathVariable = $PathVariable + ";$CommandDirectoryPath";
        [System.Environment]::SetEnvironmentVariable("Path", "$PathVariable", $EnvironmentTarget);
        RefreshSession;
        Write-Host -ForegroundColor DarkCyan "Added command path for $EnvironmentTarget target: $CommandDirectoryPath";
        return;
    }
}

function MakeCommandPathAvailableAll {
    Param (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $Command
    )

    MakeCommandPathAvailableMachine -Command $Command;
    MakeCommandPathAvailableProcess -Command $Command;
    MakeCommandPathAvailableUser -Command $Command;
}

function MakeCommandPathAvailableMachine {
    Param (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $Command
    )

    MakeCommandPathAvailable -Command $Command -EnvironmentTarget "Machine";
}

function MakeCommandPathAvailableProcess {
    Param (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $Command
    )

    MakeCommandPathAvailable -Command $Command -EnvironmentTarget "Process";
}

function MakeCommandPathAvailableUser {
    Param (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $Command
    )

    MakeCommandPathAvailable -Command $Command -EnvironmentTarget "User";
}

# Refresh
# =================================================================================================================================

function RefreshSession {
    Import-Module "$env:ChocolateyInstall\helpers\chocolateyInstaller.psm1" -Force;
    Update-SessionEnvironment;
}

# Restore
# =================================================================================================================================

function RestoreAllAutomationTools {
    Write-Host -ForegroundColor DarkCyan "Restoring all automation tools.";
    InstallPackageManagers;
    UninstallAllAutomationTools;
    InstallAllAutomationTools;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished restoring all automation tools. <<<`n";
}

function RestoreCodecov {
    Write-Host -ForegroundColor DarkCyan "Restoring Codecov.";
    UninstallCodecov;
    InstallCodecov;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished restoring Codecov. <<<`n";
}

function RestoreDocFx {
    Write-Host -ForegroundColor DarkCyan "Restoring DocFX.";
    UninstallDocFx;
    InstallDocFx;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished restoring DocFX. <<<`n";
}

function RestoreDotNetCoreSdk {
    Write-Host -ForegroundColor DarkCyan "Restoring the .NET Core SDK.";
    UninstallDotNetCoreSdk;
    InstallDotNetCoreSdk;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished restoring the .NET Core SDK. <<<`n";
}

function RestoreHtmlMinifier {
    Write-Host -ForegroundColor DarkCyan "Restoring HTMLMinifier.";
    UninstallHtmlMinifier;
    InstallHtmlMinifier;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished restoring HTMLMinifier. <<<`n";
}

function RestoreHtmlMinifier {
    Write-Host -ForegroundColor DarkCyan "Restoring hub.";
    UninstallHub;
    InstallHub;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished restoring hub. <<<`n";
}

function RestoreLeanify {
    Write-Host -ForegroundColor DarkCyan "Restoring Leanify.";
    UninstallLeanify;
    InstallLeanify;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished restoring Leanify. <<<`n";
}

function RestoreNodeJs {
    Write-Host -ForegroundColor DarkCyan "Restoring Node.js.";
    UninstallNodeJs;
    InstallNodeJs;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished restoring Node.js. <<<`n";
}

function RestoreNuGet {
    Write-Host -ForegroundColor DarkCyan "Restoring NuGet.";
    UninstallNuGet;
    InstallNuGet;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished restoring NuGet. <<<`n";
}

function RestoreOpenCover {
    Write-Host -ForegroundColor DarkCyan "Restoring OpenCover.";
    UninstallOpenCover;
    InstallOpenCover;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished restoring OpenCover. <<<`n";
}

function RestoreOpenSsl {
    Write-Host -ForegroundColor DarkCyan "Restoring OpenSSL.";
    UninstallOpenSsl;
    InstallOpenSsl;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished restoring OpenSSL. <<<`n";
}

function RestorePoshGit {
    Write-Host -ForegroundColor DarkCyan "Restoring posh-git.";
    UninstallPoshGit;
    InstallPoshGit;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished restoring posh-git. <<<`n";
}

function RestorePowershellYaml {
    Write-Host -ForegroundColor DarkCyan "Restoring powershell-yaml.";
    UninstallPowershellYaml;
    InstallPowershellYaml;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished restoring powershell-yaml. <<<`n";
}

function RestorePsake {
    Write-Host -ForegroundColor DarkCyan "Restoring psake.";
    UninstallPsake;
    InstallPsake;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished restoring psake. <<<`n";
}

# Uninstall
# =================================================================================================================================

function UninstallAllAutomationTools {
    Write-Host -ForegroundColor DarkCyan "Uninstalling all automation tools.";
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
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling all automation tools. <<<`n";
}

function UninstallCodecov {
    If ($SuppressCodecov -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing uninstallation of Codecov.";
        return;
    }
    ElseIf (GetCodecovInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Uninstalling Codecov.";
        choco uninstall $ChoclateyPackageNameForCodecov -y --confirm --limit-output
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling Codecov. <<<`n";
    }
}

function UninstallDocFx {
    If ($SuppressDocFx -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing uninstallation of DocFX.";
        return;
    }
    ElseIf (GetDocFxInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Uninstalling DocFX.";
        choco uninstall $ChoclateyPackageNameForDocFx -y --confirm --limit-output
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling DocFX. <<<`n";
    }
}

function UninstallDotNetCoreSdk {
    If ($SuppressDotNetCoreSdk -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing uninstallation of the .NET Core SDK.";
        return;
    }
    ElseIf (GetDotNetCoreSdkInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Uninstalling the .NET Core SDK.";
        choco uninstall $ChoclateyPackageNameForDotNetCoreSdk -y --confirm --limit-output
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling the .NET Core SDK. <<<`n";
    }
}

function UninstallHtmlMinifier {
    If ($SuppressHtmlMinifier -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing uninstallation of HTMLMinifier.";
        return;
    }
    ElseIf (GetHtmlMinifierInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Uninstalling HTMLMinifier.";
        npm uninstall $NpmPackageNameForHtmlMinifier -g --loglevel error
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling HTMLMinifier. <<<`n";
    }
}

function UninstallHub {
    If ($SuppressHub -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing uninstallation of hub.";
        return;
    }
    ElseIf (GetLeanifyInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Uninstalling hub.";
        choco uninstall $ChoclateyPackageNameForHub -y --confirm --limit-output
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling hub. <<<`n";
    }
}

function UninstallLeanify {
    If ($SuppressLeanify -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing uninstallation of Leanify.";
        return;
    }
    ElseIf (GetLeanifyInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Uninstalling Leanify.";
        choco uninstall $ChoclateyPackageNameForLeanify -y --confirm --limit-output
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling Leanify. <<<`n";
    }
}

function UninstallNodeJs {
    If ($SuppressNodeJs -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing uninstallation of Node.js.";
        return;
    }
    ElseIf (GetNodeJsInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Uninstalling Node.js.";
        choco uninstall $ChoclateyPackageNameForNodeJs -y --confirm --limit-output
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling Node.js. <<<`n";
    }
}

function UninstallNuGet {
    If ($SuppressNuGet -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing uninstallation of NuGet.";
        return;
    }
    ElseIf (GetNuGetInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Uninstalling NuGet.";
        Remove-Item -Path "$FilePathForNuGetExe" -Confirm:$false -Force;
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling NuGet. <<<`n";
    }
}

function UninstallOpenCover {
    If ($SuppressOpenCover -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing uninstallation of OpenCover.";
        return;
    }
    ElseIf (GetOpenCoverInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Uninstalling OpenCover.";
        choco uninstall $ChoclateyPackageNameForOpenCover -y --confirm --limit-output
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling OpenCover. <<<`n";
    }
}

function UninstallOpenSsl {
    If ($SuppressOpenSsl -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing uninstallation of OpenSSL.";
        return;
    }
    ElseIf (GetOpenSslInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Uninstalling OpenSSL.";
        choco uninstall $ChoclateyPackageNameForOpenSsl -y --confirm --limit-output
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling OpenSSL. <<<`n";
    }
}

function UninstallPoshGit {
    If ($SuppressPoshGit -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing uninstallation of posh-git.";
        return;
    }
    ElseIf (GetPoshGitInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Uninstalling posh-git.";
        Uninstall-Module -Confirm:$false -Force -Name "$PowershellModuleNameForPoshGit";
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling posh-git. <<<`n";
    }
}

function UninstallPowershellYaml {
    If ($SuppressPowershellYaml -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing uninstallation of powershell-yaml.";
        return;
    }
    ElseIf (GetPowershellYamlInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Uninstalling powershell-yaml.";
        Uninstall-Module -Confirm:$false -Force -Name "$PowershellModuleNameForPowershellYaml";
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling powershell-yaml. <<<`n";
    }
}

function UninstallPsake {
    If ($SuppressPsake -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing uninstallation of psake.";
        return;
    }
    ElseIf (GetPsakeInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Uninstalling psake.";
        choco uninstall $ChoclateyPackageNameForPsake -y --confirm --limit-output
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling psake. <<<`n";
    }
}
