using System.Windows.Controls;
using UOAssetSuite.ViewModels;

namespace UOAssetSuite.Views;

public partial class AnimationsView : UserControl
{
    public AnimationsView()
    {
        InitializeComponent();
        DataContext = new AnimationsViewModel();
    }
}
