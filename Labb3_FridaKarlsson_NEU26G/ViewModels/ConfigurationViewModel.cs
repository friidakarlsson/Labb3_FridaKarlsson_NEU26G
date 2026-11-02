
using Labb3_FridaKarlsson_NEU26G.Command;
using Labb3_FridaKarlsson_NEU26G.Models;

namespace Labb3_FridaKarlsson_NEU26G.ViewModels;

internal class ConfigurationViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;

    public QuestionPackViewModel? ActivePack { get => _mainWindowViewModel?.ActivePack; }

    public void RaiseActivePackChanged() => RaisePropertyChanged(nameof(ActivePack));


    public ConfigurationViewModel()
    {
        
    }
    public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;



    }



}
