using System.Windows.Controls;
using UOAssetSuite.ViewModels;

namespace UOAssetSuite.Views;

public partial class MapsView : UserControl
{
    public MapsView()
    {
        InitializeComponent();
        DataContext = new MapsViewModel();
    }
}
