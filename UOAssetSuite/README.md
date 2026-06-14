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
- `UOAssetSuite/Resources/` — Place `Ultima.dll` here and add/update the project reference as needed.

## Next tasks

- Wire model adapters to actual `Ultima.dll` APIs for read/write operations.
- Add preview controls for bitmap, animation, map, and tiledata records.
- Add import/export commands and validation for supported client versions.
