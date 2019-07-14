# =================================================================================================================================
# Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
# =================================================================================================================================

$BuildAndDeploymentModulePath = Join-Path -Path "$PSScriptRoot" -ChildPath "BuildAndDeployment.psm1";

Import-Module $BuildAndDeploymentModulePath -Force;

# Replace the with the real key before running the script. Revert before committing any changes.
$Key = "REPLACE-ME";

If ($Key -eq "REPLACE-ME") {
    Write-Host -ForegroundColor Red "Use a real key to encrypt the code signing certificate. Revert before committing any changes.";
    return;
}

EncryptCodeSigningCertificate -Key $Key;

Write-Host -ForegroundColor Magenta "============================================";
Write-Host -ForegroundColor Magenta ">>> IMPORTANT: Do not commit the secret! <<<";
Write-Host -ForegroundColor Magenta "============================================";