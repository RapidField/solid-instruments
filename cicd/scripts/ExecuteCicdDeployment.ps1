# =================================================================================================================================
# Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
# =================================================================================================================================

<#
.Synopsis
This script performs the deployment process for successful CI/CD master builds.
#>

Param
(
    [Parameter(Mandatory = $false, Position = 0)]
    [Switch] $Interactive
)

# Directory names
$DirectoryNameForCicd = "cicd";
$DirectoryNameForCicdModules = "modules";
$DirectoryNameForCicdScripts = "scripts";

# File names
$FileNameForAutomationToolsModule = "AutomationTools.psm1";
$FileNameForBuildAndDeploymentModule = "BuildAndDeployment.psm1";
$FileNameForCoreModule = "Core.psm1";

# Directory paths
$DirectoryPathForProjectRoot = (Get-Item $PSScriptRoot).Parent.Parent.FullName;
$DirectoryPathForCicd = Join-Path -Path "$DirectoryPathForProjectRoot" -ChildPath "$DirectoryNameForCicd";
$DirectoryPathForCicdModules = Join-Path -Path "$DirectoryPathForCicd" -ChildPath "$DirectoryNameForCicdModules";
$DirectoryPathForCicdScripts = Join-Path -Path "$DirectoryPathForCicd" -ChildPath "$DirectoryNameForCicdScripts";

# File paths
$FilePathForAutomationToolsModule = Join-Path -Path "$DirectoryPathForCicdModules" -ChildPath "$FileNameForAutomationToolsModule";
$FilePathForBuildAndDeploymentModule = Join-Path -Path "$DirectoryPathForCicdModules" -ChildPath "$FileNameForBuildAndDeploymentModule";
$FilePathForCoreModule = Join-Path -Path "$DirectoryPathForCicdModules" -ChildPath "$FileNameForCoreModule";

# Solution configurations
$SolutionConfigurationRelease = "Release";

# Branch names
$BranchNameForMaster = "master";

# Environment variables
$BranchName = $env:APPVEYOR_REPO_BRANCH;

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
    SignPackages -SolutionConfiguration $SolutionConfigurationRelease;
    PublishPackages -SolutionConfiguration $SolutionConfigurationRelease;
    PublishWebDocumentation -SolutionConfiguration $SolutionConfigurationRelease;
}

<#
.Synopsis
Initiates execution of the current script.
#>
Function EnterScript
{
    ComposeHeader "CI/CD deployment";

    If ($ContextIsInteractive)
    {
        ComposeNormal "The following process will perform a production Solid Instruments deployment.";
        $UserInput = PromptUser -QuestionText "Would you like to continue?" -PromptText "[Y] Yes [N] No";

        If (($UserInput -eq $null) -or ($UserInput -eq [String]::Empty))
        {
            ComposeNormal "Exiting.";
            Exit;
        }

        Switch ($UserInput.Trim().ToUpper().Substring(0, 1))
        {
            "Y"
            {
                ComposeNormal "Continuing.";
                Break;
            }
            Default
            {
                ComposeNormal "Exiting.";
                Exit;
            }
        }
    }

    If ($BranchName -ne $BranchNameForMaster)
    {
        ComposeNormal "Suppressing deployment. The branch is not $BranchNameForMaster.";
        Exit;
    }

    ComposeStart $("Starting CI/CD deployment at {0:yyyy-MM-dd} {0:HH:mm:ss}." -f (Get-Date));
    WriteBuildDetails;
    PerformActions;
    ComposeFinish $("Finished CI/CD deployment at {0:yyyy-MM-dd} {0:HH:mm:ss}." -f (Get-Date));
}

# Execution
EnterScript;