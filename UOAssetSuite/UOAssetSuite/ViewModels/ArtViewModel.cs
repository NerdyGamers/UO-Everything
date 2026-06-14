using UOAssetSuite.Models;

namespace UOAssetSuite.ViewModels;

public sealed class ArtViewModel : AssetViewModelBase
{
    public ArtViewModel()
        : this(new UOFileManager())
    {
    }

    public ArtViewModel(UOFileManager fileManager)
        : base(fileManager, fileManager.Art.DisplayName, fileManager.Art.Status)
    {
        File = fileManager.Art;
    }

    public ArtFile File { get; }

    protected override void ExportSelectedAssets() => File.ExportSelectedAssets();

    protected override void ImportSelectedAssets() => File.ImportSelectedAssets();

    protected override void SaveModifiedData() => File.SaveModifiedData();
}
