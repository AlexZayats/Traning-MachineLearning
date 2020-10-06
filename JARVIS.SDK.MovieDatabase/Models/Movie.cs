using Newtonsoft.Json;
using System.Collections.Generic;

namespace JARVIS.SDK.MovieDatabase.Models
{
    public class Movie : BaseMedia
    {
        [JsonProperty("adult")]
        public bool Adult { get; set; }

        [JsonProperty("video")]
        public bool Video { get; set; }

        [JsonProperty("keywords")]
        public IEnumerable<BaseEntity> Keywords { get; set; }

        public string Prediction { get; set; }
    }
}
