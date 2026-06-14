namespace UOAssetSuite.Models;

public interface IAssetFileOperations
{
    void ExportSelectedAssets(string path);

    void ImportSelectedAssets(string path);

    void SaveModifiedData();
}
