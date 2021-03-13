# =================================================================================================================================
# Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
# =================================================================================================================================

<#
.Synopsis
This module exposes the build and deployment functions that are used by the CI/CD pipeline.
#>

# Directory names
$DirectoryNameForArtifacts = "artifacts";
$DirectoryNameForCicd = "cicd";
$DirectoryNameForCicdAssets = "assets";
$DirectoryNameForCicdModules = "modules";
$DirectoryNameForCicdScripts = "scripts";
$DirectoryNameForCicdTools = "tools";
$DirectoryNameForCicdToolsAppVeyorTools = "appveyor-tools";
$DirectoryNameForDocumentation = "doc";
$DirectoryNameForDocumentationObjects = "obj";
$DirectoryNameForDocumentationWebsite = "_DocumentationWebsite";
$DirectoryNameForDocumentationWebsiteManifestSnapshots = "manifest-snapshots";
$DirectoryNameForExample = "example";
$DirectoryNameForSource = "src";
$DirectoryNameForTests = "test";

# File names
$FileNameForAppVeyorYamlConfiguration = "appveyor.yml";
$FileNameForCoverageReport = "Coverage.xml";
$FileNameForCodeSigningCertificate = "CodeSigningCertificate.pfx";
$FileNameForCoreModule = "Core.psm1";
$FileNameForDocumentationWebsiteManifest = "manifest.json";
$FileNameForDotNetCli = "dotnet.exe";
$FileNameForNugetExe = "nuget.exe";
$FileNameForSolutionFile = "RapidField.SolidInstruments.sln";

# Directory paths
$DirectoryPathForProjectRoot = (Get-Item $PSScriptRoot).Parent.Parent.FullName;
$DirectoryPathForArtifacts = Join-Path -Path "$DirectoryPathForProjectRoot" -ChildPath "$DirectoryNameForArtifacts";
$DirectoryPathForCicd = Join-Path -Path "$DirectoryPathForProjectRoot" -ChildPath "$DirectoryNameForCicd";
$DirectoryPathForCicdAssets = Join-Path -Path "$DirectoryPathForCicd" -ChildPath "$DirectoryNameForCicdAssets";
$DirectoryPathForCicdModules = Join-Path -Path "$DirectoryPathForCicd" -ChildPath "$DirectoryNameForCicdModules";
$DirectoryPathForCicdScripts = Join-Path -Path "$DirectoryPathForCicd" -ChildPath "$DirectoryNameForCicdScripts";
$DirectoryPathForCicdTools = Join-Path -Path "$DirectoryPathForCicd" -ChildPath "$DirectoryNameForCicdTools";
$DirectoryPathForCicdToolsAppVeyorTools = Join-Path -Path "$DirectoryPathForCicdTools" -ChildPath "$DirectoryNameForCicdToolsAppVeyorTools";
$DirectoryPathForDocumentation = Join-Path -Path "$DirectoryPathForProjectRoot" -ChildPath "$DirectoryNameForDocumentation";
$DirectoryPathForDocumentationObjects = Join-Path -Path "$DirectoryPathForDocumentation" -ChildPath "$DirectoryNameForDocumentationObjects";
$DirectoryPathForDocumentationWebsite = Join-Path -Path "$DirectoryPathForDocumentation" -ChildPath "$DirectoryNameForDocumentationWebsite";
$DirectoryPathForDocumentationWebsiteArtifacts = Join-Path -Path "$DirectoryPathForArtifacts" -ChildPath "$DirectoryNameForDocumentationWebsite";
$DirectoryPathForDocumentationWebsiteManifestSnapshots = Join-Path -Path "$DirectoryPathForDocumentationWebsite" -ChildPath "$DirectoryNameForDocumentationWebsiteManifestSnapshots";
$DirectoryPathForExample = Join-Path -Path "$DirectoryPathForProjectRoot" -ChildPath "$DirectoryNameForExample";
$DirectoryPathForSource = Join-Path -Path "$DirectoryPathForProjectRoot" -ChildPath "$DirectoryNameForSource";
$DirectoryPathForTests = Join-Path -Path "$DirectoryPathForProjectRoot" -ChildPath "$DirectoryNameForTests";

# File paths
$FilePathForAppVeyorYamlConfigurlation = Join-Path -Path "$DirectoryPathForProjectRoot" -ChildPath "$FileNameForAppVeyorYamlConfiguration";
$FilePathForCodeSigningCertificate = Join-Path -Path "$DirectoryPathForCicdAssets" -ChildPath "$FileNameForCodeSigningCertificate";
$FilePathForCoreModule = Join-Path -Path "$DirectoryPathForCicdModules" -ChildPath "$FileNameForCoreModule";
$FilePathForCoverageReport = Join-Path -Path "$DirectoryPathForTests" -ChildPath "$FileNameForCoverageReport";
$FilePathForDocumentationWebsiteManifest = Join-Path -Path "$DirectoryPathForDocumentationWebsite" -ChildPath "$FileNameForDocumentationWebsiteManifest";
$FilePathForEncryptedCodeSigningCertificate = "$FilePathForCodeSigningCertificate.enc";
$FilePathForNuGetExe = Join-Path -Path "$DirectoryPathForCicdTools" -ChildPath "$FileNameForNugetExe";
$FilePathForSolutionFile = Join-Path -Path "$DirectoryPathForProjectRoot" -ChildPath "$FileNameForSolutionFile";

