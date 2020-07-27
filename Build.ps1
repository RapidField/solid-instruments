# =================================================================================================================================
# Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
# =================================================================================================================================

<#
.Synopsis
This file serves as the standard entry point for the CI/CD pipeline.
#>

Param
(
    [Parameter(Mandatory = $false, Position = 0)]
    [Switch] $Interactive
)

# Directory names
$DirectoryNameForArtifacts = "artifacts";
$DirectoryNameForCicd = "cicd";
$DirectoryNameForCicdModules = "modules";
$DirectoryNameForCicdScripts = "scripts";

# File names
$FileNameForAutomationToolsModule = "AutomationTools.psm1";
$FileNameForBuildAndDeploymentModule = "BuildAndDeployment.psm1";
$FileNameForCicdLog = "cicd.log";
$FileNameForCoreModule = "Core.psm1";

# Directory paths
$DirectoryPathForProjectRoot = $PSScriptRoot;
$DirectoryPathForArtifacts = Join-Path -Path "$DirectoryPathForProjectRoot" -ChildPath "$DirectoryNameForArtifacts";
$DirectoryPathForCicd = Join-Path -Path "$DirectoryPathForProjectRoot" -ChildPath "$DirectoryNameForCicd";
$DirectoryPathForCicdModules = Join-Path -Path "$DirectoryPathForCicd" -ChildPath "$DirectoryNameForCicdModules";
$DirectoryPathForCicdScripts = Join-Path -Path "$DirectoryPathForCicd" -ChildPath "$DirectoryNameForCicdScripts";

# File paths
$FilePathForAutomationToolsModule = Join-Path -Path "$DirectoryPathForCicdModules" -ChildPath "$FileNameForAutomationToolsModule";
$FilePathForBuildAndDeploymentModule = Join-Path -Path "$DirectoryPathForCicdModules" -ChildPath "$FileNameForBuildAndDeploymentModule";
$FilePathForCicdLog = Join-Path -Path "$DirectoryPathForArtifacts" -ChildPath "$FileNameForCicdLog";
$FilePathForCoreModule = Join-Path -Path "$DirectoryPathForCicdModules" -ChildPath "$FileNameForCoreModule";

# Other configuration values
$ContextIsInteractive = $Interactive.IsPresent;

# Modules
Import-Module $FilePathForAutomationToolsModule -Force;
Import-Module $FilePathForBuildAndDeploymentModule -Force;
Import-Module $FilePathForCoreModule -Force;

<#
.Synopsis
Houses the functional body of the current script.
#>
Function PerformActions
{
    Push-Location "$DirectoryPathForCicdScripts";
    .\ResetEnvironment.ps1 -Interactive:$ContextIsInteractive;
    .\ExecuteCicdBuild.ps1 -Interactive:$ContextIsInteractive;
    .\ExecuteCicdDeployment.ps1 -Interactive:$ContextIsInteractive;
    Pop-Location;
}

<#
.Synopsis
Initiates execution of the current script.
#>
Function EnterScript
{
    RejectNonAdministratorUsers;
    Start-Transcript -Path "$FilePathForCicdLog" -Force;
    ComposeStart $("Entering CI/CD pipeline at {0:yyyy-MM-dd} {0:HH:mm:ss}." -f (Get-Date));
    PerformActions;
    ComposeFinish $("Exiting CI/CD pipeline at {0:yyyy-MM-dd} {0:HH:mm:ss}." -f (Get-Date));
    Stop-Transcript;
}

# Execution
EnterScript;