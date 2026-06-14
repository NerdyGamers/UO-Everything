using UOAssetSuite.Models;

namespace UOAssetSuite.ViewModels;

public sealed class MapsViewModel
{
    public MapsViewModel()
        : this(new MapsFile())
    {
    }

    public MapsViewModel(MapsFile file)
    {
        File = file;
    }

    public MapsFile File { get; }
    public string Title => File.DisplayName;
    public string Status => File.Status;
}
