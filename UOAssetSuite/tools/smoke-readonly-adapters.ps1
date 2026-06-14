param(
    [Parameter(Mandatory = $true)]
    [string]$ClientPath,

    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"
$repoRoot = Split-Path -Parent $PSScriptRoot
$project = Join-Path $repoRoot "UOAssetSuite/UOAssetSuite.csproj"
$resource = Join-Path $repoRoot "UOAssetSuite/Resources/Ultima.dll"

if (-not (Test-Path $resource)) {
    throw "Missing $resource. Copy UOFiddler's Ultima.dll there before running the smoke test."
}

if (-not (Test-Path $ClientPath)) {
    throw "ClientPath does not exist: $ClientPath"
}

$requiredClientFiles = @("tiledata.mul", "artidx.mul", "art.mul", "gumpidx.mul", "gumpart.mul", "map0.mul", "statics0.mul", "staidx0.mul")
$missing = $requiredClientFiles | Where-Object { -not (Test-Path (Join-Path $ClientPath $_)) }
if ($missing) {
    throw "ClientPath is missing expected Ultima Online client files: $($missing -join ', ')"
}

dotnet build $project --configuration $Configuration

$assembly = Join-Path $repoRoot "UOAssetSuite/bin/$Configuration/net6.0-windows/UOAssetSuite.dll"
Add-Type -Path $assembly

$manager = [UOAssetSuite.Models.UOFileManager]::new($ClientPath)
[void]$manager.Landtiles.Read(0)
[void]$manager.Tiledata.ReadItem(0)
[void]$manager.Art.ReadLand(0)
[void]$manager.Art.ReadStatic(1)
[void]$manager.Gumps.Read(0)
[void]$manager.Maps.ReadMapTile([UOAssetSuite.Models.UOMapId]::Felucca, 0, 0)
[void]$manager.Maps.ReadStaticTiles([UOAssetSuite.Models.UOMapId]::Felucca, 0, 0)

Write-Host "Read-only adapter smoke test passed for $ClientPath"
