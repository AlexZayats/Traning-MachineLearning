using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Traning.MachineLearning.SimpleRegression
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new MLContext();
            var data = context.Data.LoadFromTextFile<Input>(@"c:\data\AB_NYC_2019_TOP_1000.csv", separatorChar: ',', hasHeader: true);
            var split = context.Data.TrainTestSplit(data);
            var pipeline = context.Transforms.Categorical.OneHotEncoding(outputColumnName: "NeighbourhoodGroupEncoded", inputColumnName: "NeighbourhoodGroup")
                .Append(context.Transforms.Categorical.OneHotEncoding(outputColumnName: "NeighbourhoodEncoded", inputColumnName: "Neighbourhood"))
                .Append(context.Transforms.Categorical.OneHotEncoding(outputColumnName: "RoomTypeEncoded", inputColumnName: "RoomType"))
                .Append(context.Transforms.Concatenate(outputColumnName: "Features", "NeighbourhoodGroupEncoded", "NeighbourhoodEncoded", "RoomTypeEncoded", "MinimumNights"))
                .Append(context.Regression.Trainers.LightGbm());
                //.Append(context.Regression.Trainers.FastForest(numberOfTrees: 200, minimumExampleCountPerLeaf: 4));
            var model = pipeline.Fit(split.TrainSet);
            
            /*
            var predictions = model.Transform(split.TestSet);
            var metrics = context.Regression.Evaluate(predictions);
            Console.WriteLine($"R2 score: {metrics.RSquared:0.##}");
            */

            var predictor = context.Model.CreatePredictionEngine<Input, Output>(model);

            var inputs = new List<Input>();
            inputs.Add(new Input
            {
                NeighbourhoodGroup = "Queens",
                Neighbourhood = "Elmhurst",
                RoomType = "Entire home/apt",
                MinimumNights = 60,
                Label = 100
            });
            inputs.Add(new Input
            {
                NeighbourhoodGroup = "Queens",
                Neighbourhood = "Long Island City",
                RoomType = "Private room",
                MinimumNights = 14,
                Label = 70
            });
            inputs.Add(new Input
            {
                NeighbourhoodGroup = "Manhattan",
                Neighbourhood = "Morningside Heights",
                RoomType = "Private room",
                MinimumNights = 1,
                Label = 98
            });
            inputs.Add(new Input
            {
                NeighbourhoodGroup = "Brooklyn",
                Neighbourhood = "Williamsburg",
                RoomType = "Entire home/apt",
                MinimumNights = 3,
                Label = 200
            });
            inputs.Add(new Input
            {
                NeighbourhoodGroup = "Bronx",
                Neighbourhood = "Mott Haven",
                RoomType = "Private room",
                MinimumNights = 1,
                Label = 60
            });

            foreach (var input in inputs)
            {
                Console.WriteLine($"{input.NeighbourhoodGroup} - {input.Neighbourhood} ({input.RoomType}, {input.MinimumNights}) => Predicted price: {predictor.Predict(input).Score}, Real price: {input.Label}");
            }

            Console.ReadKey();
        }
    }

    class Input
    {
        [LoadColumn(4)]
        public string NeighbourhoodGroup;

        [LoadColumn(5)]
        public string Neighbourhood;

        [LoadColumn(8)]
        public string RoomType;

        [LoadColumn(10)]
        public float MinimumNights;

        [LoadColumn(9)]
        public float Label;
    }

    class Output
    {
        public float Score;
    }
}
