# =================================================================================================================================
# Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
# =================================================================================================================================

$AppVeyorSecureFileUtilityInstallerUri = "https://raw.githubusercontent.com/appveyor/secure-file/master/install.ps1";
$AppVeyorToolsDirectoryPath = Join-Path -Path "$PSScriptRoot" -ChildPath "appveyor-tools";
$ArtifactsDirectoryName = "artifacts";
$CodeSigningCertificateFileName = "CodeSigningCertificate.cer";
$CodeSigningCertificateFilePath = Join-Path -Path "$PSScriptRoot" -ChildPath "$CodeSigningCertificateFileName";
$CodeSigningCertificateTimestampServiceUri = "http://tsa.starfieldtech.com";
$ConfigurationTypeLocal = "Local";
$ConfigurationTypeProduction = "Production";
$DocumentationDirectoryName = "doc";
$DocumentationWebsiteDirectoryName = "_DocumentationWebsite";
$EncryptedCodeSigningCertificatePath = "$CodeSigningCertificateFilePath.enc";
$ExampleDirectoryName = "example";
$ExampleServiceApplicationNamespace = "RapidField.SolidInstruments.Example.ServiceApplication";
$ExampleServiceApplicationTargetFramework = "netcoreapp2.1";
$NuGetExeFilePath = Join-Path -Path "$PSScriptRoot" -ChildPath "nuget.exe";
$ObjectsDirectoryName = "obj";
$ProjectRootDirectory = (Get-Item $PSScriptRoot).Parent.FullName;
$ExampleWebApplicationNamespace = "RapidField.SolidInstruments.Example.WebApplication";
$SolutionConfigurationDebug = "Debug";
$SolutionConfigurationRelease = "Release";
$SolutionFileName = "RapidField.SolidInstruments.sln";
$SolutionPath = Join-Path -Path "$ProjectRootDirectory" -ChildPath "$SolutionFileName";
$SourceDirectoryName = "src";

Import-Module "powershell-yaml" -Force;

# Build
# =================================================================================================================================

