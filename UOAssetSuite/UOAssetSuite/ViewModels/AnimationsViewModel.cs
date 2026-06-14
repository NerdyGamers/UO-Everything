using System.Windows.Input;
using UOAssetSuite.Models;

namespace UOAssetSuite.ViewModels;

public sealed class AnimationsViewModel : AssetViewModelBase
{
    private int _bodyId;
    private int _action;
    private int _direction;
    private string _previewText = "Enter body, action, and direction to preview animation metadata.";

    public AnimationsViewModel() : this(new UOFileManager()) { }

    public AnimationsViewModel(UOFileManager fileManager) : base(fileManager, fileManager.Animations.DisplayName, fileManager.Animations.Status)
    {
        File = fileManager.Animations;
        PreviewCommand = new RelayCommand(_ => Preview());
    }

    public AnimationsFile File { get; }
    public ICommand PreviewCommand { get; }
    public int BodyId { get => _bodyId; set { _bodyId = value; OnPropertyChanged(); } }
    public int Action { get => _action; set { _action = value; OnPropertyChanged(); } }
    public int Direction { get => _direction; set { _direction = value; OnPropertyChanged(); } }
    public string PreviewText { get => _previewText; private set { _previewText = value; OnPropertyChanged(); } }

    protected override void ExportSelectedAssets(string path) => File.ExportSelectedAssets(path);
    protected override void ImportSelectedAssets(string path) => File.ImportSelectedAssets(path);
    protected override void SaveModifiedData() => File.SaveModifiedData();

    private void Preview()
    {
        PreviewText = AssetPreviewFormatter.FormatObject(File.Read(BodyId, Action, Direction));
        SetStatus($"Previewed animation body {BodyId}, action {Action}, direction {Direction}.");
    }
}
