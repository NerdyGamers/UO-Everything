using UOAssetSuite.Models;

namespace UOAssetSuite.ViewModels;

public sealed class TiledataViewModel
{
    public TiledataViewModel()
        : this(new TiledataFile())
    {
    }

    public TiledataViewModel(TiledataFile file)
    {
        File = file;
    }

    public TiledataFile File { get; }
    public string Title => File.DisplayName;
    public string Status => File.Status;
}
