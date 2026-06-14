using UOAssetSuite.Models;

namespace UOAssetSuite.ViewModels;

public sealed class GumpsViewModel
{
    public GumpsViewModel()
        : this(new GumpsFile())
    {
    }

    public GumpsViewModel(GumpsFile file)
    {
        File = file;
    }

    public GumpsFile File { get; }
    public string Title => File.DisplayName;
    public string Status => File.Status;
}