# Command names
$CommandNameForCodecov = "codecov";
$CommandNameForDotNetCli = "dotnet";
$CommandNameForHtmlMinifier = "html-minifier";
$CommandNameForNuGet = "nuget";
$CommandNameForOpenCover = "opencover.console.exe";

# Install script URIs
$InstallScriptUriForAppVeyorSecureFileUtility = "https://raw.githubusercontent.com/appveyor/secure-file/master/install.ps1";

# Other URIs
$CodeSigningCertificateTimestampServiceUri = "http://timestamp.digicert.com";
$NuGetOrgPackageSourceUri = "https://api.nuget.org/v3/index.json";
$ProductionDocumentationWebsiteFtpUri = "ftp://waws-prod-sn1-143.ftp.azurewebsites.windows.net/site/wwwroot";
$ProductionDocumentationWebsiteRootUri = "https://www.solidinstruments.com";
$ProductionDocumentationWebsiteManifestSnapshotsUri = "$ProductionDocumentationWebsiteRootUri/$DirectoryNameForDocumentationWebsiteManifestSnapshots";

# Configuration types
$ConfigurationTypeLocal = "Local";
$ConfigurationTypeProduction = "Production";

# Solution configurations
$SolutionConfigurationDebug = "Debug";
$SolutionConfigurationRelease = "Release";

# Namespaces
$ExampleAccessControlHttpApiApplicationNamespace = "RapidField.SolidInstruments.Example.Domain.AccessControl.HttpApi";
$ExampleAccessControlServiceApplicationNamespace = "RapidField.SolidInstruments.Example.Domain.AccessControl.Service";
$ExampleBeaconServiceApplicationNamespace = "RapidField.SolidInstruments.Example.BeaconService";
$ExampleIdentityServiceApplicationNamespace = "RapidField.SolidInstruments.Example.Domain.Identity.Service";

# Regular expressions
$ValidCommitMessageRegularExpressionPattern = "^(#[1-9][0-9]{0,4} )?[A-Z][A-Za-z0-9\,\.\!\;\:\'\""\@\#\$\%\^\&\*\-\+\=_\(\)\[\]\{\}\|\\\/\s]{8,144}$";
$ValidPullRequestTitleRegularExpressionPattern = "^(#[1-9][0-9]{0,4} )?[A-Z][A-Za-z0-9\,\.\!\;\:\'""\@\#\$\%\^\&\*\-\+\=_\(\)\[\]\{\}\|\\\/\s]{8,144}$";

# Environment variables
$BuildVersion = $env:APPVEYOR_BUILD_VERSION;
$CodecovToken = $env:CODECOV_TOKEN;
$CodeSigningCertificateKey = $env:RAPIDFIELD_CSCERTKEY;
$CodeSigningCertificateKeySalt = $env:RAPIDFIELD_CSCERTKEY_SALT;
$CodeSigningCertificatePassword = $env:RAPIDFIELD_CSCERTPWD;
$CommitAuthorEmail = $env:APPVEYOR_REPO_COMMIT_AUTHOR_EMAIL;
$CommitAuthorName = $env:APPVEYOR_REPO_COMMIT_AUTHOR;
$CommitId = $env:APPVEYOR_REPO_COMMIT;
$CommitMessage = $env:APPVEYOR_REPO_COMMIT_MESSAGE;
$CommitTimeStamp = $env:APPVEYOR_REPO_COMMIT_TIMESTAMP;
$DocumentationWebsiteFtpPassword = $env:DOCWEB_FTP_PASSWORD;
$DocumentationWebsiteFtpUserName = $env:DOCWEB_FTP_USERNAME;
$NuGetApiKey = $env:RAPIDFIELD_NUGETAPIKEY;
$PullRequestTitle = $env:APPVEYOR_PULL_REQUEST_TITLE;
$RepositoryName = $env:APPVEYOR_REPO_NAME;
$TagName = $env:APPVEYOR_REPO_TAG_NAME;

# Other configuration values
$TargetFrameworkForExampleHttpApiApplications = "net5.0";
$TargetFrameworkForExampleServiceApplications = "net5.0";

# Modules
Import-Module $FilePathForCoreModule -Force;

