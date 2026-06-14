using UOAssetSuite.Models;

namespace UOAssetSuite.ViewModels;

public sealed class MainViewModel
{
    public MainViewModel()
    {
        FileManager = new UOFileManager();
        Art = new ArtViewModel(FileManager);
        Landtiles = new LandtilesViewModel(FileManager);
        Animations = new AnimationsViewModel(FileManager);
        Gumps = new GumpsViewModel(FileManager);
        Maps = new MapsViewModel(FileManager);
        Tiledata = new TiledataViewModel(FileManager);
    }

    public UOFileManager FileManager { get; }
    public ArtViewModel Art { get; }
    public LandtilesViewModel Landtiles { get; }
    public AnimationsViewModel Animations { get; }
    public GumpsViewModel Gumps { get; }
    public MapsViewModel Maps { get; }
    public TiledataViewModel Tiledata { get; }
}
