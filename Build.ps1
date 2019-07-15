# =================================================================================================================================
# Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
# =================================================================================================================================

# Script configuration
# =================================================================================================================================

# Directory names
$DirectoryNameForCicd = "cicd";
$DirectoryNameForCicdScripts = "scripts";

# Directory paths
$DirectoryPathForProjectRoot = $PSScriptRoot;
$DirectoryPathForCicd = Join-Path -Path "$DirectoryPathForProjectRoot" -ChildPath "$DirectoryNameForCicd";
$DirectoryPathForCicdScripts = Join-Path -Path "$DirectoryPathForCicd" -ChildPath "$DirectoryNameForCicdScripts";

# Branch names
$BranchNameForDevelop = "develop";
$BranchNameForMaster = "master";

# Script execution
# =================================================================================================================================

Push-Location "$DirectoryPathForCicdScripts"
.\ExecuteCicdBuild.ps1
Pop-Location

$BranchName = $env:APPVEYOR_REPO_BRANCH;

If ($BranchName -eq $BranchNameForMaster) {
    Push-Location "$DirectoryPathForCicdScripts"
    .\ExecuteCicdDeployment.ps1
    Pop-Location
}