<#
.Synopsis
Compiles the current build.
#>
Function Build
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $SolutionConfiguration
    )

    $BuildVersionWithoutMetadata = GetBuildVersion;
    ComposeStart "Building $FilePathForSolutionFile using $SolutionConfiguration configuration.";
    ComposeNormal "Build version: $BuildVersionWithoutMetadata";
    ExecuteProcess -Path "$CommandNameForDotNetCli" -Arguments "build $FilePathForSolutionFile --configuration $SolutionConfiguration --nologo --no-restore --verbosity minimal /p:BuildVersion=$BuildVersionWithoutMetadata";
    $BuildArtifactsDirectoryPath = Join-Path -Path "$DirectoryPathForArtifacts" -ChildPath "$SolutionConfiguration";

    If (-not (Test-Path "$BuildArtifactsDirectoryPath"))
    {
        New-Item -ItemType Directory -Path "$BuildArtifactsDirectoryPath" -Force | Out-Null;
    }

    ComposeStart "Copying artifacts.";

    Get-ChildItem -Path "$DirectoryPathForSource" -Directory | ForEach-Object `
    {
        $ProjectOutputPath = Join-Path -Path $_.FullName -ChildPath "bin\$SolutionConfiguration";

        If (Test-Path "$ProjectOutputPath")
        {
            ComposeVerbose "Copying artifacts from $ProjectOutputPath.";
            Get-ChildItem -Path "$ProjectOutputPath" -File | Copy-Item -Container -Destination "$BuildArtifactsDirectoryPath" -Force | Out-Null;
        }
    }

    ComposeFinish "Finished copying artifacts.";
    BuildWebDocumentation -SolutionConfiguration $SolutionConfiguration;

    If (Test-Path "$DirectoryPathForDocumentationWebsite")
    {
        Get-Item -Path "$DirectoryPathForDocumentationWebsite" | Copy-Item -Destination "$DirectoryPathForArtifacts" -Force -Recurse | Out-Null;
    }

    CleanWebDocumentation -SolutionConfiguration $SolutionConfiguration;
    ComposeFinish "Finished building $FilePathForSolutionFile using $SolutionConfiguration configuration.";
}

<#
.Synopsis
Compiles the current build in debug mode.
#>
Function BuildDebug
{
    Build -SolutionConfiguration $SolutionConfigurationDebug;
}

<#
.Synopsis
Compiles the current build in release mode.
#>
Function BuildRelease
{
    Build -SolutionConfiguration $SolutionConfigurationRelease;
}

<#
.Synopsis
Compiles the documentation website.
#>
Function BuildWebDocumentation
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $SolutionConfiguration
    )

    If ($SolutionConfiguration -ne $SolutionConfigurationRelease)
    {
        Return;
    }

    ComposeStart "Compiling web documentation metadata.";
    Push-Location "$DirectoryPathForDocumentation";
    docfx metadata;
    ComposeFinish "Finished compiling web documentation metadata.";
    ComposeStart "Compiling documentation website.";
    docfx build --loglevel "Error";
    ComposeFinish "Finished compiling documentation website.";
    ComposeStart "Creating manifest snapshot for documentation website.";

    If (-not (Test-Path "$DirectoryPathForDocumentationWebsiteManifestSnapshots"))
    {
        New-Item -ItemType Directory -Path "$DirectoryPathForDocumentationWebsiteManifestSnapshots" -Force | Out-Null;
    }

    $ManifestHash = Get-FileHash -Algorithm MD5 -Path "$FilePathForDocumentationWebsiteManifest";
    $ManifestHashValue = $ManifestHash.Hash;
    $ManifestSnapshotIdentity = "$ManifestHashValue$CommitId".ToLower();
    ComposeVerbose "Using manifest snapshot identity: $ManifestSnapshotIdentity";
    $ManifestSnapshotFilePath = Join-Path -Path "$DirectoryPathForDocumentationWebsiteManifestSnapshots" -ChildPath "$ManifestSnapshotIdentity";
    $ManifestSnapshotFileContent = "$ManifestSnapshotIdentity $BuildVersion[$CommitTimeStamp] $CommitMessage";
    New-Item -ItemType File -Path "$ManifestSnapshotFilePath" -Force | Out-Null;
    Set-Content -Path "$ManifestSnapshotFilePath" -Value "$ManifestSnapshotFileContent" -Force | Out-Null;
    ComposeFinish "Finished creating manifest snapshot for documentation website.";
    ComposeStart "Minifying documentation website.";

    Get-ChildItem "$DirectoryPathForDocumentationWebsite" -Include *.html, *.css -Recurse | ForEach-Object `
    {
        $ThisFilePath = $_.FullName;
        ComposeVerbose "Minifying file: $ThisFilePath";
        html-minifier --collapse-whitespace --minify-css --minify-js --remove-comments "$ThisFilePath" -o "$ThisFilePath";
    }

    ComposeFinish "Finished minifying documentation website.";
    Pop-Location;
    ComposeFinish "Finished building web documentation.";
}

