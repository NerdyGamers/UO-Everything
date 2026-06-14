namespace UOAssetSuite.Models;

public interface IAssetFileOperations
{
    void ExportSelectedAssets();

    void ImportSelectedAssets();

    void SaveModifiedData();
}
