
using Labb3_FridaKarlsson_NEU26G.Command;
using Labb3_FridaKarlsson_NEU26G.Models;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata.Ecma335;
using System.Windows.Threading;

namespace Labb3_FridaKarlsson_NEU26G.ViewModels;

internal class PlayerViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    private readonly DispatcherTimer _timer;
    private readonly Stopwatch _stopwatch = new();
    private bool _isQuizPlaying = false;
    private int _timeLimitCounter;
    private int _currentQuestionNumber;
    private int _numbersOfQuestionsInCurrentPack;
    private Question? _currentQuestion;
    private bool _hasAnswered;
    private AnswerOption _selectedAnswer;
    private int _score;
    private bool _isQuizOver = false;



    public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;

        SelectAnswerCommand = new RelayCommand(obj => ShowAnswer(obj));
        RestartQuizCommand = new RelayCommand(_ => StartQuiz());

        ShowCorrectAnswerIcon = new List<bool> { false, false, false, false };
        ShowIncorrectAnswerIcon = new List<bool> { false, false, false, false };


        ShuffleQuestions();

        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;

    }


    public PlayerViewModel()
    {
        _timer = new DispatcherTimer();
    }



    public RelayCommand? SelectAnswerCommand { get; }
    public RelayCommand? RestartQuizCommand { get; }


    
    public bool HasAnswered
    {
        get => _hasAnswered;
        set
        {
            _hasAnswered = value;
            RaisePropertyChanged();
        }
    }

    public AnswerOption SelectedAnswer
    {
        get => _selectedAnswer;
        set
        {
            _selectedAnswer = value;
            RaisePropertyChanged();
        }
    }

    public int Score
    {
        get => _score; 
        set 
        {
            _score = value;
            RaisePropertyChanged();
        }
    }
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
                ResetRound();
            }
        }
    }

    public bool IsQuizOver
    {
        get => _isQuizOver;
        set 
        { 
            _isQuizOver = value;
            RaisePropertyChanged();
        }
    }



    public List<AnswerOption>? CurrentAnswers { get; set; }
    public List<bool>? ShowCorrectAnswerIcon { get; private set; }
    public List<bool>? ShowIncorrectAnswerIcon { get; private set; }




    public void StartQuiz()
    {
        Score = 0;
        IsQuizOver = false;
        _isQuizPlaying = true;

        CurrentQuestion = _mainWindowViewModel?.ActivePack?.Questions[0];
        CurrentQuestionNumber = 1;
        NumbersOfQuestionsInCurrentPack = _mainWindowViewModel?.ActivePack?.Questions.Count ?? 0;
    }

    public void ResetRound()
    {
        if (_isQuizPlaying == true)
        {
            _timer.Stop();
            _stopwatch.Stop();
            TimeLimitCounter = _mainWindowViewModel?.ActivePack?.TimeLimitInSeconds ?? 30;
            _stopwatch.Reset();
            _stopwatch.Start();
            _timer.Start();

            ResetAnswerIcons();
        }
    }

    public int RemainingTime()
    {
        return TimeLimitCounter = (_mainWindowViewModel?.ActivePack?.TimeLimitInSeconds ?? 30) - (int)Math.Round(_stopwatch.Elapsed.TotalSeconds);
    }
    private async void ShowAnswer(object? obj)
    {
        _timer.Stop();
        _stopwatch.Stop();

        if (obj is AnswerOption choosenAnswer)
        {
            SelectedAnswer = choosenAnswer;
            HasAnswered = true;

            if (choosenAnswer.IsCorrect == true)
            {
                Score++;
            }
        }
        else
        {
            HasAnswered = true;
        }

        ShowAnswerIcons();
        await Task.Delay(3000);

        NextQuestion();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        TimeLimitCounter = RemainingTime();
        if (TimeLimitCounter <= 0)
        {
            _timer.Stop();
            ShowAnswer(null);
        }
    }

    private void NextQuestion()
    {
        if (CurrentQuestion == null) return;

        int currentQuestionIndex = _mainWindowViewModel?.ActivePack?.Questions.IndexOf(CurrentQuestion) ?? 0;

        if (currentQuestionIndex != NumbersOfQuestionsInCurrentPack - 1)
        {
            CurrentQuestionNumber++;
            CurrentQuestion = _mainWindowViewModel?.ActivePack?.Questions[currentQuestionIndex + 1];
        }
        else
        {
            _timer.Stop();
            _stopwatch.Stop();
            _isQuizPlaying = false;
            IsQuizOver = true;
        }
    }
    private void ShowAnswerIcons()
    {
        if (CurrentAnswers == null || SelectedAnswer.Text == null) return;

        var temporaryCorrect = new List<bool> { false, false, false, false };
        var temporaryIncorrect = new List<bool> { false, false, false, false };

        for (int i = 0; i <= 3; i++)
        {
            temporaryCorrect[i] = CurrentAnswers[i].IsCorrect;
            temporaryIncorrect[i] = (CurrentAnswers[i].Text == SelectedAnswer.Text && !CurrentAnswers[i].IsCorrect);
        }

        ShowCorrectAnswerIcon = temporaryCorrect;
        ShowIncorrectAnswerIcon = temporaryIncorrect;

        RaisePropertyChanged(nameof(ShowCorrectAnswerIcon));
        RaisePropertyChanged(nameof(ShowIncorrectAnswerIcon));
    }

    private void ResetAnswerIcons()
    {
        ShowCorrectAnswerIcon = new List<bool> { false, false, false, false, };
        ShowIncorrectAnswerIcon = new List<bool> { false, false, false, false, };

        RaisePropertyChanged(nameof(ShowCorrectAnswerIcon));
        RaisePropertyChanged(nameof(ShowIncorrectAnswerIcon));
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
            List<AnswerOption> allAnswers = new List<AnswerOption>();
            allAnswers.Add(new AnswerOption(CurrentQuestion.CorrectAnswer, true));
            allAnswers.Add(new AnswerOption(CurrentQuestion.IncorrectAnswers[0], false));
            allAnswers.Add(new AnswerOption(CurrentQuestion.IncorrectAnswers[1], false));
            allAnswers.Add(new AnswerOption(CurrentQuestion.IncorrectAnswers[2], false));

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

public struct AnswerOption
{
    public AnswerOption(string text, bool isCorrect)
    {
        Text = text;
        IsCorrect = isCorrect;
    }

    public string Text { get; set; }
    public bool IsCorrect { get; set; }

}
