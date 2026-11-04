
using System.Windows;
using Labb3_FridaKarlsson_NEU26G.Dialogs;
using Labb3_FridaKarlsson_NEU26G.ViewModels;
using Labb3_FridaKarlsson_NEU26G.Views;

namespace Labb3_FridaKarlsson_NEU26G
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        public MainWindow()
        {
            InitializeComponent();

            DataContext = _mainWindowViewModel = new MainWindowViewModel();

            _mainWindowViewModel.ConfigurationViewModel?.OpenCreateNewPack += OpenCreateNewPackDialog;
            _mainWindowViewModel.ConfigurationViewModel?.OpenPackOptions += OpenPackOptionsDialog;
            _mainWindowViewModel.PlayQuiz += _mainWindowViewModel_PlayQuiz;
        }

        private void _mainWindowViewModel_PlayQuiz(object? sender, EventArgs e)
        {
            _mainWindowViewModel.CurrentView = _mainWindowViewModel.PlayerViewModel;
            
        }

        private void OpenPackOptionsDialog(object? sender, EventArgs e)
        {
            var dialog = new PackOptionsDialog();

            dialog.Owner = this;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;


            dialog.ShowDialog();
        }

        private void OpenCreateNewPackDialog(object? sender, EventArgs e)
        {
            var currentActivePack = _mainWindowViewModel.ActivePack;

            var dialog = new CreateNewPackDialog();

            dialog.Owner = this;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dialog.DataContext = _mainWindowViewModel;

            if (dialog.ShowDialog() == true)
            {
                //_mainWindowViewModel?.Packs.Add(ActivePack);                
            }
            else
            {
                _mainWindowViewModel?.ActivePack = currentActivePack;
            }


        }
    }
}