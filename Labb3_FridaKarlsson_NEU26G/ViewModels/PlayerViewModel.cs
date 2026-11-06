
using Labb3_FridaKarlsson_NEU26G.Models;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Windows.Threading;

namespace Labb3_FridaKarlsson_NEU26G.ViewModels;

internal class PlayerViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;

    private Question? _currentQuestion;
    private int _currentQuestionNumber;
    private int _numbersOfQuestionsInCurrentPack;

    private readonly DispatcherTimer _timer;
    private Stopwatch _stopwatch = new();
    private bool _isQuizPlaying = false;

    private int _timeLimitCounter;

    public int TimeLimitCounter
    {
        get => _timeLimitCounter; 
        set 
        { 
            _timeLimitCounter = value;
            RaisePropertyChanged();
        }
    }


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
                ResetTimer();
            }
        }
    }
    public List<string> CurrentAnswers { get; set; } = new List<string>();


    public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;

        ShuffleQuestions();

        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;

    }
    public PlayerViewModel()
    {
        _timer = new DispatcherTimer();
    }

    public void StartQuiz()
    {
        _isQuizPlaying = true;

        CurrentQuestion = _mainWindowViewModel?.ActivePack?.Questions[0];
        CurrentQuestionNumber = 1;
        NumbersOfQuestionsInCurrentPack = _mainWindowViewModel?.ActivePack?.Questions.Count ?? 0;
    }

    private void NextQuestion()
    {
        int currentQuestionIndex = _mainWindowViewModel?.ActivePack?.Questions.IndexOf(CurrentQuestion) ?? 0;

        if ( currentQuestionIndex != NumbersOfQuestionsInCurrentPack -1)
        {
            CurrentQuestionNumber++;
            CurrentQuestion = _mainWindowViewModel?.ActivePack?.Questions[currentQuestionIndex + 1];
        }
        else
        {
            _timer.Stop();
            _stopwatch.Stop();
            // Ska något mer hända här??
        }
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        TimeLimitCounter = RemainingTime();
        if (TimeLimitCounter <= 0)
        {
            _timer.Stop();
            NextQuestion();
        }
    }
    public void ResetTimer()
    {
        if (_isQuizPlaying == true)
        {
            _timer.Stop();
            _stopwatch.Stop();
            TimeLimitCounter = _mainWindowViewModel?.ActivePack?.TimeLimitInSeconds ?? 30;
            _stopwatch.Reset();
            _stopwatch.Start();
            _timer.Start();
        }
    }

    public int RemainingTime()
    {
        return TimeLimitCounter = (_mainWindowViewModel?.ActivePack?.TimeLimitInSeconds ?? 30) - (int)Math.Round(_stopwatch.Elapsed.TotalSeconds);
    }

    public void ShuffleQuestions()
    {
        var selectedQuestions = _mainWindowViewModel?.ActivePack?.Questions;

        if (selectedQuestions != null)
        {
            ShuffleListIndex(selectedQuestions);
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

            ShuffleListIndex(allAnswers);

            CurrentAnswers = allAnswers;
            RaisePropertyChanged(nameof(CurrentAnswers));
        }
    }

    private void ShuffleListIndex<T>(IList<T> list)
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
