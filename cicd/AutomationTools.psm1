# =================================================================================================================================
# Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
# =================================================================================================================================

$ChoclateyCommandName = "choco"
$ChoclateyInstallScriptUri = "https://chocolatey.org/install.ps1"
$ChoclateyPackageNameForDocFx = "docfx"
$ChoclateyPackageNameForLeanify = "leanify"
$ChoclateyPackageNameForNodeJs = "nodejs"
$ChoclateyPackageNameForPsake = "psake"
$HtmlMinifierCommandName = "html-minifier"
$NpmCommandName = "npm"
$NpmPackageNameForHtmlMinifier = "html-minifier"

# Get
# =================================================================================================================================

function GetChocolateyInstallationStatus {
    return (Get-Command $ChoclateyCommandName -ErrorAction SilentlyContinue)
}

function GetDocFxInstallationStatus {
    return (GetChocolateyInstallationStatus) -And (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForDocFx") })
}

function GetHtmlMinifierInstallationStatus {
    return (Get-Command $HtmlMinifierCommandName -ErrorAction SilentlyContinue)
}

function GetLeanifyInstallationStatus {
    return (GetChocolateyInstallationStatus) -And (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForLeanify") })
}

function GetNodeJsInstallationStatus {
    return (GetChocolateyInstallationStatus) -And (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForNodeJs") })
}

function GetPsakeInstallationStatus {
    return (GetChocolateyInstallationStatus) -And (choco list -lo | Where-Object { $_.ToLower().StartsWith("$ChoclateyPackageNameForPsake") })
}

# Install
# =================================================================================================================================

function InstallAllAutomationTools {
    Write-Host -ForegroundColor DarkCyan "Installing all automation tools."
    InstallPackageManagers
    InstallDocFx
    InstallHtmlMinifier
    InstallLeanify
    InstallPsake
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing all automation tools. <<<`n"
}

function InstallChocolatey {
    If (GetChocolateyInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Chocolatey is already installed."
        return
    }

    Write-Host -ForegroundColor DarkCyan "Installing Chocolatey."
    Stop-Process -Name "choco.exe" -Force -ErrorAction Ignore
    Set-ExecutionPolicy Bypass -Scope Process -Force;
    iex ((New-Object System.Net.WebClient).DownloadString($ChoclateyInstallScriptUri))
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing Chocolatey. <<<`n"
}

function InstallDocFx {
    If (GetDocFxInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "DocFX is already installed."
        return
    }

    Write-Host -ForegroundColor DarkCyan "Installing DocFX."
    choco install $ChoclateyPackageNameForDocFx -y --confirm
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing DocFX. <<<`n"
}

function InstallHtmlMinifier {
    If (GetHtmlMinifierInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "HTMLMinifier is already installed."
        return
    }

    Write-Host -ForegroundColor DarkCyan "Installing HTMLMinifier."
    npm install $NpmPackageNameForHtmlMinifier -g --loglevel error
    MakeCommandPathAvailableAll -Command $HtmlMinifierCommandName
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing HTMLMinifier. <<<`n"
}

function InstallLeanify {
    If (GetLeanifyInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Leanify is already installed."
        return
    }

    Write-Host -ForegroundColor DarkCyan "Installing Leanify."
    choco install $ChoclateyPackageNameForLeanify --confirm
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing Leanify. <<<`n"
}

function InstallNodeJs {
    If (GetNodeJsInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Node.js is already installed."
        return
    }

    Write-Host -ForegroundColor DarkCyan "Installing Node.js."
    choco install $ChoclateyPackageNameForNodeJs --confirm
    MakeCommandPathAvailableAll -Command $NpmCommandName
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing Node.js. <<<`n"
}

function InstallPackageManagers {
    Write-Host -ForegroundColor DarkCyan "Installing package managers."
    InstallChocolatey
    InstallNodeJs
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing package managers. <<<`n"
}

function InstallPsake {
    If (GetPsakeInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "psake is already installed."
        return
    }

    Write-Host -ForegroundColor DarkCyan "Installing psake."
    choco install $ChoclateyPackageNameForPsake --confirm
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing psake. <<<`n"
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
        $CommandDirectoryPath = Split-Path $_.Source
        $PathVariable = [System.Environment]::GetEnvironmentVariable("Path", $EnvironmentTarget)

        If ($PathVariable -like "*$CommandDirectoryPath*") {
            Write-Host -ForegroundColor DarkCyan "Command path already available for $EnvironmentTarget target: $CommandDirectoryPath"
            return
        }

        $PathVariable = $PathVariable + ";$CommandDirectoryPath"
        [System.Environment]::SetEnvironmentVariable("Path", "$PathVariable", $EnvironmentTarget)
        call Update-SessionEnvironment
        Write-Host -ForegroundColor DarkCyan "Added command path for $EnvironmentTarget target: $CommandDirectoryPath"
        return
    }
}

function MakeCommandPathAvailableAll {
    Param (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $Command
    )

    MakeCommandPathAvailableMachine -Command $Command
    MakeCommandPathAvailableProcess -Command $Command
    MakeCommandPathAvailableUser -Command $Command
}

function MakeCommandPathAvailableMachine {
    Param (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $Command
    )

    MakeCommandPathAvailable -Command $Command -EnvironmentTarget "Machine"
}

function MakeCommandPathAvailableProcess {
    Param (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $Command
    )

    MakeCommandPathAvailable -Command $Command -EnvironmentTarget "Process"
}

function MakeCommandPathAvailableUser {
    Param (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $Command
    )

    MakeCommandPathAvailable -Command $Command -EnvironmentTarget "User"
}

# Restore
# =================================================================================================================================

function RestoreAllAutomationTools {
    Write-Host -ForegroundColor DarkCyan "Restoring all automation tools."
    InstallPackageManagers
    UninstallAllAutomationTools
    InstallAllAutomationTools
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished restoring all automation tools. <<<`n"
}

function RestoreDocFx {
    Write-Host -ForegroundColor DarkCyan "Restoring DocFX."
    UninstallDocFx
    InstallDocFx
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished restoring DocFX. <<<`n"
}

function RestoreHtmlMinifier {
    Write-Host -ForegroundColor DarkCyan "Restoring HTMLMinifier."
    UninstallHtmlMinifier
    InstallHtmlMinifier
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished restoring HTMLMinifier. <<<`n"
}

function RestoreLeanify {
    Write-Host -ForegroundColor DarkCyan "Restoring Leanify."
    UninstallLeanify
    InstallLeanify
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished restoring Leanify. <<<`n"
}

function RestoreNodeJs {
    Write-Host -ForegroundColor DarkCyan "Restoring Node.js."
    UninstallNodeJs
    InstallNodeJs
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished restoring Node.js. <<<`n"
}

function RestorePsake {
    Write-Host -ForegroundColor DarkCyan "Restoring psake."
    UninstallPsake
    InstallPsake
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished restoring psake. <<<`n"
}

# Uninstall
# =================================================================================================================================

function UninstallAllAutomationTools {
    Write-Host -ForegroundColor DarkCyan "Uninstalling all automation tools."
    UninstallDocFx
    UninstallHtmlMinifier
    UninstallLeanify
    UninstallPsake
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling all automation tools. <<<`n"
}

function UninstallDocFx {
    If (GetDocFxInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Uninstalling DocFX."
        choco uninstall $ChoclateyPackageNameForDocFx -y --confirm
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling DocFX. <<<`n"
    }
}

function UninstallHtmlMinifier {
    Write-Host -ForegroundColor DarkCyan "Uninstalling HTMLMinifier."
    npm uninstall $NpmPackageNameForHtmlMinifier -g --loglevel error
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling HTMLMinifier. <<<`n"
}

function UninstallLeanify {
    If (GetLeanifyInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Uninstalling Leanify."
        choco uninstall $ChoclateyPackageNameForLeanify -y --confirm
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling Leanify. <<<`n"
    }
}

function UninstallNodeJs {
    If (GetNodeJsInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Uninstalling Node.js."
        choco uninstall $ChoclateyPackageNameForNodeJs -y --confirm
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling Node.js. <<<`n"
    }
}

function UninstallPsake {
    If (GetPsakeInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Uninstalling psake."
        choco uninstall $ChoclateyPackageNameForPsake --confirm
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling psake. <<<`n"
    }
}