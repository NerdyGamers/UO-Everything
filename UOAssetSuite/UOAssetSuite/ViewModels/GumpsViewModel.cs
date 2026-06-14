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

    protected override void ExportSelectedAssets(string path) => File.ExportSelectedAssets(path);

    protected override void ImportSelectedAssets(string path) => File.ImportSelectedAssets(path);

    protected override void SaveModifiedData() => File.SaveModifiedData();
}
