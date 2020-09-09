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
Task Start-All-Debug -Alias sad -Depends Stop-All, Start-ExampleBeaconServiceApplication-Debug, Start-ExampleIdentityServiceApplication-Debug, Start-ExampleAccessControlServiceApplication-Debug, Start-ExampleAccessControlHttpApiApplication-Debug;
Task Start-All-Release -Alias sar -Depends Stop-All, Start-ExampleBeaconServiceApplication-Release, Start-ExampleIdentityServiceApplication-Release, Start-ExampleAccessControlServiceApplication-Release, Start-ExampleAccessControlHttpApiApplication-Release;
Task Start-ExampleAccessControlHttpApiApplication-Debug -Alias seachad -Depends Build-Debug -Action { StartExampleAccessControlHttpApiApplicationDebug; };
Task Start-ExampleAccessControlHttpApiApplication-Release -Alias seachar -Depends Build-Release -Action { StartExampleAccessControlHttpApiApplicationRelease; };
Task Start-ExampleAccessControlServiceApplication-Debug -Alias seacsad -Depends Build-Debug -Action { StartExampleAccessControlServiceApplicationDebug; };
Task Start-ExampleAccessControlServiceApplication-Release -Alias seacsar -Depends Build-Release -Action { StartExampleAccessControlServiceApplicationRelease; };
Task Start-ExampleBeaconServiceApplication-Debug -Alias sebsad -Depends Build-Debug -Action { StartExampleBeaconServiceApplicationDebug; };
Task Start-ExampleBeaconServiceApplication-Release -Alias sebsar -Depends Build-Release -Action { StartExampleBeaconServiceApplicationRelease; };
Task Start-ExampleIdentityServiceApplication-Debug -Alias seisad -Depends Build-Debug -Action { StartExampleIdentityServiceApplicationDebug; };
Task Start-ExampleIdentityServiceApplication-Release -Alias seisar -Depends Build-Release -Action { StartExampleIdentityServiceApplicationRelease; };
Task Stop-All -Alias sa -Action { StopAllApplications; };
Task Test-All -Alias ta -Depends Build-All, Test-Debug, Test-Release;
Task Test-Debug -Alias td -Depends Build-Debug -Action { TestDebug; };
Task Test-Release -Alias tr -Depends Build-Release -Action { TestRelease; };
Task Verify -Alias v -Depends Build-All, Test-Release;