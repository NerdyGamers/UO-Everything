using System.Windows.Controls;
using UOAssetSuite.ViewModels;

namespace UOAssetSuite.Views;

public partial class ArtView : UserControl
{
    public ArtView()
    {
        InitializeComponent();
        DataContext = new ArtViewModel();
    }
}
