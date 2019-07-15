# =================================================================================================================================
# Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
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
$ChoclateyPackageNameForDocFx = "docfx";
$ChoclateyPackageNameForLeanify = "leanify";
$ChoclateyPackageNameForNodeJs = "nodejs";
$ChoclateyPackageNameForOpenSsl = "openssl.light";
$ChoclateyPackageNameForPsake = "psake";

# NPM package names
$NpmPackageNameForHtmlMinifier = "html-minifier";

# Powershell package names
$PowershellModuleNameForPowershellYaml = "powershell-yaml";

# Command names
$CommandNameForChocolatey = "choco";
$CommandNameForHtmlMinifier = "html-minifier";
$CommandNameForNpm = "npm";
$CommandNameForNuGet = "nuget";
$CommandNameForOpenSsl = "openssl";

# Installation suppression flags
$SuppressInstallationOfChocolatey = $false;
$SuppressInstallationOfDocFx = $false;
$SuppressInstallationOfHtmlMinifier = $false;
$SuppressInstallationOfLeanify = $true;
$SuppressInstallationOfNodeJs = $false;
$SuppressInstallationOfNuGet = $false;
$SuppressInstallationOfOpenSsl = $true;
$SuppressInstallationOfPackageManagers = $false;
$SuppressInstallationOfPowershellYaml = $false;
$SuppressInstallationOfPsake = $false;

# Get
# =================================================================================================================================

function GetChocolateyInstallationStatus {
    return (Get-Command $CommandNameForChocolatey -ErrorAction SilentlyContinue);
}

