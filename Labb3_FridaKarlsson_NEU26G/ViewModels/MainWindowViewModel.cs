
using Labb3_FridaKarlsson_NEU26G.Command;
using Labb3_FridaKarlsson_NEU26G.Models;
using System.Collections.ObjectModel;

namespace Labb3_FridaKarlsson_NEU26G.ViewModels;

internal class MainWindowViewModel :ViewModelBase
{
    public ObservableCollection<QuestionPackViewModel> Packs { get; }
    
	private QuestionPackViewModel? _activePack;
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


    public MainWindowViewModel()
    {
        Packs = new ObservableCollection<QuestionPackViewModel>();
		PlayerViewModel = new PlayerViewModel(this);
        ConfigurationViewModel = new ConfigurationViewModel(this);

        var questionPack = new QuestionPack("Mina frågor!", Difficulty.Easy);
        questionPack.Questions.Add(new Question("Vad heter Sveriges huvudstad?", "Stockholm", "Göteborg", "Malmö", "Kalmar"));
        questionPack.Questions.Add(new Question("Vad är 2 + 3?", "5", "10", "8", "1"));

        ActivePack = new QuestionPackViewModel(questionPack);
    }



}