function Build {
    Param (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $SolutionConfiguration
    )

    $BuildVersion = GetBuildVersion;
    Write-Host -ForegroundColor DarkCyan "Building $SolutionPath using $SolutionConfiguration configuration.";
    Write-Host -ForegroundColor DarkCyan "Build version: $BuildVersion";
    dotnet build $SolutionPath --configuration $SolutionConfiguration --no-restore --verbosity minimal /p:BuildVersion=$BuildVersion

    If ($LASTEXITCODE -ne 0) {
        Throw "The build failed for $SolutionPath using $SolutionConfiguration configuration.";
    }

    $SourceDirectoryPath = Join-Path -Path "$ProjectRootDirectory" -ChildPath "$SourceDirectoryName";
    $ArtifactsDirectoryPath = Join-Path -Path "$ProjectRootDirectory" -ChildPath "$ArtifactsDirectoryName";
    $BuildArtifactsDirectoryPath = Join-Path -Path "$ArtifactsDirectoryPath" -ChildPath "$SolutionConfiguration";

    If (-not (Test-Path "$BuildArtifactsDirectoryPath")) {
        New-Item -ItemType Directory -Path "$BuildArtifactsDirectoryPath" -Force | Out-Null;
    }

    BuildWebDocumentation -SolutionConfiguration $SolutionConfiguration;

    Get-ChildItem -Path "$SourceDirectoryPath" -Directory | ForEach-Object {
        $ProjectOutputPath = Join-Path -Path $_.FullName -ChildPath "bin\$SolutionConfiguration";

        If (Test-Path "$ProjectOutputPath") {
            Write-Host -ForegroundColor DarkCyan "Copying artifacts from $ProjectOutputPath.";
            Get-ChildItem -Path "$ProjectOutputPath" -File | Copy-Item -Container -Destination "$BuildArtifactsDirectoryPath" -Force | Out-Null;
        }
    }

    $DocumentationWebsiteDirectoryPath = Join-Path -Path $ProjectRootDirectory -ChildPath "$DocumentationDirectoryName\$DocumentationWebsiteDirectoryName";

    If (Test-Path $DocumentationWebsiteDirectoryPath) {
        Get-Item -Path $DocumentationWebsiteDirectoryPath | Copy-Item -Destination $ArtifactsDirectoryPath -Force -Recurse | Out-Null;
    }

    CleanWebDocumentation -SolutionConfiguration $SolutionConfiguration;
    SignPackages -SolutionConfiguration $SolutionConfiguration;
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
    Push-Location "$DocumentationDirectoryName"
    docfx metadata

    Write-Host -ForegroundColor DarkCyan "`nCompiling documentation website.";
    docfx build --loglevel "Error"

    Write-Host -ForegroundColor DarkCyan "`nMinifying documentation website.";

    Get-ChildItem ".\$DocumentationWebsiteDirectoryName" -Include *.html, *.css -Recurse | ForEach-Object {
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

    Write-Host -ForegroundColor DarkCyan "Cleaning $SolutionPath using $SolutionConfiguration configuration.";
    dotnet clean $SolutionPath --configuration $SolutionConfiguration --verbosity minimal

    If ($LASTEXITCODE -ne 0) {
        Throw "Cleaning failed for $SolutionPath using $SolutionConfiguration configuration.";
    }

    $SourceDirectoryPath = Join-Path -Path "$ProjectRootDirectory" -ChildPath "$SourceDirectoryName"

    Get-ChildItem -Path $SourceDirectoryPath -Directory | ForEach-Object {
        $ProjectBinPath = Join-Path -Path $_.FullName -ChildPath "bin\$SolutionConfiguration";
        $ProjectObjPath = Join-Path -Path $_.FullName -ChildPath "obj";

        If (Test-Path $ProjectBinPath) {
            Write-Host -ForegroundColor DarkCyan "Removing $ProjectBinPath.";
            Remove-Item -Path "$ProjectBinPath" -Recurse -Confirm:$false -Force;
        }

        If (Test-Path $ProjectObjPath) {
            Write-Host -ForegroundColor DarkCyan "Removing $ProjectObjPath.";
            Remove-Item -Path "$ProjectObjPath" -Recurse -Confirm:$false -Force;
        }
    }

    $ArtifactsDirectoryPath = Join-Path -Path "$ProjectRootDirectory" -ChildPath "$ArtifactsDirectoryName";
    $BuildArtifactsDirectoryPath = Join-Path -Path "$ArtifactsDirectoryPath" -ChildPath "$SolutionConfiguration";

    If (Test-Path "$BuildArtifactsDirectoryPath") {
        Write-Host -ForegroundColor DarkCyan "Removing artifacts from $ArtifactsDirectoryPath.";
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

    $ObjectsDirectoryPath = Join-Path -Path "$ProjectRootDirectory" -ChildPath "$DocumentationDirectoryName\$ObjectsDirectoryName";

    If (Test-Path "$ObjectsDirectoryPath") {
        Write-Host -ForegroundColor DarkCyan "Removing documentation website artifacts from $ObjectsDirectoryPath.";
        Remove-Item -Path "$ObjectsDirectoryPath" -Recurse -Confirm:$false -Force;
    }

    $DocumentationWebsiteDirectoryPath = Join-Path -Path "$ProjectRootDirectory" -ChildPath "$DocumentationDirectoryName\$DocumentationWebsiteDirectoryName";

    If (Test-Path "$DocumentationWebsiteDirectoryPath") {
        Write-Host -ForegroundColor DarkCyan "Removing documentation website artifacts from $DocumentationWebsiteDirectoryPath.";
        Remove-Item -Path "$DocumentationWebsiteDirectoryPath" -Recurse -Confirm:$false -Force;
    }
}

# Decrypt
# =================================================================================================================================

function DecryptCodeSigningCertificate {
    Param (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $Secret
    )

    If (-not (Test-Path "$EncryptedCodeSigningCertificatePath")) {
        Write-Host -ForegroundColor DarkYellow "The encrypted code signing certificate is not available at path $EncryptedCodeSigningCertificatePath.";
        return;
    }

    Write-Host -ForegroundColor DarkCyan "Decrypting the code signing certificate.";

    Push-Location "$PSScriptRoot"
    iex ((New-Object Net.WebClient).DownloadString($AppVeyorSecureFileUtilityInstallerUri));
    Push-Location "$AppVeyorToolsDirectoryPath"
    .\secure-file -decrypt "$EncryptedCodeSigningCertificatePath" -secret $Secret
    Pop-Location
    Remove-Item "$AppVeyorToolsDirectoryPath" -Confirm:$false -Force;
    Pop-Location
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished encrypting the code signing certificate. <<<`n";
}

# Encrypt
# =================================================================================================================================

function EncryptCodeSigningCertificate {
    Param (
        [Parameter(Mandatory = $true, Position = 0)]
        [String] $Secret
    )

    If (-not (Test-Path "$CodeSigningCertificateFilePath")) {
        Write-Host -ForegroundColor DarkYellow "The code signing certificate is not available at path $CodeSigningCertificateFilePath.";
        return;
    }

    Write-Host -ForegroundColor DarkCyan "Encrypting the code signing certificate.";
    Push-Location "$PSScriptRoot"
    iex ((New-Object Net.WebClient).DownloadString($AppVeyorSecureFileUtilityInstallerUri));
    Push-Location "$AppVeyorToolsDirectoryPath"
    .\secure-file -encrypt "$CodeSigningCertificateFilePath" -secret $Secret
    Pop-Location
    Remove-Item -Path "$CodeSigningCertificateFilePath" -Confirm:$false -Force;
    Remove-Item -Path "$AppVeyorToolsDirectoryPath" -Confirm:$false -Force;
    Pop-Location
    Write-Host -ForegroundColor DarkCyan "`n>>> Finished encrypting the code signing certificate. <<<`n";
}

# Get
# =================================================================================================================================

function GetAppVeyorConfiguration {
    $AppveyorYamlConfigurationPath = Join-Path -Path "$ProjectRootDirectory" -ChildPath "appveyor.yml";
    return Get-Content -Path $AppveyorYamlConfigurationPath | ConvertFrom-Yaml;
}

function GetBuildVersion {
    $AppVeyorConfiguration = GetAppVeyorConfiguration;
    return $AppVeyorConfiguration.version.trimend(".{build}");
}

# Restore
# =================================================================================================================================

function RestoreDependencies {
    Write-Host -ForegroundColor DarkCyan "Restoring dependencies for $SolutionPath.";
    dotnet restore $SolutionPath --verbosity minimal

    If ($LASTEXITCODE -ne 0) {
        Throw "One or more dependencies could not be restored for $SolutionPath.";
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

    $CodeSigningCertificateKeyValue = $env:RAPIDFIELD_CSCERTKEY;

    If (($CodeSigningCertificateKeyValue -eq $null) -or ($CodeSigningCertificateKeyValue -eq "")) {
        Write-Host -ForegroundColor DarkCyan "Packages will not be signed. The code signing certificate key is unavailable.";
        return;
    }

    $ArtifactsDirectoryPath = Join-Path -Path "$ProjectRootDirectory" -ChildPath "$ArtifactsDirectoryName";
    $BuildArtifactsDirectoryPath = Join-Path -Path "$ArtifactsDirectoryPath" -ChildPath "$SolutionConfiguration";

    If (-not (Test-Path "$BuildArtifactsDirectoryPath")) {
        Write-Host -ForegroundColor DarkCyan "No packages are available to sign. The path does not exist: $BuildArtifactsDirectoryPath.";
        return;
    }

    If (-not (Test-Path "$CodeSigningCertificateFilePath")) {
        DecryptCodeSigningCertificate -Secret $CodeSigningCertificateKeyValue
    }

    If (-not (Test-Path "$CodeSigningCertificateFilePath")) {
        Write-Host -ForegroundColor DarkYellow "Packages will not be signed. The code signing certificate is not available at path $CodeSigningCertificateFilePath.";
        return;
    }

    Write-Host -ForegroundColor DarkCyan "Signing packages in the directory: $BuildArtifactsDirectoryPath.";
    Push-Location "$PSScriptRoot"

    Get-ChildItem -Path "$BuildArtifactsDirectoryPath" -File | ForEach-Object {
        $PackageFilePath = $_.FullName;

        If ($PackageFilePath -like "*.nupkg") {
            Write-Host -ForegroundColor DarkCyan "Signing package $PackageFilePath.";
            .\nuget.exe sign "$PackageFilePath" -CertificatePath "$CodeSigningCertificateFilePath" -Timestamper "$CodeSigningCertificateTimestampServiceUri";
        }
    }

    Pop-Location
    Remove-Item -Path "$CodeSigningCertificateFilePath" -Confirm:$false -Force;
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
    $BinaryFilePath = Join-Path -Path "$ProjectRootDirectory" -ChildPath "$ExampleDirectoryName\$ExampleServiceApplicationNamespace\bin\$SolutionConfiguration\$ExampleServiceApplicationTargetFramework\$ExampleServiceApplicationNamespace.dll";
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
    $ProjectFilePath = Join-Path -Path "$ProjectRootDirectory" -ChildPath "$ExampleDirectoryName\$ExampleWebApplicationNamespace\$ExampleWebApplicationNamespace.csproj";
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

    ForEach ($TestDirectoryPath In (Get-ChildItem -Path "test" -Directory)) {
        Write-Host -ForegroundColor DarkCyan "Running tests for $TestDirectoryPath using $SolutionConfiguration configuration.";
        dotnet test $TestDirectoryPath.FullName --configuration $SolutionConfiguration --no-build --no-restore --verbosity minimal

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