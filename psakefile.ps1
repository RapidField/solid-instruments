# =================================================================================================================================
# Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
# =================================================================================================================================

Import-Module .\cicd\AutomationTools.psm1 -Force;
Import-Module .\cicd\BuildAndDeployment.psm1 -Force;
Import-Module .\cicd\DevelopmentTools.psm1 -Force;

# Build
# =================================================================================================================================

Task Build-All -Alias ba -Depends Clean-All, Restore-Dependencies, Build-Debug, Build-Release
Task Build-Debug -Alias bd -Depends Clean-Debug, Restore-Dependencies { BuildDebug }
Task Build-Release -Alias br -Depends Clean-Release, Restore-Dependencies { BuildRelease }

# Clean
# =================================================================================================================================

Task Clean-All -Alias ca -Depends Clean-Debug, Clean-Release
Task Clean-Debug -Alias cd { CleanDebug }
Task Clean-Release -Alias cr { CleanRelease }

# List
# =================================================================================================================================

Task List -Alias l { psake -docs }

# Restore
# =================================================================================================================================

Task Restore-Dependencies -Alias rd { RestoreDependencies }

# Start
# =================================================================================================================================

Task Start-All-Debug -Alias sad -Depends Stop-All, Start-ExampleServiceApplication-Debug, Start-ExampleWebApplication-Debug
Task Start-All-Release -Alias sar -Depends Stop-All, Start-ExampleServiceApplication-Release, Start-ExampleWebApplication-Release
Task Start-ExampleServiceApplication-Debug -Alias spsad -Depends Build-Debug { StartExampleServiceApplicationDebug }
Task Start-ExampleServiceApplication-Release -Alias spsar -Depends Build-Release { StartExampleServiceApplicationRelease }
Task Start-ExampleWebApplication-Debug -Alias spwad -Depends Build-Debug { StartExampleWebApplicationDebug }
Task Start-ExampleWebApplication-Release -Alias spwar -Depends Build-Release { StartExampleWebApplicationRelease }

# Start
# =================================================================================================================================

Task Stop-All -Alias sa { StopAllApplications }

# Test
# =================================================================================================================================

Task Test-All -Alias ta -Depends Build-All, Test-Debug, Test-Release
Task Test-Debug -Alias td -Depends Build-Debug { TestDebug }
Task Test-Release -Alias tr -Depends Build-Release { TestRelease }