# =================================================================================================================================
# Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
# =================================================================================================================================

<#
.Synopsis
This script performs the build environment setup/reset process.
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
$DirectoryPathForProjectRoot = (Get-Item "$PSScriptRoot").Parent.Parent.FullName;
$DirectoryPathForCicd = Join-Path -Path "$DirectoryPathForProjectRoot" -ChildPath "$DirectoryNameForCicd";
$DirectoryPathForCicdModules = Join-Path -Path "$DirectoryPathForCicd" -ChildPath "$DirectoryNameForCicdModules";
$DirectoryPathForCicdScripts = Join-Path -Path "$DirectoryPathForCicd" -ChildPath "$DirectoryNameForCicdScripts";

# File paths
$FilePathForAutomationToolsModule = Join-Path -Path "$DirectoryPathForCicdModules" -ChildPath "$FileNameForAutomationToolsModule";
$FilePathForBuildAndDeploymentModule = Join-Path -Path "$DirectoryPathForCicdModules" -ChildPath "$FileNameForBuildAndDeploymentModule";
$FilePathForCoreModule = Join-Path -Path "$DirectoryPathForCicdModules" -ChildPath "$FileNameForCoreModule";

# Other configuration values
$ContextIsInteractive = $Interactive.IsPresent;

# Modules
Import-Module "$FilePathForAutomationToolsModule" -Force;
Import-Module "$FilePathForBuildAndDeploymentModule" -Force;
Import-Module "$FilePathForCoreModule" -Force;

<#
.Synopsis
Houses the functional body of the current script.
#>
Function PerformActions
{
    RestoreAllAutomationTools;
}

<#
.Synopsis
Initiates execution of the current script.
#>
Function EnterScript
{
    ComposeHeader "Environment setup/reset";

    If ($ContextIsInteractive)
    {
        ComposeNormal "The following process will install the Solid Instruments automation tooling.";
        ComposeNormal "By continuing, you agree to accept the license agreements for the various software packages contained herein.";
        ComposeNormal "For more information, see https://github.com/RapidField/solid-instruments/blob/master/CONTRIBUTING.md#tooling";
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

    RejectNonAdministratorUsers;
    ComposeStart $("Resetting the build environment at {0:yyyy-MM-dd} {0:HH:mm:ss}." -f (Get-Date));
    WriteBuildDetails;
    PerformActions;
    ComposeFinish $("Finished resetting the build environment at {0:yyyy-MM-dd} {0:HH:mm:ss}." -f (Get-Date));
}

# Execution
EnterScript;