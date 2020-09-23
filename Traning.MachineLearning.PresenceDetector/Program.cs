using AForge.Video;
using AForge.Video.DirectShow;
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;

namespace Traning.MachineLearning.PresenceDetector
{
    class Program
    {
        class HumanData
        {
            [LoadColumn(1)]
            public string Path { get; set; }

            [LoadColumn(2)]
            public bool Label { get; set; }
        }

        class HumanPrediction
        {
            [ColumnName("PredictedLabel")]
            public bool Prediction { get; set; }
        }

        static string dir = @"h:\data\human-detection";
        static Stopwatch FrameStopwatch = new Stopwatch();
        static int FrameIndex = 0;
        static bool Learnign = false;
        static PredictionEngine<HumanData, HumanPrediction> PredictionEngine;

        static void Main(string[] args)
        {
            FrameStopwatch.Start();
            var mlContext = new MLContext();
            var data = mlContext.Data.LoadFromTextFile<HumanData>($"{dir}\\data.csv", separatorChar: ',');
            var split = mlContext.Data.TrainTestSplit(data, 0.8);
            var tfm = @"h:\data\models\tensorflow_inception_graph.pb";
            var pipe = mlContext.Transforms.LoadImages("Image", dir, "Path")
                .Append(mlContext.Transforms.ResizeImages("ImageResized", 244, 244, "Image"))
                .Append(mlContext.Transforms.ExtractPixels("input", "ImageResized", interleavePixelColors: true))
                .Append(mlContext.Model.LoadTensorFlowModel(tfm).ScoreTensorFlowModel("softmax1_pre_activation", "input", true))
                .Append(mlContext.BinaryClassification.Trainers.LbfgsLogisticRegression(labelColumnName: "Label", featureColumnName: "softmax1_pre_activation"));

            var model = pipe.Fit(split.TrainSet);
            var test = model.Transform(split.TestSet);
            FrameStopwatch.Stop();
            var metrics = mlContext.BinaryClassification.Evaluate(test, "Label");
            Console.WriteLine($"Traning done. Accuracy: {metrics.Accuracy:P2}, Time: {FrameStopwatch.Elapsed}");

            //ctx.Model.Save(model, data.Schema, "model.zip");
            //DataViewSchema modelSchema;
            //TransformerModel = ctx.Model.Load("model.zip", out modelSchema);

            PredictionEngine = mlContext.Model.CreatePredictionEngine<HumanData, HumanPrediction>(model);
            var filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            var videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[0].MonikerString);
            videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
            videoCaptureDevice.Start();
            FrameStopwatch.Restart();

            Console.ReadKey();
        }

        private static void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (FrameStopwatch.Elapsed.TotalSeconds > 1)
            {
                var path = Learnign ? $"{dir}\\{FrameIndex}.png" : $"{dir}\\temp.png";
                FrameStopwatch.Restart();
                eventArgs.Frame.Save(path, ImageFormat.Png);
                FrameIndex++;
                var prediction = PredictionEngine.Predict(new HumanData { Path = path });
                Console.WriteLine(prediction.Prediction);
            }
        }
    }
}
