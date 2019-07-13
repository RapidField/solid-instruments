# =================================================================================================================================
# Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
# =================================================================================================================================

Write-Host -ForegroundColor Cyan $("`nStarting CI/CD build at {0:yyyy-MM-dd} {0:HH:mm:ss}." -f (Get-Date));

# Establish automation tools.
Push-Location "$PSScriptRoot"
.\ResetEnvironment.ps1
Pop-Location

# Execute the build.
Push-Location (Get-Item $PSScriptRoot).Parent.FullName;
psake Test-All
Pop-Location

Write-Host -ForegroundColor Cyan $("`nFinished CI/CD build at {0:yyyy-MM-dd} {0:HH:mm:ss}." -f (Get-Date));