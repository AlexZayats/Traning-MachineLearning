using Microsoft.ML.Data;

namespace Traning.MachineLearning.ObjectDetection.Data
{
    public class ObjectDetectionPrediction
    {
        [ColumnName("grid")]
        public float[] PredictedLabels;
    }
}
