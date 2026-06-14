using System.Windows.Controls;
using UOAssetSuite.ViewModels;

namespace UOAssetSuite.Views;

public partial class LandtilesView : UserControl
{
    public LandtilesView()
    {
        InitializeComponent();
        DataContext = new LandtilesViewModel();
    }
}
