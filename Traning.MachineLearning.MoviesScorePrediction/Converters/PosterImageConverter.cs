using JARVIS.SDK.MovieDatabase;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Traning.MachineLearning.MoviesScorePrediction.Converters
{
    public class PosterImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (MDBSDK.Configuration == null) return null;
            return new Uri($"{MDBSDK.Configuration.Images.BaseUrl}/{MDBSDK.Configuration.Images.PosterSizes.ToArray()[3]}/{value as string}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
