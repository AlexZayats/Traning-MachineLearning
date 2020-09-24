using Microsoft.ML.Data;

namespace Traning.MachineLearning.ObjectDetection.Data
{
    public class HumanData
    {
        [LoadColumn(1)]
        public string Path { get; set; }

        [LoadColumn(2)]
        public bool Label { get; set; }
    }
}