<#
.Synopsis
Cleans the artifacts for the current build.
#>
Function Clean
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $SolutionConfiguration
    )

    ComposeStart "Cleaning $FilePathForSolutionFile using $SolutionConfiguration configuration.";
    ExecuteProcess -Path "$CommandNameForDotNetCli" -Arguments "clean $FilePathForSolutionFile --configuration $SolutionConfiguration --nologo --verbosity minimal";
    ComposeStart "Destroying build artifacts.";

    Get-ChildItem -Path "$DirectoryPathForSource" -Directory | ForEach-Object `
    {
        $ProjectBinPath = Join-Path -Path $_.FullName -ChildPath "bin\$SolutionConfiguration";
        $ProjectObjPath = Join-Path -Path $_.FullName -ChildPath "obj";

        If (Test-Path "$ProjectBinPath")
        {
            ComposeVerbose "Removing $ProjectBinPath.";
            Remove-Item -Path "$ProjectBinPath" -Recurse -Confirm:$false -Force;
        }

        If (Test-Path "$ProjectObjPath")
        {
            ComposeVerbose "Removing $ProjectObjPath.";
            Remove-Item -Path "$ProjectObjPath" -Recurse -Confirm:$false -Force;
        }
    }

    $BuildArtifactsDirectoryPath = Join-Path -Path "$DirectoryPathForArtifacts" -ChildPath "$SolutionConfiguration";

    If (Test-Path "$BuildArtifactsDirectoryPath")
    {
        ComposeVerbose "Removing artifacts from $BuildArtifactsDirectoryPath.";
        Remove-Item -Path "$BuildArtifactsDirectoryPath" -Recurse -Confirm:$false -Force;
    }

    CleanWebDocumentation -SolutionConfiguration $SolutionConfiguration;
    ComposeFinish "Finished cleaning.";
}

<#
.Synopsis
Cleans the debug mode artifacts for the current build.
#>
Function CleanDebug
{
    Clean -SolutionConfiguration $SolutionConfigurationDebug;
}

<#
.Synopsis
Cleans the release mode artifacts for the current build.
#>
Function CleanRelease
{
    Clean -SolutionConfiguration $SolutionConfigurationRelease;
}

<#
.Synopsis
Cleans the documentation website artifacts for the current build.
#>
Function CleanWebDocumentation
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $SolutionConfiguration
    )

    If ($SolutionConfiguration -ne $SolutionConfigurationRelease)
    {
        Return;
    }

    ComposeStart "Cleaning documentation website.";

    If (Test-Path "$DirectoryPathForDocumentationObjects")
    {
        ComposeNormal "Removing documentation website artifacts from $DirectoryPathForDocumentationObjects.";
        Remove-Item -Path "$DirectoryPathForDocumentationObjects" -Recurse -Confirm:$false -Force;
    }

    If (Test-Path "$DirectoryPathForDocumentationWebsite")
    {
        ComposeNormal "Removing documentation website artifacts from $DirectoryPathForDocumentationWebsite.";
        Remove-Item -Path "$DirectoryPathForDocumentationWebsite" -Recurse -Confirm:$false -Force;
    }

    ComposeFinish "Finished cleaning documentation website.";
}

<#
.Synopsis
Decrypts the RapidField code signing certificate.
#>
Function DecryptCodeSigningCertificate
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $Key,
        [Parameter(Mandatory = $true, Position = 1)]
        [String] $Salt
    )

    If (-not (Test-Path "$FilePathForEncryptedCodeSigningCertificate"))
    {
        ComposeWarning "The encrypted code signing certificate is not available at path $FilePathForEncryptedCodeSigningCertificate.";
        Return;
    }

    If (-not (Test-Path "$DirectoryPathForCicdTools"))
    {
        New-Item -ItemType Directory -Path "$DirectoryPathForCicdTools" -Force | Out-Null;
    }

    ComposeStart "Decrypting the code signing certificate.";
    Push-Location "$DirectoryPathForCicdTools";
    iex ((New-Object Net.WebClient).DownloadString($InstallScriptUriForAppVeyorSecureFileUtility));
    Push-Location "$DirectoryPathForCicdToolsAppVeyorTools";
    .\secure-file -decrypt "$FilePathForEncryptedCodeSigningCertificate" -secret "$Key" -salt "$Salt";
    Pop-Location;
    Remove-Item "$DirectoryPathForCicdToolsAppVeyorTools" -Recurse -Confirm:$false -Force;
    Pop-Location;
    ComposeFinish "Finished decrypting the code signing certificate.";
}

<#
.Synopsis
Encrypts the RapidField code signing certificate.
#>
Function EncryptCodeSigningCertificate
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $Key
    )

    If (-not (Test-Path "$FilePathForCodeSigningCertificate"))
    {
        ComposeWarning "The code signing certificate is not available at path $FilePathForCodeSigningCertificate.";
        Return;
    }

    If (-not (Test-Path "$DirectoryPathForCicdTools"))
    {
        New-Item -ItemType Directory -Path "$DirectoryPathForCicdTools" -Force | Out-Null;
    }

    ComposeStart "Encrypting the code signing certificate.";
    Push-Location "$DirectoryPathForCicdTools";
    iex ((New-Object Net.WebClient).DownloadString($InstallScriptUriForAppVeyorSecureFileUtility));
    Push-Location "$DirectoryPathForCicdToolsAppVeyorTools";
    .\secure-file -encrypt "$FilePathForCodeSigningCertificate" -secret "$Key";
    Pop-Location;
    Remove-Item -Path "$FilePathForCodeSigningCertificate" -Confirm:$false -Force;
    Remove-Item -Path "$DirectoryPathForCicdToolsAppVeyorTools" -Recurse -Confirm:$false -Force;
    Pop-Location;
    ComposeFinish "Finished encrypting the code signing certificate.";
}

<#
.Synopsis
Gets the contents of appveyor.yml.
#>
Function GetAppVeyorConfiguration
{
    Import-Module "powershell-yaml" -Force;
    Return Get-Content -Path "$FilePathForAppVeyorYamlConfigurlation" | ConvertFrom-Yaml;
}

<#
.Synopsis
Extracts the build version from appveyor.yml.
#>
Function GetBuildVersion
{
    $AppVeyorConfiguration = GetAppVeyorConfiguration;
    Return $AppVeyorConfiguration.version.trimend("+{build}");
}

<#
.Synopsis
Publishes the NuGet packages for the current build.
#>
Function PublishPackages
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $SolutionConfiguration
    )

    If ($SolutionConfiguration -ne $SolutionConfigurationRelease)
    {
        Return;
    }

    If (($NuGetApiKey -eq $null) -or ($NuGetApiKey -eq ""))
    {
        ComposeWarning "Packages will not be published. The NuGet API key is unavailable.";
        Return;
    }

    $BuildArtifactsDirectoryPath = Join-Path -Path "$DirectoryPathForArtifacts" -ChildPath "$SolutionConfiguration";

    If (-not (Test-Path "$BuildArtifactsDirectoryPath"))
    {
        ComposeWarning "No packages are available to publish. The path does not exist: $BuildArtifactsDirectoryPath.";
        Return;
    }

    ComposeStart "Publishing packages in the directory: $BuildArtifactsDirectoryPath.";
    Push-Location "$DirectoryPathForCicdTools";

    Get-ChildItem -Path "$BuildArtifactsDirectoryPath" -File | ForEach-Object `
    {
        $PackageFilePath = $_.FullName;

        If ($PackageFilePath -like "*.nupkg")
        {
            ComposeNormal "Publishing package $PackageFilePath.";
            .\nuget.exe push $PackageFilePath -ApiKey "$NuGetApiKey" -Source "$NuGetOrgPackageSourceUri" -SkipDuplicate;
        }
    }

    Pop-Location;
    ComposeFinish "Finished publishing packages.";
}

