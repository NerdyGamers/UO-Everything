using UOAssetSuite.Models;

namespace UOAssetSuite.ViewModels;

public sealed class TiledataViewModel : AssetViewModelBase
{
    public TiledataViewModel()
        : this(new UOFileManager())
    {
    }

    public TiledataViewModel(UOFileManager fileManager)
        : base(fileManager, fileManager.Tiledata.DisplayName, fileManager.Tiledata.Status)
    {
        File = fileManager.Tiledata;
    }

    public TiledataFile File { get; }

    protected override void ExportSelectedAssets() => File.ExportSelectedAssets();

    protected override void ImportSelectedAssets() => File.ImportSelectedAssets();

    protected override void SaveModifiedData() => File.SaveModifiedData();
}
