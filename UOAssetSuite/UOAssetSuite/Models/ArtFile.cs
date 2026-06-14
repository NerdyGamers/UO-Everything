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
    public void ExportSelectedAssets() => _ultima.InvokeFirstAvailable("Ultima.Art", "Export", "ExportSelected", "Save", "SaveMul");

    public void ImportSelectedAssets() => _ultima.InvokeFirstAvailable("Ultima.Art", "Import", "ImportSelected", "Replace", "ReplaceSelected");

    public void SaveModifiedData() => _ultima.InvokeFirstAvailable("Ultima.Art", "Save", "SaveMul", "SaveChanges", "Write");

}