<#
.Synopsis
Publishes the documentation website for the current build.
#>
Function PublishWebDocumentation
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $SolutionConfiguration
    )

    If ($SolutionConfiguration -ne $SolutionConfigurationRelease)
    {
        Return;
    }

    If (($DocumentationWebsiteFtpUserName -eq $null) -or ($DocumentationWebsiteFtpUserName -eq ""))
    {
        ComposeWarning "The documentation website artifacts will not be published. The FTP account username is unavailable.";
        Return;
    }

    If (($DocumentationWebsiteFtpPassword -eq $null) -or ($DocumentationWebsiteFtpPassword -eq ""))
    {
        ComposeWarning "The documentation website artifacts will not be published. The FTP account password is unavailable.";
        Return;
    }

    If (-not (Test-Path "$DirectoryPathForDocumentationWebsiteArtifacts"))
    {
        ComposeWarning "No documentation website artifacts are available to publish. The path does not exist: $DirectoryPathForDocumentationWebsiteArtifacts.";
        Return;
    }

    ComposeStart "Publishing documentation website artifacts in the directory: $DirectoryPathForDocumentationWebsiteArtifacts.";
    $FtpCredentials = New-Object System.Net.NetworkCredential($DocumentationWebsiteFtpUserName, $DocumentationWebsiteFtpPassword);
    TransferFiles -LocalSourcePath "$DirectoryPathForDocumentationWebsiteArtifacts" -DestinationFtpPath "$ProductionDocumentationWebsiteFtpUri" -Credentials $FtpCredentials;
    ComposeFinish "Finished publishing web documentation.";
}

<#
.Synopsis
Restores dependencies for the current build.
#>
Function RestoreDependencies
{
    ComposeStart "Restoring dependencies for $FilePathForSolutionFile.";
    dotnet restore $FilePathForSolutionFile --nologo --verbosity minimal;

    If ($LASTEXITCODE -ne 0)
    {
        Throw "One or more dependencies could not be restored for $FilePathForSolutionFile.";
    }

    ComposeFinish "Finished restoring dependencies for $FilePathForSolutionFile.";
}

