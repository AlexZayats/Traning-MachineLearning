using Microsoft.ML.Data;

namespace Traning.MachineLearning.MoviesScorePrediction.Models
{
    public class MovieScoreInput
    {
        public int MovieId;

        public string Title;

        [ColumnName("Description")]
        public string Overview;

        [ColumnName("Keywords")]
        public string[] Keywords;

        [ColumnName("Genres")]
        public string[] Genres;

        [ColumnName("Label")]
        public bool Like;
    }
}
