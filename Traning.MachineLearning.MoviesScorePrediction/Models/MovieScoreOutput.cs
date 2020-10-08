using Microsoft.ML.Data;

namespace Traning.MachineLearning.MoviesScorePrediction.Models
{
    public class MovieScoreOutput
    {
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }

        [ColumnName("Probability")]
        public float Probability { get; set; }
    }
}