<#
.Synopsis
Signs the NuGet packages for the current build using the RapidField code signing certificate.
#>
Function SignPackages
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $SolutionConfiguration
    )

    If ($SolutionConfiguration -ne $SolutionConfigurationRelease)
    {
        Return;
    }

    If (($CodeSigningCertificateKey -eq $null) -or ($CodeSigningCertificateKey -eq ""))
    {
        ComposeWarning "Packages will not be signed. The code signing certificate key is unavailable.";
        Return;
    }

    If (($CodeSigningCertificateKeySalt -eq $null) -or ($CodeSigningCertificateKeySalt -eq ""))
    {
        ComposeWarning "Packages will not be signed. The code signing certificate key salt is unavailable.";
        Return;
    }

    If (($CodeSigningCertificatePassword -eq $null) -or ($CodeSigningCertificatePassword -eq ""))
    {
        ComposeWarning "Packages will not be signed. The code signing certificate password is unavailable.";
        Return;
    }

    $BuildArtifactsDirectoryPath = Join-Path -Path "$DirectoryPathForArtifacts" -ChildPath "$SolutionConfiguration";

    If (-not (Test-Path "$BuildArtifactsDirectoryPath"))
    {
        ComposeWarning "No packages are available to sign. The path does not exist: $BuildArtifactsDirectoryPath.";
        Return;
    }

    If (-not (Test-Path "$FilePathForCodeSigningCertificate"))
    {
        DecryptCodeSigningCertificate -Key "$CodeSigningCertificateKey" -Salt "$CodeSigningCertificateKeySalt";
    }

    If (-not (Test-Path "$FilePathForCodeSigningCertificate"))
    {
        ComposeWarning "Packages will not be signed. The code signing certificate is not available at path $FilePathForCodeSigningCertificate.";
        Return;
    }

    ComposeStart "Signing packages in the directory: $BuildArtifactsDirectoryPath.";
    Push-Location "$DirectoryPathForCicdTools";

    Get-ChildItem -Path "$BuildArtifactsDirectoryPath" -File | ForEach-Object `
    {
        $PackageFilePath = $_.FullName;

        If ($PackageFilePath -like "*.nupkg")
        {
            ComposeStart "Signing package $PackageFilePath.";
            .\nuget.exe sign "$PackageFilePath" -CertificatePath "$FilePathForCodeSigningCertificate" -Timestamper "$CodeSigningCertificateTimestampServiceUri" -CertificatePassword "$CodeSigningCertificatePassword";
        }
    }

    Pop-Location;
    Remove-Item -Path "$FilePathForCodeSigningCertificate" -Confirm:$false -Force;
    ComposeFinish "Finished signing packages.";
}

<#
.Synopsis
Starts the example AccessControl domain HTTP API application.
#>
Function StartExampleAccessControlHttpApiApplication
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $SolutionConfiguration
    )

    ComposeStart "Starting the example AccessControl domain HTTP API application using $SolutionConfiguration configuration.";
    $BinaryDirectoryPath = Join-Path -Path "$DirectoryPathForExample" -ChildPath "$ExampleAccessControlHttpApiApplicationNamespace\bin\$SolutionConfiguration\$TargetFrameworkForExampleHttpApiApplications";
    $BinaryFileName = "$ExampleAccessControlHttpApiApplicationNamespace.dll";
    $BinaryFilePath = Join-Path -Path "$BinaryDirectoryPath" -ChildPath "$BinaryFileName";
    ComposeNormal "Using binary path: $BinaryFilePath";
    Push-Location "$BinaryDirectoryPath";
    Start-Process -ArgumentList "$BinaryFileName --nologo" -FilePath "$CommandNameForDotNetCli" -WindowStyle Minimized;
    Pop-Location;
    ComposeFinish "Finished starting the application.";
}

<#
.Synopsis
Starts the example AccessControl domain HTTP API application in debug mode.
#>
Function StartExampleAccessControlHttpApiApplicationDebug
{
    StartExampleAccessControlHttpApiApplication -SolutionConfiguration $SolutionConfigurationDebug;
}

<#
.Synopsis
Starts the example AccessControl domain HTTP API application in release mode.
#>
Function StartExampleAccessControlHttpApiApplicationRelease
{
    StartExampleAccessControlHttpApiApplication -SolutionConfiguration $SolutionConfigurationRelease;
}

<#
.Synopsis
Starts the example AccessControl domain service application.
#>
Function StartExampleAccessControlServiceApplication
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $SolutionConfiguration
    )

    ComposeStart "Starting the example AccessControl domain service application using $SolutionConfiguration configuration.";
    $BinaryDirectoryPath = Join-Path -Path "$DirectoryPathForExample" -ChildPath "$ExampleAccessControlServiceApplicationNamespace\bin\$SolutionConfiguration\$TargetFrameworkForExampleServiceApplications";
    $BinaryFileName = "$ExampleAccessControlServiceApplicationNamespace.dll";
    $BinaryFilePath = Join-Path -Path "$BinaryDirectoryPath" -ChildPath "$BinaryFileName";
    ComposeNormal "Using binary path: $BinaryFilePath";
    Push-Location "$BinaryDirectoryPath";
    Start-Process -ArgumentList "$BinaryFileName --nologo" -FilePath "$CommandNameForDotNetCli" -WindowStyle Minimized;
    Pop-Location;
    ComposeFinish "Finished starting the application.";
}

<#
.Synopsis
Starts the example AccessControl domain service application in debug mode.
#>
Function StartExampleAccessControlServiceApplicationDebug
{
    StartExampleAccessControlServiceApplication -SolutionConfiguration $SolutionConfigurationDebug;
}

<#
.Synopsis
Starts the example AccessControl domain service application in release mode.
#>
Function StartExampleAccessControlServiceApplicationRelease
{
    StartExampleAccessControlServiceApplication -SolutionConfiguration $SolutionConfigurationRelease;
}

<#
.Synopsis
Starts the example Beacon service application.
#>
Function StartExampleBeaconServiceApplication
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $SolutionConfiguration
    )

    ComposeStart "Starting the example beacon service application using $SolutionConfiguration configuration.";
    $BinaryDirectoryPath = Join-Path -Path "$DirectoryPathForExample" -ChildPath "$ExampleBeaconServiceApplicationNamespace\bin\$SolutionConfiguration\$TargetFrameworkForExampleServiceApplications";
    $BinaryFileName = "$ExampleBeaconServiceApplicationNamespace.dll";
    $BinaryFilePath = Join-Path -Path "$BinaryDirectoryPath" -ChildPath "$BinaryFileName";
    ComposeNormal "Using binary path: $BinaryFilePath";
    Push-Location "$BinaryDirectoryPath";
    Start-Process -ArgumentList "$BinaryFileName --nologo" -FilePath "$CommandNameForDotNetCli" -WindowStyle Minimized;
    Pop-Location;
    ComposeFinish "Finished starting the application.";
}

<#
.Synopsis
Starts the example Beacon service application in debug mode.
#>
Function StartExampleBeaconServiceApplicationDebug
{
    StartExampleBeaconServiceApplication -SolutionConfiguration $SolutionConfigurationDebug;
}

<#
.Synopsis
Starts the example Beacon service application in release mode.
#>
Function StartExampleBeaconServiceApplicationRelease
{
    StartExampleBeaconServiceApplication -SolutionConfiguration $SolutionConfigurationRelease;
}

<#
.Synopsis
Starts the example Identity domain service application.
#>
Function StartExampleIdentityServiceApplication
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $SolutionConfiguration
    )

    ComposeStart "Starting the example Identity domain service application using $SolutionConfiguration configuration.";
    $BinaryDirectoryPath = Join-Path -Path "$DirectoryPathForExample" -ChildPath "$ExampleIdentityServiceApplicationNamespace\bin\$SolutionConfiguration\$TargetFrameworkForExampleServiceApplications";
    $BinaryFileName = "$ExampleIdentityServiceApplicationNamespace.dll";
    $BinaryFilePath = Join-Path -Path "$BinaryDirectoryPath" -ChildPath "$BinaryFileName";
    ComposeNormal "Using binary path: $BinaryFilePath";
    Push-Location "$BinaryDirectoryPath";
    Start-Process -ArgumentList "$BinaryFileName --nologo" -FilePath "$CommandNameForDotNetCli" -WindowStyle Minimized;
    Pop-Location;
    ComposeFinish "Finished starting the application.";
}

<#
.Synopsis
Starts the example Identity domain service application in debug mode.
#>
Function StartExampleIdentityServiceApplicationDebug
{
    StartExampleIdentityServiceApplication -SolutionConfiguration $SolutionConfigurationDebug;
}

<#
.Synopsis
Starts the example Identity domain service application in release mode.
#>
Function StartExampleIdentityServiceApplicationRelease
{
    StartExampleIdentityServiceApplication -SolutionConfiguration $SolutionConfigurationRelease;
}

<#
.Synopsis
Stops all running .NET applications.
#>
Function StopAllApplications
{
    ComposeStart "Stopping all applications.";
    Stop-Process -Name "$CommandNameForDotNetCli" -ErrorAction SilentlyContinue -Force;
    ComposeFinish "Finished stopping all applications.";
}

<#
.Synopsis
Executes the test suite against the current build.
#>
Function Test
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $SolutionConfiguration
    )

    Get-ChildItem -Path "$DirectoryPathForTests" -Directory | ForEach-Object `
    {
        $TestDirectoryPath = $_.FullName;
        ComposeStart "Running tests for $TestDirectoryPath using $SolutionConfiguration configuration.";
        OpenCover.Console.exe -excludebyattribute:*.Debugger* -log:Error -mergeoutput -oldstyle -output:"$FilePathForCoverageReport" -register:user -returntargetcode -skipautoprops -target:"$FileNameForDotNetCli" -targetargs:"test $TestDirectoryPath --configuration $SolutionConfiguration --no-build --nologo --no-restore --verbosity minimal";

        If ($LASTEXITCODE -ne 0)
        {
            Throw "One or more tests failed for $TestDirectoryPath using $SolutionConfiguration configuration.";
        }

        If (($CodecovToken -eq $null) -or ($CodecovToken -eq ""))
        {
            ComposeWarning "A code coverage report will not be published. The Codecov token is unavailable.";
        }
        Else
        {
            codecov -f "$FilePathForCoverageReport" -t $CodecovToken;
        }

        ComposeFinish "Finished running tests for $TestDirectoryPath using $SolutionConfiguration configuration.";
    }
}

