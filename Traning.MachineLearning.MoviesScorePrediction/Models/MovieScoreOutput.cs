using Microsoft.ML.Data;

namespace Traning.MachineLearning.MoviesScorePrediction.Models
{
    public class MovieScoreOutput
    {
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }

        public float Probability { get; set; }
    }
}
