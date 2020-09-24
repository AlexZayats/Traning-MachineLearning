using Microsoft.ML.Data;

namespace Traning.MachineLearning.ObjectDetection.Data
{
    public class ObjectDetectionPrediction
    {
        [ColumnName("PredictedLabel")]
        public float Prediction { get; set; }
    }
}
