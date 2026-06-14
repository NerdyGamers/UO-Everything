using UOAssetSuite.Models;

namespace UOAssetSuite.ViewModels;

public sealed class ArtViewModel
{
    public ArtViewModel()
        : this(new ArtFile())
    {
    }

    public ArtViewModel(ArtFile file)
    {
        File = file;
    }

    public ArtFile File { get; }
    public string Title => File.DisplayName;
    public string Status => File.Status;
}