<#
.Synopsis
Executes the test suite against the current build in debug mode.
#>
Function TestDebug
{
    Test -SolutionConfiguration $SolutionConfigurationDebug;
}

<#
.Synopsis
Executes the test suite against the current build in release mode.
#>
Function TestRelease
{
    Test -SolutionConfiguration $SolutionConfigurationRelease;
}

<#
.Synopsis
Transfers the specified source file to the specified destination path using FTPS.
#>
Function TransferFile
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $LocalSourcePath,
        [Parameter(Mandatory = $true, Position = 1)]
        [String] $DestinationFtpPath,
        [Parameter(Mandatory = $true, Position = 2)]
        [System.Net.NetworkCredential] $Credentials
    )

    ComposeVerbose "Transferring file ""$LocalSourcePath"".";
    $FileContent = [System.IO.File]::ReadAllBytes("$LocalSourcePath");
    $FileContentLengthInBytes = $FileContent.Length;
    $FtpRequest = [System.Net.FtpWebRequest]::Create("$DestinationFtpPath");
    $FtpRequest = [System.Net.FtpWebRequest]$FtpRequest;
    $FtpRequest.ContentLength = $FileContentLengthInBytes;
    $FtpRequest.Credentials = $Credentials;
    $FtpRequest.Method = [System.Net.WebRequestMethods+Ftp]::UploadFile;
    $FtpRequest.UseBinary = $true;
    $FtpRequest.UsePassive = $true;
    $RequestStream = $FtpRequest.GetRequestStream();

    Try
    {
        $RequestStream.Write($FileContent, 0, $FileContentLengthInBytes);
    }
    Catch
    {
        ComposeError "Failed to transfer file ""$LocalSourcePath"".";
        Throw;
    }
    Finally
    {
        $RequestStream.Close();
        $RequestStream.Dispose();
    }
}

<#
.Synopsis
Transfers all files and subdirectories in the specified source path to the specified destination path using FTPS.
#>
Function TransferFiles
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $LocalSourcePath,
        [Parameter(Mandatory = $true, Position = 1)]
        [String] $DestinationFtpPath,
        [Parameter(Mandatory = $true, Position = 2)]
        [System.Net.NetworkCredential] $Credentials
    )

    ComposeNormal "Transferring files in source directory ""$LocalSourcePath"".";
    ComposeVerbose "Destination directory: ""$DestinationFtpPath"".";

    Get-ChildItem -Path "$LocalSourcePath" -File | ForEach-Object `
    {
        $SourceFilePath = $_.FullName;
        $SourceFileName = $_.Name;
        $DestinationFilePath = "$DestinationFtpPath/$SourceFileName";
        TransferFile -LocalSourcePath "$SourceFilePath" -DestinationFtpPath "$DestinationFilePath" -Credentials $Credentials;
    }

    Get-ChildItem -Path "$LocalSourcePath" -Directory | ForEach-Object `
    {
        $SourceDirectoryPath = $_.FullName;
        $SourceDirectoryName = $_.Name;
        $DestinationDirectoryPath = "$DestinationFtpPath/$SourceDirectoryName";
        TransferFiles -LocalSourcePath "$SourceDirectoryPath" -DestinationFtpPath "$DestinationDirectoryPath" -Credentials $Credentials;
    }
}

