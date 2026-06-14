namespace UOAssetSuite.Models;

public sealed record ItemTileInfo(int Id, string? Name, int Flags, int Weight, int Quality, int Quantity, int AnimationId, int Hue);

public sealed class TiledataFile : IAssetFileOperations
{
    private readonly UltimaApi _ultima;

    public TiledataFile(UltimaApi? ultima = null)
    {
        _ultima = ultima ?? new UltimaApi();
    }

    public string DisplayName => "Tiledata";

    public string Status => "Ready for Ultima.dll-backed file operations.";

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
    public void ExportSelectedAssets() => _ultima.InvokeFirstAvailable("Ultima.TileData", "Export", "ExportSelected", "Save", "SaveMul");

    public void ImportSelectedAssets() => _ultima.InvokeFirstAvailable("Ultima.TileData", "Import", "ImportSelected", "Replace", "ReplaceSelected");

    public void SaveModifiedData() => _ultima.InvokeFirstAvailable("Ultima.TileData", "Save", "SaveMul", "SaveChanges", "Write");

}
