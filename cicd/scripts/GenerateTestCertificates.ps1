# =================================================================================================================================
# Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
# =================================================================================================================================

<#
.Synopsis
This script generates self-signed X509 certificates that are used for testing.
#>

Param
(
    [Parameter(Mandatory = $false, Position = 0)]
    [Switch] $Interactive
)

# Directory names
$DirectoryNameForCicd = "cicd";
$DirectoryNameForCicdAssets = "assets";
$DirectoryNameForCicdModules = "modules";
$DirectoryNameForCicdScripts = "scripts";

# File names
$FileNameForCertificateOne = "TestRootOne.testcert";
$FileNameForCertificateTwo = "TestRootTwo.testcert";
$FileNameForCertificateThree = "TestRootThree.testcert";
$FileNameForCoreModule = "Core.psm1";

# Directory paths
$DirectoryPathForProjectRoot = (Get-Item $PSScriptRoot).Parent.Parent.FullName;
$DirectoryPathForCicd = Join-Path -Path "$DirectoryPathForProjectRoot" -ChildPath "$DirectoryNameForCicd";
$DirectoryPathForCicdAssets = Join-Path -Path "$DirectoryPathForCicd" -ChildPath "$DirectoryNameForCicdAssets";
$DirectoryPathForCicdModules = Join-Path -Path "$DirectoryPathForCicd" -ChildPath "$DirectoryNameForCicdModules";
$DirectoryPathForCicdScripts = Join-Path -Path "$DirectoryPathForCicd" -ChildPath "$DirectoryNameForCicdScripts";

# File paths
$FilePathForCertificateOne = Join-Path -Path "$DirectoryPathForCicdAssets" -ChildPath "$FileNameForCertificateOne";
$FilePathForCertificateTwo = Join-Path -Path "$DirectoryPathForCicdAssets" -ChildPath "$FileNameForCertificateTwo";
$FilePathForCertificateThree = Join-Path -Path "$DirectoryPathForCicdAssets" -ChildPath "$FileNameForCertificateThree";
$FilePathForCoreModule = Join-Path -Path "$DirectoryPathForCicdModules" -ChildPath "$FileNameForCoreModule";

# Other configuration values
$ContextIsInteractive = $Interactive.IsPresent;
$CertificateHashAlgorithm = "SHA256";
$CertificateKeyExportPolicy = "Exportable";
$CertificateKeyLength = 2048;
$CertificateKeySpec = "Signature";
$CertificateKeyUsage = "CertSign";
$CertificateKeyUsageProperty = "Sign";
$CertificatePasswordOne = ConvertTo-SecureString -AsPlainText -Force -String ‘Password1!’;
$CertificatePasswordTwo = ConvertTo-SecureString -AsPlainText -Force -String ‘Password2!’;
$CertificatePasswordThree = ConvertTo-SecureString -AsPlainText -Force -String ‘Password3!’;
$CertificateStoreLocation = "Cert:\CurrentUser\My";
$CertificateSubjectOne = "CN=TestRootOne";
$CertificateSubjectTwo = "CN=TestRootTwo";
$CertificateSubjectThree = "CN=TestRootThree";
$CertificateType = "Custom";

# Modules
Import-Module $FilePathForCoreModule -Force;

<#
.Synopsis
Houses the functional body of the current script.
#>
Function PerformActions
{
    $CertificateOne = New-SelfSignedCertificate `
        -CertStoreLocation "$CertificateStoreLocation" `
        -HashAlgorithm "$CertificateHashAlgorithm" `
        -KeyExportPolicy "$CertificateKeyExportPolicy" `
        -KeyLength $CertificateKeyLength `
        -KeySpec "$CertificateKeySpec" `
        -KeyUsage "$CertificateKeyUsage" `
        -KeyUsageProperty "$CertificateKeyUsageProperty" `
        -Type "$CertificateType" `
        -Subject "$CertificateSubjectOne";

    $CertificateTwo = New-SelfSignedCertificate `
        -CertStoreLocation "$CertificateStoreLocation" `
        -HashAlgorithm "$CertificateHashAlgorithm" `
        -KeyExportPolicy "$CertificateKeyExportPolicy" `
        -KeyLength $CertificateKeyLength `
        -KeySpec "$CertificateKeySpec" `
        -KeyUsage "$CertificateKeyUsage" `
        -KeyUsageProperty "$CertificateKeyUsageProperty" `
        -Type "$CertificateType" `
        -Subject "$CertificateSubjectTwo";

    $CertificateThree = New-SelfSignedCertificate `
        -CertStoreLocation "$CertificateStoreLocation" `
        -HashAlgorithm "$CertificateHashAlgorithm" `
        -KeyExportPolicy "$CertificateKeyExportPolicy" `
        -KeyLength $CertificateKeyLength `
        -KeySpec "$CertificateKeySpec" `
        -KeyUsage "$CertificateKeyUsage" `
        -KeyUsageProperty "$CertificateKeyUsageProperty" `
        -Type "$CertificateType" `
        -Subject "$CertificateSubjectThree";

    Export-Certificate -Cert $CertificateOne -FilePath "$FilePathForCertificateOne";
    Export-Certificate -Cert $CertificateTwo -FilePath "$FilePathForCertificateTwo";
    Export-Certificate -Cert $CertificateThree -FilePath "$FilePathForCertificateThree";
}

<#
.Synopsis
Initiates execution of the current script.
#>
Function EnterScript
{
    ComposeHeader "Self-signed X509 test certificate generation";

    If ($ContextIsInteractive)
    {
        ComposeNormal "The following process will generate self-signed X509 test certificates.";
        $UserInput = PromptUser -QuestionText "Would you like to continue?" -PromptText "[Y] Yes [N] No";

        If (($UserInput -eq $null) -or ($UserInput -eq [String]::Empty))
        {
            ComposeNormal "Exiting.";
            Exit;
        }

        Switch ($UserInput.Trim().ToUpper().Substring(0, 1))
        {
            "Y"
            {
                ComposeNormal "Continuing.";
                Break;
            }
            Default
            {
                ComposeNormal "Exiting.";
                Exit;
            }
        }
    }

    ComposeStart $("Generating self-signed X509 test certificates at {0:yyyy-MM-dd} {0:HH:mm:ss}." -f (Get-Date));
    PerformActions;
    ComposeFinish $("Finished generating self-signed X509 test certificates at {0:yyyy-MM-dd} {0:HH:mm:ss}." -f (Get-Date));
}

# Execution
EnterScript;