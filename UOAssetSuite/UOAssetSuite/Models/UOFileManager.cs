using System.IO;

namespace UOAssetSuite.Models;

public sealed class UOFileManager
{
    private readonly UltimaApi _ultima;

    public UOFileManager(string? clientPath = null)
    {
        _ultima = new UltimaApi(clientPath);
        Art = new ArtFile(_ultima);
        Landtiles = new LandtilesFile(_ultima);
        Animations = new AnimationsFile(_ultima);
        Gumps = new GumpsFile(_ultima);
        Maps = new MapsFile(_ultima);
        Tiledata = new TiledataFile(_ultima);
    }

    public ArtFile Art { get; }

    public LandtilesFile Landtiles { get; }

    public AnimationsFile Animations { get; }

    public GumpsFile Gumps { get; }

    public MapsFile Maps { get; }

    public TiledataFile Tiledata { get; }

    public void SetClientPath(string clientPath) => _ultima.SetClientPath(clientPath);
    public UOFileManager()
    {
        Art = new ArtFile();
        Landtiles = new LandtilesFile();
        Animations = new AnimationsFile();
        Gumps = new GumpsFile();
        Maps = new MapsFile();
        Tiledata = new TiledataFile();
    }

    public ArtFile Art { get; }
    public LandtilesFile Landtiles { get; }
    public AnimationsFile Animations { get; }
    public GumpsFile Gumps { get; }
    public MapsFile Maps { get; }
    public TiledataFile Tiledata { get; }

    public bool HasUltimaReference(string applicationBasePath)
    {
        var expectedPath = Path.Combine(applicationBasePath, "Resources", "Ultima.dll");
        return File.Exists(expectedPath);
    }
}
