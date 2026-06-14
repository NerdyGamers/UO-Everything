using System.Windows.Controls;
using UOAssetSuite.ViewModels;

namespace UOAssetSuite.Views;

public partial class TiledataView : UserControl
{
    public TiledataView()
    {
        InitializeComponent();
        DataContext = new TiledataViewModel();
    }
}
