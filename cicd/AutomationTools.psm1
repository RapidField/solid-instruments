# =================================================================================================================================
# Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
# =================================================================================================================================

$ChoclateyInstallScriptUri = "https://chocolatey.org/install.ps1"

# Get
# =================================================================================================================================

function GetChocolateyInstallationStatus {
    return (Get-Command "choco.exe" -ErrorAction SilentlyContinue)
}

function GetPsakeInstallationStatus {
    return (GetChocolateyInstallationStatus) -And (choco list -lo | Where-Object { $_.ToLower().StartsWith("psake") })
}

# Install
# =================================================================================================================================

function InstallAllAutomationTools {
    Write-Host -ForegroundColor DarkCyan "Installing all automation tools."
    InstallChocolatey
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

function InstallPsake {
    If (GetPsakeInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "psake is already installed."
        return
    }

    Write-Host -ForegroundColor DarkCyan "Installing psake."
    choco install psake --confirm
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing psake. <<<`n"
}

# Restore
# =================================================================================================================================

function RestoreAllAutomationTools {
    Write-Host -ForegroundColor DarkCyan "Restoring all automation tools."
    UninstallAllAutomationTools
    InstallAllAutomationTools
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished restoring all automation tools. <<<`n"
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
    UninstallPsake
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling all automation tools. <<<`n"
}

function UninstallPsake {
    If (GetPsakeInstallationStatus) {
        Write-Host -ForegroundColor DarkCyan "Uninstalling psake."
        choco uninstall psake --confirm
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling psake. <<<`n"
    }
}