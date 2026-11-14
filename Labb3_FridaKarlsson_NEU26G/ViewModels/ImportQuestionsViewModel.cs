using Labb3_FridaKarlsson_NEU26G.Models;
using Labb3_FridaKarlsson_NEU26G.Data;
using System.Collections.ObjectModel;

namespace Labb3_FridaKarlsson_NEU26G.ViewModels;


internal class ImportQuestionsViewModel : ViewModelBase
{
	public ObservableCollection<TriviaCategory> Categories { get; } = [];
    public TriviaDifficulty[] TriviaDifficulties => (TriviaDifficulty[])Enum.GetValues(typeof(TriviaDifficulty));


	private TriviaCategory? _category;

	public TriviaCategory? Category
	{
		get => _category; 
		set 
		{ 
			_category = value;
			RaisePropertyChanged();
		}
	}

	private TriviaDifficulty _triviaDifficulty;

	public TriviaDifficulty TriviaDifficulty
	{
		get =>  _triviaDifficulty; 
		set
		{ 
			_triviaDifficulty = value;
			RaisePropertyChanged();
		}
	}

	private int _numberOfQuestions = 10;

	public int NumberOfQuestions
	{
		get => _numberOfQuestions;
		set 
		{ 
			_numberOfQuestions = value;
			RaisePropertyChanged();
		}
	}


	public async Task LoadCategoriesAsync()
	{
		OpenTriviaData openTriviaData = new OpenTriviaData();
		var categories = await openTriviaData.GetTriviaCategoriesAsync();

		Categories.Clear();

		foreach (var category in categories)
		{
			Categories.Add(category);
		}

		Category = Categories.FirstOrDefault();
	}




}
