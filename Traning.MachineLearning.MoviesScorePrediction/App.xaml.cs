using JARVIS.Core.Networking.Clients;
using JARVIS.SDK.MovieDatabase;
using System.Threading.Tasks;
using System.Windows;

namespace Traning.MachineLearning.MoviesScorePrediction
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Task.Run(Load);
        }

        private async Task Load()
        {
            MDBSDK.ApiKey = ""; //API KEY FROM TMDb
            MDBSDK.Language = "en-US";
            using (var client = new ApiClient())
            {
                MDBSDK.Configuration = await client.Configuration();
            }
        }
    }
}
