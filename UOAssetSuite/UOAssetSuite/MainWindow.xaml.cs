using System.Windows;
using UOAssetSuite.ViewModels;

namespace UOAssetSuite;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}
