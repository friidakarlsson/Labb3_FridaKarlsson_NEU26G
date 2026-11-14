
using System.Windows;

namespace Labb3_FridaKarlsson_NEU26G.Dialogs;

/// <summary>
/// Interaction logic for ImportQuestionsDialog.xaml
/// </summary>
public partial class ImportQuestionsDialog : Window
{
    public ImportQuestionsDialog()
    {
        InitializeComponent();
    }

    private void ImportButton_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = true;
    }
}
