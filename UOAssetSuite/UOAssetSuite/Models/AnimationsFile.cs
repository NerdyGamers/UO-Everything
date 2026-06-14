namespace UOAssetSuite.Models;

public sealed class AnimationsFile
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
}
