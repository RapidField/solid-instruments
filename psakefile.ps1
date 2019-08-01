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

# Directory paths
$DirectoryPathForProjectRoot = $PSScriptRoot;
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

# Build
# =================================================================================================================================

Task Build-Debug -Alias bd -Depends Clean-Debug, Restore-Dependencies { BuildDebug }
Task Build-Release -Alias br -Depends Clean-Release, Restore-Dependencies { BuildRelease }

# Clean
# =================================================================================================================================

Task Clean-Debug -Alias cd { CleanDebug }
Task Clean-Release -Alias cr { CleanRelease }

# List
# =================================================================================================================================

Task List -Alias l { psake -docs }

# Restore
# =================================================================================================================================

Task Restore-Dependencies -Alias rd { RestoreDependencies }

# Start
# =================================================================================================================================

Task Start-All-Debug -Alias sad -Depends Stop-All, Start-ExampleServiceApplication-Debug, Start-ExampleWebApplication-Debug
Task Start-All-Release -Alias sar -Depends Stop-All, Start-ExampleServiceApplication-Release, Start-ExampleWebApplication-Release
Task Start-ExampleServiceApplication-Debug -Alias spsad -Depends Build-Debug { StartExampleServiceApplicationDebug }
Task Start-ExampleServiceApplication-Release -Alias spsar -Depends Build-Release { StartExampleServiceApplicationRelease }
Task Start-ExampleWebApplication-Debug -Alias spwad -Depends Build-Debug { StartExampleWebApplicationDebug }
Task Start-ExampleWebApplication-Release -Alias spwar -Depends Build-Release { StartExampleWebApplicationRelease }

# Start
# =================================================================================================================================

Task Stop-All -Alias sa { StopAllApplications }

# Test
# =================================================================================================================================

Task Test-Debug -Alias td -Depends Build-Debug { TestDebug }
Task Test-Release -Alias tr -Depends Build-Release { TestRelease }

# Verify
# =================================================================================================================================

Task Verify -Alias v -Depends Test-Release