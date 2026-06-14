namespace UOAssetSuite.Models;

public sealed class GumpsFile
{
    private readonly UltimaApi _ultima;

    public GumpsFile(UltimaApi? ultima = null)
    {
        _ultima = ultima ?? new UltimaApi();
    }

    public object? Read(int gumpId) => _ultima.InvokeStatic("Ultima.Gumps", "GetGump", gumpId);
    public string DisplayName => "Gumps";
    public string Status => "Ready for Ultima.dll-backed file operations.";
}