function GetDocFxInstallationStatus {
    return (GetChocolateyInstallationStatus) -And (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForDocFx") });
}

function GetHtmlMinifierInstallationStatus {
    return (Get-Command $CommandNameForHtmlMinifier -ErrorAction SilentlyContinue);
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

function GetOpenSslInstallationStatus {
    return (GetChocolateyInstallationStatus) -And (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForOpenSsl") });
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
    InstallDocFx;
    InstallHtmlMinifier;
    InstallLeanify;
    InstallOpenSsl;
    InstallPowershellYaml;
    InstallPsake;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing all automation tools. <<<`n";
}

function InstallChocolatey {
    If ($SuppressInstallationOfChoclatey -eq $true) {
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

function InstallDocFx {
    If ($SuppressInstallationOfDocFx -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing installation of DocFX.";
        return;
    }
    ElseIf (GetDocFxInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "DocFX is already installed.";
        return;
    }

    Write-Host -ForegroundColor DarkCyan "Installing DocFX.";
    choco install $ChoclateyPackageNameForDocFx -y --confirm
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing DocFX. <<<`n";
}

function InstallHtmlMinifier {
    If ($SuppressInstallationOfHtmlMinifier -eq $true) {
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

function InstallLeanify {
    If ($SuppressInstallationOfLeanify -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing installation of Leanify.";
        return;
    }
    ElseIf (GetLeanifyInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Leanify is already installed.";
        return;
    }

    Write-Host -ForegroundColor DarkCyan "Installing Leanify.";
    choco install $ChoclateyPackageNameForLeanify -y --confirm
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing Leanify. <<<`n";
}

function InstallNodeJs {
    If ($SuppressInstallationOfNodeJs -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing installation of Node.js.";
        return;
    }
    ElseIf (GetNodeJsInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Node.js is already installed.";
        return;
    }

    Write-Host -ForegroundColor DarkCyan "Installing Node.js.";
    choco install $ChoclateyPackageNameForNodeJs -y --confirm
    MakeCommandPathAvailableAll -Command $CommandNameForNpm;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing Node.js. <<<`n";
}

function InstallNuGet {
    If ($SuppressInstallationOfNuGet -eq $true) {
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

function InstallOpenSsl {
    If ($SuppressInstallationOfOpenSsl -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing installation of OpenSSL.";
        return;
    }
    ElseIf (GetOpenSslInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "OpenSSL is already installed.";
        return;
    }

    Write-Host -ForegroundColor DarkCyan "Installing OpenSSL.";
    choco install $ChoclateyPackageNameForOpenSsl -y --confirm
    MakeCommandPathAvailableAll -Command $CommandNameForOpenSsl;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing OpenSSL. <<<`n";
}

function InstallPackageManagers {
    If ($SuppressInstallationOfPackageManagers -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing installation of package managers.";
        return;
    }

    Write-Host -ForegroundColor DarkCyan "Installing package managers.";
    InstallChocolatey;
    InstallNodeJs;
    InstallNuGet;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing package managers. <<<`n";
}

function InstallPowershellYaml {
    If ($SuppressInstallationOfPowershellYaml -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing installation of powershell-yaml.";
        return;
    }
    ElseIf (GetPowershellYamlInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "powershell-yaml is already installed.";
        return;
    }

    Write-Host -ForegroundColor DarkCyan "Installing powershell-yaml.";
    Install-Module -Confirm:$false -Force -Name "$PowershellModuleNameForPowershellYaml";
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing powershell-yaml. <<<`n";
}

function InstallPsake {
    If ($SuppressInstallationOfPsake -eq $true) {
        Write-Host -ForegroundColor DarkCyan "Suppressing installation of psake.";
        return;
    }
    ElseIf (GetPsakeInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "psake is already installed.";
        return;
    }

    Write-Host -ForegroundColor DarkCyan "Installing psake.";
    choco install $ChoclateyPackageNameForPsake -y --confirm
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

function RestoreDocFx {
    Write-Host -ForegroundColor DarkCyan "Restoring DocFX.";
    UninstallDocFx;
    InstallDocFx;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished restoring DocFX. <<<`n";
}

function RestoreHtmlMinifier {
    Write-Host -ForegroundColor DarkCyan "Restoring HTMLMinifier.";
    UninstallHtmlMinifier;
    InstallHtmlMinifier;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished restoring HTMLMinifier. <<<`n";
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

function RestoreOpenSsl {
    Write-Host -ForegroundColor DarkCyan "Restoring OpenSSL.";
    UninstallOpenSsl;
    InstallOpenSsl;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished restoring OpenSSL. <<<`n";
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
    UninstallDocFx;
    UninstallHtmlMinifier;
    UninstallLeanify;
    UninstallOpenSsl;
    UninstallPowershellYaml;
    UninstallPsake;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling all automation tools. <<<`n";
}

function UninstallDocFx {
    If (GetDocFxInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Uninstalling DocFX.";
        choco uninstall $ChoclateyPackageNameForDocFx -y --confirm
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling DocFX. <<<`n";
    }
}

function UninstallHtmlMinifier {
    Write-Host -ForegroundColor DarkCyan "Uninstalling HTMLMinifier.";
    npm uninstall $NpmPackageNameForHtmlMinifier -g --loglevel error
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling HTMLMinifier. <<<`n";
}

function UninstallLeanify {
    If (GetLeanifyInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Uninstalling Leanify.";
        choco uninstall $ChoclateyPackageNameForLeanify -y --confirm
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling Leanify. <<<`n";
    }
}

function UninstallNodeJs {
    If (GetNodeJsInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Uninstalling Node.js.";
        choco uninstall $ChoclateyPackageNameForNodeJs -y --confirm
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling Node.js. <<<`n";
    }
}

function UninstallNuGet {
    If (GetNuGetInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Uninstalling NuGet.";
        Remove-Item -Path "$FilePathForNuGetExe" -Confirm:$false -Force;
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling NuGet. <<<`n";
    }
}

function UninstallOpenSsl {
    If (GetOpenSslInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Uninstalling OpenSSL.";
        choco uninstall $ChoclateyPackageNameForOpenSsl -y --confirm
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling OpenSSL. <<<`n";
    }
}

function UninstallPowershellYaml {
    If (GetPowershellYamlInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Uninstalling powershell-yaml.";
        Uninstall-Module -Confirm:$false -Force -Name "$PowershellModuleNameForPowershellYaml";
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling powershell-yaml. <<<`n";
    }
}

function UninstallPsake {
    If (GetPsakeInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Uninstalling psake.";
        choco uninstall $ChoclateyPackageNameForPsake -y --confirm
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling psake. <<<`n";
    }
}
