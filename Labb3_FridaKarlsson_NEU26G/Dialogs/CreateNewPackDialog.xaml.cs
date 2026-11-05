
using Labb3_FridaKarlsson_NEU26G.ViewModels;
using System.Windows;


namespace Labb3_FridaKarlsson_NEU26G.Dialogs;

/// <summary>
/// Interaction logic for CreateNewPackDialog.xaml
/// </summary>
public partial class CreateNewPackDialog : Window
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    public CreateNewPackDialog()
    {
        InitializeComponent();

        Loaded += (_, _) =>
        {
            textBoxNewPackName.Focus();
            textBoxNewPackName.SelectAll();
        };

        DataContext = _mainWindowViewModel?.ConfigurationViewModel; 

        
    }

    private void CreateButton_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = true;
    }
}
