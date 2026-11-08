
using Labb3_FridaKarlsson_NEU26G.Dialogs;
using Labb3_FridaKarlsson_NEU26G.Models;
using Labb3_FridaKarlsson_NEU26G.ViewModels;
using Labb3_FridaKarlsson_NEU26G.Views;
using System.Windows;

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

            _mainWindowViewModel.ConfigurationViewModel?.CreateNewPackOpened += OpenCreateNewPackDialog;
            _mainWindowViewModel.ConfigurationViewModel?.PackOptionsOpened += OpenPackOptionsDialog;
            _mainWindowViewModel.PlayerViewOpened += OpenPlayerView;
            _mainWindowViewModel.ConfigurationViewOpened += OpenConfigurationView;
            _mainWindowViewModel.ProgramExited += ExitProgram;
        }

        private void ExitProgram(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void OpenConfigurationView(object? sender, EventArgs e)
        {
            _mainWindowViewModel.CurrentView = _mainWindowViewModel.ConfigurationViewModel;
        }
        private void OpenPlayerView(object? sender, EventArgs e)
        {
            _mainWindowViewModel.CurrentView = _mainWindowViewModel.PlayerViewModel;
            _mainWindowViewModel?.PlayerViewModel?.StartQuiz();            
        }

        private void OpenPackOptionsDialog(object? sender, EventArgs e)
        {
            var dialog = new PackOptionsDialog();

            SetDialogSettings(dialog);

            dialog.ShowDialog();
        }

        private void OpenCreateNewPackDialog(object? sender, EventArgs e)
        {

            var currentActivePack = _mainWindowViewModel?.ActivePack;

            _mainWindowViewModel?.ConfigurationViewModel?.CreateNewPack();

            var dialog = new CreateNewPackDialog();

            SetDialogSettings(dialog);

            if (dialog.ShowDialog() != true)
            {
                if(_mainWindowViewModel?.ActivePack != null)
                {
                    _mainWindowViewModel.Packs.Remove(_mainWindowViewModel.ActivePack);
                }
                _mainWindowViewModel?.ActivePack = currentActivePack;
            }
        }
        private void SetDialogSettings(Window dialog)
        {
            dialog.Owner = this;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dialog.DataContext = _mainWindowViewModel?.ConfigurationViewModel?.ActivePack;
        }
    }
}