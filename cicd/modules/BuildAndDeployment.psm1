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
$DirectoryNameForExample = "example";
$DirectoryNameForSource = "src";
$DirectoryNameForTests = "test";

# File names
$FileNameForAppVeyorYamlConfiguration = "appveyor.yml";
$FileNameForCoverageReport = "Coverage.xml";
$FileNameForCodeSigningCertificate = "CodeSigningCertificate.pfx";
$FileNameForCoreModule = "Core.psm1";
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
$DirectoryPathForExample = Join-Path -Path "$DirectoryPathForProjectRoot" -ChildPath "$DirectoryNameForExample";
$DirectoryPathForSource = Join-Path -Path "$DirectoryPathForProjectRoot" -ChildPath "$DirectoryNameForSource";
$DirectoryPathForTests = Join-Path -Path "$DirectoryPathForProjectRoot" -ChildPath "$DirectoryNameForTests";

# File paths
$FilePathForAppVeyorYamlConfigurlation = Join-Path -Path "$DirectoryPathForProjectRoot" -ChildPath "$FileNameForAppVeyorYamlConfiguration";
$FilePathForCodeSigningCertificate = Join-Path -Path "$DirectoryPathForCicdAssets" -ChildPath "$FileNameForCodeSigningCertificate";
$FilePathForCoreModule = Join-Path -Path "$DirectoryPathForCicdModules" -ChildPath "$FileNameForCoreModule";
$FilePathForCoverageReport = Join-Path -Path "$DirectoryPathForTests" -ChildPath "$FileNameForCoverageReport";
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

# Configuration types
$ConfigurationTypeLocal = "Local";
$ConfigurationTypeProduction = "Production";

# Solution configurations
$SolutionConfigurationDebug = "Debug";
$SolutionConfigurationRelease = "Release";

# Namespaces
$ExampleServiceApplicationNamespace = "RapidField.SolidInstruments.Example.ServiceApplication";
$ExampleWebApplicationNamespace = "RapidField.SolidInstruments.Example.WebApplication";

# Environment variables
$BuildVersion = $env:APPVEYOR_BUILD_VERSION;
$CodecovToken = $env:CODECOV_TOKEN;
$CodeSigningCertificateKey = $env:RAPIDFIELD_CSCERTKEY;
$CodeSigningCertificatePassword = $env:RAPIDFIELD_CSCERTPWD;
$CommitAuthorEmail = $env:APPVEYOR_REPO_COMMIT_AUTHOR_EMAIL;
$CommitAuthorName = $env:APPVEYOR_REPO_COMMIT_AUTHOR;
$CommitId = $env:APPVEYOR_REPO_COMMIT;
$CommitMessage = $env:APPVEYOR_REPO_COMMIT_MESSAGE;
$CommitTimeStamp = $env:APPVEYOR_REPO_COMMIT_TIMESTAMP;
$NuGetApiKey = $env:RAPIDFIELD_NUGETAPIKEY;
$RepositoryName = $env:APPVEYOR_REPO_NAME;
$TagName = $env:APPVEYOR_REPO_TAG_NAME;

# Other configuration values
$TargetFrameworkForExampleServiceApplication = "netcoreapp2.1";

# Modules
Import-Module $FilePathForCoreModule -Force;

Function Build
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $SolutionConfiguration
    )

    $BuildVersion = GetBuildVersion;
    ComposeStart "Building $FilePathForSolutionFile using $SolutionConfiguration configuration.";
    ComposeNormal "Build version: $BuildVersion";
    ExecuteProcess -Path "$CommandNameForDotNetCli" -Arguments "build $FilePathForSolutionFile --configuration $SolutionConfiguration --no-restore --verbosity minimal /p:BuildVersion=$BuildVersion";
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

Function BuildDebug
{
    Build -SolutionConfiguration $SolutionConfigurationDebug;
}

Function BuildRelease
{
    Build -SolutionConfiguration $SolutionConfigurationRelease;
}

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

Function Clean
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $SolutionConfiguration
    )

    ComposeStart "Cleaning $FilePathForSolutionFile using $SolutionConfiguration configuration.";
    ExecuteProcess -Path "$CommandNameForDotNetCli" -Arguments "clean $FilePathForSolutionFile --configuration $SolutionConfiguration --verbosity minimal";
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

Function CleanDebug
{
    Clean -SolutionConfiguration $SolutionConfigurationDebug;
}

Function CleanRelease
{
    Clean -SolutionConfiguration $SolutionConfigurationRelease;
}

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

Function DecryptCodeSigningCertificate
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $Key
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
    .\secure-file -decrypt "$FilePathForEncryptedCodeSigningCertificate" -secret $Key;
    Pop-Location;
    Remove-Item "$DirectoryPathForCicdToolsAppVeyorTools" -Recurse -Confirm:$false -Force;
    Pop-Location;
    ComposeFinish "Finished decrypting the code signing certificate.";
}

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
    .\secure-file -encrypt "$FilePathForCodeSigningCertificate" -secret $Key;
    Pop-Location;
    Remove-Item -Path "$FilePathForCodeSigningCertificate" -Confirm:$false -Force;
    Remove-Item -Path "$DirectoryPathForCicdToolsAppVeyorTools" -Recurse -Confirm:$false -Force;
    Pop-Location;
    ComposeFinish "Finished encrypting the code signing certificate.";
}

Function GetAppVeyorConfiguration
{
    Import-Module "powershell-yaml" -Force;
    Return Get-Content -Path "$FilePathForAppVeyorYamlConfigurlation" | ConvertFrom-Yaml;
}

Function GetBuildVersion
{
    $AppVeyorConfiguration = GetAppVeyorConfiguration;
    Return $AppVeyorConfiguration.version.trimend(".{build}");
}

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

