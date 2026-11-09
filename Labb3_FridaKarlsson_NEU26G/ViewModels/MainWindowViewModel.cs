
using Labb3_FridaKarlsson_NEU26G.Command;
using Labb3_FridaKarlsson_NEU26G.Models;
using System.Collections.ObjectModel;

namespace Labb3_FridaKarlsson_NEU26G.ViewModels;

internal class MainWindowViewModel : ViewModelBase
{
	private QuestionPackViewModel? _activePack;
    private object? _currentView;


    public object? CurrentView 
    { 
        get => _currentView;
        set
        {
            SetProperty(ref _currentView, value);
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
        Packs = new ObservableCollection<QuestionPackViewModel>();
        ConfigurationViewModel = new ConfigurationViewModel(this);


        var questionPack = new QuestionPack("Mina frågor!", Difficulty.Easy);
        questionPack.Questions.Add(new Question("Vad heter Sveriges huvudstad?", "Stockholm", "Göteborg", "Malmö", "Kalmar"));
        questionPack.Questions.Add(new Question("Vad är 2 + 3?", "5", "10", "8", "1"));

        ActivePack = new QuestionPackViewModel(questionPack);
		PlayerViewModel = new PlayerViewModel(this);

        PlayQuizCommand = new RelayCommand(_ => RaiseOpenPlayerView());
        OpenConfigurationViewCommand = new RelayCommand(_ => RaiseOpenConfigurationView());
        ExitProgramCommand = new RelayCommand(_ => RaiseExitProgram());

        CurrentView = ConfigurationViewModel;
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

    private void RaiseOpenConfigurationView()
    {
        ConfigurationViewOpened?.Invoke(this, EventArgs.Empty);
    }

}
