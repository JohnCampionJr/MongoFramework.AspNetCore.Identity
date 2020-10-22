[CmdletBinding(PositionalBinding=$false)]
param(
	[bool] $RunTests = $true,
	[bool] $CreatePackages,
	[string] $BuildVersion
)

$packageOutputFolder = "$PSScriptRoot\build-artifacts"
mkdir -Force $packageOutputFolder | Out-Null

if (-not $BuildVersion) {
	$lastTaggedVersion = git describe --tags --abbrev=0 2>$1;
	if (-not $lastTaggedVersion) {
		$lastTaggedVersion = "0.0.0"
	}
  $lastTaggedVersion = $lastTaggedVersion -replace "v", ""
  
  $BuildVersion = $lastTaggedVersion

  #if we're using a published tag from git, we don't want artifacts to have the same version number'
  $NextVersion = [version]($BuildVersion)
  $NextVersion = "{0}.{1}.{2}" -f $NextVersion.Major, $NextVersion.Minor, ($NextVersion.Build + 1)	

  $BuildVersion = $NextVersion
}

Write-Host "Run Parameters:" -ForegroundColor Cyan
Write-Host "  RunTests: $RunTests"
Write-Host "  CreatePackages: $CreatePackages"
Write-Host "  BuildVersion: $BuildVersion"
Write-Host "Environment:" -ForegroundColor Cyan
Write-Host "  .NET Version:" (dotnet --version)
Write-Host "  Artifact Path: $packageOutputFolder"

Write-Host "Building solution..." -ForegroundColor "Magenta"
dotnet build -c Release /p:Version=$BuildVersion
if ($LastExitCode -ne 0) {
	Write-Host "Build failed, aborting!" -Foreground "Red"
	Exit 1
}
Write-Host "Solution built!" -ForegroundColor "Green"

if ($RunTests) {
	Write-Host "Running tests..." -ForegroundColor "Magenta"
	dotnet test
	if ($LastExitCode -ne 0) {
		Write-Host "Tests failed, aborting build!" -Foreground "Red"
		Exit 1
	}
	Write-Host "Tests passed!" -ForegroundColor "Green"
}

if ($CreatePackages) {
	Write-Host "Clearing existing $packageOutputFolder... " -NoNewline
	Get-ChildItem $packageOutputFolder | Remove-Item
	Write-Host "Packages cleared!" -ForegroundColor "Green"
	
	Write-Host "Packing..." -ForegroundColor "Magenta"
	dotnet pack --no-build -c Release /p:Version=$BuildVersion /p:PackageOutputPath=$packageOutputFolder
	Write-Host "Packing complete!" -ForegroundColor "Green"
}