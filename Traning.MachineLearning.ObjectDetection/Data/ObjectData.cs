using Microsoft.ML.Data;

namespace Traning.MachineLearning.ObjectDetection.Data
{
    public class ObjectData
    {
        [ColumnName("Path")]
        public string Path { get; set; }

        [ColumnName("Label")]
        public float Label { get; set; }
    }
}
