using Microsoft.ML.Data;

namespace Traning.MachineLearning.ObjectDetection.Data
{
    public class HumanPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }
    }
}
