using AForge.Video;
using AForge.Video.DirectShow;
using Microsoft.ML;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Traning.MachineLearning.ObjectDetection.Builder;
using Traning.MachineLearning.ObjectDetection.Data;
using Traning.MachineLearning.ObjectDetection.Helpers;
using Traning.MachineLearning.ObjectDetection.YoloParser;

namespace Traning.MachineLearning.ObjectDetection
{
    public partial class MainWindow : Window
    {
        private VideoCaptureDevice _video;
        private PredictionEngine<HumanData, HumanPrediction> _predictionEngine1;
        private PredictionEngine<ObjectData, ObjectDetectionPrediction> _predictionEngine3;
        private string _imagesFolder1 = @"h:\data\human-detection";
        private YoloOutputParser _parser = new YoloOutputParser();
        private Stopwatch _stopwatch = new Stopwatch();

        private string[] _builder2Results = new[] { "buildings", "forest", "glacier", "mountain", "sea", "street" };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Train1()
        {
            var tfm = @"h:\data\models\tensorflow_inception_graph.pb";
            var mlContext = new MLContext();
            var data = mlContext.Data.LoadFromTextFile<HumanData>($"{_imagesFolder1}\\data.csv", separatorChar: ',');
            var split = mlContext.Data.TrainTestSplit(data, 0.8);
            var pipe = mlContext.Transforms.LoadImages("Image", _imagesFolder1, "Path")
                .Append(mlContext.Transforms.ResizeImages("ImageResized", 244, 244, "Image"))
                .Append(mlContext.Transforms.ExtractPixels("input", "ImageResized", interleavePixelColors: true))
                .Append(mlContext.Model.LoadTensorFlowModel(tfm).ScoreTensorFlowModel("softmax1_pre_activation", "input", true))
                .Append(mlContext.BinaryClassification.Trainers.LbfgsLogisticRegression(labelColumnName: "Label", featureColumnName: "softmax1_pre_activation"));

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var model = pipe.Fit(split.TrainSet);
            var test = model.Transform(split.TestSet);
            stopwatch.Stop();
            var metrics = mlContext.BinaryClassification.Evaluate(test, "Label");
            Dispatcher.Invoke(() =>
            {
                Info1.Content = $"Traning done. Accuracy: {metrics.Accuracy:P2}, Time: {stopwatch.Elapsed}";
            });
            _predictionEngine1 = mlContext.Model.CreatePredictionEngine<HumanData, HumanPrediction>(model);
        }

        private void T1_Start_Button_Click(object sender, RoutedEventArgs e)
        {
            if (_predictionEngine1 == null)
            {
                Task.Run(Train1);
            }

            var filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            _video = new VideoCaptureDevice(filterInfoCollection[0].MonikerString);
            _video.NewFrame += _video1_NewFrame;
            _video.Start();
        }

        private void T1_Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            _video?.SignalToStop();
            Image1.Source = null;
            _predictionEngine1 = null;
        }

