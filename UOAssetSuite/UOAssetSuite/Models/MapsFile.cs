namespace UOAssetSuite.Models;

public enum UOMapId
{
    Felucca = 0,
    Trammel = 1,
    Ilshenar = 2,
    Malas = 3,
    Tokuno = 4,
    TerMur = 5
}

public sealed class MapsFile
{
    private readonly UltimaApi _ultima;

    public MapsFile(UltimaApi? ultima = null)
    {
        _ultima = ultima ?? new UltimaApi();
    }

    public object? ReadMapTile(UOMapId map, int x, int y)
    {
        var mapInstance = GetMapInstance(map);
        var method = mapInstance?.GetType().GetMethod("GetTile", new[] { typeof(int), typeof(int) });
        if (method is null)
        {
            throw new MissingMethodException(mapInstance?.GetType().FullName, "GetTile");
        }

        return method.Invoke(mapInstance, new object[] { x, y });
    }

    public object? ReadStaticTiles(UOMapId map, int x, int y)
    {
        var mapInstance = GetMapInstance(map);
        var method = mapInstance?.GetType().GetMethod("GetStatics", new[] { typeof(int), typeof(int) });
        if (method is null)
        {
            throw new MissingMethodException(mapInstance?.GetType().FullName, "GetStatics");
        }

        return method.Invoke(mapInstance, new object[] { x, y });
    }

    private object? GetMapInstance(UOMapId map)
    {
        var propertyName = map switch
        {
            UOMapId.Felucca => "Felucca",
            UOMapId.Trammel => "Trammel",
            UOMapId.Ilshenar => "Ilshenar",
            UOMapId.Malas => "Malas",
            UOMapId.Tokuno => "Tokuno",
            UOMapId.TerMur => "TerMur",
            _ => throw new ArgumentOutOfRangeException(nameof(map), map, null)
        };

        return _ultima.ReadStaticProperty("Ultima.Map", propertyName);
    }
public sealed class MapsFile
{
    public string DisplayName => "Maps";
    public string Status => "Ready for Ultima.dll-backed file operations.";
}
