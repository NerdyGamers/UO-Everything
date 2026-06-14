namespace UOAssetSuite.Models;

public sealed class ArtFile
{
    private readonly UltimaApi _ultima;

    public ArtFile(UltimaApi? ultima = null)
    {
        _ultima = ultima ?? new UltimaApi();
    }

    public object? ReadLand(int tileId) => _ultima.InvokeStatic("Ultima.Art", "GetLand", tileId);

    public object? ReadStatic(int itemId) => _ultima.InvokeStatic("Ultima.Art", "GetStatic", itemId);
}
