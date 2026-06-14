using System.Windows.Input;
using UOAssetSuite.Models;

namespace UOAssetSuite.ViewModels;

public sealed class MapsViewModel : AssetViewModelBase
{
    private UOMapId _map;
    private int _x;
    private int _y;
    private string _previewText = "Choose a facet and coordinates to preview map and static tile records.";

    public MapsViewModel() : this(new UOFileManager()) { }

    public MapsViewModel(UOFileManager fileManager) : base(fileManager, fileManager.Maps.DisplayName, fileManager.Maps.Status)
    {
        File = fileManager.Maps;
        PreviewCommand = new RelayCommand(_ => Preview());
    }

    public MapsFile File { get; }
    public ICommand PreviewCommand { get; }
    public Array Maps => Enum.GetValues(typeof(UOMapId));
    public UOMapId Map { get => _map; set { _map = value; OnPropertyChanged(); } }
    public int X { get => _x; set { _x = value; OnPropertyChanged(); } }
    public int Y { get => _y; set { _y = value; OnPropertyChanged(); } }
    public string PreviewText { get => _previewText; private set { _previewText = value; OnPropertyChanged(); } }

    protected override void ExportSelectedAssets(string path) => File.ExportSelectedAssets(path);
    protected override void ImportSelectedAssets(string path) => File.ImportSelectedAssets(path);
    protected override void SaveModifiedData() => File.SaveModifiedData();

    private void Preview()
    {
        var land = AssetPreviewFormatter.FormatObject(File.ReadMapTile(Map, X, Y));
        var statics = AssetPreviewFormatter.FormatObject(File.ReadStaticTiles(Map, X, Y));
        PreviewText = $"Land:{Environment.NewLine}{land}{Environment.NewLine}{Environment.NewLine}Statics:{Environment.NewLine}{statics}";
        SetStatus($"Previewed {Map} map tile at {X}, {Y}.");
    }
}
