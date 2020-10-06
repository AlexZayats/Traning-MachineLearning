using Microsoft.ML.Data;

namespace Traning.MachineLearning.MoviesScorePrediction.Models
{
    public class MovieScoreInput
    {
        [LoadColumn(0)]
        public int MovieId;

        [LoadColumn(1)]
        public string Title;

        [LoadColumn(2)]
        [ColumnName("Label")]
        public bool Like;

        [LoadColumn(3)]
        [ColumnName("Keywords")]
        public string Keywords;

        [LoadColumn(4)]
        [ColumnName("Genres")]
        public string Genres;
    }
}
