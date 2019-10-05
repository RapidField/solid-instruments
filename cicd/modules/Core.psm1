# =================================================================================================================================
# Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
# =================================================================================================================================

<#
.Synopsis
This module exposes shared utility functions for the CI/CD pipeline.
#>

# Command names
$CommandNameForPowerShell = "powershell";

Function ComposeError
{
    Param
    (
        [Parameter(Mandatory = $false, Position = 0)]
        [String] $Text = ""
    )

    If (($Text -eq $null) -or ($Text -eq ""))
    {
        Return;
    }

    Write-Host -ForegroundColor Red "$Text";
}

Function ComposeFinish
{
    Param
    (
        [Parameter(Mandatory = $false, Position = 0)]
        [String] $Text = ""
    )

    If (($Text -eq $null) -or ($Text -eq ""))
    {
        Return;
    }

    Write-Host -ForegroundColor Cyan "$Text`n";
}

Function ComposeHeader
{
    Param
    (
        [Parameter(Mandatory = $false, Position = 0)]
        [String] $Text = ""
    )

    If (($Text -eq $null) -or ($Text -eq ""))
    {
        Return;
    }

    Write-Host -ForegroundColor Magenta "`n$Text";
    Write-Host -ForegroundColor Magenta "================================================================================================`n";
}

Function ComposeNormal
{
    Param
    (
        [Parameter(Mandatory = $false, Position = 0)]
        [String] $Text = ""
    )

    If (($Text -eq $null) -or ($Text -eq ""))
    {
        Return;
    }

    Write-Host -ForegroundColor White "$Text";
}

Function ComposeStart
{
    Param
    (
        [Parameter(Mandatory = $false, Position = 0)]
        [String] $Text = ""
    )

    If (($Text -eq $null) -or ($Text -eq ""))
    {
        Return;
    }

    Write-Host -ForegroundColor DarkCyan "$Text";
}

Function ComposeSuccess
{
    Param
    (
        [Parameter(Mandatory = $false, Position = 0)]
        [String] $Text = ""
    )

    If (($Text -eq $null) -or ($Text -eq ""))
    {
        Return;
    }

    Write-Host -ForegroundColor Green "$Text";
}

Function ComposeVerbose
{
    Param
    (
        [Parameter(Mandatory = $false, Position = 0)]
        [String] $Text = ""
    )

    If (($Text -eq $null) -or ($Text -eq ""))
    {
        Return;
    }

    Write-Host -ForegroundColor DarkGray "  $Text";
}

Function ComposeWarning
{
    Param
    (
        [Parameter(Mandatory = $false, Position = 0)]
        [String] $Text = ""
    )

    If (($Text -eq $null) -or ($Text -eq ""))
    {
        Return;
    }

    Write-Host -ForegroundColor Yellow "$Text";
}

Function ExecuteProcess
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $Path,
        [Parameter(Mandatory = $false, Position = 1)]
        [String] $Arguments = ""
    )

    $ProcessDefinition = "$Path $Arguments";
    ComposeStart "Starting process: $ProcessDefinition";
    $Process = Start-Process -FilePath "$Path" -ArgumentList "$Arguments" -NoNewWindow -PassThru;
    Wait-Process -ErrorAction Stop -InputObject $Process;

    If ($Process.HasExited)
    {
        $ProcessExitCode = $Process.ExitCode;

        If ($ProcessExitCode -eq 0)
        {
            ComposeFinish "Process completed: $ProcessDefinition";
            Return;
        }

        Throw "Process failed (exit code $ProcessExitCode): $ProcessDefinition";
    }
}

Function PromptUser
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $QuestionText = "",
        [Parameter(Mandatory = $true, Position = 1)]
        [String] $PromptText = ""
    )

    If (($QuestionText -ne $null) -and ($QuestionText -ne ""))
    {
        Write-Host -ForegroundColor White "`n$QuestionText";
    }

    If (($PromptText -ne $null) -and ($PromptText -ne ""))
    {
        Return Read-Host -Prompt "$PromptText";
    }

    Return $null;
}

Function RejectNonAdministratorUsers
{
    $CurrentPrincipal = New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent());
    $CurrentPrincipalIsAdministrator = $CurrentPrincipal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator);

    If ($CurrentPrincipalIsAdministrator -eq $false)
    {
        Throw "This script must be executed with administrative privilege.";
    }
}