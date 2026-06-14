namespace UOAssetSuite.Models;

public sealed class AnimationsFile : IAssetFileOperations
{
    private readonly UltimaApi _ultima;

    public AnimationsFile(UltimaApi? ultima = null)
    {
        _ultima = ultima ?? new UltimaApi();
    }

    public object? Read(int bodyId, int action, int direction, int hue = 0, bool preserveHue = true)
    {
        return _ultima.InvokeStatic("Ultima.Animations", "GetAnimation", bodyId, action, direction, hue, preserveHue);
    }
    public string DisplayName => "Animations";
    public string Status => "Ready for Ultima.dll-backed file operations.";
    public void ReplaceFrame(int bodyId, int action, int direction, int frame, string path) => _ultima.InvokeFirstAvailable("Ultima.Animations", new object?[] { bodyId, action, direction, frame, path }, "ReplaceFrame", "Replace", "Import");

    public void ExportSelectedAssets(string path) => _ultima.InvokeFirstAvailable("Ultima.Animations", new object?[] { path }, "Export", "ExportSelected", "Save", "SaveMul");

    public void ImportSelectedAssets(string path) => _ultima.InvokeFirstAvailable("Ultima.Animations", new object?[] { path }, "Import", "ImportSelected", "Replace", "ReplaceSelected");

    public void SaveModifiedData() => _ultima.InvokeFirstAvailable("Ultima.Animations", "Save", "SaveMul", "SaveChanges", "Write");

}
