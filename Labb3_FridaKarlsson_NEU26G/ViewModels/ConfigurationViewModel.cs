


using Labb3_FridaKarlsson_NEU26G.Command;
using Labb3_FridaKarlsson_NEU26G.Dialogs;
using Labb3_FridaKarlsson_NEU26G.Models;
using System.Windows;

namespace Labb3_FridaKarlsson_NEU26G.ViewModels;

internal class ConfigurationViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;

    public QuestionPackViewModel? ActivePack { get => _mainWindowViewModel?.ActivePack; }

    public void RaiseActivePackChanged() => RaisePropertyChanged(nameof(ActivePack));

    public event EventHandler OpenCreateNewPack;
    public event EventHandler OpenPackOptions;

    public RelayCommand OpenCreateNewPackDialogCommand { get; }
    public RelayCommand OpenPackOptionsDialogCommand { get; }
    public RelayCommand CreateNewPackCommand { get; }
    public RelayCommand DeletePackCommand { get; }

    public ConfigurationViewModel()
    {
        
    }
    public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;

        OpenCreateNewPackDialogCommand = new RelayCommand(_ => OpenCreateNewPackDialog());
        OpenPackOptionsDialogCommand = new RelayCommand(_ => OpenPackOptionsDialog());
        //CreateNewPackCommand = new RelayCommand(_ => CreateNewPack());
        DeletePackCommand = new RelayCommand(DeletePack, CanDeletePack);



    }

    private void OpenPackOptionsDialog()
    {
        OpenPackOptions.Invoke(this, EventArgs.Empty);
    }

    private void OpenCreateNewPackDialog()
    {
        OpenCreateNewPack.Invoke(this, EventArgs.Empty);
    }

    //private void CreateNewPack()
    //{
    //    var newPack = new QuestionPack("<PackName>", Difficulty.Medium, 60);
    //    var newPackViewModel = new QuestionPackViewModel(newPack);

    //    _mainWindowViewModel?.Packs.Add(newPackViewModel);
    //    _mainWindowViewModel?.ActivePack = newPackViewModel;
    //}

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
