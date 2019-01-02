# =================================================================================================================================
# Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
# =================================================================================================================================

$BuildVersion = "1.0.23-preview"

$Configuration = [PSCustomObject]@{
    Local = [PSCustomObject]@{
        PackageDeploymentUri = "C:\LocalPackageSource"
    }
    Production = [PSCustomObject]@{
        PackageDeploymentUri = "C:\LocalPackageSource"
    }
}

# Get
# =================================================================================================================================

function GetBuildVersion {
    return $BuildVersion
}

function GetConfigurationValue {
    Param (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $Type,
        [Parameter(Mandatory = $true, Position = 1)]
        [String] $Key
    )

    return $Configuration.$Type.$Key
}