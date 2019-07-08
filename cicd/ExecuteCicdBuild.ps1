# =================================================================================================================================
# Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
# =================================================================================================================================

Write-Host -ForegroundColor Cyan $("`nStarting a CI/CD build at {0:yyyy-MM-dd} {0:HH:mm:ss}." -f (Get-Date))

# Establish automation tools.
cd cicd
.\ResetEnvironment.ps1

# Execute the build.
cd ..
psake ta

Write-Host -ForegroundColor Cyan $("`nCompleting a CI/CD build at {0:yyyy-MM-dd} {0:HH:mm:ss}." -f (Get-Date))