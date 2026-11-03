
using System.Windows;
using Labb3_FridaKarlsson_NEU26G.Dialogs;
using Labb3_FridaKarlsson_NEU26G.ViewModels;

namespace Labb3_FridaKarlsson_NEU26G
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _mainWindowViewModel;
        public MainWindow()
        {
            InitializeComponent();

            DataContext = _mainWindowViewModel = new MainWindowViewModel();

            _mainWindowViewModel.ConfigurationViewModel?.OpenCreateNewPack += ConfigurationViewModel_OpenCreateNewPack;
            _mainWindowViewModel.ConfigurationViewModel?.OpenPackOptions += ConfigurationViewModel_OpenPackOptions;
        }

        private void ConfigurationViewModel_OpenPackOptions(object? sender, EventArgs e)
        {
            var dialog = new PackOptionsDialog();

            dialog.ShowDialog();
        }

        private void ConfigurationViewModel_OpenCreateNewPack(object? sender, EventArgs e)
        {
            var dialog = new CreateNewPackDialog();

            

            dialog.ShowDialog();

        }
    }
}