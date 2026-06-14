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

    public ClientVersionValidation ValidateClientDirectory(string clientPath)
    {
        if (!Directory.Exists(clientPath))
        {
            return new ClientVersionValidation(false, $"Client directory does not exist: {clientPath}");
        }

        var hasClassicMul = File.Exists(Path.Combine(clientPath, "art.mul")) && File.Exists(Path.Combine(clientPath, "tiledata.mul"));
        var hasEnhancedUop = File.Exists(Path.Combine(clientPath, "artLegacyMUL.uop")) || File.Exists(Path.Combine(clientPath, "map0LegacyMUL.uop"));
        if (!hasClassicMul && !hasEnhancedUop)
        {
            return new ClientVersionValidation(false, "Unsupported client version: expected classic MUL files or legacyMUL UOP assets.");
        }

        return new ClientVersionValidation(true, $"Loaded supported Ultima Online client directory: {clientPath}");
    }

    public bool HasUltimaReference(string applicationBasePath)
    {
        var expectedPath = Path.Combine(applicationBasePath, "Resources", "Ultima.dll");
        return File.Exists(expectedPath);
    }
}
