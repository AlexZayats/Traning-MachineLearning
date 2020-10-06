using JARVIS.Core.Networking.Clients;
using JARVIS.SDK.MovieDatabase;
using JARVIS.SDK.MovieDatabase.Models;
using Microsoft.ML;
using Microsoft.ML.Trainers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Traning.MachineLearning.MoviesScorePrediction.Models;

namespace Traning.MachineLearning.MoviesScorePrediction
{
    public partial class MainWindow : Window
    {
        private Dictionary<int, string> Genres;
        private List<Movie> TestMovies = new List<Movie>();
        private List<Movie> TrainMovies = new List<Movie>();

        public MainWindow()
        {
            InitializeComponent();
            Load();
        }

        public async void Load()
        {
            var client = new ApiClient();
            /*
            for(var i = 1; i <= 200; i++)
            {
                TrainMovies.AddRange((await client.DiscoverMovie(page: i)).Results);
            }
            foreach(var movie in TrainMovies)
            {
                movie.Keywords = (await client.GetMovieKeywords(movie.Id)).Keywords;
                if (movie.Keywords?.Any() ?? false)
                {
                    await File.AppendAllTextAsync($"h:\\data\\movies2\\{movie.Id}.json", JsonConvert.SerializeObject(movie));
                }
            }
            */
            for (var i = 1; i <= 5; i++)
            {
                TestMovies.AddRange((await client.DiscoverMovie(page: i, sortBy: "vote_count.desc")).Results);//vote_average.desc vote_count.desc
            }
            foreach (var movie in TestMovies)
            {
                movie.Keywords = (await client.GetMovieKeywords(movie.Id)).Keywords;
            }
            Genres = (await client.GetGenresMovie()).Genres.ToDictionary(k => k.Id, v => v.Name);
            TrainMovies.AddRange(Directory.GetFiles(@"h:\data\movies2").Select(x => File.ReadAllText(x)).Select(x => JsonConvert.DeserializeObject<Movie>(x)).Where(x => !TestMovies.Any(e => e.Id == x.Id)));
            Train();
            Movies.ItemsSource = TestMovies;
        }

        public void Train()
        {
            var context = new MLContext();
            var trainInputs = ConvertToInputs(TrainMovies);
            var data = context.Data.LoadFromEnumerable(trainInputs);
            var trainTestData = context.Data.TrainTestSplit(data, testFraction: 0.2);

            /*
            var pipeline = context.Transforms.Text.FeaturizeText(outputColumnName: "FeaturizedKeywords", inputColumnName: "Keywords")
                .Append(context.Transforms.Text.FeaturizeText(outputColumnName: "FeaturizedGenres", inputColumnName: "Genres"))
                .Append(context.Transforms.Concatenate("Features", "FeaturizedKeywords", "FeaturizedGenres"))
                .Append(context.BinaryClassification.Trainers.LightGbm());
            */

            /*
            var pipeline = context.Transforms.Text.FeaturizeText(outputColumnName: "Features", inputColumnName: "Keywords")
               .Append(context.BinaryClassification.Trainers.LightGbm());
            */

            var pipeline = context.Transforms.Text.FeaturizeText(outputColumnName: "Features", inputColumnName: "Keywords")
               .Append(context.BinaryClassification.Trainers.SdcaLogisticRegression());

            var model = pipeline.Fit(trainTestData.TrainSet);
            var predictions = model.Transform(trainTestData.TestSet);
            var metrics = context.BinaryClassification.Evaluate(predictions, "Label");
            Info.Text = $"Accuracy: {metrics.Accuracy:P2}";
            var predictor = context.Model.CreatePredictionEngine<MovieScoreInput, MovieScoreOutput>(model);
            var testInputs = ConvertToInputs(TestMovies);
            foreach (var input in testInputs)
            {
                var output = predictor.Predict(input);
                var movie = TestMovies.Find(x => x.Id == input.MovieId);
                var exists = TrainMovies.Any(x => x.Id == movie.Id);
                if (!exists)
                {
                    movie.Prediction = $"Prediction: {output.Prediction}, Probability: {output.Probability:P2}";
                }
            }
        }

        private IEnumerable<MovieScoreInput> ConvertToInputs(IEnumerable<Movie> movies)
        {
            return movies.Where(x => x.Keywords?.Any() ?? false).Select(x => new MovieScoreInput
            {
                MovieId = x.Id,
                Like = x.VoteAverage >= 6,
                Title = x.Title,
                Keywords = string.Join(" ", x.Keywords.Select(e => e.Name)),
                Genres = string.Join(" ", x.GenreIds.Select(e => Genres[e])),
            }).ToArray();
        }
    }
}
