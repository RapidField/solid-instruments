# =================================================================================================================================
# Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
# =================================================================================================================================

Write-Host -ForegroundColor Cyan $("`nStarting CI/CD build at {0:yyyy-MM-dd} {0:HH:mm:ss}." -f (Get-Date));

# Establish automation tools.
cd "$PSScriptRoot"
.\ResetEnvironment.ps1

# Execute the build.
cd "$PSScriptRoot"
cd ..
psake Test-All

Write-Host -ForegroundColor Cyan $("`nFinished CI/CD build at {0:yyyy-MM-dd} {0:HH:mm:ss}." -f (Get-Date));