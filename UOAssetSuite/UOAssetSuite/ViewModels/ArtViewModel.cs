using System.Drawing;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UOAssetSuite.Models;

namespace UOAssetSuite.ViewModels;

public sealed class ArtViewModel : AssetViewModelBase
{
    private int _assetId;
    private bool _isLandTile;
    private ImageSource? _previewImage;
    private string _previewText = "Enter an art ID and preview a land or static bitmap.";

    public ArtViewModel() : this(new UOFileManager()) { }

    public ArtViewModel(UOFileManager fileManager) : base(fileManager, fileManager.Art.DisplayName, fileManager.Art.Status)
    {
        File = fileManager.Art;
        PreviewCommand = new RelayCommand(_ => Preview());
    }

    public ArtFile File { get; }
    public ICommand PreviewCommand { get; }
    public int AssetId { get => _assetId; set { _assetId = value; OnPropertyChanged(); } }
    public bool IsLandTile { get => _isLandTile; set { _isLandTile = value; OnPropertyChanged(); } }
    public ImageSource? PreviewImage { get => _previewImage; private set { _previewImage = value; OnPropertyChanged(); } }
    public string PreviewText { get => _previewText; private set { _previewText = value; OnPropertyChanged(); } }

    protected override void ExportSelectedAssets(string path) => File.ExportSelectedAssets(path);
    protected override void ImportSelectedAssets(string path) => File.ImportSelectedAssets(path);
    protected override void SaveModifiedData() => File.SaveModifiedData();

    private void Preview()
    {
        var record = IsLandTile ? File.ReadLand(AssetId) : File.ReadStatic(AssetId);
        PreviewText = AssetPreviewFormatter.FormatObject(record);
        PreviewImage = record is Bitmap bitmap ? Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()) : null;
        SetStatus($"Previewed {(IsLandTile ? "land" : "static")} art #{AssetId}.");
    }
}