Function RestoreDependencies
{
    ComposeStart "Restoring dependencies for $FilePathForSolutionFile.";
    dotnet restore $FilePathForSolutionFile --verbosity minimal;

    If ($LASTEXITCODE -ne 0)
    {
        Throw "One or more dependencies could not be restored for $FilePathForSolutionFile.";
    }

    ComposeFinish "Finished restoring dependencies for $FilePathForSolutionFile.";
}

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
        DecryptCodeSigningCertificate -Key $CodeSigningCertificateKey;
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
            .\nuget.exe sign "$PackageFilePath" -CertificatePath "$FilePathForCodeSigningCertificate" -CertificatePassword $CodeSigningCertificatePassword -Timestamper "$CodeSigningCertificateTimestampServiceUri";
        }
    }

    Pop-Location;
    Remove-Item -Path "$FilePathForCodeSigningCertificate" -Confirm:$false -Force;
    ComposeFinish "Finished signing packages.";
}

Function StartExampleServiceApplication
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $SolutionConfiguration
    )

    ComposeStart "Starting the example service application using $SolutionConfiguration configuration.";
    $BinaryFilePath = Join-Path -Path "$DirectoryPathForExample" -ChildPath "$ExampleServiceApplicationNamespace\bin\$SolutionConfiguration\$TargetFrameworkForExampleServiceApplication\$ExampleServiceApplicationNamespace.dll";
    ComposeNormal "Using binary path: $BinaryFilePath";
    Start-Process -ArgumentList "$BinaryFilePath" -FilePath "dotnet" -WindowStyle Minimized;
    ComposeFinish "Finished starting the application.";
}

Function StartExampleServiceApplicationDebug
{
    StartExampleServiceApplication -SolutionConfiguration $SolutionConfigurationDebug;
}

Function StartExampleServiceApplicationRelease
{
    StartExampleServiceApplication -SolutionConfiguration $SolutionConfigurationRelease;
}

Function StartExampleWebApplication
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $SolutionConfiguration
    )

    ComposeStart "Starting the example web application using $SolutionConfiguration configuration.";
    $ProjectFilePath = Join-Path -Path "$DirectoryPathForExample" -ChildPath "$ExampleWebApplicationNamespace\$ExampleWebApplicationNamespace.csproj";
    ComposeNormal "Using project path: $ProjectFilePath";
    Start-Process -ArgumentList "run --project ""$ProjectFilePath"" --configuration $SolutionConfiguration" -FilePath "dotnet" -WindowStyle Minimized;
    ComposeFinish "Finished starting the application.";
}

Function StartExampleWebApplicationDebug
{
    StartExampleWebApplication -SolutionConfiguration $SolutionConfigurationDebug;
}

Function StartExampleWebApplicationRelease
{
    StartExampleWebApplication -SolutionConfiguration $SolutionConfigurationRelease;
}

Function StopAllApplications
{
    ComposeStart "Stopping all applications.";
    Stop-Process -Name "dotnet" -ErrorAction SilentlyContinue -Force;
    ComposeFinish "Finished stopping all applications.";
}

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
        OpenCover.Console.exe -excludebyattribute:*.Debugger* -log:Error -mergeoutput -oldstyle -output:"$FilePathForCoverageReport" -register:user -skipautoprops -target:"dotnet.exe" -targetargs:"test $TestDirectoryPath --configuration $SolutionConfiguration --no-build --no-restore --verbosity minimal";

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

Function TestDebug
{
    Test -SolutionConfiguration $SolutionConfigurationDebug;
}

Function TestRelease
{
    Test -SolutionConfiguration $SolutionConfigurationRelease;
}

Function VerifyBuild
{
    Push-Location "$DirectoryPathForProjectRoot";
    psake Verify;
    Pop-Location;
}

Function WriteBuildDetails
{
    WriteRepositoryName;
    WriteBuildVersion;
    WriteTagName;
    WriteCommitId;
    WriteCommitTimeStamp;
    WriteCommitMessage;
    WriteCommitAuthorName;
    WriteCommitAuthorEmail;
}

Function WriteBuildVersion
{
    If (($BuildVersion -eq $null) -or ($BuildVersion -eq ""))
    {
        Return;
    }

    ComposeNormal "Build version: $BuildVersion";
}

Function WriteCommitAuthorEmail
{
    If (($CommitAuthorEmail -eq $null) -or ($CommitAuthorEmail -eq ""))
    {
        Return;
    }

    ComposeNormal "Commit author email: $CommitAuthorEmail";
}

Function WriteCommitAuthorName
{
    If (($CommitAuthorName -eq $null) -or ($CommitAuthorName -eq "")) {
        Return;
    }

    ComposeNormal "Commit author name: $CommitAuthorName";
}

Function WriteCommitId
{
    If (($CommitId -eq $null) -or ($CommitId -eq ""))
    {
        Return;
    }

    ComposeNormal "Commit ID: $CommitId";
}

Function WriteCommitMessage
{
    If (($CommitMessage -eq $null) -or ($CommitMessage -eq ""))
    {
        Return;
    }

    ComposeNormal "Commit message: $CommitMessage";
}

Function WriteCommitTimeStamp
{
    If (($CommitTimeStamp -eq $null) -or ($CommitTimeStamp -eq ""))
    {
        Return;
    }

    ComposeNormal "Commit time stamp: $CommitTimeStamp";
}

Function WriteRepositoryName
{
    If (($RepositoryName -eq $null) -or ($RepositoryName -eq ""))
    {
        Return;
    }

    ComposeNormal "Repository name: $RepositoryName";
}

Function WriteTagName
{
    If (($TagName -eq $null) -or ($TagName -eq ""))
    {
        Return;
    }

    ComposeNormal "Tag name: $TagName";
}