        private void _video1_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            var path = $"{_imagesFolder1}\\temp.png";
            eventArgs.Frame.Save(path, ImageFormat.Png);
            if (_predictionEngine1 != null)
            {
                var prediction = _predictionEngine1.Predict(new HumanData { Path = path });
                using (var gr = Graphics.FromImage(eventArgs.Frame))
                {
                    gr.SmoothingMode = SmoothingMode.AntiAlias;
                    using (var thick_pen = new Pen(prediction.Prediction ? Color.Green : Color.Red, 5))
                    {
                        gr.DrawRectangle(thick_pen, 0, 0, 640, 480);
                    }
                }
            }
            Dispatcher.Invoke(() =>
            {
                Image1.Source = eventArgs.Frame.ToBitmapImage();
            });
        }

        private void T2_Start_Button_Click(object sender, RoutedEventArgs e)
        {
            var filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            _video = new VideoCaptureDevice(filterInfoCollection[0].MonikerString);
            _video.NewFrame += _video2_NewFrame;
            _video.Start();
        }

        private void T2_Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            _video?.SignalToStop();
            Image2.Source = null;
        }

        private void _video2_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            var path = $"{_imagesFolder1}\\temp.png";
            eventArgs.Frame.Save(path, ImageFormat.Png);
            var prediction = ConsumeModel1.Predict(new ModelInput { ImageSource = path });
            using (var gr = Graphics.FromImage(eventArgs.Frame))
            {
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                using (var thick_pen = new Pen(prediction.Prediction == "1" ? Color.Green : Color.Red, 5))
                {
                    gr.DrawRectangle(thick_pen, 0, 0, 640, 480);
                }
            }
            Dispatcher.Invoke(() =>
            {
                Info2.Content = $"Score: {prediction.Score[1]:P2}";
                Image2.Source = eventArgs.Frame.ToBitmapImage();
            });
        }

        private void T3_Start_Button_Click(object sender, RoutedEventArgs e)
        {
            var onnx = @"h:\data\models\tinyyolov2-8.onnx";
            var mlContext = new MLContext();
            var pipe = mlContext.Transforms.LoadImages("image", _imagesFolder1, "Path")
                .Append(mlContext.Transforms.ResizeImages("image", 416, 416))
                .Append(mlContext.Transforms.ExtractPixels("image"))
                .Append(mlContext.Transforms.ApplyOnnxModel(modelFile: onnx, outputColumnNames: new[] { "grid" }, inputColumnNames: new[] { "image" }));

            var data = mlContext.Data.LoadFromEnumerable(new List<ObjectData>());
            _predictionEngine3 = mlContext.Model.CreatePredictionEngine<ObjectData, ObjectDetectionPrediction>(pipe.Fit(data));

            var filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            _video = new VideoCaptureDevice(filterInfoCollection[0].MonikerString);
            _video.NewFrame += _video3_NewFrame;
            _video.Start();
            _stopwatch.Restart();
        }

        private void T3_Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            _video?.SignalToStop();
            Image3.Source = null;
            _stopwatch.Stop();
        }

        private IList<YoloBoundingBox> _boundingBoxes;

        private void _video3_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            var path = $"{_imagesFolder1}\\temp.png";
            eventArgs.Frame.Save(path, ImageFormat.Png);
            if (_predictionEngine3 != null)
            {
                if (_stopwatch.ElapsedMilliseconds > 200)
                {
                    var prediction = _predictionEngine3.Predict(new ObjectData { Path = path });
                    _boundingBoxes = _parser.FilterBoundingBoxes(_parser.ParseOutputs(prediction.PredictedLabels), 5, .5F);

                    _stopwatch.Restart();
                }
                if (_boundingBoxes != null)
                {
                    using (var gr = Graphics.FromImage(eventArgs.Frame))
                    {
                        gr.SmoothingMode = SmoothingMode.AntiAlias;
                        foreach (var boundingBox in _boundingBoxes)
                        {
                            using (var thick_pen = new Pen(boundingBox.BoxColor, 2))
                            {
                                var drawFont = new Font("Arial", 12);
                                var drawBrush = new SolidBrush(boundingBox.BoxColor);
                                gr.DrawString($"{boundingBox.Label} [{boundingBox.Confidence:P2}]", drawFont, drawBrush, boundingBox.Rect.X * 640 / 416, boundingBox.Rect.Y * 480 / 416 - 20);
                                gr.DrawRectangle(thick_pen, boundingBox.Rect.X * 640 / 416, boundingBox.Rect.Y * 480 / 416, boundingBox.Rect.Width * 640 / 416, boundingBox.Rect.Height * 480 / 416);
                            }
                        }
                    }
                }
            }
            Dispatcher.Invoke(() =>
            {
                Image3.Source = eventArgs.Frame.ToBitmapImage();
            });
        }

        private void T4_Browse_Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() ?? false)
            {
                var prediction = ConsumeModel2.Predict(new ModelInput { ImageSource = dialog.FileName });
                using (var image = Image.FromStream(dialog.OpenFile()))
                using (var bitmap = new Bitmap(image))
                {
                    var index = Array.IndexOf(_builder2Results, prediction.Prediction);
                    Dispatcher.Invoke(() =>
                    {
                        Info4.Content = $"{prediction.Prediction} ({prediction.Score[index]:P2})";
                        Image4.Source = bitmap.ToBitmapImage();
                    });
                }
            }
        }
    }
}
