
using Labb3_FridaKarlsson_NEU26G.ViewModels;
using System.Windows;


namespace Labb3_FridaKarlsson_NEU26G.Dialogs;

/// <summary>
/// Interaction logic for CreateNewPackDialog.xaml
/// </summary>
public partial class CreateNewPackDialog : Window
{

    public CreateNewPackDialog()
    {
        InitializeComponent();

        Loaded += (_, _) =>
        {
            textBoxNewPackName.Focus();
            textBoxNewPackName.SelectAll();
        };
     }

    private void CreateButton_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = true;
    }
}
