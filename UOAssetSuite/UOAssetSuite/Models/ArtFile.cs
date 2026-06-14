namespace UOAssetSuite.Models;

public sealed class ArtFile : IAssetFileOperations
{
    private readonly UltimaApi _ultima;

    public ArtFile(UltimaApi? ultima = null)
    {
        _ultima = ultima ?? new UltimaApi();
    }

    public object? ReadLand(int tileId) => _ultima.InvokeStatic("Ultima.Art", "GetLand", tileId);

    public object? ReadStatic(int itemId) => _ultima.InvokeStatic("Ultima.Art", "GetStatic", itemId);
    public string DisplayName => "Art";
    public string Status => "Ready for Ultima.dll-backed file operations.";
    public void ReplaceStatic(int itemId, string path) => _ultima.InvokeFirstAvailable("Ultima.Art", new object?[] { itemId, path }, "ReplaceStatic", "Replace", "Import");

    public void ReplaceLand(int tileId, string path) => _ultima.InvokeFirstAvailable("Ultima.Art", new object?[] { tileId, path }, "ReplaceLand", "Replace", "Import");

    public void ExportSelectedAssets(string path) => _ultima.InvokeFirstAvailable("Ultima.Art", new object?[] { path }, "Export", "ExportSelected", "Save", "SaveMul");

    public void ImportSelectedAssets(string path) => _ultima.InvokeFirstAvailable("Ultima.Art", new object?[] { path }, "Import", "ImportSelected", "Replace", "ReplaceSelected");

    public void SaveModifiedData() => _ultima.InvokeFirstAvailable("Ultima.Art", "Save", "SaveMul", "SaveChanges", "Write");

}
