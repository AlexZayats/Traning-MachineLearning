using Microsoft.ML;
using System;
using System.IO;
using Traning.MachineLearning.ObjectDetection.Data;

namespace Traning.MachineLearning.ObjectDetection.Builder
{
    public class ConsumeModel3
    {
        private static Lazy<PredictionEngine<HumanData, HumanPrediction>> PredictionEngine = new Lazy<PredictionEngine<HumanData, HumanPrediction>>(CreatePredictionEngine);

        public static string MLNetModelPath = Path.GetFullPath("Builder\\MLModel3.zip");

        public static HumanPrediction Predict(HumanData input)
        {
            var result = PredictionEngine.Value.Predict(input);
            return result;
        }

        public static PredictionEngine<HumanData, HumanPrediction> CreatePredictionEngine()
        {
            var mlContext = new MLContext();
            var mlModel = mlContext.Model.Load(MLNetModelPath, out var modelInputSchema);
            var predEngine = mlContext.Model.CreatePredictionEngine<HumanData, HumanPrediction>(mlModel);
            return predEngine;
        }
    }
}
