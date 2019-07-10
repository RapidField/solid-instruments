# =================================================================================================================================
# Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
# =================================================================================================================================

$AutomationToolsModulePath = Join-Path -Path $PSScriptRoot -ChildPath "AutomationTools.psm1"
$DevelopmentToolsModulePath = Join-Path -Path $PSScriptRoot -ChildPath "DevelopmentTools.psm1"

Import-Module $AutomationToolsModulePath
Import-Module $DevelopmentToolsModulePath

$CurrentPrincipal = New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())

If ($CurrentPrincipal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
    RestoreAllAutomationTools
}
Else {
    $CurrentInvocationPath = $MyInvocation.MyCommand.Path
    $CurrentInvocationArguments = $MyInvocation.UnboundArguments

    Start-Process -FilePath powershell.exe -Verb RunAs -ArgumentList "-File `"$CurrentInvocationPath`" $CurrentInvocationArguments"
    RestoreAllDevelopmentTools
    Exit
}