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
$DirectoryPathForProjectRoot = (Get-Item $PSScriptRoot).Parent.Parent.FullName;
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

Write-Host -ForegroundColor DarkCyan "`nResetting the build environment.`n";

$CurrentPrincipal = New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent());

If ($CurrentPrincipal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
    RestoreAllAutomationTools;
}
Else {
    $CurrentInvocationPath = $MyInvocation.MyCommand.Path;
    $CurrentInvocationArguments = $MyInvocation.UnboundArguments;
    Start-Process -FilePath powershell.exe -Verb RunAs -ArgumentList "-File `"$CurrentInvocationPath`" $CurrentInvocationArguments";
    RestoreAllDevelopmentTools;
    Exit;
}

Write-Host -ForegroundColor DarkCyan "`n>>> Finished resetting the build environment. <<<`n";
