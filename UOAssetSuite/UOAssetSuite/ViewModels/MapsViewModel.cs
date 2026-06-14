using UOAssetSuite.Models;

namespace UOAssetSuite.ViewModels;

public sealed class MapsViewModel : AssetViewModelBase
{
    public MapsViewModel()
        : this(new UOFileManager())
    {
    }

    public MapsViewModel(UOFileManager fileManager)
        : base(fileManager, fileManager.Maps.DisplayName, fileManager.Maps.Status)
    {
        File = fileManager.Maps;
    }

    public MapsFile File { get; }

    protected override void ExportSelectedAssets() => File.ExportSelectedAssets();

    protected override void ImportSelectedAssets() => File.ImportSelectedAssets();

    protected override void SaveModifiedData() => File.SaveModifiedData();
}
