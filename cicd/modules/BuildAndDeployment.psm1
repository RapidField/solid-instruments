# =================================================================================================================================
# Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
# =================================================================================================================================

# Module configuration
# =================================================================================================================================

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
$FileNameForNugetExe = "nuget.exe";
$FileNameForCodeSigningCertificate = "CodeSigningCertificate.pfx";
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
$FilePathForNuGetExe = Join-Path -Path "$DirectoryPathForCicdTools" -ChildPath "$FileNameForNugetExe";
$FilePathForCodeSigningCertificate = Join-Path -Path "$DirectoryPathForCicdAssets" -ChildPath "$FileNameForCodeSigningCertificate";
$FilePathForEncryptedCodeSigningCertificate = "$FilePathForCodeSigningCertificate.enc";
$FilePathForSolutionFile = Join-Path -Path "$DirectoryPathForProjectRoot" -ChildPath "$FileNameForSolutionFile";

# Install script URIs
$InstallScriptUriForAppVeyorSecureFileUtility = "https://raw.githubusercontent.com/appveyor/secure-file/master/install.ps1";

# Other URIs
$CodeSigningCertificateTimestampServiceUri = "http://timestamp.digicert.com";
$NuGetOrgPackageSourceUri = "https://api.nuget.org/v3/index.json"

# Configuration types
$ConfigurationTypeLocal = "Local";
$ConfigurationTypeProduction = "Production";

# Solution configurations
$SolutionConfigurationDebug = "Debug";
$SolutionConfigurationRelease = "Release";

# Namespaces
$ExampleServiceApplicationNamespace = "RapidField.SolidInstruments.Example.ServiceApplication";
$ExampleWebApplicationNamespace = "RapidField.SolidInstruments.Example.WebApplication";

# Other configuration values
$TargetFrameworkForExampleServiceApplication = "netcoreapp2.1";

# Build
# =================================================================================================================================

