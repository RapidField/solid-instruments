# =================================================================================================================================
# Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
# =================================================================================================================================

# Execute a full CI/CD build.
cd $PSScriptRoot
.\ExecuteCicdBuild.ps1

Write-Host -ForegroundColor Cyan $("`nStarting CI/CD deployment at {0:yyyy-MM-dd} {0:HH:mm:ss}." -f (Get-Date))

# Deploy the build.
cd $PSScriptRoot
cd ..
psake Deploy-Production

Write-Host -ForegroundColor Cyan $("`nFinished CI/CD deployment at {0:yyyy-MM-dd} {0:HH:mm:ss}." -f (Get-Date))