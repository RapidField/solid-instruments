# =================================================================================================================================
# Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
# =================================================================================================================================

# Install
# =================================================================================================================================

function InstallAllDevelopmentTools {
    Write-Host -ForegroundColor DarkCyan "Installing all development tools.";
    InstallPoshGit;
    RestorePowerShellProfile;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing all development tools. <<<`n";
}

function InstallPoshGit {
    If (Get-Module -Name "posh-git") {
        Write-Host -ForegroundColor DarkCyan "PoshGit is already installed.";
    }
    Else {
        Write-Host -ForegroundColor DarkCyan "Installing PoshGit.";
        Install-Module posh-git;
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing PoshGit. <<<`n";
    }

    Import-Module posh-git -Force;
}

# Restore
# =================================================================================================================================

function RestoreAllDevelopmentTools {
    Write-Host -ForegroundColor DarkCyan "Restoring all development tools.";
    UninstallAllDevelopmentTools;
    InstallAllDevelopmentTools;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished restoring all development tools. <<<`n";
}

function RestorePoshGit {
    Write-Host -ForegroundColor DarkCyan "Restoring PoshGit.";
    UninstallPoshGit;
    InstallPoshGit;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished restoring PoshGit. <<<`n";
}

# Uninstall
# =================================================================================================================================

function UninstallAllDevelopmentTools {
    Write-Host -ForegroundColor DarkCyan "Uninstalling all development tools.";
    UninstallPoshGit;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling all development tools. <<<`n";
}

function UninstallPoshGit {
    If (Get-Module -Name "posh-git") {
        Write-Host -ForegroundColor DarkCyan "Uninstalling PoshGit.";
        Remove-Module posh-git;
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling PoshGit. <<<`n";
    }
}