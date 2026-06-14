using UOAssetSuite.Models;

namespace UOAssetSuite.ViewModels;

public sealed class GumpsViewModel : AssetViewModelBase
{
    public GumpsViewModel()
        : this(new UOFileManager())
    {
    }

    public GumpsViewModel(UOFileManager fileManager)
        : base(fileManager, fileManager.Gumps.DisplayName, fileManager.Gumps.Status)
    {
        File = fileManager.Gumps;
    }

    public GumpsFile File { get; }

    protected override void ExportSelectedAssets() => File.ExportSelectedAssets();

    protected override void ImportSelectedAssets() => File.ImportSelectedAssets();

    protected override void SaveModifiedData() => File.SaveModifiedData();
}
