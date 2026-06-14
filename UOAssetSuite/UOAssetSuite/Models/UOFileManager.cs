using System.IO;

namespace UOAssetSuite.Models;

public sealed class UOFileManager
{
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
