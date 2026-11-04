
using Labb3_FridaKarlsson_NEU26G.Command;
using Labb3_FridaKarlsson_NEU26G.Models;
using System.Collections.ObjectModel;

namespace Labb3_FridaKarlsson_NEU26G.ViewModels;

internal class MainWindowViewModel :ViewModelBase
{
	private QuestionPackViewModel? _activePack;

    public object CurrentView { get; set; }
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

    public event EventHandler PlayQuiz;
    public RelayCommand PlayQuizCommand { get; }


    public MainWindowViewModel()
    {
        Packs = new ObservableCollection<QuestionPackViewModel>();
        ConfigurationViewModel = new ConfigurationViewModel(this);
        CurrentView = ConfigurationViewModel;

        var questionPack = new QuestionPack("Mina frågor!", Difficulty.Easy);
        questionPack.Questions.Add(new Question("Vad heter Sveriges huvudstad?", "Stockholm", "Göteborg", "Malmö", "Kalmar"));
        questionPack.Questions.Add(new Question("Vad är 2 + 3?", "5", "10", "8", "1"));

        ActivePack = new QuestionPackViewModel(questionPack);
		PlayerViewModel = new PlayerViewModel(this);

        PlayQuizCommand = new RelayCommand(_ => RaisePlayQuiz());
    }

    private void RaisePlayQuiz()
    {
        if (ActivePack != null)
        {
            PlayQuiz.Invoke(this, EventArgs.Empty);
        }
    }

}
