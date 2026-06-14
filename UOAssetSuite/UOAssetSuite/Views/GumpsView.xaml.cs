using System.Windows.Controls;
using UOAssetSuite.ViewModels;

namespace UOAssetSuite.Views;

public partial class GumpsView : UserControl
{
    public GumpsView()
    {
        InitializeComponent();
        DataContext = new GumpsViewModel();
    }
}
