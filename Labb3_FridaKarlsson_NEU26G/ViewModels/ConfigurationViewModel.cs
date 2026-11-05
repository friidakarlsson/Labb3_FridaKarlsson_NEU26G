


using Labb3_FridaKarlsson_NEU26G.Command;
using Labb3_FridaKarlsson_NEU26G.Dialogs;
using Labb3_FridaKarlsson_NEU26G.Models;
using System.Windows;

namespace Labb3_FridaKarlsson_NEU26G.ViewModels;

internal class ConfigurationViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;

    public QuestionPackViewModel? ActivePack { get => _mainWindowViewModel?.ActivePack; } 


    public event EventHandler? CreateNewPackOpened;
    public event EventHandler? PackOptionsOpened;

    public RelayCommand? OpenCreateNewPackDialogCommand { get; }
    public RelayCommand? OpenPackOptionsDialogCommand { get; }
    public RelayCommand? DeletePackCommand { get; }

    public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;

        OpenCreateNewPackDialogCommand = new RelayCommand(_ => RaiseOpenCreateNewPack());
        OpenPackOptionsDialogCommand = new RelayCommand(_ => RaiseOpenPackOptions());
        DeletePackCommand = new RelayCommand(DeletePack, CanDeletePack);
    }

    public ConfigurationViewModel()
    {
        
    }

    public void RaiseActivePackChanged() => RaisePropertyChanged(nameof(ActivePack));
    private void RaiseOpenPackOptions()
    {
        PackOptionsOpened?.Invoke(this, EventArgs.Empty);
    }

    private void RaiseOpenCreateNewPack()
    {
        CreateNewPackOpened?.Invoke(this, EventArgs.Empty);
    }

    public void CreateNewPack()
    {
        var newPack = new QuestionPack("<PackName>", Difficulty.Medium, 60);
        var newPackViewModel = new QuestionPackViewModel(newPack);

        _mainWindowViewModel?.Packs.Add(newPackViewModel);
        _mainWindowViewModel?.ActivePack = newPackViewModel;

        RaiseActivePackChanged();    
    }

    private void DeletePack(object? obj)
    {
        if (ActivePack != null)
        {

            _mainWindowViewModel?.Packs.Remove(ActivePack);
        }
    }
    private bool CanDeletePack(object? arg)
    {
        return ActivePack != null;

    }

}