function Build {
    Param (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $SolutionConfiguration
    )

    $BuildVersion = GetBuildVersion;
    Write-Host -ForegroundColor DarkCyan "Building $FilePathForSolutionFile using $SolutionConfiguration configuration.";
    Write-Host -ForegroundColor DarkCyan "Build version: $BuildVersion";
    dotnet build $FilePathForSolutionFile --configuration $SolutionConfiguration --no-restore --verbosity minimal /p:BuildVersion=$BuildVersion

    If ($LASTEXITCODE -ne 0) {
        Throw "The build failed for $FilePathForSolutionFile using $SolutionConfiguration configuration.";
    }

    $BuildArtifactsDirectoryPath = Join-Path -Path "$DirectoryPathForArtifacts" -ChildPath "$SolutionConfiguration";

    If (-not (Test-Path "$BuildArtifactsDirectoryPath")) {
        New-Item -ItemType Directory -Path "$BuildArtifactsDirectoryPath" -Force | Out-Null;
    }

    Get-ChildItem -Path "$DirectoryPathForSource" -Directory | ForEach-Object {
        $ProjectOutputPath = Join-Path -Path $_.FullName -ChildPath "bin\$SolutionConfiguration";

        If (Test-Path "$ProjectOutputPath") {
            Write-Host -ForegroundColor DarkCyan "Copying artifacts from $ProjectOutputPath.";
            Get-ChildItem -Path "$ProjectOutputPath" -File | Copy-Item -Container -Destination "$BuildArtifactsDirectoryPath" -Force | Out-Null;
        }
    }

    BuildWebDocumentation -SolutionConfiguration $SolutionConfiguration;

    If (Test-Path "$DirectoryPathForDocumentationWebsite") {
        Get-Item -Path "$DirectoryPathForDocumentationWebsite" | Copy-Item -Destination "$DirectoryPathForArtifacts" -Force -Recurse | Out-Null;
    }

    CleanWebDocumentation -SolutionConfiguration $SolutionConfiguration;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished building. <<<`n";
}

function BuildDebug {
    Build -SolutionConfiguration $SolutionConfigurationDebug;
}

function BuildRelease {
    Build -SolutionConfiguration $SolutionConfigurationRelease;
}

function BuildWebDocumentation {
    Param (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $SolutionConfiguration
    )

    If ($SolutionConfiguration -ne $SolutionConfigurationRelease) {
        return;
    }

    Write-Host -ForegroundColor DarkCyan "`nCompiling web documentation metadata.";
    Push-Location "$DirectoryPathForDocumentation"
    docfx metadata

    Write-Host -ForegroundColor DarkCyan "`nCompiling documentation website.";
    docfx build --loglevel "Error"

    Write-Host -ForegroundColor DarkCyan "`nMinifying documentation website.";

    Get-ChildItem "$DirectoryPathForDocumentationWebsite" -Include *.html, *.css -Recurse | ForEach-Object {
        $ThisFilePath = $_.FullName;
        Write-Host -ForegroundColor DarkCyan "Minifying file: $ThisFilePath";
        html-minifier --collapse-whitespace --minify-css --minify-js --remove-comments "$ThisFilePath" -o "$ThisFilePath"
    }

    Pop-Location
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished building web documentation. <<<`n";
}

# Clean
# =================================================================================================================================

function Clean {
    Param (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $SolutionConfiguration
    )

    Write-Host -ForegroundColor DarkCyan "Cleaning $FilePathForSolutionFile using $SolutionConfiguration configuration.";
    dotnet clean $FilePathForSolutionFile --configuration $SolutionConfiguration --verbosity minimal

    If ($LASTEXITCODE -ne 0) {
        Throw "Cleaning failed for $FilePathForSolutionFile using $SolutionConfiguration configuration.";
    }

    Get-ChildItem -Path "$DirectoryPathForSource" -Directory | ForEach-Object {
        $ProjectBinPath = Join-Path -Path $_.FullName -ChildPath "bin\$SolutionConfiguration";
        $ProjectObjPath = Join-Path -Path $_.FullName -ChildPath "obj";

        If (Test-Path "$ProjectBinPath") {
            Write-Host -ForegroundColor DarkCyan "Removing $ProjectBinPath.";
            Remove-Item -Path "$ProjectBinPath" -Recurse -Confirm:$false -Force;
        }

        If (Test-Path "$ProjectObjPath") {
            Write-Host -ForegroundColor DarkCyan "Removing $ProjectObjPath.";
            Remove-Item -Path "$ProjectObjPath" -Recurse -Confirm:$false -Force;
        }
    }

    $BuildArtifactsDirectoryPath = Join-Path -Path "$DirectoryPathForArtifacts" -ChildPath "$SolutionConfiguration";

    If (Test-Path "$BuildArtifactsDirectoryPath") {
        Write-Host -ForegroundColor DarkCyan "Removing artifacts from $BuildArtifactsDirectoryPath.";
        Remove-Item -Path "$BuildArtifactsDirectoryPath" -Recurse -Confirm:$false -Force;
    }

    CleanWebDocumentation -SolutionConfiguration $SolutionConfiguration;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished cleaning. <<<`n";
}

function CleanDebug {
    Clean -SolutionConfiguration $SolutionConfigurationDebug;
}

function CleanRelease {
    Clean -SolutionConfiguration $SolutionConfigurationRelease;
}

function CleanWebDocumentation {
    Param (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $SolutionConfiguration
    )

    If ($SolutionConfiguration -ne $SolutionConfigurationRelease) {
        return;
    }

    Write-Host -ForegroundColor DarkCyan "Cleaning documentation website.";

    If (Test-Path "$DirectoryPathForDocumentationObjects") {
        Write-Host -ForegroundColor DarkCyan "Removing documentation website artifacts from $DirectoryPathForDocumentationObjects.";
        Remove-Item -Path "$DirectoryPathForDocumentationObjects" -Recurse -Confirm:$false -Force;
    }

    If (Test-Path "$DirectoryPathForDocumentationWebsite") {
        Write-Host -ForegroundColor DarkCyan "Removing documentation website artifacts from $DirectoryPathForDocumentationWebsite.";
        Remove-Item -Path "$DirectoryPathForDocumentationWebsite" -Recurse -Confirm:$false -Force;
    }
}

# Decrypt
# =================================================================================================================================

