using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Input;
using UOAssetSuite.Models;

namespace UOAssetSuite.ViewModels;

public abstract class AssetViewModelBase : INotifyPropertyChanged
{
    private readonly UOFileManager _fileManager;
    private string _status;

    protected AssetViewModelBase(UOFileManager fileManager, string title, string initialStatus)
    {
        _fileManager = fileManager;
        Title = title;
        _status = initialStatus;
        LoadClientDirectoryCommand = new RelayCommand(_ => LoadClientDirectory());
        ExportSelectedAssetsCommand = new RelayCommand(_ => ExportSelectedAssetsWithDialog());
        ImportSelectedAssetsCommand = new RelayCommand(_ => ImportSelectedAssetsWithDialog());
        SaveModifiedDataCommand = new RelayCommand(_ => RunAssetOperation("Saved modified data through Ultima.dll.", SaveModifiedData));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Title { get; }

    public string Status
    {
        get => _status;
        private set
        {
            if (_status == value)
            {
                return;
            }

            _status = value;
            OnPropertyChanged();
        }
    }

    public ICommand LoadClientDirectoryCommand { get; }

    public ICommand ExportSelectedAssetsCommand { get; }

    public ICommand ImportSelectedAssetsCommand { get; }

    public ICommand SaveModifiedDataCommand { get; }

    protected abstract void ExportSelectedAssets(string path);

    protected abstract void ImportSelectedAssets(string path);

    protected abstract void SaveModifiedData();

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void LoadClientDirectory()
    {
        using var dialog = new FolderBrowserDialog
        {
            Description = "Select the Ultima Online client directory",
            UseDescriptionForTitle = true
        };

        if (dialog.ShowDialog() != DialogResult.OK || string.IsNullOrWhiteSpace(dialog.SelectedPath))
        {
            Status = "Client directory selection canceled.";
            return;
        }

        var validation = _fileManager.ValidateClientDirectory(dialog.SelectedPath);
        if (!validation.IsSupported)
        {
            Status = validation.Message;
            return;
        }

        _fileManager.SetClientPath(dialog.SelectedPath);
        Status = validation.Message;
    }

    private void ExportSelectedAssetsWithDialog()
    {
        using var dialog = new SaveFileDialog
        {
            Title = $"Export {Title}",
            Filter = "Supported asset files (*.bmp;*.png;*.csv;*.uoa;*.xml;*.txt)|*.bmp;*.png;*.csv;*.uoa;*.xml;*.txt|All files (*.*)|*.*"
        };

        if (dialog.ShowDialog() != DialogResult.OK || string.IsNullOrWhiteSpace(dialog.FileName))
        {
            Status = "Export canceled.";
            return;
        }

        RunAssetOperation($"Exported {Title} to {dialog.FileName}.", () => ExportSelectedAssets(dialog.FileName));
    }

    private void ImportSelectedAssetsWithDialog()
    {
        using var dialog = new OpenFileDialog
        {
            Title = $"Import {Title}",
            Filter = "Supported asset files (*.bmp;*.png;*.csv;*.uoa;*.xml;*.txt)|*.bmp;*.png;*.csv;*.uoa;*.xml;*.txt|All files (*.*)|*.*"
        };

        if (dialog.ShowDialog() != DialogResult.OK || string.IsNullOrWhiteSpace(dialog.FileName))
        {
            Status = "Import canceled.";
            return;
        }

        RunAssetOperation($"Imported {Title} from {dialog.FileName}.", () => ImportSelectedAssets(dialog.FileName));
    }

    protected void SetStatus(string status) => Status = status;

    private void RunAssetOperation(string successMessage, Action operation)
    {
        operation();
        Status = successMessage;
    }
}
