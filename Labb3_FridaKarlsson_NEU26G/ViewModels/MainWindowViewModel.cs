
using Labb3_FridaKarlsson_NEU26G.Command;
using Labb3_FridaKarlsson_NEU26G.Data;
using Labb3_FridaKarlsson_NEU26G.Models;
using System.Collections.ObjectModel;


namespace Labb3_FridaKarlsson_NEU26G.ViewModels;

internal class MainWindowViewModel : ViewModelBase
{
	private QuestionPackViewModel? _activePack;
    private object? _currentView;
    private readonly JsonData _jsonData;
    private bool _isFullscreen;


    public bool IsNotFullscreen => !IsFullscreen;
    public bool CanSelectPack => ActivePack != null && CurrentView is not ViewModels.PlayerViewModel;

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

            RaisePropertyChanged(nameof(CanSelectPack));
            UpdateCommands();
        }
    }

    public bool IsFullscreen
    {
        get => _isFullscreen;
        set 
        { 
            if (SetProperty(ref _isFullscreen, value))
            {
                RaisePropertyChanged(nameof(IsNotFullscreen));
                ExitFullscreenCommand.RaiseCanExecuteChanged();
            }            
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
                ConfigurationViewModel?.DeletePackCommand?.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(CanSelectPack));
            }
        }
	}

    public PlayerViewModel? PlayerViewModel { get;}
    public ConfigurationViewModel? ConfigurationViewModel { get; }


    public event EventHandler? PlayerViewOpened;
    public event EventHandler? ConfigurationViewOpened;
    public event EventHandler? ProgramExited;
    public event EventHandler? FullscreenToggled;


    public RelayCommand PlayQuizCommand { get; }
    public RelayCommand OpenConfigurationViewCommand { get; }
    public RelayCommand ExitProgramCommand { get; }
    public RelayCommand ToggleFullscreenCommand { get; }
    public RelayCommand ExitFullscreenCommand { get; }


    public MainWindowViewModel()
    {
        _jsonData = new JsonData();
        Packs = new ObservableCollection<QuestionPackViewModel>();
        ConfigurationViewModel = new ConfigurationViewModel(this);

		PlayerViewModel = new PlayerViewModel(this);

        PlayQuizCommand = new RelayCommand(_ => RaiseOpenPlayerView());
        OpenConfigurationViewCommand = new RelayCommand(obj => RaiseOpenConfigurationView(obj));
        ExitProgramCommand = new RelayCommand(_ => RaiseExitProgram());
        ToggleFullscreenCommand = new RelayCommand(_ => RaiseToggleFullscreen());
        ExitFullscreenCommand = new RelayCommand(_ =>  RaiseToggleFullscreen(), CanExitFullscreen);


        CurrentView = ConfigurationViewModel;

        _ = LoadDataAsync();
    }

    private void RaiseToggleFullscreen()
    {
        FullscreenToggled?.Invoke(this, EventArgs.Empty);
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

    private bool CanExitFullscreen(object? obj)
    {
        return IsFullscreen;
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
                UpdateCommands();
            }
        });
    }

    public async Task SaveDataAsync()
    {
        var modelsToSave = Packs.Select(p => p.Model).ToList();

        var packsData = new Labb3_FridaKarlsson_NEU26G.Data.JsonData();
        await packsData.SavePacksAsync(modelsToSave);
    }

    private void UpdateCommands()
    {
        ConfigurationViewModel?.OpenCreateNewPackDialogCommand?.RaiseCanExecuteChanged();
        ConfigurationViewModel?.OpenPackOptionsDialogCommand?.RaiseCanExecuteChanged();
        ConfigurationViewModel?.DeletePackCommand?.RaiseCanExecuteChanged();
        ConfigurationViewModel?.OpenImportQuestionsDialogCommand?.RaiseCanExecuteChanged();
    }
    
}
