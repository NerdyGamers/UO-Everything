# UO Asset Suite

**Non-Canon.** Tooling project for Ultima Online client asset workflows.

A comprehensive C# WPF application for managing Ultima Online client files. The project is structured around UOFiddler's `Ultima.dll` file-operation model, with separate views, view models, and model adapters for major client asset categories.

## Project layout

- `UOAssetSuite.sln` — Visual Studio solution.
- `UOAssetSuite/App.xaml` — WPF application entry point.
- `UOAssetSuite/MainWindow.xaml` — Shell window and tab navigation.
- `UOAssetSuite/Models/` — Asset file adapters for art, landtiles, animations, gumps, maps, and tiledata.
- `UOAssetSuite/ViewModels/` — MVVM state for the shell and each asset category.
- `UOAssetSuite/Views/` — WPF user controls for each asset category.
- `UOAssetSuite/UOAssetSuite/Resources/` — Place `Ultima.dll` here; the project reference is already configured to copy it when present.

## Build prerequisites

- Install the .NET SDK that supports `net6.0-windows` and WPF builds. On non-Windows build hosts, `Directory.Build.props` enables Windows targeting so compile-only validation can run without the Windows desktop runtime.
- Copy UOFiddler's `Ultima.dll` to `UOAssetSuite/UOAssetSuite/Resources/Ultima.dll` before building adapter-backed functionality. The project reference is conditional so the project file remains loadable before the DLL is supplied.
- Before adding write support, run a full build and smoke-test the read-only adapters against a real Ultima Online client directory:

```powershell
pwsh ./tools/smoke-readonly-adapters.ps1 -ClientPath "C:\Program Files (x86)\Electronic Arts\Ultima Online Classic"
```

## Next tasks

- Wire model adapters to actual `Ultima.dll` APIs for read/write operations.
- Add preview controls for bitmap, animation, map, and tiledata records.
- Add import/export commands and validation for supported client versions.
