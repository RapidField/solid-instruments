# =================================================================================================================================
# Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
# =================================================================================================================================

<#
.Synopsis
This file organizes, aggregates and abstracts CI/CD operations.
#>

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
Import-Module $FilePathForAutomationToolsModule -Force;
Import-Module $FilePathForBuildAndDeploymentModule -Force;

# Tasks
Task Build-All -Alias ba -Depends Clean-All, Restore-Dependencies, Build-Debug, Build-Release;
Task Build-Debug -Alias bd -Depends Clean-Debug, Restore-Dependencies -Action { BuildDebug; };
Task Build-Release -Alias br -Depends Clean-Release, Restore-Dependencies -Action { BuildRelease; };
Task Clean-All -Alias ca -Depends Clean-Debug, Clean-Release;
Task Clean-Debug -Alias cd -Action { CleanDebug; };
Task Clean-Release -Alias cr -Action { CleanRelease; };
Task List -Alias l -Action { psake -docs };
Task Restore-Dependencies -Alias rd -Action { RestoreDependencies; };
Task Start-All-Debug -Alias sad -Depends Stop-All, Start-ExampleServiceApplication-Debug, Start-ExampleWebApplication-Debug;
Task Start-All-Release -Alias sar -Depends Stop-All, Start-ExampleServiceApplication-Release, Start-ExampleWebApplication-Release;
Task Start-ExampleServiceApplication-Debug -Alias sesad -Depends Build-Debug -Action { StartExampleServiceApplicationDebug; };
Task Start-ExampleServiceApplication-Release -Alias sesar -Depends Build-Release -Action { StartExampleServiceApplicationRelease; };
Task Start-ExampleWebApplication-Debug -Alias sewad -Depends Build-Debug -Action { StartExampleWebApplicationDebug; };
Task Start-ExampleWebApplication-Release -Alias sewar -Depends Build-Release -Action { StartExampleWebApplicationRelease; };
Task Stop-All -Alias sa -Action { StopAllApplications; };
Task Test-All -Alias ta -Depends Build-All, Test-Debug, Test-Release;
Task Test-Debug -Alias td -Depends Build-Debug -Action { TestDebug; };
Task Test-Release -Alias tr -Depends Build-Release -Action { TestRelease; };
Task Verify -Alias v -Depends Build-All, Test-Release;