<#
.Synopsis
Compiles the current build and executes the test suite against it.
#>
Function VerifyBuild
{
    Push-Location "$DirectoryPathForProjectRoot";
    psake Verify -nologo;
    Pop-Location;
}

<#
.Synopsis
Validates the commit message, if any, against a regular expression pattern.
#>
Function ValidateCommitMessageFormat
{
    If (($CommitMessage -eq $null) -or ($CommitMessage -eq ""))
    {
        ComposeVerbose "Commit message validation will not be performed. No commit message is available.";
        Return;
    }
    ElseIf (($CommitMessage -match $ValidCommitMessageRegularExpressionPattern))
    {
        ComposeVerbose "The commit message for this build is properly formatted.";
        Return;
    }

    $ErrorMessage = "The commit message for this build is improperly formatted.";
    ComposeError "$ErrorMessage";
    ComposeNormal "Commit message:";
    ComposeVerbose "$CommitMessage"
    ComposeNormal "Expected format:";
    ComposeVerbose "$ValidCommitMessageRegularExpressionPattern";
    Throw "$ErrorMessage";
}

<#
.Synopsis
Validates the pull request title, if any, against a regular expression pattern.
#>
Function ValidatePullRequestTitleFormat
{
    If (($PullRequestTitle -eq $null) -or ($PullRequestTitle -eq ""))
    {
        ComposeVerbose "Pull request title validation will not be performed. No pull request title is available.";
        Return;
    }
    ElseIf (($PullRequestTitle -match $ValidPullRequestTitleRegularExpressionPattern))
    {
        ComposeVerbose "The pull request title for this build is properly formatted.";
        Return;
    }

    $ErrorMessage = "The pull request title for this build is improperly formatted.";
    ComposeError "$ErrorMessage";
    ComposeNormal "Pull request title:";
    ComposeVerbose "$PullRequestTitle"
    ComposeNormal "Expected format:";
    ComposeVerbose "$ValidPullRequestTitleRegularExpressionPattern";
    Throw "$ErrorMessage";
}

<#
.Synopsis
Writes various details about the build.
#>
Function WriteBuildDetails
{
    WriteRepositoryName;
    WriteBuildVersion;
    WriteTagName;
    WritePullRequestTitle;
    WriteCommitId;
    WriteCommitTimeStamp;
    WriteCommitMessage;
    WriteCommitAuthorName;
    WriteCommitAuthorEmail;
}

<#
.Synopsis
Writes the build version.
#>
Function WriteBuildVersion
{
    If (($BuildVersion -eq $null) -or ($BuildVersion -eq ""))
    {
        Return;
    }

    ComposeNormal "Build version: $BuildVersion";
}

<#
.Synopsis
Writes the commit author's email address for the build.
#>
Function WriteCommitAuthorEmail
{
    If (($CommitAuthorEmail -eq $null) -or ($CommitAuthorEmail -eq ""))
    {
        Return;
    }

    ComposeNormal "Commit author email: $CommitAuthorEmail";
}

<#
.Synopsis
Writes the commit author's name for the build.
#>
Function WriteCommitAuthorName
{
    If (($CommitAuthorName -eq $null) -or ($CommitAuthorName -eq "")) {
        Return;
    }

    ComposeNormal "Commit author name: $CommitAuthorName";
}

<#
.Synopsis
Writes the commit ID for the build.
#>
Function WriteCommitId
{
    If (($CommitId -eq $null) -or ($CommitId -eq ""))
    {
        Return;
    }

    ComposeNormal "Commit ID: $CommitId";
}

<#
.Synopsis
Writes the commit message for the build.
#>
Function WriteCommitMessage
{
    If (($CommitMessage -eq $null) -or ($CommitMessage -eq ""))
    {
        Return;
    }

    ComposeNormal "Commit message: $CommitMessage";
}

<#
.Synopsis
Writes the commit time stamp for the build.
#>
Function WriteCommitTimeStamp
{
    If (($CommitTimeStamp -eq $null) -or ($CommitTimeStamp -eq ""))
    {
        Return;
    }

    ComposeNormal "Commit time stamp: $CommitTimeStamp";
}

<#
.Synopsis
Writes the pull request title for the build.
#>
Function WritePullRequestTitle
{
    If (($PullRequestTitle -eq $null) -or ($PullRequestTitle -eq ""))
    {
        Return;
    }

    ComposeNormal "Pull request title: $PullRequestTitle";
}

<#
.Synopsis
Writes the repository name for the build.
#>
Function WriteRepositoryName
{
    If (($RepositoryName -eq $null) -or ($RepositoryName -eq ""))
    {
        Return;
    }

    ComposeNormal "Repository name: $RepositoryName";
}

<#
.Synopsis
Writes the tag name for the build.
#>
Function WriteTagName
{
    If (($TagName -eq $null) -or ($TagName -eq ""))
    {
        Return;
    }

    ComposeNormal "Tag name: $TagName";
}