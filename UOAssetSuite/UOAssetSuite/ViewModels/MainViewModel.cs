using UOAssetSuite.Models;

namespace UOAssetSuite.ViewModels;

public sealed class MainViewModel
{
    public MainViewModel()
    {
        FileManager = new UOFileManager();
        Art = new ArtViewModel(FileManager.Art);
        Landtiles = new LandtilesViewModel(FileManager.Landtiles);
        Animations = new AnimationsViewModel(FileManager.Animations);
        Gumps = new GumpsViewModel(FileManager.Gumps);
        Maps = new MapsViewModel(FileManager.Maps);
        Tiledata = new TiledataViewModel(FileManager.Tiledata);
    }

    public UOFileManager FileManager { get; }
    public ArtViewModel Art { get; }
    public LandtilesViewModel Landtiles { get; }
    public AnimationsViewModel Animations { get; }
    public GumpsViewModel Gumps { get; }
    public MapsViewModel Maps { get; }
    public TiledataViewModel Tiledata { get; }
}
