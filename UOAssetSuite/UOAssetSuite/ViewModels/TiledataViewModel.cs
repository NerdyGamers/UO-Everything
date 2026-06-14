using System.Windows.Input;
using UOAssetSuite.Models;

namespace UOAssetSuite.ViewModels;

public sealed class TiledataViewModel : AssetViewModelBase
{
    private int _itemId;
    private string _previewText = "Enter an item tile ID to preview tiledata fields.";

    public TiledataViewModel() : this(new UOFileManager()) { }

    public TiledataViewModel(UOFileManager fileManager) : base(fileManager, fileManager.Tiledata.DisplayName, fileManager.Tiledata.Status)
    {
        File = fileManager.Tiledata;
        PreviewCommand = new RelayCommand(_ => Preview());
    }

    public TiledataFile File { get; }
    public ICommand PreviewCommand { get; }
    public int ItemId { get => _itemId; set { _itemId = value; OnPropertyChanged(); } }
    public string PreviewText { get => _previewText; private set { _previewText = value; OnPropertyChanged(); } }

    protected override void ExportSelectedAssets(string path) => File.ExportSelectedAssets(path);
    protected override void ImportSelectedAssets(string path) => File.ImportSelectedAssets(path);
    protected override void SaveModifiedData() => File.SaveModifiedData();

    private void Preview()
    {
        PreviewText = AssetPreviewFormatter.FormatObject(File.ReadItem(ItemId));
        SetStatus($"Previewed tiledata item #{ItemId}.");
    }
}
