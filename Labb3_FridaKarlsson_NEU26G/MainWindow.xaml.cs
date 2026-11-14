
using Labb3_FridaKarlsson_NEU26G.Dialogs;
using Labb3_FridaKarlsson_NEU26G.ViewModels;
using System.Windows;
using System.Net.Http;

namespace Labb3_FridaKarlsson_NEU26G;

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

        SubscribeToAllEvents();
    }

    private void SubscribeToAllEvents()
    {
        _mainWindowViewModel.ConfigurationViewModel?.CreateNewPackOpened += OpenCreateNewPackDialog;
        _mainWindowViewModel.ConfigurationViewModel?.PackOptionsOpened += OpenPackOptionsDialog;
        _mainWindowViewModel.PlayerViewOpened += OpenPlayerView;
        _mainWindowViewModel.ConfigurationViewOpened += OpenConfigurationView;
        _mainWindowViewModel.ProgramExited += ExitProgram;
        _mainWindowViewModel.ConfigurationViewModel?.ImportQuestionsOpened += OpenImportQuestionsDialog;
        _mainWindowViewModel.FullscreenToggled += ToggleFullScreen;
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

    private void ExitProgram(object? sender, EventArgs e)
    {
        this.Close();
    }

    protected override async void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
        e.Cancel = true;

        await _mainWindowViewModel.SaveDataAsync();

        App.Current.Shutdown();
    }

    private void ToggleFullScreen(object? sender, EventArgs e)
    {
        if (WindowStyle != WindowStyle.None)
        {
            _mainWindowViewModel.IsFullscreen = true;

            WindowState = WindowState.Normal;
            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Maximized;
        }
        else
        {
            _mainWindowViewModel.IsFullscreen = false;

            WindowStyle = WindowStyle.SingleBorderWindow;
            WindowState = WindowState.Normal;
        }
    }

    private void OpenCreateNewPackDialog(object? sender, EventArgs e)
    {
        var currentActivePack = _mainWindowViewModel?.ActivePack;

        _mainWindowViewModel?.ConfigurationViewModel?.CreateNewPack();

        var dialog = new CreateNewPackDialog();

        SetDialogSettings(dialog, _mainWindowViewModel?.ActivePack);

        if (dialog.ShowDialog() != true)
        {
            if (_mainWindowViewModel?.ActivePack != null)
            {
                _mainWindowViewModel.Packs.Remove(_mainWindowViewModel.ActivePack);
            }
            _mainWindowViewModel?.ActivePack = currentActivePack;
        }
    }

    private void OpenPackOptionsDialog(object? sender, EventArgs e)
    {
        var currentActivePack = _mainWindowViewModel?.ActivePack;
        var dialog = new PackOptionsDialog();

        SetDialogSettings(dialog,currentActivePack);

        dialog.ShowDialog();
    }

    private async void OpenImportQuestionsDialog(object? sender, EventArgs e)
    {
        var importQuestionsViewModel = new ImportQuestionsViewModel();
        var dialog = new ImportQuestionsDialog();

        try 
        {
            await importQuestionsViewModel.LoadCategoriesAsync();
        }
        catch (HttpRequestException)
        {
            MessageBox.Show("Could not connect to Open Trivia Database, no categories found.", "Network error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        catch( Exception ex)
        {
            MessageBox.Show(ex.Message, "Load categories failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        SetDialogSettings(dialog, importQuestionsViewModel);

        if (dialog.ShowDialog() == true)
        {
            try
            {
                int count = await _mainWindowViewModel.ConfigurationViewModel!.ImportQuestionsAsync(importQuestionsViewModel);

                MessageBox.Show($"{count} questions were successfully imported", "Import Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Network Error: Could not connect to Open Trivia Database. Please Check your Internet connection", "Import failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Import failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }

    private void SetDialogSettings(Window dialog, object? dataContext)
    {
        dialog.Owner = this;
        dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        dialog.DataContext = dataContext;
    }
}