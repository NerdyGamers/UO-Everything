using UOAssetSuite.Models;

namespace UOAssetSuite.ViewModels;

public sealed class LandtilesViewModel : AssetViewModelBase
{
    public LandtilesViewModel()
        : this(new UOFileManager())
    {
    }

    public LandtilesViewModel(UOFileManager fileManager)
        : base(fileManager, fileManager.Landtiles.DisplayName, fileManager.Landtiles.Status)
    {
        File = fileManager.Landtiles;
    }

    public LandtilesFile File { get; }

    protected override void ExportSelectedAssets() => File.ExportSelectedAssets();

    protected override void ImportSelectedAssets() => File.ImportSelectedAssets();

    protected override void SaveModifiedData() => File.SaveModifiedData();
}