function DecryptCodeSigningCertificate {
    Param (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $Key
    )

    If (-not (Test-Path "$FilePathForEncryptedCodeSigningCertificate")) {
        Write-Host -ForegroundColor DarkYellow "The encrypted code signing certificate is not available at path $FilePathForEncryptedCodeSigningCertificate.";
        return;
    }

    If (-not (Test-Path "$DirectoryPathForCicdTools")) {
        New-Item -ItemType Directory -Path "$DirectoryPathForCicdTools" -Force | Out-Null;
    }

    Write-Host -ForegroundColor DarkCyan "Decrypting the code signing certificate.";
    Push-Location "$DirectoryPathForCicdTools"
    iex ((New-Object Net.WebClient).DownloadString($InstallScriptUriForAppVeyorSecureFileUtility));
    Push-Location "$DirectoryPathForCicdToolsAppVeyorTools"
    .\secure-file -decrypt "$FilePathForEncryptedCodeSigningCertificate" -secret $Key
    Pop-Location
    Remove-Item "$DirectoryPathForCicdToolsAppVeyorTools" -Recurse -Confirm:$false -Force;
    Pop-Location
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished decrypting the code signing certificate. <<<`n";
}

# Encrypt
# =================================================================================================================================

function EncryptCodeSigningCertificate {
    Param (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $Key
    )

    If (-not (Test-Path "$FilePathForCodeSigningCertificate")) {
        Write-Host -ForegroundColor DarkYellow "The code signing certificate is not available at path $FilePathForCodeSigningCertificate.";
        return;
    }

    If (-not (Test-Path "$DirectoryPathForCicdTools")) {
        New-Item -ItemType Directory -Path "$DirectoryPathForCicdTools" -Force | Out-Null;
    }

    Write-Host -ForegroundColor DarkCyan "Encrypting the code signing certificate.";
    Push-Location "$DirectoryPathForCicdTools"
    iex ((New-Object Net.WebClient).DownloadString($InstallScriptUriForAppVeyorSecureFileUtility));
    Push-Location "$DirectoryPathForCicdToolsAppVeyorTools"
    .\secure-file -encrypt "$FilePathForCodeSigningCertificate" -secret $Key
    Pop-Location
    Remove-Item -Path "$FilePathForCodeSigningCertificate" -Confirm:$false -Force;
    Remove-Item -Path "$DirectoryPathForCicdToolsAppVeyorTools" -Recurse -Confirm:$false -Force;
    Pop-Location
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished encrypting the code signing certificate. <<<`n";
}

# Get
# =================================================================================================================================

function GetAppVeyorConfiguration {
    Import-Module "powershell-yaml" -Force;
    return Get-Content -Path "$FilePathForAppVeyorYamlConfigurlation" | ConvertFrom-Yaml;
}

function GetBuildVersion {
    $AppVeyorConfiguration = GetAppVeyorConfiguration;
    return $AppVeyorConfiguration.version.trimend(".{build}");
}

# Publish
# =================================================================================================================================

