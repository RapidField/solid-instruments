# =================================================================================================================================
# Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
# =================================================================================================================================

Import-Module .\cicd\Configuration.psm1
Import-Module .\cicd\AutomationTools.psm1
Import-Module .\cicd\BuildAndDeployment.psm1
Import-Module .\cicd\DevelopmentTools.psm1

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

# Deploy
# =================================================================================================================================

Task Deploy-Local -Alias dl -Depends Build-All { DeployLocal }
Task Deploy-Production -Alias dp -Depends Build-All { DeployProduction }

# List
# =================================================================================================================================

Task List -Alias l { psake -docs }

# Restore
# =================================================================================================================================

Task Restore-Dependencies -Alias rd { RestoreDependencies }

# Start
# =================================================================================================================================

Task Start-All-Debug -Alias sad -Depends Stop-All, Start-PrototypeServiceApplication-Debug, Start-PrototypeWebApplication-Debug
Task Start-All-Release -Alias sar -Depends Stop-All, Start-PrototypeServiceApplication-Release, Start-PrototypeWebApplication-Release
Task Start-PrototypeServiceApplication-Debug -Alias spsad -Depends Build-Debug { StartPrototypeServiceApplicationDebug }
Task Start-PrototypeServiceApplication-Release -Alias spsar -Depends Build-Release { StartPrototypeServiceApplicationRelease }
Task Start-PrototypeWebApplication-Debug -Alias spwad -Depends Build-Debug { StartPrototypeWebApplicationDebug }
Task Start-PrototypeWebApplication-Release -Alias spwar -Depends Build-Release { StartPrototypeWebApplicationRelease }

# Start
# =================================================================================================================================

Task Stop-All -Alias sa { StopAllApplications }

# Test
# =================================================================================================================================

Task Test-All -Alias ta -Depends Build-All, Test-Debug, Test-Release
Task Test-Debug -Alias td -Depends Build-Debug { TestDebug }
Task Test-Release -Alias tr -Depends Build-Release { TestRelease }