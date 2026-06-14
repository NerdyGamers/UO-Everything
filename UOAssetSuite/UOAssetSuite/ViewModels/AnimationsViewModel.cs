using UOAssetSuite.Models;

namespace UOAssetSuite.ViewModels;

public sealed class AnimationsViewModel
{
    public AnimationsViewModel()
        : this(new AnimationsFile())
    {
    }

    public AnimationsViewModel(AnimationsFile file)
    {
        File = file;
    }

    public AnimationsFile File { get; }
    public string Title => File.DisplayName;
    public string Status => File.Status;
}