function PublishPackages {
    Param (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $SolutionConfiguration
    )

    If ($SolutionConfiguration -ne $SolutionConfigurationRelease) {
        return;
    }

    $NuGetApiKey = $env:RAPIDFIELD_NUGETAPIKEY;

    If (($NuGetApiKey -eq $null) -or ($NuGetApiKey -eq "")) {
        Write-Host -ForegroundColor DarkCyan "Packages will not be published. The NuGet API key is unavailable.";
        return;
    }

    $BuildArtifactsDirectoryPath = Join-Path -Path "$DirectoryPathForArtifacts" -ChildPath "$SolutionConfiguration";

    If (-not (Test-Path "$BuildArtifactsDirectoryPath")) {
        Write-Host -ForegroundColor DarkCyan "No packages are available to publish. The path does not exist: $BuildArtifactsDirectoryPath.";
        return;
    }

    Write-Host -ForegroundColor DarkCyan "Publishing packages in the directory: $BuildArtifactsDirectoryPath.";
    Push-Location "$DirectoryPathForCicdTools"

    Get-ChildItem -Path "$BuildArtifactsDirectoryPath" -File | ForEach-Object {
        $PackageFilePath = $_.FullName;

        If ($PackageFilePath -like "*.nupkg") {
            Write-Host -ForegroundColor DarkCyan "Publishing package $PackageFilePath.";
            .\nuget.exe push $PackageFilePath -ApiKey "$NuGetApiKey" -Source "$NuGetOrgPackageSourceUri" -SkipDuplicate
        }
    }

    Pop-Location
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished publishing packages. <<<`n";
}

# Restore
# =================================================================================================================================

function RestoreDependencies {
    Write-Host -ForegroundColor DarkCyan "Restoring dependencies for $FilePathForSolutionFile.";
    dotnet restore $FilePathForSolutionFile --verbosity minimal

    If ($LASTEXITCODE -ne 0) {
        Throw "One or more dependencies could not be restored for $FilePathForSolutionFile.";
    }

    Write-Host -ForegroundColor DarkCyan "`n>>> Finished restoring dependencies. <<<`n";
}

# Sign
# =================================================================================================================================

function SignPackages {
    Param (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $SolutionConfiguration
    )

    If ($SolutionConfiguration -ne $SolutionConfigurationRelease) {
        return;
    }

    $CodeSigningCertificateKey = $env:RAPIDFIELD_CSCERTKEY;

    If (($CodeSigningCertificateKey -eq $null) -or ($CodeSigningCertificateKey -eq "")) {
        Write-Host -ForegroundColor DarkCyan "Packages will not be signed. The code signing certificate key is unavailable.";
        return;
    }

    $CodeSigningCertificatePassword = $env:RAPIDFIELD_CSCERTPWD;

    If (($CodeSigningCertificatePassword -eq $null) -or ($CodeSigningCertificatePassword -eq "")) {
        Write-Host -ForegroundColor DarkCyan "Packages will not be signed. The code signing certificate password is unavailable.";
        return;
    }

    $BuildArtifactsDirectoryPath = Join-Path -Path "$DirectoryPathForArtifacts" -ChildPath "$SolutionConfiguration";

    If (-not (Test-Path "$BuildArtifactsDirectoryPath")) {
        Write-Host -ForegroundColor DarkCyan "No packages are available to sign. The path does not exist: $BuildArtifactsDirectoryPath.";
        return;
    }

    If (-not (Test-Path "$FilePathForCodeSigningCertificate")) {
        DecryptCodeSigningCertificate -Key $CodeSigningCertificateKey
    }

    If (-not (Test-Path "$FilePathForCodeSigningCertificate")) {
        Write-Host -ForegroundColor DarkYellow "Packages will not be signed. The code signing certificate is not available at path $FilePathForCodeSigningCertificate.";
        return;
    }

    Write-Host -ForegroundColor DarkCyan "Signing packages in the directory: $BuildArtifactsDirectoryPath.";
    Push-Location "$DirectoryPathForCicdTools"

    Get-ChildItem -Path "$BuildArtifactsDirectoryPath" -File | ForEach-Object {
        $PackageFilePath = $_.FullName;

        If ($PackageFilePath -like "*.nupkg") {
            Write-Host -ForegroundColor DarkCyan "Signing package $PackageFilePath.";
            .\nuget.exe sign "$PackageFilePath" -CertificatePath "$FilePathForCodeSigningCertificate" -CertificatePassword $CodeSigningCertificatePassword -Timestamper "$CodeSigningCertificateTimestampServiceUri";
        }
    }

    Pop-Location
    Remove-Item -Path "$FilePathForCodeSigningCertificate" -Confirm:$false -Force;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished signing packages. <<<`n";
}

# Start
# =================================================================================================================================

function StartExampleServiceApplication {
    Param (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $SolutionConfiguration
    )

    Write-Host -ForegroundColor DarkCyan "Starting the example service application using $SolutionConfiguration configuration.";
    $BinaryFilePath = Join-Path -Path "$DirectoryPathForExample" -ChildPath "$ExampleServiceApplicationNamespace\bin\$SolutionConfiguration\$TargetFrameworkForExampleServiceApplication\$ExampleServiceApplicationNamespace.dll";
    Write-Host -ForegroundColor DarkCyan "Using binary path: $BinaryFilePath";
    Start-Process -FilePath "dotnet" -ArgumentList "$BinaryFilePath" -WindowStyle Minimized;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished starting the application. <<<`n";
}

function StartExampleServiceApplicationDebug {
    StartExampleServiceApplication -SolutionConfiguration $SolutionConfigurationDebug;
}

function StartExampleServiceApplicationRelease {
    StartExampleServiceApplication -SolutionConfiguration $SolutionConfigurationRelease;
}

function StartExampleWebApplication {
    Param (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $SolutionConfiguration
    )

    Write-Host -ForegroundColor DarkCyan "Starting the example web application using $SolutionConfiguration configuration.";
    $ProjectFilePath = Join-Path -Path "$DirectoryPathForExample" -ChildPath "$ExampleWebApplicationNamespace\$ExampleWebApplicationNamespace.csproj";
    Write-Host -ForegroundColor DarkCyan "Using project path: $ProjectFilePath";
    Start-Process -FilePath "dotnet" -ArgumentList "run --project ""$ProjectFilePath"" --configuration $SolutionConfiguration" -WindowStyle Minimized;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished starting the application. <<<`n";
}

