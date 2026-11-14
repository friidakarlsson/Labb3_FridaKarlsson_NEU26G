
using Labb3_FridaKarlsson_NEU26G.Command;
using Labb3_FridaKarlsson_NEU26G.Data;
using Labb3_FridaKarlsson_NEU26G.Models;


namespace Labb3_FridaKarlsson_NEU26G.ViewModels;

internal class ConfigurationViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    public QuestionPackViewModel? ActivePack { get => _mainWindowViewModel?.ActivePack; } 


    public event EventHandler? CreateNewPackOpened;
    public event EventHandler? PackOptionsOpened;
    public event EventHandler? ImportQuestionsOpened;

    public RelayCommand? OpenCreateNewPackDialogCommand { get; }
    public RelayCommand? OpenPackOptionsDialogCommand { get; }
    public RelayCommand? DeletePackCommand { get; }
    public RelayCommand? OpenImportQuestionsDialogCommand { get; }

    public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;

        OpenCreateNewPackDialogCommand = new RelayCommand(_ => RaiseOpenCreateNewPack(), CanEdit);
        OpenPackOptionsDialogCommand = new RelayCommand(_ => RaiseOpenPackOptions(), CanEdit);
        DeletePackCommand = new RelayCommand(DeletePack, CanDeletePack);
        OpenImportQuestionsDialogCommand = new RelayCommand(_ => RaiseOpenImportQuestions(), CanEdit);
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
    private void RaiseOpenImportQuestions()
    {
        ImportQuestionsOpened?.Invoke(this, EventArgs.Empty);
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
            _mainWindowViewModel?.ActivePack = _mainWindowViewModel.Packs.FirstOrDefault();
        }
    }

    public async Task<int> ImportQuestionsAsync(ImportQuestionsViewModel importQuestionsViewModel)
    {
        var triviaData = new OpenTriviaData();
        var importedQuestions = await triviaData.GetQuestionsAsync(importQuestionsViewModel.NumberOfQuestions, importQuestionsViewModel.Category?.Id ?? 0, importQuestionsViewModel.TriviaDifficulty.ToString());

        if (ActivePack == null)
        {
            CreateNewPack();
        }

        foreach (var question in importedQuestions)
        {
            ActivePack?.AddQuestionToLists(question);
        }

        return importedQuestions.Count;
    }

    private bool CanDeletePack(object? arg)
    {
        return ActivePack != null && (_mainWindowViewModel?.CurrentView is not PlayerViewModel);
    }

    private bool CanEdit(object? obj)
    {
        if (ActivePack != null)
        {
            return ActivePack.IsEditingAllowed;
        }

        return true;
    }

}
