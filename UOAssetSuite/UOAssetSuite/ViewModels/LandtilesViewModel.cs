using UOAssetSuite.Models;

namespace UOAssetSuite.ViewModels;

public sealed class LandtilesViewModel
{
    public LandtilesViewModel()
        : this(new LandtilesFile())
    {
    }

    public LandtilesViewModel(LandtilesFile file)
    {
        File = file;
    }

    public LandtilesFile File { get; }
    public string Title => File.DisplayName;
    public string Status => File.Status;
}
