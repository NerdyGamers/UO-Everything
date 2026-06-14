using UOAssetSuite.Models;

namespace UOAssetSuite.ViewModels;

public sealed class AnimationsViewModel : AssetViewModelBase
{
    public AnimationsViewModel()
        : this(new UOFileManager())
    {
    }

    public AnimationsViewModel(UOFileManager fileManager)
        : base(fileManager, fileManager.Animations.DisplayName, fileManager.Animations.Status)
    {
        File = fileManager.Animations;
    }

    public AnimationsFile File { get; }

    protected override void ExportSelectedAssets() => File.ExportSelectedAssets();

    protected override void ImportSelectedAssets() => File.ImportSelectedAssets();

    protected override void SaveModifiedData() => File.SaveModifiedData();
}
