# =================================================================================================================================
# Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
# =================================================================================================================================

$AutomationToolsModulePath = Join-Path -Path "$PSScriptRoot" -ChildPath "AutomationTools.psm1";
$DevelopmentToolsModulePath = Join-Path -Path "$PSScriptRoot" -ChildPath "DevelopmentTools.psm1";

Import-Module $AutomationToolsModulePath -Force;
Import-Module $DevelopmentToolsModulePath -Force;

# Replace the with the real secret before running the script. Revert before committing any changes.
$Secret = "REPLACE-ME";

If ($Secret -eq "REPLACE-ME") {
    Write-Host -ForegroundColor Red "Use a real secret to encrypt the code signing certificate. Revert before committing any changes.";
    return;
}

EncryptCodeSigningCertificate -Secret $Secret;
Write-Host -ForegroundColor Magenta "`n>>> IMPORTANT: Do not commit the secret! <<<`n";