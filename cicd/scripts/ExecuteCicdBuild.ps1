# =================================================================================================================================
# Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
# =================================================================================================================================

# This file scripts the standard CI/CD build process.
# =================================================================================================================================

# Script configuration
# =================================================================================================================================

# Directory names
$DirectoryNameForCicd = "cicd";
$DirectoryNameForCicdModules = "modules";
$DirectoryNameForCicdScripts = "scripts";

# File names
$FileNameForAutomationToolsModule = "AutomationTools.psm1";
$FileNameForBuildAndDeploymentModule = "BuildAndDeployment.psm1";

# Directory paths
$DirectoryPathForProjectRoot = (Get-Item $PSScriptRoot).Parent.Parent.FullName;
$DirectoryPathForCicd = Join-Path -Path "$DirectoryPathForProjectRoot" -ChildPath "$DirectoryNameForCicd";
$DirectoryPathForCicdModules = Join-Path -Path "$DirectoryPathForCicd" -ChildPath "$DirectoryNameForCicdModules";
$DirectoryPathForCicdScripts = Join-Path -Path "$DirectoryPathForCicd" -ChildPath "$DirectoryNameForCicdScripts";

# File paths
$FilePathForAutomationToolsModule = Join-Path -Path "$DirectoryPathForCicdModules" -ChildPath "$FileNameForAutomationToolsModule";
$FilePathForBuildAndDeploymentModule = Join-Path -Path "$DirectoryPathForCicdModules" -ChildPath "$FileNameForBuildAndDeploymentModule";

# Modules
# =================================================================================================================================

Import-Module $FilePathForAutomationToolsModule -Force;
Import-Module $FilePathForBuildAndDeploymentModule -Force;

# Script execution
# =================================================================================================================================

Write-Host -ForegroundColor Cyan $("`nStarting CI/CD build at {0:yyyy-MM-dd} {0:HH:mm:ss}.`n" -f (Get-Date));
WriteBuildDetails

# Establish automation tools.
Push-Location "$DirectoryPathForCicdScripts"
.\ResetEnvironment.ps1
Pop-Location

# Execute the build.
Push-Location "$DirectoryPathForProjectRoot";
psake Verify
Pop-Location

Write-Host -ForegroundColor Cyan $("`nFinished CI/CD build at {0:yyyy-MM-dd} {0:HH:mm:ss}.`n" -f (Get-Date));
WriteBuildDetails