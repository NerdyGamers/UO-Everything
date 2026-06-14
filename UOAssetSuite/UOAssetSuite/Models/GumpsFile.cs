namespace UOAssetSuite.Models;

public sealed class GumpsFile : IAssetFileOperations
{
    private readonly UltimaApi _ultima;

    public GumpsFile(UltimaApi? ultima = null)
    {
        _ultima = ultima ?? new UltimaApi();
    }

    public object? Read(int gumpId) => _ultima.InvokeStatic("Ultima.Gumps", "GetGump", gumpId);
    public string DisplayName => "Gumps";
    public string Status => "Ready for Ultima.dll-backed file operations.";
    public void ReplaceGump(int gumpId, string path) => _ultima.InvokeFirstAvailable("Ultima.Gumps", new object?[] { gumpId, path }, "ReplaceGump", "Replace", "Import");

    public void ExportSelectedAssets(string path) => _ultima.InvokeFirstAvailable("Ultima.Gumps", new object?[] { path }, "Export", "ExportSelected", "Save", "SaveMul");

    public void ImportSelectedAssets(string path) => _ultima.InvokeFirstAvailable("Ultima.Gumps", new object?[] { path }, "Import", "ImportSelected", "Replace", "ReplaceSelected");

    public void SaveModifiedData() => _ultima.InvokeFirstAvailable("Ultima.Gumps", "Save", "SaveMul", "SaveChanges", "Write");

}
