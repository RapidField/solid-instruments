# =================================================================================================================================
# Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
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
$FileNameForDevelopmentToolsModule = "DevelopmentTools.psm1";

# Directory paths
$DirectoryPathForProjectRoot = $PSScriptRoot;
$DirectoryPathForCicd = Join-Path -Path "$DirectoryPathForProjectRoot" -ChildPath "$DirectoryNameForCicd";
$DirectoryPathForCicdModules = Join-Path -Path "$DirectoryPathForCicd" -ChildPath "$DirectoryNameForCicdModules";
$DirectoryPathForCicdScripts = Join-Path -Path "$DirectoryPathForCicd" -ChildPath "$DirectoryNameForCicdScripts";

# File paths
$FilePathForAutomationToolsModule = Join-Path -Path "$DirectoryPathForCicdModules" -ChildPath "$FileNameForAutomationToolsModule";
$FilePathForBuildAndDeploymentModule = Join-Path -Path "$DirectoryPathForCicdModules" -ChildPath "$FileNameForBuildAndDeploymentModule";
$FilePathForDevelopmentToolsModule = Join-Path -Path "$DirectoryPathForCicdModules" -ChildPath "$FileNameForDevelopmentToolsModule";

# Modules
# =================================================================================================================================

Import-Module $FilePathForAutomationToolsModule -Force;
Import-Module $FilePathForBuildAndDeploymentModule -Force;
Import-Module $FilePathForDevelopmentToolsModule -Force;

# Script execution
# =================================================================================================================================

# This script is intended to be used in containerized build environments. Successful execution requires administrative privilege on
# the host. Automation tools are installed during the process.
Push-Location "$DirectoryPathForCicdScripts"
.\ExecuteCicdBuild.ps1
Pop-Location