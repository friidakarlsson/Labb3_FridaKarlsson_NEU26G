
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
            _mainWindowViewModel.QuizStarted += _mainWindowViewModel_PlayQuiz;
            _mainWindowViewModel.ConfigurationViewOpened += _mainWindowViewModel_OpenConfigurationView;
            _mainWindowViewModel.ProgramExited += _mainWindowViewModel_ExitProgram;
        }

        private void _mainWindowViewModel_ExitProgram(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void _mainWindowViewModel_OpenConfigurationView(object? sender, EventArgs e)
        {
            _mainWindowViewModel.CurrentView = _mainWindowViewModel.ConfigurationViewModel;
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

            var currentActivePack = _mainWindowViewModel?.ActivePack;

            _mainWindowViewModel?.ConfigurationViewModel?.CreateNewPack();

            var dialog = new CreateNewPackDialog();
            {
                dialog.Owner = this;
                dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                dialog.DataContext = _mainWindowViewModel?.ConfigurationViewModel?.ActivePack ;
            }


            if (dialog.ShowDialog() != true)
            {
                if(_mainWindowViewModel?.ActivePack != null)
                {
                    _mainWindowViewModel.Packs.Remove(_mainWindowViewModel.ActivePack);
                }
                _mainWindowViewModel?.ActivePack = currentActivePack;
            }


        }
    }
}