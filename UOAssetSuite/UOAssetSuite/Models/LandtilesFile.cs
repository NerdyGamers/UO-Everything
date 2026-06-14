namespace UOAssetSuite.Models;

public sealed record LandTileInfo(int Id, string? Name, int Flags, int TextureId);

public sealed class LandtilesFile
{
    private readonly UltimaApi _ultima;

    public LandtilesFile(UltimaApi? ultima = null)
    {
        _ultima = ultima ?? new UltimaApi();
    }

    public LandTileInfo Read(int tileId)
    {
        var tile = _ultima.ReadIndexedValue("Ultima.TileData", "LandTable", tileId);
        return new LandTileInfo(
            tileId,
            UltimaApi.GetProperty<string>(tile, "Name"),
            UltimaApi.GetProperty<int>(tile, "Flags"),
            UltimaApi.GetProperty<int>(tile, "TextureID"));
    }
public sealed class LandtilesFile
{
    public string DisplayName => "Landtiles";
    public string Status => "Ready for Ultima.dll-backed file operations.";
}