function StartExampleWebApplicationDebug {
    StartExampleWebApplication -SolutionConfiguration $SolutionConfigurationDebug;
}

function StartExampleWebApplicationRelease {
    StartExampleWebApplication -SolutionConfiguration $SolutionConfigurationRelease;
}

# Stop
# =================================================================================================================================

function StopAllApplications {
    Write-Host -ForegroundColor DarkCyan "Stopping all applications.";
    Stop-Process -Name "dotnet" -ErrorAction SilentlyContinue -Force;
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished stopping all applications. <<<`n";
}

# Test
# =================================================================================================================================

function Test {
    Param (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $SolutionConfiguration
    )

    Get-ChildItem -Path "$DirectoryPathForTests" -Directory | ForEach-Object {
        $TestDirectoryPath = $_.FullName;
        Write-Host -ForegroundColor DarkCyan "Running tests for $TestDirectoryPath using $SolutionConfiguration configuration.";
        dotnet test $TestDirectoryPath --configuration $SolutionConfiguration --no-build --no-restore --verbosity minimal

        If ($LASTEXITCODE -ne 0) {
            Throw "One or more tests failed for $TestDirectoryPath using $SolutionConfiguration configuration.";
        }

        Write-Host -ForegroundColor DarkCyan "`n>>> Finished running tests. <<<`n";
    }
}

function TestDebug {
    Test -SolutionConfiguration $SolutionConfigurationDebug;
}

function TestRelease {
    Test -SolutionConfiguration $SolutionConfigurationRelease;
}

# Write
# =================================================================================================================================

function WriteBuildDetails {
    WriteRepositoryName
    WriteBuildVersion
    WriteTagName
    WriteCommitId
    WriteCommitTimeStamp
    WriteCommitMessage
    WriteCommitAuthorName
    WriteCommitAuthorEmail
}

function WriteBuildVersion {
    $BuildVersion = $env:APPVEYOR_BUILD_VERSION;

    If (($BuildVersion -eq $null) -or ($BuildVersion -eq "")) {
        return;
    }
    
    Write-Host -ForegroundColor DarkCyan "Build version: $BuildVersion";
}

function WriteCommitAuthorEmail {
    $CommitAuthorEmail = $env:APPVEYOR_REPO_AUTHOR_EMAIL;

    If (($CommitAuthorEmail -eq $null) -or ($CommitAuthorEmail -eq "")) {
        return;
    }
    
    Write-Host -ForegroundColor DarkCyan "Commit author email: $CommitAuthorEmail";
}

function WriteCommitAuthorName {
    $CommitAuthorName = $env:APPVEYOR_REPO_AUTHOR;

    If (($CommitAuthorName -eq $null) -or ($CommitAuthorName -eq "")) {
        return;
    }
    
    Write-Host -ForegroundColor DarkCyan "Commit author name: $CommitAuthorName";
}

function WriteCommitId {
    $CommitId = $env:APPVEYOR_REPO_COMMIT;

    If (($CommitId -eq $null) -or ($CommitId -eq "")) {
        return;
    }
    
    Write-Host -ForegroundColor DarkCyan "Commit ID: $CommitId";
}

function WriteCommitMessage {
    $CommitMessage = $env:APPVEYOR_REPO_COMMIT_TIMESTAMP;

    If (($CommitMessage -eq $null) -or ($CommitMessage -eq "")) {
        return;
    }
    
    Write-Host -ForegroundColor DarkCyan "Commit message: $CommitMessage";
}

function WriteCommitTimeStamp {
    $CommitTimeStamp = $env:APPVEYOR_REPO_COMMIT_TIMESTAMP;

    If (($CommitTimeStamp -eq $null) -or ($CommitTimeStamp -eq "")) {
        return;
    }
    
    Write-Host -ForegroundColor DarkCyan "Commit time stamp: $CommitTimeStamp";
}

function WriteRepositoryName {
    $RepositoryName = $env:APPVEYOR_REPO_NAME;

    If (($RepositoryName -eq $null) -or ($RepositoryName -eq "")) {
        return;
    }
    
    Write-Host -ForegroundColor DarkCyan "Repository name: $RepositoryName";
}

function WriteTagName {
    $TagName = $env:APPVEYOR_REPO_TAG_NAME;

    If (($TagName -eq $null) -or ($TagName -eq "")) {
        return;
    }
    
    Write-Host -ForegroundColor DarkCyan "Tag name: $TagName";
}
