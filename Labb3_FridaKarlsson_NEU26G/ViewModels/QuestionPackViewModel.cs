
using Labb3_FridaKarlsson_NEU26G.Command;
using Labb3_FridaKarlsson_NEU26G.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Labb3_FridaKarlsson_NEU26G.ViewModels;

internal class QuestionPackViewModel : ViewModelBase
{
    private readonly QuestionPack _model;
    private Question? _activeQuestion;


    public QuestionPackViewModel(QuestionPack model)
    {
        _model = model;
        Questions = new ObservableCollection<Question>(_model.Questions);

        AddNewQuestionCommand = new RelayCommand(_ => AddNewQuestion(), CanAddQuestion);
        DeleteQuestionCommand = new RelayCommand(DeleteQuestion, CanDeleteQuestion);
    }



    public string Name
    {
        get => _model.Name; 
        set 
        {
            if (_model.Name == value)
                return;

            _model.Name = value;
            RaisePropertyChanged();
        }
    }

    public Difficulty Difficulty
    {
        get => _model.Difficulty; 
        set
        {
            if (_model.Difficulty == value)
                return;

            _model.Difficulty = value;
            RaisePropertyChanged();
        }
    }

    public int TimeLimitInSeconds
    {
        get => _model.TimeLimitInSeconds;
        set
        {
            if (_model.TimeLimitInSeconds == value)
                return;

            _model.TimeLimitInSeconds = value;
            RaisePropertyChanged();
        }
    }

    public ObservableCollection<Question> Questions { get; }

    public Question? ActiveQuestion
    {
        get => _activeQuestion;
        set
        {
            if (SetProperty(ref _activeQuestion, value))
            {
                DeleteQuestionCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public QuestionPack Model => _model;

    private bool _isEditingAllowed = true;

    public bool IsEditingAllowed
    {
        get => _isEditingAllowed; 
        set 
        {
            if (SetProperty(ref _isEditingAllowed, value))
            {
                AddNewQuestionCommand?.RaiseCanExecuteChanged();
                DeleteQuestionCommand?.RaiseCanExecuteChanged();
            }
        }
    }


    public Difficulty[] Difficulties => (Difficulty[])Enum.GetValues(typeof(Difficulty));



    public RelayCommand AddNewQuestionCommand { get; }
    public RelayCommand DeleteQuestionCommand { get; }


    public void AddNewQuestion()
    {
        var newQuestion = new Question("New question", "Correct answer", "First incorrect answer", "Second incorrect answer", "Third incorrect answer");

        Questions.Add(newQuestion);
        _model.Questions.Add(newQuestion);
    }

    private bool CanAddQuestion(object? obj)
    {
        return IsEditingAllowed;
    }

    public void DeleteQuestion(object? obj)
    {
        if (ActiveQuestion != null)
        {
            _model.Questions.Remove(ActiveQuestion);
            Questions.Remove(ActiveQuestion);
        }
    }
    private bool CanDeleteQuestion(object? obj)
    {
        return ActiveQuestion != null && IsEditingAllowed;
    }
}
