
using Labb3_FridaKarlsson_NEU26G.Command;
using Labb3_FridaKarlsson_NEU26G.Data;
using Labb3_FridaKarlsson_NEU26G.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace Labb3_FridaKarlsson_NEU26G.ViewModels;

internal class MainWindowViewModel : ViewModelBase
{
	private QuestionPackViewModel? _activePack;
    private object? _currentView;
    private readonly JsonData _jsonData;


    public object? CurrentView 
    { 
        get => _currentView;
        set
        {
            SetProperty(ref _currentView, value);

            if (ActivePack != null)
            {
                ActivePack.IsEditingAllowed = value is not ViewModels.PlayerViewModel;
            }

            ConfigurationViewModel?.OpenCreateNewPackDialogCommand?.RaiseCanExecuteChanged();
            ConfigurationViewModel?.OpenPackOptionsDialogCommand?.RaiseCanExecuteChanged();
            ConfigurationViewModel?.DeletePackCommand?.RaiseCanExecuteChanged();
        }
    }

    public ObservableCollection<QuestionPackViewModel> Packs { get; }
    public QuestionPackViewModel? ActivePack
	{
		get => _activePack;
        set
        {
            if (SetProperty(ref _activePack, value))
            {
                ConfigurationViewModel?.RaiseActivePackChanged();
            }
        }
	}

    public PlayerViewModel? PlayerViewModel { get;}
    public ConfigurationViewModel? ConfigurationViewModel { get; }

    public event EventHandler? PlayerViewOpened;
    public event EventHandler? ConfigurationViewOpened;
    public event EventHandler? ProgramExited;

    public RelayCommand PlayQuizCommand { get; }
    public RelayCommand OpenConfigurationViewCommand { get; }
    public RelayCommand ExitProgramCommand { get; }


    public MainWindowViewModel()
    {
        _jsonData = new JsonData();
        Packs = new ObservableCollection<QuestionPackViewModel>();
        ConfigurationViewModel = new ConfigurationViewModel(this);

		PlayerViewModel = new PlayerViewModel(this);

        PlayQuizCommand = new RelayCommand(_ => RaiseOpenPlayerView());
        OpenConfigurationViewCommand = new RelayCommand(obj => RaiseOpenConfigurationView(obj));
        ExitProgramCommand = new RelayCommand(_ => RaiseExitProgram());

        CurrentView = ConfigurationViewModel;

        _ = LoadDataAsync();
    }

    private void RaiseExitProgram()
    {
        ProgramExited?.Invoke(this, EventArgs.Empty);
    }

    private void RaiseOpenPlayerView()
    {
        if (ActivePack != null)
        {
            PlayerViewOpened?.Invoke(this, EventArgs.Empty);
        }
    }

    private void RaiseOpenConfigurationView(object? obj)
    {
        if (obj is QuestionPackViewModel selectedPack)
        {
            ActivePack = selectedPack;
        }
        ConfigurationViewOpened?.Invoke(this, EventArgs.Empty);
    }

    private async Task LoadDataAsync()
    {
        var savedModels = await _jsonData.LoadPacksAsync();

        System.Windows.Application.Current.Dispatcher.Invoke(() =>
        {
            foreach (var model in savedModels)
            {
                Packs.Add(new QuestionPackViewModel(model));
            }

            if (Packs.Count > 0)
            {
                ActivePack = Packs[0];

                ActivePack.IsEditingAllowed = CurrentView is not ViewModels.PlayerViewModel;

                ConfigurationViewModel?.RaiseActivePackChanged();
                ConfigurationViewModel?.OpenCreateNewPackDialogCommand?.RaiseCanExecuteChanged();
                ConfigurationViewModel?.OpenPackOptionsDialogCommand?.RaiseCanExecuteChanged();
                ConfigurationViewModel?.DeletePackCommand?.RaiseCanExecuteChanged();
            }
        });
    }

}
