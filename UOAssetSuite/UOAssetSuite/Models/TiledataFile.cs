namespace UOAssetSuite.Models;

public sealed record ItemTileInfo(int Id, string? Name, int Flags, int Weight, int Quality, int Quantity, int AnimationId, int Hue);

public sealed class TiledataFile
{
    private readonly UltimaApi _ultima;

    public TiledataFile(UltimaApi? ultima = null)
    {
        _ultima = ultima ?? new UltimaApi();
    }

    public ItemTileInfo ReadItem(int itemId)
    {
        var item = _ultima.ReadIndexedValue("Ultima.TileData", "ItemTable", itemId);
        return new ItemTileInfo(
            itemId,
            UltimaApi.GetProperty<string>(item, "Name"),
            UltimaApi.GetProperty<int>(item, "Flags"),
            UltimaApi.GetProperty<int>(item, "Weight"),
            UltimaApi.GetProperty<int>(item, "Quality"),
            UltimaApi.GetProperty<int>(item, "Quantity"),
            UltimaApi.GetProperty<int>(item, "Animation"),
            UltimaApi.GetProperty<int>(item, "Hue"));
    }
public sealed class TiledataFile
{
    public string DisplayName => "Tiledata";
    public string Status => "Ready for Ultima.dll-backed file operations.";
}
