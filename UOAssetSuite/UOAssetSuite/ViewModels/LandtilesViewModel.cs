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

    protected override void ExportSelectedAssets(string path) => File.ExportSelectedAssets(path);

    protected override void ImportSelectedAssets(string path) => File.ImportSelectedAssets(path);

    protected override void SaveModifiedData() => File.SaveModifiedData();
}
