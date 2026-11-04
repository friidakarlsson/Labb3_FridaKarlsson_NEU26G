
using System.Windows;


namespace Labb3_FridaKarlsson_NEU26G.Dialogs;

/// <summary>
/// Interaction logic for PackOptionsDialog.xaml
/// </summary>
public partial class PackOptionsDialog : Window
{
    public PackOptionsDialog()
    {
        InitializeComponent();

        Loaded += (_, _) =>
        {
            textBoxEditPackName.Focus();
            textBoxEditPackName.SelectAll();
        };
    }

}
