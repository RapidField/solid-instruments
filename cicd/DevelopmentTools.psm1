# =================================================================================================================================
# Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
# =================================================================================================================================

$ProjectRootDirectory = (Get-Item $PSScriptRoot).Parent.FullName
$PsProfileAppendage = "cd `"$ProjectRootDirectory`";Import-Module .\cicd\DevelopmentTools.psm1;Import-Module posh-git;"

# Git
# =================================================================================================================================

function GitLatest {
    Write-Host -ForegroundColor DarkCyan "Fetching commits from the remote repository."
    Push-Location $PSScriptRoot
    git fetch --all
    Write-Host -ForegroundColor DarkCyan "Pulling commits into the current branch."
    git pull
    Pop-Location
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished pulling in remote commits. <<<`n"
}

function GitList {
    Write-Host -ForegroundColor White "========================================================================"
    Write-Host -ForegroundColor White "GitLatest    - Fetches remote commits and pulls into the current branch."
    Write-Host -ForegroundColor White "GitMergeFrom - Merges the specified branch into the current branch."
    Write-Host -ForegroundColor White "GitState     - Fetches remote commits and displays status information."
    Write-Host -ForegroundColor White "GitSync      - Fully synchronizes the current local and remote branches."
    Write-Host -ForegroundColor White "========================================================================`n"
}

function GitMergeFrom {
    Param (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $SourceBranch
    )

    If ([String]::IsNullOrEmpty($SourceBranch)) {
        Write-Host -ForegroundColor Red "`n>>> Please specify a source branch name. <<<`n"
    }
    Else {
        Write-Host -ForegroundColor DarkCyan "Fetching commits from the remote repository."
        Push-Location $ProjectRootDirectory
        git fetch --all
        Write-Host -ForegroundColor DarkCyan "Merging commits from $SourceBranch into the current local branch."
        git merge $SourceBranch
        Pop-Location
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished merging commits from $SourceBranch into the current local branch. <<<`n"
    }
}

function GitState {
    Write-Host -ForegroundColor DarkCyan "Fetching commits from the remote repository."
    Push-Location $ProjectRootDirectory
    git fetch --all
    Write-Host -ForegroundColor DarkCyan "Getting local branch information."
    git branch
    git status
    Pop-Location
    Write-Host -ForegroundColor DarkCyan "`n>>> Ready. <<<`n"
}

function GitSync {
    Param (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $CommitMessage
    )

    If ([String]::IsNullOrEmpty($CommitMessage)) {
        Write-Host -ForegroundColor Red "`n>>> Please specify a commit message for uncommitted changes. <<<`n"
    }
    Else {
        Write-Host -ForegroundColor DarkCyan "Committing changes to the current local repository."
        Push-Location $ProjectRootDirectory
        git fetch --all
        git add .
        git commit -m $CommitMessage
        git pull
        Write-Host -ForegroundColor DarkCyan "Pushing commits to the remote repository."
        git push
        Pop-Location
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished syncing the current branch with the remote repository. <<<`n"
    }
}

# Install
# =================================================================================================================================

function InstallAllDevelopmentTools {
    Write-Host -ForegroundColor DarkCyan "Installing all development tools."
    InstallPoshGit
    RestorePowerShellProfile
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing all development tools. <<<`n"
}

function InstallPoshGit {
    If (Get-Module -Name "posh-git") {
        Write-Host -ForegroundColor DarkCyan "PoshGit is already installed."
    }
    Else {
        Write-Host -ForegroundColor DarkCyan "Installing PoshGit."
        Install-Module posh-git
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished installing PoshGit. <<<`n"
    }

    Import-Module posh-git
}

# Restore
# =================================================================================================================================

function RestoreAllDevelopmentTools {
    Write-Host -ForegroundColor DarkCyan "Restoring all development tools."
    UninstallAllDevelopmentTools
    InstallAllDevelopmentTools
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished restoring all development tools. <<<`n"
}

function RestorePoshGit {
    Write-Host -ForegroundColor DarkCyan "Restoring PoshGit."
    UninstallPoshGit
    InstallPoshGit
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished restoring PoshGit. <<<`n"
}

# Uninstall
# =================================================================================================================================

function UninstallAllDevelopmentTools {
    Write-Host -ForegroundColor DarkCyan "Uninstalling all development tools."
    UninstallPoshGit
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling all development tools. <<<`n"
}

function UninstallPoshGit {
    If (Get-Module -Name "posh-git") {
        Write-Host -ForegroundColor DarkCyan "Uninstalling PoshGit."
        Remove-Module posh-git
        Write-Host -ForegroundColor DarkCyan "`n>>> Finished uninstalling PoshGit. <<<`n"
    }
}