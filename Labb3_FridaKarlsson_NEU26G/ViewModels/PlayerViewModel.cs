
using Labb3_FridaKarlsson_NEU26G.Models;
using System.Windows.Threading;

namespace Labb3_FridaKarlsson_NEU26G.ViewModels;

internal class PlayerViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    private Question? _currentQuestion;
    private int _currentQuestionNumber;
    private int _numbersOfQuestionsInCurrentPack;
    private int? counter = 0;

    public int CurrentQuestionNumber
    {
        get => _currentQuestionNumber;
        set
        {
            _currentQuestionNumber = value;
            RaisePropertyChanged();
        }
    }

    public int NumbersOfQuestionsInCurrentPack
    {
        get => _numbersOfQuestionsInCurrentPack;
        set
        {
            _numbersOfQuestionsInCurrentPack = value;
            RaisePropertyChanged();
        }
    }

    public Question? CurrentQuestion
    {
        get => _currentQuestion; 
        set 
        { 
            _currentQuestion = value;
            RaisePropertyChanged();

            if (value != null)
            {
                ShuffleAnswers();
            }
        }
    }
    public List<string> CurrentAnswers { get; set; } = new List<string>();


    public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;

        ShuffleQuestions();
        CurrentQuestion = _mainWindowViewModel?.ActivePack?.Questions[0];
        CurrentQuestionNumber = 1;
        NumbersOfQuestionsInCurrentPack = _mainWindowViewModel?.ActivePack?.Questions.Count ?? 0;
        counter = _mainWindowViewModel?.ActivePack?.TimeLimitInSeconds;

        DispatcherTimer timer = new();

        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += Timer_Tick;
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
         counter--;
    }

    public PlayerViewModel()
    {
        
    }


    public void ShuffleQuestions()
    {
        var selectedQuestions = _mainWindowViewModel?.ActivePack?.Questions;

        if (selectedQuestions != null)
        {
            ChangeListIndex(selectedQuestions);
        }
    }

    public void ShuffleAnswers()
    {
        if (CurrentQuestion != null)
        {
            List<string> allAnswers = new List<string>();
            allAnswers.Add(CurrentQuestion.CorrectAnswer);
            allAnswers.Add(CurrentQuestion.IncorrectAnswers[0]);
            allAnswers.Add(CurrentQuestion.IncorrectAnswers[1]);
            allAnswers.Add(CurrentQuestion.IncorrectAnswers[2]);

            ChangeListIndex(allAnswers);

            CurrentAnswers = allAnswers;
            RaisePropertyChanged(nameof(CurrentAnswers));
        }
    }

    private void ChangeListIndex<T>(IList<T> list)
    {
        int numberOfListItems = list.Count;

        Random random = new Random();

        for (int i = numberOfListItems - 1; i > 0; i--)
        {
            int newIndex = random.Next(0, i + 1);
            var temporaryItemHolder = list[i];

            list[i] = list[newIndex];
            list[newIndex] = temporaryItemHolder;
        }
    }

}
